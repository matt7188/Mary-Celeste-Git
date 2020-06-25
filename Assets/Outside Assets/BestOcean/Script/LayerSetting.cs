using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSetting : MonoBehaviour {

	// Use this for initialization
	void Awake () {
#if UNITY_EDITOR
        LayerUtil.CreateLayer("Terrain");
#endif
    }
}
