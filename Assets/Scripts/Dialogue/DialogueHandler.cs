using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterOpinion
{
    public People Person;
    public DialogueNode TargetDialogueNode;
}

[System.Serializable]
public class ItemOpinion
{
    public Items Item;
    public DialogueNode TargetDialogueNode;
}

[System.Serializable]
public class CurrentOpinion
{
    public People Person;
    public DialogueNode[] TargetDialogueNodes;
    public int LowerThreshold;
    public int UpperThreshold;
}

public class DialogueHandler : MonoBehaviour
{
    public DialogueSystem2 DialogueSystemReference;
    public bool firstTime = true;
    public DialogueNode firstTimeNode;
    public DialogueNode SubsequentNode;
    public DialogueNode SpecialCaseStart;
    
    //CharacterDialogueHandler
    private People HandledCharacter;

    //Character Opinion Handler
    public List<CharacterOpinion> CharacterOpinions;

    //Item Dialogue Handler
    public List<ItemOpinion> ItemOpinions;

    //Current Opinion Handler
    public List<CurrentOpinion> CurrentOpinions;

    private void Start()
    {
        firstTime = true;
        HandledCharacter = GetComponent<People>();

        foreach (var opinion in CurrentOpinions)
        {
            if (opinion.TargetDialogueNodes.Length != 3)
            {
                throw new System.Exception("Improper People Opinion initialization.");
            }
        }
    }

    public void SelectedCharacter()
    {
        Debug.Log("Spoke to " + HandledCharacter.MyName);

        DialogueSystemReference.HubNode = SubsequentNode;

        if(SpecialCaseStart == null)
        {
            if (firstTime)
            {
                DialogueSystemReference.OpenDialogueSystem(firstTimeNode);
                DialogueSystemReference.UpdateDialogueCanvas(HandledCharacter);
                firstTime = false;
            }
            else
            {
                DialogueSystemReference.OpenDialogueSystem(SubsequentNode);
                DialogueSystemReference.UpdateDialogueCanvas(HandledCharacter);
            }
        }
        else
        {
            DialogueSystemReference.OpenDialogueSystem(SpecialCaseStart);
            DialogueSystemReference.UpdateDialogueCanvas(HandledCharacter);
            SpecialCaseStart = null;
        }
    }

    public void UpdateItemOpinion(Items item, DialogueNode newTargetDialogueNode)
    {
        for (int i = 0; i < ItemOpinions.Count; i++)
        {
            if (ItemOpinions[i].Item == item)
            {
                ItemOpinions[i].TargetDialogueNode = newTargetDialogueNode;
                break;
            }
        }
    }
}
