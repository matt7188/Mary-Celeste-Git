using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBackCam : MonoBehaviour {


    public GameObject BookCam;
    public GameObject ProperCam;

    void OnMouseDown()
    {
        BookCam.SetActive(false);
        ProperCam.SetActive(true);
        StartCoroutine(PreventRepress());
    }

    IEnumerator PreventRepress()
    {

        yield return new WaitForSeconds(.5f);
        FindObjectOfType<GameMananger>().ToggleCursor(false);
    }

}
