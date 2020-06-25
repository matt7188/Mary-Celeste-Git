using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Diologue_Box : MonoBehaviour {

    public Text MyText;
    public Text MyText2;

    public GameObject PersonTalking;
    public BurningImage FindImage;

    public Font murder;
    bool AloneWithMurder;

    public void ItemDescription(Items LookingAt)
    {
        MyText.text = LookingAt.name+": "+LookingAt.description+". ";
        if (LookingAt.used)
            MyText.text += LookingAt.UsedDescription;
    }

        public void WriteWords()
    {
        AloneWithMurder = false;
        People PersonRefrence = PersonTalking.GetComponent<People>();

        MyText.font = PersonRefrence.myHandwriting;
        FindImage.SetUpImage();
        FindImage.BurnPicture(PersonRefrence.MyPicture, true);

        if (PersonRefrence.murder && FindObjectOfType<GameMananger>().HowMenyInRoom(PersonRefrence.Here) == 2)
        {
            MyText.font = murder;
            MyText.text = "I WILL KILL YOU!";
        }
        else
        {

            if (PersonRefrence.alive)
            {
                Debug.Log("NEED TO ADD CLUE!!!");
                //FindObjectOfType<Notebook>().ClueFlip(PersonRefrence, 0);
                MyText.text = PersonRefrence.Diologue[0];
            }
            else
            {
                Debug.Log("NEED TO ADD CLUE!!!");
                //FindObjectOfType<Notebook>().ClueFlip(PersonRefrence, 1);
                FindObjectOfType<GameMananger>().InfoOutput.text = PersonRefrence.description;
            }
        }

    }

    public void DisplayMurder()
    {

        // MyText.text = "The murder is: " +
        //FindObjectOfType<GameMananger>().Murder.MyName;
        /*
        Tutorial hold = FindObjectOfType<Tutorial>();

        if (hold == null || hold.JournalChecked)
        {

            MyText.text = " * so young to be promoted so high. Quit the protege, but how would he react if I made him question his abilities* \n" +
                "* weak of the mind, consumed by fear. Will be an easy target*\n" +
                "* idolizes his older brother, even this late in their life. The threat of being alone in this world maybe too much for him to bare*\n" +
                "* A man of honor, a man of dignity, and a man of classes. Would you give it ";
            MyText2.text = "all up to save his daughter?*\n*old and sturdy, has years of experience. Perhaps he has had enough time here*\n" +
        "*A small child, so easy to convince to do just about anything*\n" +
            "*Annoying little brother, I desired to be alone, promises to parents long dead*\n" +
            "* His mind is the strongest, there is no question about that. His will require a match subtler play*";
        }
        else
        {
            hold.JournalChecked = true;
            MyText.text = "I know this is a lot to take in, but just take this one step at a time. The people have slates for you to read in order to communicate with them. " +
                "For now, you can use it to learn of where they were at the time of the disappearance. At least one of them would have had to have been alone at the time. " +
                "There are also several items and clues to look at that can help you figure out who is possessed. My friend has a habit of keeping notes in ciphers, which may need " +
                "solving, but they can give you ";
            MyText2.text = "some good information. If, at any point, you need help, the ships log on the desk will tell you everyone on the ship. Otherwise, you can come back to read me. I can give you advice, but it will cost a lot of time. Remember, as time goes on, the crew will get more and more nervous, and may become aggressive, even violent. Time is a precious friend.\nGood Luck Sarah";

        }*/

    }

    public void DisplayPeoplesThoughts()
    {
        if (AloneWithMurder)
        {
            MyText.text = "You would ask me to reveal my secrets? The clues that would lead to my failure? You are more naive than I thought!";

        }
        else
        {
            People PersonRefrence = PersonTalking.GetComponent<People>();
            MyText.text = PersonRefrence.Diologue[PersonRefrence.Stress + 2];
            FindObjectOfType<GameMananger>().TimePass(5);
        }



    }


    public void AlibiOutput()
    {
        if (AloneWithMurder)
        {
            MyText.text = "You would ask me to reveal my secrets? The clues that would lead to my failure? You are more naive than I thought!";

        }
        else
        {
            MyText.text = PersonTalking.GetComponent<People>().Diologue[1];
            FindObjectOfType<GameMananger>().TimePass(5);
        }
    }


    }
