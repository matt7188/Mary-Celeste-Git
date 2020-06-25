using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Opinions : MonoBehaviour
{
    public People [] Persons;
    
    public int [] Thoughts;

    public int MyThoughtsOn(People Person)
    {
        for(int i= 0; i< Persons.Length;i++)
            if (Persons[i] == Person)
            {
                return Thoughts[i];
            }

        return -1;
    }

    public int MyThoughtsOnMe()
    {
        return Thoughts[Thoughts.Length - 1];
    }

}
/*
#if UNITY_EDITOR
// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(Opinions), true)]
[CanEditMultipleObjects]
public class Opinions_Editor : Editor
{
    private Editor cachedEditor;

    private bool cachedEditorNeedsRefresh = true;

    Opinions opinion;

    bool Pickup = true;

    public void OnEnable()
    {

        opinion = (Opinions)target;
        if (opinion == null)
            opinion = new Opinions();
    }



    public override void OnInspectorGUI()
    {

        opinion = (Opinions)target;



        opinion.Persons = EditorGUILayout.(opinion.Persons) ;
        
        EditorUtility.SetDirty(opinion);

        //AssetDatabase.Refresh();

    }

}


#endif*/