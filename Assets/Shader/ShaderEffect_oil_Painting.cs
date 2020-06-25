using System.Collections;
using UnityEngine;

[ExecuteInEditMode]
public class ShaderEffect_oil_Painting : MonoBehaviour
{
    public Material mat;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, dest, mat);

    }
}