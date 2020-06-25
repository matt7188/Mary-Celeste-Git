using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem2 : MonoBehaviour
{
    public DialogueNode currentDialogueNode;

    public GameObject DialogueCanvas;
    public Text IntroText;
    public Text[] OptionsText;
    public GameObject[] OptionsButtons;
    public GameObject PageChangeButton;

    public Text CharacterName;
    public Text CharacterTrust;
    public Text CharacterFear;

    //public DialogueNode MartensStartingNode;
    //public DialogueNode AlbertStartingNode;
    //public DialogueNode BenjaminStartingNode;
    //public DialogueNode GillingStartingNode;
    //public DialogueNode EdwardStartingNode;

    //public Character TempMartens;

    public GameObject TestCanvas;

    public People TargetCharacter;

    public GameMananger GM;

    public Notebook NotebookRef;

    public DialogueNode HubNode;

    private bool FirstPage = true;

    public List<int> ValidOptionButtons = new List<int>();

    public void PageToggle()
    {
        foreach (var button in OptionsButtons)
        {
            button.SetActive(false);
        }

        if (FirstPage == true)
        {
            if (ValidOptionButtons.Count > 5)
            {
                for (int i = 5; i < ValidOptionButtons.Count; i++)
                {
                    OptionsButtons[i].SetActive(true);
                }
                FirstPage = false;
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                OptionsButtons[i].SetActive(true);
            }

            FirstPage = true;
        }
    }

    public void OpenDialogueSystem(DialogueNode node)
    {
        currentDialogueNode = node;
        DialogueCanvas.SetActive(true);
        //if (TestCanvas != null)
        //{
        //    TestCanvas.SetActive(false);
        //}
    }

    public void CloseDialogueSystem()
    {
        TargetCharacter.ResetQueryLimit();
        currentDialogueNode = null;
        DialogueCanvas.SetActive(false);
        //if (TestCanvas != null)
        //{
        //    TestCanvas.SetActive(true);
        //}
    }

    public void UpdateDialogueCanvas(People target)
    {
        ValidOptionButtons.Clear();

        
        if (TargetCharacter==null)
            TargetCharacter = target;

        if (currentDialogueNode.modifiesQueryLimit && TargetCharacter.CurrentQueryLimit >= 0)
        {
            TargetCharacter.CurrentQueryLimit--;
        }

        IntroText.text = currentDialogueNode.introText;

        if (currentDialogueNode.targetDialogueNodes.Count != currentDialogueNode.optionsText.Count && !currentDialogueNode.isEndDialogue)
        {
            throw new System.Exception("Node " + currentDialogueNode + " has a different number of options text and target nodes.");
        }

        IntroText.text = currentDialogueNode.introText;

        CharacterName.text = TargetCharacter.MyName;

        //TODO: Remove after testing
        CharacterTrust.text = "Trust: " + TargetCharacter.Trust.ToString();
        CharacterFear.text = "Fear: " + TargetCharacter.Stress.ToString();

        foreach (var button in OptionsButtons)
        {
            button.SetActive(false);
        }

        if (!currentDialogueNode.isEndDialogue)
        {
            if (currentDialogueNode.trait == DialogueNode.Trait.N_A)
            {
                for (int i = 0; i < currentDialogueNode.optionsText.Count; i++)
                {
                    OptionsText[i].text = currentDialogueNode.optionsText[i];
                    ValidOptionButtons.Add(i);
                    //OptionsButtons[i].SetActive(true);
                }
            }
            else
            {
                for (int i = 0; i < currentDialogueNode.optionsText.Count - 1; i++)
                {
                    OptionsText[i].text = currentDialogueNode.optionsText[i];
                    ValidOptionButtons.Add(i);
                    //OptionsButtons[i].SetActive(true);
                }

                if (currentDialogueNode.trait == DialogueNode.Trait.Trust)
                {
                    switch (currentDialogueNode.relation)
                    {
                        case DialogueNode.Relation.GreaterThan:
                            if (TargetCharacter.Trust > currentDialogueNode.traitIndex)
                            {
                                int index = currentDialogueNode.optionsText.Count - 1;
                                ValidOptionButtons.Add(index);
                                //OptionsButtons[index].SetActive(true);
                                OptionsText[index].text = currentDialogueNode.optionsText[index];
                            }
                            break;
                        case DialogueNode.Relation.LessThan:
                            if (TargetCharacter.Trust < currentDialogueNode.traitIndex)
                            {
                                int index = currentDialogueNode.optionsText.Count - 1;
                                ValidOptionButtons.Add(index);
                                //OptionsButtons[index].SetActive(true);
                                OptionsText[index].text = currentDialogueNode.optionsText[index];
                            }
                            break;
                        case DialogueNode.Relation.EqualTo:
                            if (TargetCharacter.Trust == currentDialogueNode.traitIndex)
                            {
                                int index = currentDialogueNode.optionsText.Count - 1;
                                ValidOptionButtons.Add(index);
                                //OptionsButtons[index].SetActive(true);
                                OptionsText[index].text = currentDialogueNode.optionsText[index];
                            }
                            break;
                    }
                }
                else if (currentDialogueNode.trait == DialogueNode.Trait.Stress)
                {
                    switch (currentDialogueNode.relation)
                    {
                        case DialogueNode.Relation.GreaterThan:
                            if (TargetCharacter.Stress > currentDialogueNode.traitIndex)
                            {
                                int index = currentDialogueNode.optionsText.Count - 1;
                                ValidOptionButtons.Add(index);
                                //OptionsButtons[index].SetActive(true);
                                OptionsText[index].text = currentDialogueNode.optionsText[index];
                            }
                            break;
                        case DialogueNode.Relation.LessThan:
                            if (TargetCharacter.Stress < currentDialogueNode.traitIndex)
                            {
                                int index = currentDialogueNode.optionsText.Count - 1;
                                ValidOptionButtons.Add(index);
                                //OptionsButtons[index].SetActive(true);
                                OptionsText[index].text = currentDialogueNode.optionsText[index];
                            }
                            break;
                        case DialogueNode.Relation.EqualTo:
                            if (TargetCharacter.Stress == currentDialogueNode.traitIndex)
                            {
                                int index = currentDialogueNode.optionsText.Count - 1;
                                ValidOptionButtons.Add(index);
                                //OptionsButtons[index].SetActive(true);
                                OptionsText[index].text = currentDialogueNode.optionsText[index];
                            }
                            break;
                    }
                }
            }

            for(int i = 0; i < ValidOptionButtons.Count && i < 5; i++)
            {
                OptionsButtons[i].SetActive(true);
            }

            if(ValidOptionButtons.Count > 5)
            {
                FirstPage = true;
                PageChangeButton.SetActive(true);
            }
        }
        else
        {
            OptionsText[0].text = currentDialogueNode.optionsText[0];
            OptionsButtons[0].SetActive(true);
        }
    }

    public void SelectOption(int input)
    {
        PageChangeButton.SetActive(false);

        if (TargetCharacter.CurrentQueryLimit < 0)
        {
            currentDialogueNode = TargetCharacter.DismissalNode;
            UpdateDialogueCanvas(TargetCharacter);
            TargetCharacter.ResetQueryLimit();
        }

        else
        {
            //Default Node Structure
            if (!currentDialogueNode.isEndDialogue && !currentDialogueNode.isTimeQuery)
            {
                Debug.Log("Selected " + input);
                currentDialogueNode = currentDialogueNode.targetDialogueNodes[input];

                //Inventory Query
                if (currentDialogueNode.isInventoryQuery)
                {
                    InventoryDialogueNode castedCurrentNode = currentDialogueNode as InventoryDialogueNode;
                    castedCurrentNode.UpdateInventoryOptions(NotebookRef, TargetCharacter);
                }

                //Character Opinion Query
                if (currentDialogueNode.isCharacterDialogueQuery)
                {
                    CharacterDialogueNode castedCurrentNode = currentDialogueNode as CharacterDialogueNode;
                    castedCurrentNode.UpdateCharacterDialogueOptions(TargetCharacter);
                }

                //Murderer Opinion Query
                if (currentDialogueNode.isOpinionQuery)
                {
                    OpinionNode castedCurrentNode = currentDialogueNode as OpinionNode;
                    castedCurrentNode.UpdateOpinionOptions(TargetCharacter);
                }

                if(currentDialogueNode == TargetCharacter.DismissalNode && TargetCharacter.CurrentQueryLimit >= 0)
                {
                    currentDialogueNode = HubNode;
                }

                UpdateDialogueCanvas(TargetCharacter);

                if (currentDialogueNode.traitToModify == DialogueNode.Trait.Trust)
                {
                    currentDialogueNode.characterToModify.Trust = currentDialogueNode.characterToModify.Trust + currentDialogueNode.modificationAmount;
                }

                if (currentDialogueNode.traitToModify == DialogueNode.Trait.Stress)
                {
                    currentDialogueNode.characterToModify.Stress = currentDialogueNode.characterToModify.Stress + currentDialogueNode.modificationAmount;
                }
            }

            //Time Query
            else if (!currentDialogueNode.isEndDialogue && currentDialogueNode.isTimeQuery)
            {
                Debug.Log("Selected " + input);
                currentDialogueNode = currentDialogueNode.targetDialogueNodes[input];

                if (currentDialogueNode is TimeDialogueNode)
                {
                    TimeDialogueNode castedCurrentNode = currentDialogueNode as TimeDialogueNode;

                    RoomName targetLocation = RoomName.Person;

                    switch (input)
                    {
                        case 0:
                            if (TargetCharacter.WhereIHaveBeen.Count >= 11)
                            {
                                targetLocation = TargetCharacter.WhereIHaveBeen[TargetCharacter.WhereIHaveBeen.Count - 11];
                            }
                            else
                            {
                                targetLocation = RoomName.Person;
                            }
                            break;
                        case 1:
                            if (TargetCharacter.WhereIHaveBeen.Count >= 61)
                            {
                                targetLocation = TargetCharacter.WhereIHaveBeen[TargetCharacter.WhereIHaveBeen.Count - 61];
                            }
                            else
                            {
                                targetLocation = RoomName.Person;
                            }
                            break;
                        case 2:
                            if (TargetCharacter.SignificantLocations.Count > 1)
                            {
                                targetLocation = TargetCharacter.SignificantLocations[1];
                            }
                            break;
                        case 3:
                            targetLocation = TargetCharacter.SignificantLocations[0];
                            break;
                    }

                    castedCurrentNode.UpdateTimeNodeStats(targetLocation);
                }
                else
                {
                    throw new System.Exception("Selected node isn't a Time Dialogue Node");
                }

                UpdateDialogueCanvas(TargetCharacter);
            }

            else if (currentDialogueNode.isEndDialogue)
            {
                GM.ToggleCursor(false);
                CloseDialogueSystem();
            }
        }
    }
}
