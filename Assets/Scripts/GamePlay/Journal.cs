using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : Interactable {

    protected override void ShowInformation()
    {
        if (Play_On_Click != null)
        {
            Play_On_Click.Play();
        }
        //Display_Object.SetActive(true);
        Book_Flip FindBook = Display_Object.GetComponent<Book_Flip>();

        FindObjectOfType<GameMananger>().ToggleCursor(true);
        FindObjectOfType<GameMananger>().TimePass(2);
        FindBook.setSelf(this.GetComponent<Book>());
    }
}
