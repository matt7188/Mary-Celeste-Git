using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lagging_Hallway : MonoBehaviour {

    Vector3 start;
    public GameObject Camera;

	// Use this for initialization
	void Start () {
        start = Camera.transform.position;
        StartCoroutine(RunLag());

    }

    IEnumerator RunLag()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, .5f));
            Camera.transform.position = new Vector3(start.x-Random.Range(1f,5f), start.y, start.z);
        }
    }
    }
