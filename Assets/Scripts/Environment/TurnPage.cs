using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPage : MonoBehaviour {

    public Book_Flip BookToTurn;
    public Notebook notebookToTurn;
    public bool Forward;

    public AudioSource turnPage;

    public void CheckActive()
    {
        if (Forward)
        {
            if (notebookToTurn.Page== notebookToTurn.MaxPage)
            {
                Button myButtonScript = GetComponent<Button>();
                myButtonScript.interactable = false;
            }
            else
            {
                Button myButtonScript = GetComponent<Button>();
                myButtonScript.interactable = true;
            }
        }
        else
        {
            if (notebookToTurn.Page == 1)
            {
                Button myButtonScript = GetComponent<Button>();
                myButtonScript.interactable = false;
            }
            else
            {
                Button myButtonScript = GetComponent<Button>();
                myButtonScript.interactable = true;
            }
        }
    }


        public void OnClick()
    {
        turnPage.Play();
        notebookToTurn.TurnPage(Forward);
        CheckActive();

    }
    
    void OnMouseDown()
    {
        turnPage.Play();
        BookToTurn.TurnPage(Forward);
    }

}
