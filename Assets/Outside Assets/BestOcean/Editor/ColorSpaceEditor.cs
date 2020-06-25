using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class ColorSpaceEditor : ScriptableObject
{
    // Start is called before the first frame update
    static ColorSpaceEditor()
    {
        if(PlayerSettings.colorSpace != ColorSpace.Linear)
        {
            PlayerSettings.colorSpace = ColorSpace.Linear;
            Debug.Log("Color space change to linear");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
