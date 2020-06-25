using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainRoomWind : MonoBehaviour
{
    Vector3 startPosition;

    bool reset = false;

    private void Start()
    {

        startPosition = this.transform.localPosition;
        
    }


    void Update()
    {
        if (Random.Range(0, 500) < 2)
        {
            
        StartCoroutine(DoFlicker());
        }
        if (!reset)
        this.transform.localPosition = new Vector3(startPosition.x, this.transform.localPosition.y+ (Random.Range(0f, 10f)/50f), startPosition.z );
    }


    private IEnumerator DoFlicker()
    {
        reset = true;
        transform.localPosition = new Vector3(startPosition.x + 1000, this.transform.localPosition.y, startPosition.z);
        yield return new WaitForSeconds(.5f);
        transform.localPosition = new Vector3(startPosition.x + 1000, startPosition.y, startPosition.z);
        yield return new WaitForSeconds(.5f);
        transform.localPosition = startPosition;

        reset = false;
    }

}
