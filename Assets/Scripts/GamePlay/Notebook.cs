using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notebook : MonoBehaviour {

    public GameMananger GM;
    public GameObject Image;
    public Dialogue_Storage DiologueOptions;
    public int Page;
    public int MaxPage;
    public string FullText;
    public TextGenerator FullTextGen;


    People[] Person;
    int numofpep;

    public List<string>[] people_Diologue;
    

    Rooms[] Room;
    int numofrooms;
    public bool[] Rooms_checked;
    public bool[] Rooms_Murdered;

    Items[] Item;
    public Rooms[] Item_home;
    public bool[] Item_Murdered;
    int numofItems;
    
     public List<string> otherFacts;

    bool Displayed;

    static public string CiphersStored="";

    public void Start()
    {
        Displayed = false;
        Image.SetActive(false);
        this.GetComponent<Text>().text = "";
        Page = 1;
        otherFacts = new List<string>();
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Displayed = !Displayed;
            if (Displayed)
            {

                GM.ToggleCursor(true);
                Page = 1;
                Image.SetActive(true);
                FullText= check();
                this.GetComponent<Text>().text = FullText;
                Text myText = GetComponent<Text>();
                myText.verticalOverflow = VerticalWrapMode.Overflow;
                Canvas.ForceUpdateCanvases();

                FullTextGen = myText.cachedTextGenerator;

                MaxPage = (int)Mathf.Ceil(myText.cachedTextGenerator.lines.Count/29f);


                myText.verticalOverflow = VerticalWrapMode.Truncate;


            }
            else
            {

                GM.ToggleCursor(false);
                Image.SetActive(false);
                this.GetComponent<Text>().text = "";
            }
        }
    }


    public void TurnPage(bool Incress)
    {
        this.GetComponent<Text>().text = FullText;
        Text myText = GetComponent<Text>();
        myText.verticalOverflow = VerticalWrapMode.Overflow;

        Canvas.ForceUpdateCanvases();

        FullTextGen = myText.cachedTextGenerator;
        if (Incress)
        {
            string holdText = "";
            Page++;

            for (int i = ((Page-1)*29); i < FullTextGen.lines.Count; i++)
            {
                int startIndex = FullTextGen.lines[i].startCharIdx;
                int endIndex = (i == FullTextGen.lines.Count - 1) ? myText.text.Length
                    : FullTextGen.lines[i + 1].startCharIdx;
                int length = endIndex - startIndex;

                holdText += myText.text.Substring(startIndex, length);
            }
            myText.text = holdText;
        }
        else
        {
            string holdText = "";
            Page--;

            for (int i = ((Page - 1) * 29); i <= (Page * 29); i++)
            {
                int startIndex = FullTextGen.lines[i].startCharIdx;
                int endIndex = (i == FullTextGen.lines.Count - 1) ? myText.text.Length
                    : FullTextGen.lines[i + 1].startCharIdx;
                int length = endIndex - startIndex;

                holdText += myText.text.Substring(startIndex, length);
            }
            myText.text = holdText;
        }

        myText.verticalOverflow = VerticalWrapMode.Truncate;
    }


    public void InitializeNotebook(People[] Personin, Rooms[] Roomin)
    {
        Person = Personin;
        Room = Roomin;
        

        numofpep = Person.Length;

        DiologueOptions.CheckMurdersForFilter(GM.Murder);

        people_Diologue = new List<string>[numofpep];

        for (int i = 0; i< numofpep;i++)
        {
            people_Diologue[i] = new List<string>();
        }

        numofrooms = Room.Length;
        Rooms_checked = new bool[numofrooms];
        Rooms_Murdered= new bool[numofrooms];

        for (int i = 0; i < numofrooms; i++)
        {
            Rooms_checked[i] = false;
            Rooms_Murdered[i] = false;
        }
        

        numofItems = 0;

    }

    public void addItem(Items Itemin)
    {
        for (int j = 0; j < numofItems; j++)
            if (Item[j] == Itemin)
            {
                Item_home[j] = Itemin.Home;
                Item_Murdered[j] = Itemin.used;
                return;
            }

        Items[] hold;

        hold = Item;
        Item = new Items[numofItems + 1];
        for (int j = 0; j < numofItems; j++)
            Item[j] = hold[j];
        Item[numofItems] = Itemin;


        Rooms[] Item_home_hold;

        Item_home_hold = Item_home;
        Item_home = new Rooms[numofItems + 1];
        for (int j = 0; j < numofItems; j++)
            Item_home[j] = Item_home_hold[j];
        Item_home[numofItems] = Itemin.Home;
        


        bool[] Item_Murdered_hold;

        Item_Murdered_hold = Item_Murdered;
        Item_Murdered = new bool[numofItems + 1];
        for (int j = 0; j < numofItems; j++)
            Item_Murdered[j] = Item_Murdered_hold[j];
        Item_Murdered[numofItems] = Itemin.used;

        numofItems++;

        GM.AddClue(Itemin.name);
    }

    public List<Items> SeenItems()
    {
        List<Items> OutputList= new List<Items>();
        for (int j = 0; j < numofItems; j++)
            OutputList.Add(Item[j]);
        return OutputList;
    }

    public void ClueFlip(People PersonToFind, string Diologue)
    {
        for (int i = 0; i < numofpep; i++)
        {
            if (PersonToFind == Person[i])
            {
                people_Diologue[i].Add(Diologue);

                GM.AddClue(PersonToFind.name + Diologue);
            }
        }
    }

    public string check()
    {
        string NotebooksOutput = "";

        NotebooksOutput += "              People: \n";

        for (int i = 0; i < numofpep; i++)
        {
            if (people_Diologue[i].Count != 0)
            {

                NotebooksOutput += "\n" + Person[i].name + ":\n";

                foreach (string Output in people_Diologue[i])
                {
                    NotebooksOutput += Output + "\n";
                }

                /*
            if (people_checked[i, 0] == true)
                NotebooksOutput += "  They claimed the following when you asked: " + Person[i].Diologue[1] + "\n";


            if (people_checked[i, 1] == true)
               NotebooksOutput += "  When you looked at thier body, you saw the following: " +Person[i].description + "\n";
           */
            }
        }


        NotebooksOutput += "\n              Rooms: \n";

        for (int i = 0; i < numofrooms; i++)
        {
            if (Rooms_checked[i] == true)
            {
            NotebooksOutput += "\n" + Room[i].type.ToString() + ":\n";
            
                NotebooksOutput += "  " + Room[i].description;

                if (Rooms_Murdered[i] == true)
                    NotebooksOutput += "\n  There was a dead body in this room.";
            }
        }

        NotebooksOutput += "\n              Objects: \n";

        for (int i = 0; i < numofItems; i++)
        {
            NotebooksOutput += "\n" + Item[i].name + ":\n  ";
            NotebooksOutput += Item[i].description+". ";

            if (Item_Murdered[i])
                NotebooksOutput +=  Item[i].UsedDescription;

            if (Item_home[i] != null && !Item[i].held)
            {
                NotebooksOutput += "\n  This has been moved from its original position in the ";
                /*NotebooksOutput += Item[i].home.Name.ToString();
                switch (Item[i].home)
                {
                    case Name.Deck:
                        NotebooksOutput += " Room";
                        break;
                    case Name.Bilge:
                        NotebooksOutput += " Hallway";
                        break;
                }*/
            }
            NotebooksOutput += "\n";


        }


        NotebooksOutput += "\n              General Facts: \n";

        for (int j = 0; j < otherFacts.Count; j++)
                NotebooksOutput += "\n\n  " + otherFacts[j];
        //if(General_found[8])


       
        return NotebooksOutput;

    }

}

