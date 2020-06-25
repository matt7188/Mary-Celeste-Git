using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Murder Progression", menuName = "Murder Progression", order = 1)]
public class MurderProgression : ScriptableObject
{

    public int NumberOfSteps;

    public Step [] Steps;

    [System.Serializable]
    public class Step { 
    public People PersonToExecute;
    public Command NewCommand;
    public bool TimeOrClues;
    public int TimeNeeded;
    public int CluesNeeded;
    public bool LinkToNextStep;
    }

}
 
 [CustomEditor(typeof(MurderProgression))]
public class MurderProgressionEditor : Editor
{
    private Editor cachedEditor;
    
    //private bool cachedEditorNeedsRefresh = true;

    MurderProgression Prog;

        bool Pickup = true;

    public void OnEnable()
    {

        Prog = (MurderProgression)target;
        if (Prog == null)
            Prog = new MurderProgression();
    }
    

    
    public override void OnInspectorGUI()
    {

        Prog= (MurderProgression)target;

        Prog.NumberOfSteps = EditorGUILayout.IntField("How meny steps", Prog.NumberOfSteps);

        if (GUILayout.Button("Add Step"))
        {
            Prog.NumberOfSteps++;
        }
        if (GUILayout.Button("Remove Step"))
        {
            Prog.NumberOfSteps--;
        }

        if (Prog.Steps== null )
        {
            
            Prog.Steps = new MurderProgression.Step[Prog.NumberOfSteps];
        }

        if (Prog.Steps.Length < Prog.NumberOfSteps) {

            MurderProgression.Step [] hold = new MurderProgression.Step[Prog.NumberOfSteps];

            for (int i=0;i< Prog.Steps.Length; i++)
            {
                hold[i] = Prog.Steps[i];
            }
            Prog.Steps = hold;
        }


        for (int i = 0; i < Prog.NumberOfSteps; i++)
        {

            EditorGUILayout.LabelField("Command " + (i+1).ToString());

            Prog.Steps[i].PersonToExecute = EditorGUILayout.ObjectField("Who Is Doing It:", Prog.Steps[i].PersonToExecute, typeof(People), true) as People;

            if (Prog.Steps[i].NewCommand==null)
            {
                Prog.Steps[i].NewCommand = new Command();
            }

            Prog.Steps[i].NewCommand.CommandType = (Command.DestinationType)EditorGUILayout.EnumPopup("What Kind of Command:", Prog.Steps[i].NewCommand.CommandType);

            switch (Prog.Steps[i].NewCommand.CommandType) {
                case Command.DestinationType.Items:
                    Pickup = EditorGUILayout.Toggle("Pickup(T) or drop off (F):", Pickup);
                    if (Pickup)
                    {
                        Prog.Steps[i].NewCommand.ItemPickupOrDrop = EditorGUILayout.ObjectField("what Item to Find:", Prog.Steps[i].NewCommand.ItemPickupOrDrop, typeof(Items), true) as Items;
                        Prog.Steps[i].NewCommand.DropingItem = RoomName.Person;
                    }
                    else
                    {
                        Prog.Steps[i].NewCommand.ItemPickupOrDrop = EditorGUILayout.ObjectField("what Item to Drop:", Prog.Steps[i].NewCommand.ItemPickupOrDrop, typeof(Items), true) as Items;
                        Prog.Steps[i].NewCommand.DropingItem = (RoomName)EditorGUILayout.EnumPopup("Drop off where:", Prog.Steps[i].NewCommand.GoingTo);
                    }
                    break;
                case Command.DestinationType.Person:
                    Prog.Steps[i].NewCommand.Persuing = EditorGUILayout.ObjectField("Who are they finding:", Prog.Steps[i].NewCommand.Persuing, typeof(People), true) as People;
                    break;
                case Command.DestinationType.Room:
                    Prog.Steps[i].NewCommand.GoingTo = (RoomName)EditorGUILayout.EnumPopup("Where are they going:", Prog.Steps[i].NewCommand.GoingTo);
                    break;
            }

            if (i == 0 || !Prog.Steps[i - 1].LinkToNextStep)
            {

                Prog.Steps[i].TimeOrClues = EditorGUILayout.Toggle("Will this be based on Time (T) or Clues (F):", Prog.Steps[i].TimeOrClues);
                if (Prog.Steps[i].TimeOrClues)
                {
                    Prog.Steps[i].TimeNeeded = EditorGUILayout.IntField("What time should it be in minuits (0=9:00):", Prog.Steps[i].TimeNeeded);
                }
                else
                    Prog.Steps[i].CluesNeeded = EditorGUILayout.IntField("How meny clues should be found:", Prog.Steps[i].CluesNeeded);
            }
            Prog.Steps[i].LinkToNextStep =   EditorGUILayout.Toggle("Connect to next step?", Prog.Steps[i].LinkToNextStep);

        }

        EditorUtility.SetDirty(Prog);
        
        //AssetDatabase.Refresh();

    }

}
