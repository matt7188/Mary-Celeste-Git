using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CipherBook : Interactable
{

 


    protected override void ShowInformation()
    {
        //Display_Object.SetActive(true);
        Book_Flip FindBook = Display_Object.GetComponent<Book_Flip>();

        FindObjectOfType<GameMananger>().ToggleCursor(true);
        FindObjectOfType<GameMananger>().TimePass(2);
        FindBook.setSelf(this.GetComponent<Book>());
    }
}