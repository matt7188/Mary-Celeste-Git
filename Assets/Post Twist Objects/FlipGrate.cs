using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipGrate : MonoBehaviour
{
    public GameObject grate;
    public GameObject sea;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine("Rotate");
    }

    private IEnumerator Rotate()
    {
        int check=0;
        while (check < 15)
        {
            yield return new WaitForSeconds(.01f);
            grate.transform.Rotate(0, 0, 5);
            check++;
        }
        sea.SetActive(false);
    }

}
