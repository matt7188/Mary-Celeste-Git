using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public bool RunTutorial;
    public bool MouseClick;
    public bool TabClick;
    public bool ItemsSeen;
    public bool PeopleTalkedTo;
    public bool JournalChecked;
    public bool AccuseAttempted;
    public Text Info;
    public GameObject SpiningLight;
    public Image[] Arrows;
    public GameObject Tab;
    public GameObject JournalObject;
    FullGameMananger Meta;
    Items[] AllItem;
    People[] AllPeople;

    void Awake()
    {

        Meta = FindObjectOfType<FullGameMananger>();
         if(Meta != null)
        {
            RunTutorial = Meta.RunTutorial;
           
        }

        if (!RunTutorial)
        {
            this.gameObject.SetActive(false);
        }
        else
        {

            AllItem = FindObjectsOfType<Items>();
            foreach (Items Change in AllItem)
            {
                Change.gameObject.layer = 11;
                foreach (Transform children in Change.transform)
                    children.gameObject.layer = 11;
            }

            AllPeople = FindObjectsOfType<People>();
            foreach (People Change in AllPeople)
            {
                Change.gameObject.layer = 11;
                foreach (Transform children in Change.transform)
                    children.gameObject.layer = 11;
            }
            JournalObject.layer = 11;
            FindObjectOfType<Accuse>().GetComponentsInChildren<Transform>()[1].gameObject.layer = 11;
            Tab.SetActive(false);
        }

    }
    void Update()
    {
        SpiningLight.transform.Rotate(3, 0, 0);

        if (!MouseClick && Input.GetMouseButtonDown(0))
        {
            MouseClick = true;
            GetComponent<Image>().enabled = false;
            foreach (Image switchOff in Arrows)
                switchOff.enabled = false;
            Tab.SetActive(true);
        }
        if (!TabClick && Input.GetKeyDown(KeyCode.Tab))
        {
            TabClick = true;
            Tab.SetActive(false);
        }

        if (JournalChecked && JournalObject.layer == 11)
        {
            JournalObject.layer = 0;
        }
    }
    public void ChangeTutorial(int whichOne)
    {
        switch (whichOne)
        {
            case 0:
        this.gameObject.SetActive(false);
                break;
            case 1:
                foreach (People Change in AllPeople)
                {
                    Change.gameObject.layer = 0;
                    foreach (Transform children in Change.transform)
                        children.gameObject.layer = 0;
                }
                break;
            case 2:
                //Info.text;
                    break;
        }

    }

}
