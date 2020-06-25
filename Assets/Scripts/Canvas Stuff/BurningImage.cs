using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningImage : MonoBehaviour
{

    public GameObject[] ImagesToSelect;
    public float StartState;
    public float EndState;
    public float Step;

    public void SetUpImage()
    {
        foreach(GameObject Changing in ImagesToSelect)
        {
            Changing.SetActive(false);
            Changing.GetComponent<MeshRenderer>().material.SetFloat("_DissolveMaskOffset", StartState);
        }
    }


    public void BurnPicture(GameObject WhichPic, bool Appearing)
    {
        WhichPic.SetActive(true);
        StartCoroutine(BurnPictureInTime(WhichPic,Appearing));
    }


    IEnumerator BurnPictureInTime(GameObject WhichPic, bool Appearing)
    {
        float CurentState = StartState;
        //float Multi = 1f;

        Material BurningShader = WhichPic.GetComponent<MeshRenderer>().material;

        if (Appearing) {
            while (CurentState <= EndState)
            {
                yield return new WaitForSeconds(.01f);
                BurningShader.SetFloat("_DissolveMaskOffset", CurentState);
                CurentState += Step;


            } }
        else
        {
            BurningShader.SetFloat("_DissolveMaskOffset", EndState);
            while (CurentState >= StartState)
            {
                yield return new WaitForSeconds(.01f);
                BurningShader.SetFloat("_DissolveMaskOffset", CurentState);
                CurentState -= Step;


            }
        }

    }

}
