using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnOffPuzzles : MonoBehaviour
{

    public GameObject[] turnOff;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(AfterSetup());
        
    }
     IEnumerator AfterSetup()
    {
        yield return new WaitForFixedUpdate();

        foreach (GameObject swtiching in turnOff)
        {
            swtiching.SetActive(false);
        }
    }
   
}
