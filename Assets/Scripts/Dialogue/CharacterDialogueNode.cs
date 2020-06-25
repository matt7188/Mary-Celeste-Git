using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDialogueNode", menuName = "DialogueSystem/CharacterDialogueNode", order = 5)]
public class CharacterDialogueNode : DialogueNode
{
    public DialogueNode DismissalNode;

    public void UpdateCharacterDialogueOptions(People TargetPerson)
    {
        List<CharacterOpinion> characterOpinions = TargetPerson.gameObject.GetComponent<DialogueHandler>().CharacterOpinions;

        optionsText.Clear();
        targetDialogueNodes.Clear();

        for(int i = 0; i < characterOpinions.Count; i++)
        {
            optionsText.Add(characterOpinions[i].Person.MyName);
            targetDialogueNodes.Add(characterOpinions[i].TargetDialogueNode);
        }

        if(characterOpinions.Count == 0)
        {
            optionsText.Add("Character Opinion Error");
            targetDialogueNodes.Add(DismissalNode);
        }
    }
}
