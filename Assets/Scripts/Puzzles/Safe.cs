using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : Interactable
{
    protected override void ShowInformation()
    {
        if (Play_On_Click != null)
        {
            Play_On_Click.Play();
        }
        FindObjectOfType<GameMananger>().ToggleCursor(true);
        FindObjectOfType<GameMananger>().TimePass(2);
        Display_Object.GetComponent<SafePuzzleMananger>().SetUp();
        Display_Object.SetActive(true);
    }
}
