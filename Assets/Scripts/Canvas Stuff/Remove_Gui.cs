using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Remove_Gui : MonoBehaviour {

    public GameObject TurnOffObject;
    public Text InfoOut;

    public void OnClick()
    {
        TurnOffObject.SetActive(false);
        FindObjectOfType<GameMananger>().ToggleCursor(false);
        if(InfoOut!=null)
        InfoOut.text = "";
    }


}
