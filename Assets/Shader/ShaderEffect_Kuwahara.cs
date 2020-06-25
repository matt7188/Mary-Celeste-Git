using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderEffect_Kuwahara : MonoBehaviour
{
    
    	public float StrokeSize = 1;
    private Material material_Kuwahara;

    void Awake()
    {
        material_Kuwahara = new Material(Shader.Find("Custom/Kuwahara"));
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material_Kuwahara.SetFloat("Size of the Strokes", StrokeSize);

        Graphics.Blit(source, destination, material_Kuwahara);

    }
}
