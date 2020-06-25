

using UnityEngine;


/// <summary>
/// Tags this object as an ocean depth provider. Renders depth every frame and should only be used for dynamic objects.
/// For static objects, use an Ocean Depth Cache.
/// </summary>
public class RegisterSeaFloorDepthInput : RegisterLodDataInput<LodDataMgrSeaFloorDepth>
{
    [SerializeField] bool _assignOceanDepthMaterial = true;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (_assignOceanDepthMaterial)
        {
            var rend = GetComponent<Renderer>();
            rend.material = new Material(Shader.Find("Ocean/Ocean Depth From Geometry"));
        }
    }
}

