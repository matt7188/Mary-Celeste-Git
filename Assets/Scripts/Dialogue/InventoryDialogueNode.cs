using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDialogueNode", menuName = "DialogueSystem/ItemNode", order = 4)]
public class InventoryDialogueNode : DialogueNode
{
    public DialogueNode DismissalNode;
    public void UpdateInventoryOptions(Notebook notebook, People TargetPerson)
    {
        List<ItemOpinion> itemOpinions = TargetPerson.gameObject.GetComponent<DialogueHandler>().ItemOpinions;

        optionsText.Clear();
        targetDialogueNodes.Clear();

        foreach (var item in notebook.SeenItems())
        {
            optionsText.Add(item.name);
            
            for(int i = 0; i < itemOpinions.Count; i++)
            {
                if(itemOpinions[i].Item == item)
                {
                    targetDialogueNodes.Add(itemOpinions[i].TargetDialogueNode);
                }
            }
        }

        if(notebook.SeenItems().Count == 0)
        {
            optionsText.Add("No Items have been seen till now.");
            targetDialogueNodes.Add(DismissalNode);
        }
    }
}
