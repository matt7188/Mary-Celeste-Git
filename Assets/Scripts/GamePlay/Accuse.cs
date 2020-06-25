using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accuse : Interactable{

    protected override void ShowInformation()
    {
        Display_Object.SetActive(true);
        Select_person Send_Info = Display_Object.GetComponent<Select_person>();
        GetComponentsInChildren<Transform>()[1].gameObject.layer = 8;
        FindObjectOfType<GameMananger>().ToggleCursor(true);
        FindObjectOfType<GameMananger>().InfoOutput.text = "Select which person to accuse. A wrong result will result in the death of an innocent.";
        
        Send_Info.CreateButtons();
    }
}
