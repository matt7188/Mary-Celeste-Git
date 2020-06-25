
using UnityEngine;


/// <summary>
/// Sets shader parameters for each geometry tile/chunk.
/// </summary>
public class OceanChunkRenderer : MonoBehaviour
{
    public bool _drawRenderBounds = false;


    Renderer _rend;
    MaterialPropertyBlock _mpb;

    int _lodIndex = -1;
    int _totalLodCount = -1;
    float _baseVertDensity = 32f;

    int _reflectionTexId = -1;

    void Start()
    {
        _rend = GetComponent<Renderer>();
        _reflectionTexId = Shader.PropertyToID("_ReflectionTex");

    }



    // Called when visible to a camera
    void OnWillRenderObject()
    {
        // Depth texture is used by ocean shader for transparency/depth fog, and for fading out foam at shoreline.
        Camera.current.depthTextureMode |= DepthTextureMode.Depth;

        // per instance data

        if (_mpb == null)
        {
            _mpb = new MaterialPropertyBlock();
        }
        _rend.GetPropertyBlock(_mpb);

        float meshScaleLerp = 0f;
        float farNormalsWeight = 1f;
        _mpb.SetVector("_InstanceData", new Vector4(meshScaleLerp, farNormalsWeight, _lodIndex));

        // geometry data
        // compute grid size of geometry. take the long way to get there - make sure we land exactly on a power of two
        // and not inherit any of the lossy-ness from lossyScale.
        float squareSize = Mathf.Pow(2f, Mathf.Round(Mathf.Log(transform.lossyScale.x) / Mathf.Log(2f))) / _baseVertDensity;
        float mul = 1.875f; // fudge 1
        float pow = 1.4f; // fudge 2
        float normalScrollSpeed0 = Mathf.Pow(Mathf.Log(1f + 2f * squareSize) * mul, pow);
        float normalScrollSpeed1 = Mathf.Pow(Mathf.Log(1f + 4f * squareSize) * mul, pow);
        _mpb.SetVector("_GeomData", new Vector3(squareSize, normalScrollSpeed0, normalScrollSpeed1));

        // assign lod data to ocean shader
        var ldaws = Ocean.Instance._lodDataAnimWaves;
        var ldsds = Ocean.Instance._lodDataSeaDepths;
        var ldfoam = Ocean.Instance._lodDataFoam;

        ldaws.BindResultData(_lodIndex, 0, _mpb);
        if (Ocean.Instance._createFoamSim) ldfoam.BindResultData(_lodIndex, 0, _mpb);
        if (Ocean.Instance._createSeaFloorDepthData) ldsds.BindResultData(_lodIndex, 0, _mpb);

        if (_lodIndex + 1 < Ocean.Instance.CurrentLodCount)
        {
            ldaws.BindResultData(_lodIndex + 1, 1, _mpb);
            if (Ocean.Instance._createFoamSim) ldfoam.BindResultData(_lodIndex + 1, 1, _mpb);
            if (Ocean.Instance._createSeaFloorDepthData) ldsds.BindResultData(_lodIndex + 1, 1, _mpb);
        }

        var reflTex = OceanPlanarReflection.GetRenderTexture(Camera.current.targetDisplay);
        if (reflTex)
        {
            _mpb.SetTexture(_reflectionTexId, reflTex);
        }
        else
        {
            _mpb.SetTexture(_reflectionTexId, Texture2D.blackTexture);
        }

        // Hack - due to SV_IsFrontFace occasionally coming through as true for backfaces,
        // add a param here that forces ocean to be in undrwater state. I think the root
        // cause here might be imprecision or numerical issues at ocean tile boundaries, although
        // i'm not sure why cracks are not visible in this case.
        float heightOffset = Ocean.Instance.ViewerHeightAboveWater;
        _mpb.SetFloat("_ForceUnderwater", heightOffset < -2f ? 1f : 0f);

        _rend.SetPropertyBlock(_mpb);
    }



    public void SetInstanceData(int lodIndex, int totalLodCount, float baseVertDensity)
    {
        _lodIndex = lodIndex; _totalLodCount = totalLodCount; _baseVertDensity = baseVertDensity;
    }

   
}

