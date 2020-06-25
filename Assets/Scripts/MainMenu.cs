using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public FullGameMananger.TurnPageTo whichPage;
    public GameObject[] TurnOn;

    public bool Book;


    void OnMouseDown()
    {
        

            foreach (GameObject Turning in TurnOn)
            {
                Turning.SetActive(true);
            }
            FindObjectOfType<FullGameMananger>().TurnPage(whichPage,Book);
        


    }
}
