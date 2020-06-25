using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book_Flip : MonoBehaviour {

    //CAN YOU SEE THIS?

    public enum Whichbook {Journal, Captians, Cipher, Diary};

    public GameMananger GM;
    public PageCurl pageFlip;
    public GameObject leftPage;
    public TextFx.TextFxTextMeshPro leftPageText;
    public GameObject rightPage;
    public TextFx.TextFxTextMeshPro rightPageText;

    public GameObject BookCam;
    public GameObject ProperCam;

   
    Texture[] Pages;
    Material[] PagesMat;
    public Texture JournalPages;
    public Material JournalPagesMat;

    Whichbook ThisIs;
    bool IsThisJournal;

    bool lastFlip;
    int CurrentPage;

    public GameObject[] CaptianBack;
    public GameObject[] JournalBack;

    string LeftOutput;
    string RightOutput;
    


    public void setSelf(Book SendbookInfo)
    {

        Pages= SendbookInfo.Pages;
        PagesMat=SendbookInfo.PagesMat;

        ThisIs = SendbookInfo.IAmThe;

        IsThisJournal = (ThisIs == Whichbook.Journal);

        CurrentPage = 0;
        BookCam.SetActive(true);
        ProperCam.SetActive(false);

        leftPageText.fontSize = 15f;
        rightPageText.fontSize = 14f;


        if (SendbookInfo.AlternatePage != null)
        {
            Pages[SendbookInfo.number] = SendbookInfo.AlternatePage;
            PagesMat[SendbookInfo.number] = SendbookInfo.AlternatePageMat;
        }


        pageFlip.Reset();

        foreach (GameObject flip in CaptianBack)
            flip.SetActive(!IsThisJournal);
        foreach (GameObject flip in JournalBack)
            flip.SetActive(IsThisJournal);

        if (IsThisJournal)
        {
            LeftOutput = "Hello Sarah\n\n\nI can help you, at the cost of time___";
            RightOutput = "I need a help\n\nShow me where everyone is\n\nSolve the Ciphers____";
            leftPage.GetComponent<Renderer>().material = JournalPagesMat;
            pageFlip.FrontTexture = JournalPages;
            pageFlip.BackTexture = JournalPages;
            rightPage.GetComponent<Renderer>().material = JournalPagesMat;
            rightPageText.AnimationManager.PlayAnimation(1f, 0);
            leftPageText.AnimationManager.PlayAnimation(1f, 0);
            lastFlip = false;

            leftPageText.text = LeftOutput;
            rightPageText.text = RightOutput;
        }
        else
        {
            leftPage.GetComponent<Renderer>().material = PagesMat[0];
            pageFlip.FrontTexture = Pages[1];
            pageFlip.BackTexture = Pages[2];
            rightPage.GetComponent<Renderer>().material = PagesMat[3];
            lastFlip = false;
            
            

}

    }
    public void TurnPage(bool Forward)
    {

        if (IsThisJournal)
        {
            if (lastFlip)
            {

                rightPageText.AnimationManager.ContinuePastBreak();
                leftPageText.AnimationManager.ContinuePastBreak();

                StartCoroutine(FlipJournal(Forward));

                LeftOutput = "Hello Sarah\n\n\nI can help you, at the cost of time___";
                RightOutput = "I need a help\n\nShow me where everyone is\n\nSolve the Ciphers____";
            }

        }

        else
        {
            if (Forward == lastFlip)
            {
                if (Forward)
                {

                    if (CurrentPage + 4 < PagesMat.Length)
                        CurrentPage += 2;
                    else
                        return;
                }
                else
                {
                    if (CurrentPage - 2 >= 0)
                        CurrentPage -= 2;
                    else
                        return;
                }

                leftPage.GetComponent<Renderer>().material = PagesMat[CurrentPage];

                pageFlip.FrontTexture = Pages[CurrentPage + 1];
                pageFlip.BackTexture = Pages[CurrentPage + 2];

                rightPage.GetComponent<Renderer>().material = PagesMat[CurrentPage + 3];
            }
            pageFlip.Flip(!Forward);

            lastFlip = Forward;
        }
    }

    public void TurnPage(JournalClick.WhatToSay GoToWhere)
    {
        if (!lastFlip)
        {
            rightPageText.AnimationManager.ContinuePastBreak();
            leftPageText.AnimationManager.ContinuePastBreak();

            StartCoroutine(FlipJournal(true));

            switch (GoToWhere)
            {
                case JournalClick.WhatToSay.Help:
                    LeftOutput = "I know this is a lot to take in, but just take this one step at a time. " +
                        "The people have slates for you to read in order to communicate with them. " +
                        "For now, you can use it to learn of where they were at the time of the disappearance. " +
                        "At least one of them would have had to have been alone at the time. " +
                    "There are also several items and clues to look at that can help you figure" +
                    " out who is possessed. My friend has a habit of keeping notes in ciphers, ";
                    RightOutput = "which may need solving, but they can give you some good information. If, at any point, " +
                    "you need help, the ships log on the desk will tell you everyone on the ship. Otherwise, " +
                    "you can come back to read me. I can give you advice, but it will cost a lot of time. " +
                    "Remember, as time goes on, the crew will get more and more nervous, and may become " +
                    "aggressive, even violent. Time is a precious friend. Good Luck Sarah";

                    break;
                case JournalClick.WhatToSay.Where:
                    LeftOutput = "They are here";
                    RightOutput = "They are here";
                    break;
                case JournalClick.WhatToSay.Cipher:
                    LeftOutput = " *So young to be promoted so high. Quite the protege, but how would he react if I made him question his abilities* \n" +
                    "* weak of the mind, consumed by fear. Will be an easy target*\n" +
                    "* idolizes his older brother, even this late in their life. The threat of being alone in this world maybe too much for him to bare*\n" +
                    "* A man of honor, a man of dignity, and a man of classes. Would he     tou";
                    RightOutput = "give it all up to save his daughter?*\n*old and sturdy, has years of experience. Perhaps he has had enough time here*\n" +
            "*A small child, so easy to convince to do just about anything*\n" +
                "*Annoying little brother, I desired to be alone, promises to parents long dead*\n" +
                "* His mind is the strongest, there is no question about that. His will require a much subtler play     toue*";
                    break;

            }
        }
    }
   
    IEnumerator FlipJournal(bool Forward)
    {
        

        while(rightPageText.AnimationManager.Playing)
            yield return new WaitForSeconds(.1f);

        pageFlip.Flip(!Forward);

        lastFlip = Forward;
        yield return new WaitForSeconds(1f);
        if (Forward)
        {
            leftPageText.fontSize = 9.3f;
            rightPageText.fontSize = 8f;
        }
        else
        {

            leftPageText.fontSize = 15f;
            rightPageText.fontSize = 14f;
        }
        leftPageText.text = LeftOutput;
        rightPageText.text = RightOutput;



        rightPageText.AnimationManager.PlayAnimation(1f, 0);
        leftPageText.AnimationManager.PlayAnimation(1f, 0);

        lastFlip = Forward;
    }


    }
