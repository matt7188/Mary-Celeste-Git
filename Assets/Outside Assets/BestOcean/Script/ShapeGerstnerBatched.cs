using UnityEngine;
using UnityEngine.Rendering;

public class ShapeGerstnerBatched : MonoBehaviour
{
    public Mesh _rasterMesh;
    public Shader _waveShader;
    public OceanWaveSpectrum _spectrum;

    public int _componentsPerOctave = 5;

    [Range(0f, 1f)]
    public float _weight = 1f;


    float[] _wavelengths;
    float[] _amplitudes;
    float[] _angleDegs;
    float[] _phases;

    Material[] _materials = null;
    bool[] _drawLOD;

    // IMPORTANT - this mirrors the constant with the same name in ShapeGerstnerBatch.shader, both must be updated together!
    const int BATCH_SIZE = 32;

    enum CmdBufStatus
    {
        NoStatus,
        NotAttached,
        Attached
    }

    // scratch data used by batching code
    struct UpdateBatchScratchData
    {
        public static Vector4[] _wavelengthsBatch = new Vector4[BATCH_SIZE / 4];
        public static Vector4[] _ampsBatch = new Vector4[BATCH_SIZE / 4];
        public static Vector4[] _anglesBatch = new Vector4[BATCH_SIZE / 4];
        public static Vector4[] _phasesBatch = new Vector4[BATCH_SIZE / 4];
        public static Vector4[] _chopScalesBatch = new Vector4[BATCH_SIZE / 4];
        public static Vector4[] _gravityScalesBatch = new Vector4[BATCH_SIZE / 4];
    }

    void Start()
    {
        if (Ocean.Instance == null)
        {
            enabled = false;
            return;
        }

        if (_spectrum == null)
        {
            _spectrum = ScriptableObject.CreateInstance<OceanWaveSpectrum>();
            _spectrum.name = "Default Waves (auto)";
        }
    }

    void Update()
    {
        Random.State randomStateBkp = Random.state;
        Random.InitState(0);

        _spectrum.GenerateWaveData(_componentsPerOctave, ref _wavelengths, ref _angleDegs, ref _phases);

        Random.state = randomStateBkp;

        UpdateAmplitudes();

        if (_materials == null)
        {
            InitMaterials();
        }
    }

    void UpdateAmplitudes()
    {
        if (_amplitudes == null || _amplitudes.Length != _wavelengths.Length)
        {
            _amplitudes = new float[_wavelengths.Length];
        }

        for (int i = 0; i < _wavelengths.Length; i++)
        {
            _amplitudes[i] = _weight * _spectrum.GetAmplitude(_wavelengths[i], _componentsPerOctave);
        }
    }


    void InitMaterials()
    {
        foreach (var child in transform)
        {
            Destroy((child as Transform).gameObject);
        }

        // num octaves plus one, because there is an additional last bucket for large wavelengths
        _materials = new Material[Ocean.Instance.CurrentLodCount];
        _drawLOD = new bool[_materials.Length];

        for (int i = 0; i < _materials.Length; i++)
        {
            _materials[i] = new Material(_waveShader);
            _drawLOD[i] = false;
        }
    }

    /// <summary>
    /// Computes Gerstner params for a set of waves, for the given lod idx. Writes shader data to the given material.
    /// Returns number of wave components rendered in this batch.
    /// </summary>
    int UpdateBatch(int lodIdx, int firstComponent, int lastComponentNonInc, Material material)
    {
        int numComponents = lastComponentNonInc - firstComponent;
        int numInBatch = 0;
        int dropped = 0;

        // register any nonzero components
        for (int i = 0; i < numComponents; i++)
        {
            float wl = _wavelengths[firstComponent + i];
            float amp = _amplitudes[firstComponent + i];

            if (amp >= 0.001f)
            {
                if (numInBatch < BATCH_SIZE)
                {
                    int vi = numInBatch / 4;
                    int ei = numInBatch - vi * 4;
                    UpdateBatchScratchData._wavelengthsBatch[vi][ei] = wl;
                    UpdateBatchScratchData._ampsBatch[vi][ei] = amp;
                    UpdateBatchScratchData._anglesBatch[vi][ei] =
                        Mathf.Deg2Rad * (Ocean.Instance._windDirectionAngle + _angleDegs[firstComponent + i]);
                    UpdateBatchScratchData._phasesBatch[vi][ei] = _phases[firstComponent + i];
                    UpdateBatchScratchData._chopScalesBatch[vi][ei] = _spectrum._chopScales[(firstComponent + i) / _componentsPerOctave];
                    UpdateBatchScratchData._gravityScalesBatch[vi][ei] = _spectrum._gravityScales[(firstComponent + i) / _componentsPerOctave];
                    numInBatch++;
                }
                else
                {
                    dropped++;
                }
            }
        }

        if (dropped > 0)
        {
            Debug.LogWarning(string.Format("Gerstner LOD{0}: Batch limit reached, dropped {1} wavelengths. To support bigger batch sizes, see the comment around the BATCH_SIZE declaration.", lodIdx, dropped), this);
            numComponents = BATCH_SIZE;
        }

        if (numInBatch == 0)
        {
            // no waves to draw - abort
            return numInBatch;
        }

        // if we did not fill the batch, put a terminator signal after the last position
        if (numInBatch < BATCH_SIZE)
        {
            int vi = numInBatch / 4;
            int ei = numInBatch - vi * 4;
            UpdateBatchScratchData._wavelengthsBatch[vi][ei] = 0f;
        }

        // apply the data to the shape material
        material.SetVectorArray("_Wavelengths", UpdateBatchScratchData._wavelengthsBatch);
        material.SetVectorArray("_Amplitudes", UpdateBatchScratchData._ampsBatch);
        material.SetVectorArray("_Angles", UpdateBatchScratchData._anglesBatch);
        material.SetVectorArray("_Phases", UpdateBatchScratchData._phasesBatch);
        material.SetVectorArray("_ChopScales", UpdateBatchScratchData._chopScalesBatch);
        material.SetVectorArray("_GravityScales", UpdateBatchScratchData._gravityScalesBatch);
        material.SetFloat("_NumInBatch", numInBatch);
        material.SetFloat("_Chop", _spectrum._chop);
        material.SetFloat("_Gravity", Ocean.Instance.Gravity * _spectrum._gravityScale);
        material.SetFloat("_GridSize", Ocean.Instance._lods[lodIdx]._renderData._texelWidth);

        Ocean.Instance._lodDataAnimWaves.BindResultData(lodIdx, 0, material);

        if (Ocean.Instance._createSeaFloorDepthData)
        {
            Ocean.Instance._lodDataSeaDepths.BindResultData(lodIdx, 0, material, false);
        }

        return numInBatch;
    }

    void LateUpdate()
    {
        int componentIdx = 0;

        float minWl = Ocean.Instance._lods[0].MaxWavelength() / 2f;
        while (_wavelengths[componentIdx] < minWl && componentIdx < _wavelengths.Length)
        {
            componentIdx++;
        }

        // batch together appropriate wavelengths for each lod, except the last lod, which are handled separately below
        for (int lod = 0; lod < Ocean.Instance.CurrentLodCount - 1; lod++, minWl *= 2f)
        {
            int startCompIdx = componentIdx;
            while (componentIdx < _wavelengths.Length && _wavelengths[componentIdx] < 2f * minWl)
            {
                componentIdx++;
            }

            _drawLOD[lod] = UpdateBatch(lod, startCompIdx, componentIdx, _materials[lod]) > 0;
        }

        _drawLOD[Ocean.Instance.CurrentLodCount - 1] =
            UpdateBatch(Ocean.Instance.CurrentLodCount - 1, componentIdx, _wavelengths.Length, _materials[Ocean.Instance.CurrentLodCount - 1]) > 0;
    }

    public void BuildCommandBuffer(int lodIdx, Ocean ocean, CommandBuffer buf)
    {
        var lodCount = ocean.CurrentLodCount;

        // LODs up to but not including the last lod get the normal sets of waves
        if (lodIdx < lodCount - 1 && _drawLOD[lodIdx])
        {
            buf.DrawMesh(_rasterMesh, Matrix4x4.identity, _materials[lodIdx]);
        }


        // Last lod gets the big wavelengths
        if (lodIdx == lodCount - 1 && _drawLOD[lodIdx])
        {
            buf.DrawMesh(_rasterMesh, Matrix4x4.identity, _materials[Ocean.Instance.CurrentLodCount - 1]);
        }
    }

    float ComputeWaveSpeed(float wavelength/*, float depth*/)
    {
        // wave speed of deep sea ocean waves: https://en.wikipedia.org/wiki/Wind_wave
        // https://en.wikipedia.org/wiki/Dispersion_(water_waves)#Wave_propagation_and_dispersion
        float g = 9.81f;
        float k = 2f * Mathf.PI / wavelength;
        //float h = max(depth, 0.01);
        //float cp = sqrt(abs(tanh_clamped(h * k)) * g / k);
        float cp = Mathf.Sqrt(g / k);
        return cp;
    }


    void OnEnable()
    {
        if (Ocean.Instance != null && Ocean.Instance._lodDataAnimWaves != null)
        {
            Ocean.Instance._lodDataAnimWaves.AddGerstnerComponent(this);
        }
    }

    void OnDisable()
    {
        if (Ocean.Instance != null && Ocean.Instance._lodDataAnimWaves != null)
        {
            Ocean.Instance._lodDataAnimWaves.RemoveGerstnerComponent(this);
        }
    }
}
