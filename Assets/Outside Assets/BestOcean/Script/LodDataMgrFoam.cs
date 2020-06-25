using UnityEngine;


/// <summary>
/// A persistent foam simulation that moves around with a displacement LOD. The input is fully combined water surface shape.
/// </summary>
public class LodDataMgrFoam : LodDataMgrPersistent
{
    public override SimType LodDataType { get { return SimType.Foam; } }
    protected override string ShaderSim { get { return "Hidden/Ocean/Simulation/Update Foam"; } }
    public override RenderTextureFormat TextureFormat { get { return RenderTextureFormat.RHalf; } }

    public override SimSettingsBase CreateDefaultSettings()
    {
        var settings = ScriptableObject.CreateInstance<SimSettingsFoam>();
        settings.name = SimName + " Auto-generated Settings";
        return settings;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetAdditionalSimParams(int lodIdx, Material simMaterial)
    {
        base.SetAdditionalSimParams(lodIdx, simMaterial);

        simMaterial.SetFloat("_FoamFadeRate", Settings._foamFadeRate);
        simMaterial.SetFloat("_WaveFoamStrength", Settings._waveFoamStrength);
        simMaterial.SetFloat("_WaveFoamCoverage", Settings._waveFoamCoverage);
        simMaterial.SetFloat("_ShorelineFoamMaxDepth", Settings._shorelineFoamMaxDepth);
        simMaterial.SetFloat("_ShorelineFoamStrength", Settings._shorelineFoamStrength);

        // assign animated waves - to slot 1 current frame data
        Ocean.Instance._lodDataAnimWaves.BindResultData(lodIdx, 1, simMaterial);

        // assign sea floor depth - to slot 1 current frame data
        if (Ocean.Instance._createSeaFloorDepthData)
        {
            Ocean.Instance._lodDataSeaDepths.BindResultData(lodIdx, 1, simMaterial);
        }
        else
        {
            LodDataMgrSeaFloorDepth.BindNull(1, simMaterial);
        }
    }

    protected override int GetNumSubsteps(float dt)
    {
        // foam always does just one sim step
        return 1;
    }

    static int[] _paramsSampler;
    public static int ParamIdSampler(int slot)
    {
        if (_paramsSampler == null)
            LodTransform.CreateParamIDs(ref _paramsSampler, "_LD_Sampler_Foam_");
        return _paramsSampler[slot];
    }
    protected override int GetParamIdSampler(int slot)
    {
        return ParamIdSampler(slot);
    }
    public static void BindNull(int shapeSlot, Material properties)
    {
        properties.SetTexture(ParamIdSampler(shapeSlot), Texture2D.blackTexture);
    }
    public static void BindNull(int shapeSlot, MaterialPropertyBlock properties)
    {
        properties.SetTexture(ParamIdSampler(shapeSlot), Texture2D.blackTexture);
    }

    SimSettingsFoam Settings { get { return _settings as SimSettingsFoam; } }
}

