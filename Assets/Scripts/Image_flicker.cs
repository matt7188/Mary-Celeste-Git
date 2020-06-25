using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image_flicker : MonoBehaviour
{
    public Material[] Images;
    new Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        StartCoroutine(ImageFlip());
    }

    IEnumerator ImageFlip()
    {
        while (true)
        {
            renderer.material = Images[Random.Range(0, Images.Length)];

            yield return new WaitForSeconds(Random.Range(0, 2f));
        }

    }
}

