using UnityEngine;

[CreateAssetMenu(fileName = "OceanWaves", menuName = "Ocean/Ocean Wave Spectrum", order = 10000)]
public class OceanWaveSpectrum : ScriptableObject
{
    private const int NUM_OCTAVES = 12;
    public static readonly float SMALLEST_WL_POW_2 = -2f;

    public static readonly float MIN_POWER_LOG = -6f;
    public static readonly float MAX_POWER_LOG = 3f;

    [Tooltip("Variance of flow direction, in degrees"), Range(0f, 180f)]
    public float _waveDirectionVariance = 90f;

    [Tooltip("More gravity means faster waves."), Range(0f, 25f)]
    public float _gravityScale = 1f;

    [SerializeField, HideInInspector]
    float[] _powerLog = new float[NUM_OCTAVES]
        { -6f, -4.0088496f, -3.4452133f, -2.6996124f, -2.615044f, -1.2080691f, -0.53905386f, 0.27448857f, 0.53627354f, 1.0282621f, 1.4403292f, -6f };

    [SerializeField, HideInInspector]
    bool[] _powerDisabled = new bool[NUM_OCTAVES];

    [HideInInspector]
    public float[] _chopScales = new float[NUM_OCTAVES]
        { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };

    [HideInInspector]
    public float[] _gravityScales = new float[NUM_OCTAVES]
        { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };

    [Tooltip("Scales horizontal displacement"), Range(0f, 2f)]
    public float _chop = 1f;


    public static float SmallWavelength(float octaveIndex) { return Mathf.Pow(2f, SMALLEST_WL_POW_2 + octaveIndex); }

    public float GetAmplitude(float wavelength, float componentsPerOctave)
    {
        if (wavelength <= 0.001f)
        {
            Debug.LogError("Wavelength must be >= 0f");
            return 0f;
        }

        float wl_pow2 = Mathf.Log(wavelength) / Mathf.Log(2f);
        wl_pow2 = Mathf.Clamp(wl_pow2, SMALLEST_WL_POW_2, SMALLEST_WL_POW_2 + NUM_OCTAVES - 1f);

        int index = (int)(wl_pow2 - SMALLEST_WL_POW_2);

        if (index >= _powerLog.Length)
        {
            Debug.LogError("Out of bounds index");
            return 0f;
        }

        if (_powerDisabled[index])
        {
            return 0f;
        }

        // The amplitude calculation follows this nice paper from Frechot:
        // https://hal.archives-ouvertes.fr/file/index/docid/307938/filename/frechot_realistic_simulation_of_ocean_surface_using_wave_spectra.pdf
        float wl_lo = Mathf.Pow(2f, Mathf.Floor(wl_pow2));
        float k_lo = 2f * Mathf.PI / wl_lo;
        float omega_lo = k_lo * ComputeWaveSpeed(wl_lo);
        float wl_hi = 2f * wl_lo;
        float k_hi = 2f * Mathf.PI / wl_hi;
        float omega_hi = k_hi * ComputeWaveSpeed(wl_hi);

        float domega = (omega_lo - omega_hi) / componentsPerOctave;

        float a_2 = 2f * Mathf.Pow(10f, _powerLog[index]) * domega;
        var a = Mathf.Sqrt(a_2);
        return a;
    }

    float ComputeWaveSpeed(float wavelength)
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

    public void GenerateWaveData(int componentsPerOctave, ref float[] wavelengths, ref float[] anglesDeg, ref float[] phases)
    {
        int totalComponents = NUM_OCTAVES * componentsPerOctave;

        if (wavelengths == null || wavelengths.Length != totalComponents) wavelengths = new float[totalComponents];
        if (anglesDeg == null || anglesDeg.Length != totalComponents) anglesDeg = new float[totalComponents];
        if (phases == null || phases.Length != totalComponents) phases = new float[totalComponents];

        float minWavelength = Mathf.Pow(2f, SMALLEST_WL_POW_2);
        float invComponentsPerOctave = 1f / componentsPerOctave;

        for (int octave = 0; octave < NUM_OCTAVES; octave++)
        {
            for (int i = 0; i < componentsPerOctave; i++)
            {
                int index = octave * componentsPerOctave + i;
                float minWavelengthi = minWavelength + invComponentsPerOctave * minWavelength * i;
                float maxWavelengthi = Mathf.Min(minWavelengthi + invComponentsPerOctave * minWavelength, 2f * minWavelength);
                wavelengths[index] = Mathf.Lerp(minWavelengthi, maxWavelengthi, Random.value);

                float rnd;

                rnd = (i + Random.value) * invComponentsPerOctave;
                anglesDeg[index] = (2f * rnd - 1f) * _waveDirectionVariance;

                rnd = (i + Random.value) * invComponentsPerOctave;
                phases[index] = 2f * Mathf.PI * rnd;
            }

            minWavelength *= 2f;
        }
    }
}
