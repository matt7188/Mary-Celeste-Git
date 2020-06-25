using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talking_Script :Interactable {

    protected override void ShowInformation()
    {

        Tutorial tut = FindObjectOfType<Tutorial>();
            if (tut!=null)
                tut.ChangeTutorial(1);

        if (this.GetComponent<People>().alive)
        {
            this.GetComponent<DialogueHandler>().SelectedCharacter();
            //Send_Info.OpenDialogueSystem(this.GetComponent<DialogueHandler>().firstTimeNode);
            //Send_Info.UpdateDialogueCanvas();
            FindObjectOfType<GameMananger>().ToggleCursor(true);
        }
        
   // Send_Info.PersonTalking = this.gameObject;
     //   Send_Info.WriteWords();
    }

    }
