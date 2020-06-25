using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "DialogueSystem/Node", order = 2)]
public class DialogueNode : ScriptableObject
{
    public enum Relation { GreaterThan, LessThan, EqualTo }

    public enum Trait { N_A, Trust, Stress }

    public string introText;
    public List<string> optionsText;
    public List<DialogueNode> targetDialogueNodes;
    public bool isEndDialogue;

    public bool isTimeQuery;
    public bool isInventoryQuery;
    public bool isCharacterDialogueQuery;
    public bool isOpinionQuery;
    public bool modifiesQueryLimit;

    public Trait trait;
    public Relation relation;
    public int traitIndex;

    //Modify Stats:
    public People characterToModify;
    public Trait traitToModify;
    public int modificationAmount;
}