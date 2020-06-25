using UnityEngine;


public class Ocean : MonoBehaviour
{
    public float xPosition;
    public float yPosition;
    public float zPosition;
    public float CurrentTime { get { return Time.time; } }

    [SerializeField]
    Material _material;

    public Material OceanMaterial { get { return _material; } }

    [SerializeField]
    string _layerName = "Water";
    public string LayerName { get { return _layerName; } }

    [Range(-180, 180)]
    public float _windDirectionAngle = 0f;
    public Vector2 WindDir { get { return new Vector2(Mathf.Cos(Mathf.PI * _windDirectionAngle / 180f), Mathf.Sin(Mathf.PI * _windDirectionAngle / 180f)); } }

    [SerializeField, Delayed, Range(0f, 10f)]
    float _gravityMultiplier = 1f;
    public float Gravity { get { return _gravityMultiplier * Physics.gravity.magnitude; } }

    [Range(0, 15)]
    public float _minTexelsPerWave = 3f;

    [SerializeField, Delayed]
    public float _baseVertDensity = 64f;

    [SerializeField, Delayed, Range(2, LodDataMgr.MAX_LOD_COUNT)]
    int _lodCount = 7;
    public int LodDataResolution { get { return (int)(4f * _baseVertDensity); } }

    public SimSettingsAnimatedWaves _simSettingsAnimatedWaves;

    public bool _createSeaFloorDepthData = true;

    public bool _createFoamSim = true;
    public SimSettingsFoam _simSettingsFoam;



    [HideInInspector] public LodTransform[] _lods;
    [HideInInspector] public LodDataMgrAnimWaves _lodDataAnimWaves;
    [HideInInspector] public LodDataMgrSeaFloorDepth _lodDataSeaDepths;
    [HideInInspector] public LodDataMgrFoam _lodDataFoam;
    public int CurrentLodCount { get { return _lods.Length; } }
    public GameObject OceanInputs;

    BuildCommandBuffer buf;
    public float ViewerHeightAboveWater { get; private set; }
    public float depthMax = 1000f;
    void Awake()
    {
        _instance = this;

        OceanBuilder.GenerateMesh(this, _baseVertDensity, _lodCount);

        if (null == GetComponent<BuildCommandBufferBase>())
        {
            buf = gameObject.AddComponent<BuildCommandBuffer>();
            buf.enabled = false;
        }
    }

    private void Start()
    {
        Shader.SetGlobalFloat("depthMax", depthMax);
        OceanInputs.SetActive(true);
        buf.enabled = true;
        
    }

    void LateUpdate()
    {
        //set global shader params
        
       Shader.SetGlobalFloat("_TexelsPerWave", _minTexelsPerWave);
       Shader.SetGlobalVector("_WindDirXZ", WindDir);
       Shader.SetGlobalFloat("NowTime", CurrentTime);
        
       Shader.SetGlobalVector("_OceanCenterPosWorld", transform.position);
        Shader.SetGlobalFloat("ViewY", yPosition);
        Vector3 pos = new Vector3(xPosition,yPosition,zPosition);
        pos.y = transform.position.y;

        transform.position = pos;
        LateUpdateLods();  
    }

    void LateUpdateLods()
    {
        foreach (var lt in _lods)
        {
            lt.UpdateTransform();
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }


    public int GetLodIndex(float gridSize)
    {
        //gridSize = 4f * transform.lossyScale.x * Mathf.Pow(2f, result) / (4f * _baseVertDensity);
        int result = Mathf.RoundToInt(Mathf.Log((4f * _baseVertDensity) * gridSize / (4f * transform.lossyScale.x)) / Mathf.Log(2f));

        if (result < 0 || result >= _lodCount)
        {
            result = -1;
        }

        return result;
    }

    static Ocean _instance;
    public static Ocean Instance { get { return _instance ?? (_instance = FindObjectOfType<Ocean>()); } }
}
