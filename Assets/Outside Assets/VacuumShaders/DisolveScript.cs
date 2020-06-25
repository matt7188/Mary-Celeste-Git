using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisolveScript : MonoBehaviour
{
    public Material Disolve;
    float Offset = -5;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunLag());

    }

    IEnumerator RunLag()
    {
        while (true)
        {
            yield return new WaitForSeconds(.01f);
            Disolve.SetFloat("_DissolveMaskOffset", Offset);
            Offset += .03f;


        }
    }
}

