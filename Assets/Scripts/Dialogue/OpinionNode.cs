using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OpinionNode", menuName = "DialogueSystem/OpinionNode", order = 6)]
public class OpinionNode : DialogueNode
{
    public DialogueNode DismissalNode;

    public void UpdateOpinionOptions(People TargetPerson)
    {
        List<CurrentOpinion> currentOpinions = TargetPerson.gameObject.GetComponent<DialogueHandler>().CurrentOpinions;

        Opinions opinionStats = TargetPerson.gameObject.GetComponent<Opinions>();

        optionsText.Clear();
        targetDialogueNodes.Clear();

        for (int i = 0; i < currentOpinions.Count; i++)
        {
            optionsText.Add(currentOpinions[i].Person.MyName);

            for(int j = 0; j < opinionStats.Persons.Length; j++)
            {
                if(currentOpinions[i].Person == opinionStats.Persons[j])
                {
                    int beliefIndex = opinionStats.Thoughts[j];
                    if(beliefIndex > currentOpinions[i].UpperThreshold)
                    {
                        targetDialogueNodes.Add(currentOpinions[i].TargetDialogueNodes[2]);
                    }
                    else if(beliefIndex < currentOpinions[i].LowerThreshold)
                    {
                        targetDialogueNodes.Add(currentOpinions[i].TargetDialogueNodes[0]);
                    }
                    else
                    {
                        targetDialogueNodes.Add(currentOpinions[i].TargetDialogueNodes[1]);
                    }
                }
            }
        }

        if (currentOpinions.Count == 0)
        {
            optionsText.Add("Character Opinion Error");
            targetDialogueNodes.Add(DismissalNode);
        }
    }
}
