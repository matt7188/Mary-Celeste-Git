using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CipherPuzzle : Interactable
{
    protected override void ShowInformation()
    {
        if (Play_On_Click != null)
        {
            Play_On_Click.Play();
        }

        FindObjectOfType<GameMananger>().ToggleCursor(true);
        FindObjectOfType<GameMananger>().TimePass(5);
        Display_Object.GetComponent<CipherOutput>().Init();
        Display_Object.SetActive(true);
    }
}
