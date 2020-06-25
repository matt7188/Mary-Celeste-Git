using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class ItemOpinion
//{
//    public Items Item;
//    public DialogueNode TargetDialogueNode;
//}

public class ItemDialogueHandler : MonoBehaviour
{
    public List<ItemOpinion> ItemOpinions;

    public void UpdateItemOpinion(Items item, DialogueNode newTargetDialogueNode)
    {
        for(int i = 0; i < ItemOpinions.Count; i++)
        {
            if(ItemOpinions[i].Item == item)
            {
                ItemOpinions[i].TargetDialogueNode = newTargetDialogueNode;
                break;
            }
        }
    }
}
