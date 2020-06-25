using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLayer : MonoBehaviour {
    public string layerName;
	// Use this for initialization
	void Start () {
        setLayer(gameObject);

	}

    void setLayer(GameObject obj)
    {
        obj.layer = LayerMask.NameToLayer(layerName);
        for(int i = 0; i < obj.transform.childCount; i++)
        {
            Transform tr = obj.transform.GetChild(i);
            setLayer(tr.gameObject);
        }
    }
	
}
