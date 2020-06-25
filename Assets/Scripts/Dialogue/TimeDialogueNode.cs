using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeDialogueNode", menuName = "DialogueSystem/TimeNode", order = 3)]
public class TimeDialogueNode : DialogueNode
{
    public string LocationPrompt;
    public void UpdateTimeNodeStats(RoomName targetLocation)
    {
        //switch(input)
        //{
        //    case 0:
        //        TargetLocation = character.WhereIHaveBeen[character.WhereIHaveBeen.Count-11];
        //        break;
        //    case 1:
        //        TargetLocation = character.WhereIHaveBeen[character.WhereIHaveBeen.Count - 61];
        //        break;
        //    case 2:
        //        if (character.SignificantLocations.Count > 1)
        //        {
        //            TargetLocation = character.SignificantLocations[1];
        //        }
        //        break;
        //    case 3:
        //        TargetLocation = character.SignificantLocations[0];
        //        break;
        //}

        if (targetLocation != RoomName.Person)
        {
            introText = LocationPrompt + targetLocation;
        }
        else
        {
            introText = "It hasn't been that long since we turned invisible.";
        }
    }
}