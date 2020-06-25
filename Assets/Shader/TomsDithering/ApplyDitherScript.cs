using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ApplyDitherScript : MonoBehaviour
{
    public Material mat;

    void OnRenderImage( RenderTexture src, RenderTexture dest)
    {
        //  src is the fully rendered scene that you would normally send to the monitor

        Graphics.Blit(src, dest, mat);

    }
}
