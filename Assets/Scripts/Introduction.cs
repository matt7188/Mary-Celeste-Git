using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Introduction : MonoBehaviour
{

    public bool Run;

    public Image Blackout;
    public Image Logo;

    public GameObject ButtonActive;

    public GameObject MainCamera;
    public GameObject ShipCamera;
    public GameObject FakeShipCamera;

    private Vector3 StartMain;
    private Vector3 ShipMain;
    private Quaternion QStartMain;
    private Quaternion QShipMain;
    public Transform EndMain;
    public Transform EndShipMain;

    // Use this for initialization
    void Start()
    {

        FullGameMananger[] objs = GameObject.FindObjectsOfType<FullGameMananger>();

        if (objs.Length > 2)
        {
            Run = false;
        }

        if (Run)
        {
            ButtonActive.SetActive(false);
            StartMain = MainCamera.transform.position;
            ShipMain = ShipCamera.transform.position;
            QStartMain = MainCamera.transform.rotation;
            QShipMain = ShipCamera.transform.rotation;
            MainCamera.transform.position = EndMain.position;
            ShipCamera.transform.position = EndShipMain.position;
            MainCamera.transform.rotation = EndMain.rotation;
            ShipCamera.transform.rotation = EndShipMain.rotation;
            var tempcolor = Logo.color;
            tempcolor.a = 0;
            Logo.color = tempcolor;
            //ShipCamera.tag = "MainCamera";
            // MainCamera.tag = "Untagged";
            MainCamera.gameObject.SetActive(false);

            StartCoroutine(RunIntro());

        }
        else
        {
            FakeShipCamera.gameObject.SetActive(false);
            Blackout.gameObject.SetActive(false);
            Logo.gameObject.SetActive(false);
        }

    }

    IEnumerator RunIntro()
    {
        float j = 0;

        while (Time.time < 7.5)
        {
            yield return new WaitForSeconds(.5f);
        }

            while (Logo.color.a < 1)
        {
            j++;
            
                var tempcolor = Logo.color;
                tempcolor.a = j / 70;
            Logo.color = tempcolor;

            yield return new WaitForSeconds(0.01f);

        }
        float startTime = Time.time;
        while (Time.time- startTime < 2)
        {
            yield return new WaitForSeconds(1);
        }
        while (Logo.color.a > 0)
        {
            j--;

            var tempcolor = Logo.color;
            tempcolor.a = j / 70;
            Logo.color = tempcolor;
            
            yield return new WaitForSeconds(0.01f);

        }
        startTime = Time.time;
        j = 20;
        while (ShipCamera.transform.position != ShipMain)
        {
            if (j > 0) { 
            j--;
            var tempcolor = Blackout.color;
            tempcolor.a = j / 20;
            Blackout.color = tempcolor;
        }
            else
            {
                Blackout.gameObject.SetActive(false);
                var tempcolor =Color.white;
                tempcolor.a = 0;
                Blackout.color = tempcolor;
                Logo.gameObject.SetActive(false);
            }
            
            ShipCamera.transform.position = Vector3.Lerp(EndShipMain.position, ShipMain, (Time.time- startTime) / 10);
            if (Time.time>=1)
            {
                ShipCamera.transform.rotation = Quaternion.Lerp(EndShipMain.rotation, QShipMain, ((Time.time-1)- startTime) / 9);
            }
            yield return new WaitForSeconds(.01f);
        }
        Blackout.gameObject.SetActive(true);
        while (Blackout.color.a < 1)
        {
            j++;

            var tempcolor = Blackout.color;
            tempcolor.a = j / 20;
            Blackout.color = tempcolor;

            yield return new WaitForSeconds(0.01f);

        }
        MainCamera.gameObject.SetActive( true);
        FakeShipCamera.gameObject.SetActive(false);
        startTime = Time.time;
        while (Time.time - startTime < 1)
        {
            yield return new WaitForSeconds(1);
        }
        while (Blackout.color.a > 0)
        {
            j--;

            var tempcolor = Blackout.color;
            tempcolor.a = j / 30;
            Blackout.color = tempcolor;

            yield return new WaitForSeconds(0.01f);

        }

         startTime = Time.time;
        Blackout.gameObject.SetActive(false);
        while (MainCamera.transform.position != StartMain)
        {
            MainCamera.transform.position = Vector3.Lerp(EndMain.position, StartMain, (Time.time-startTime) / 10);
            MainCamera.transform.rotation = Quaternion.Lerp(EndMain.rotation, QStartMain, (Time.time - startTime) / 10);

            yield return new WaitForSeconds(.01f);
        }
        
    }
}
