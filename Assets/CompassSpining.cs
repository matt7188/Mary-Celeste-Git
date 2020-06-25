using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassSpining : MonoBehaviour
{
    RectTransform rectTransform;
    public RectTransform Pointer;
    // Start is called before the first frame update
    void Start()
    {
        
         rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.Rotate(new Vector3(0, 0, 1));
        Pointer.Rotate(new Vector3(0, 0, -1));
    }
}
