using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryMassCreator : EditorWindow
{

    bool setup;

    List<GameObject> PersonRefrence;
    GameObject Items;


    private enum WhoIsTalking
    {
        Sophia,Captian_Benjimin, First_Mate_Albert, Second_Mate_Andrew_Gilling,
        Edward_William_Head, Volkert_Lorenzen, Boz_Lorenzen,Arian_Martens,Gottlieb_Goodschaad
    }
    WhoIsTalking Character;

    People CurrentlyFocusing;
    DialogueHandler CurrentlyFocusingDialogue;

    bool Itemquestion;
    List<bool> ApplyToItem;

    List<string> DiologueToCreate;
    int numberOfLines;
    string initialDesignation;

    DialogueNode Dismissal;

    [MenuItem("Window/Story Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(StoryMassCreator));
    }

    void OnGUI()
    {
        setup = EditorGUILayout.Toggle(setup);

        if (PersonRefrence==null|| PersonRefrence.Count!=9)
            {
                PersonRefrence = new List<GameObject>();
                PersonRefrence.Add(null);
                PersonRefrence.Add(null);
                PersonRefrence.Add(null);
                PersonRefrence.Add(null);
                PersonRefrence.Add(null);
                PersonRefrence.Add(null);
                PersonRefrence.Add(null);
                PersonRefrence.Add(null);
                PersonRefrence.Add(null);
            }

        if (setup)
        {
            
            PersonRefrence[1] = (GameObject)EditorGUILayout.ObjectField("Sophia", (GameObject)PersonRefrence[1], typeof(GameObject), true);
            PersonRefrence[0] = (GameObject)EditorGUILayout.ObjectField("Captian_Benjimin", (GameObject)PersonRefrence[0], typeof(GameObject), true);
            PersonRefrence[2] = (GameObject)EditorGUILayout.ObjectField("First_Mate_Albert", (GameObject)PersonRefrence[2], typeof(GameObject), true);
            PersonRefrence[3] = (GameObject)EditorGUILayout.ObjectField("Second_Mate_Andrew_Gilling", (GameObject)PersonRefrence[3], typeof(GameObject), true);
            PersonRefrence[4] = (GameObject)EditorGUILayout.ObjectField("Edward_William_Head", (GameObject)PersonRefrence[4], typeof(GameObject), true);
            PersonRefrence[5] = (GameObject)EditorGUILayout.ObjectField("Volkert_Lorenzen", (GameObject)PersonRefrence[5], typeof(GameObject), true);
            PersonRefrence[6] = (GameObject)EditorGUILayout.ObjectField("Boz_Lorenzen", (GameObject)PersonRefrence[6], typeof(GameObject), true);
            PersonRefrence[7] = (GameObject)EditorGUILayout.ObjectField("Arian_Martens", (GameObject)PersonRefrence[7], typeof(GameObject), true);
            PersonRefrence[8] = (GameObject)EditorGUILayout.ObjectField("Gottlieb_Goodschaad", (GameObject)PersonRefrence[8], typeof(GameObject), true);

            
            

                EditorGUILayout.LabelField("Other Refrences");
                

           Items = (GameObject)EditorGUILayout.ObjectField("Items", (GameObject)Items, typeof(GameObject), true);

        }
        else
        {
            bool canMake = true;

            for (int i=0; i<9;i++)
            {
                if (PersonRefrence[i] == null)
                {
                    if (SceneManager.GetActiveScene().name== "Adventure Manager Additive Scene")
                    {
                        GameObject AllPeople = FindObjectOfType<Dialogue_Storage>().gameObject;
                        PersonRefrence[i] = AllPeople.transform.GetChild(i).gameObject;
                    }
                }
            }

            
            if (Items == null)
            {
                if (SceneManager.GetActiveScene().name == "Adventure Manager Additive Scene")
                {
                    GameObject ItemCheck = FindObjectOfType<Item_Master>().gameObject;
                    Items = ItemCheck;
                }
            }

            WhoIsTalking holdCheck = Character;
            Character = (WhoIsTalking)EditorGUILayout.EnumPopup(Character);


            string PlaceIn = "Assets/Stories/";

                    switch (Character)
                    {
                        case WhoIsTalking.Sophia:
                            PlaceIn += "Sophia/";
                            break;
                        case WhoIsTalking.Captian_Benjimin:
                            PlaceIn += "Benjamin/";
                            break;
                        case WhoIsTalking.First_Mate_Albert:
                            PlaceIn += "Albert/";
                            break;
                        case WhoIsTalking.Second_Mate_Andrew_Gilling:
                            PlaceIn += "Andrew/";
                            break;
                        case WhoIsTalking.Edward_William_Head:
                            PlaceIn += "Edward/";
                            break;
                        case WhoIsTalking.Volkert_Lorenzen:
                            PlaceIn += "Volkert/";
                            break;
                        case WhoIsTalking.Boz_Lorenzen:
                            PlaceIn += "Boz/";
                            break;
                        case WhoIsTalking.Arian_Martens:
                            PlaceIn += "Arian/";
                            break;
                        case WhoIsTalking.Gottlieb_Goodschaad:
                            PlaceIn += "Gootlieb/";
                            break;
                    }

                Dismissal = (DialogueNode)EditorGUILayout.ObjectField("Dismisal Node", Dismissal, typeof(DialogueNode), true);

            string[] CheckingFiles = new string[1];
            CheckingFiles[0] = "Assets/Stories";

            if (Character != holdCheck || Dismissal == null)
                Dismissal = AssetDatabase.LoadAssetAtPath<DialogueNode>(PlaceIn + "Dismissal.asset");

            if (Dismissal==null)
            {
                EditorGUILayout.LabelField(PlaceIn + "Dismissal not found");

            }

            if (Character != holdCheck || CurrentlyFocusing==null)
            {
                switch (Character)
                {
                    case WhoIsTalking.Sophia:
                        CurrentlyFocusing = PersonRefrence[1].GetComponent<People>();
                        break;
                    case WhoIsTalking.Captian_Benjimin:
                        CurrentlyFocusing = PersonRefrence[0].GetComponent<People>();
                        break;
                    case WhoIsTalking.First_Mate_Albert:
                        CurrentlyFocusing = PersonRefrence[2].GetComponent<People>();
                        break;
                    case WhoIsTalking.Second_Mate_Andrew_Gilling:
                        CurrentlyFocusing = PersonRefrence[3].GetComponent<People>();
                        break;
                    case WhoIsTalking.Edward_William_Head:
                        CurrentlyFocusing = PersonRefrence[4].GetComponent<People>();
                        break;
                    case WhoIsTalking.Volkert_Lorenzen:
                        CurrentlyFocusing = PersonRefrence[5].GetComponent<People>();
                        break;
                    case WhoIsTalking.Boz_Lorenzen:
                        CurrentlyFocusing = PersonRefrence[6].GetComponent<People>();
                        break;
                    case WhoIsTalking.Arian_Martens:
                        CurrentlyFocusing = PersonRefrence[7].GetComponent<People>();
                        break;
                    case WhoIsTalking.Gottlieb_Goodschaad:
                        CurrentlyFocusing = PersonRefrence[8].GetComponent<People>();
                        break;
                }
            }
            EditorGUILayout.LabelField("Looking at " + CurrentlyFocusing.name);

            if (Character != holdCheck || CurrentlyFocusingDialogue == null)
            {
                CurrentlyFocusingDialogue = CurrentlyFocusing.GetComponent<DialogueHandler>();
            }
            initialDesignation = EditorGUILayout.TextField(initialDesignation);
            if (initialDesignation == "")
                canMake = false;

            Itemquestion = EditorGUILayout.Toggle("Item Description?", Itemquestion);
            if (Itemquestion)
            {
                    if (Items == null)
                    {
                        EditorGUILayout.LabelField("Items Not Set");
                        canMake = false;
                    }
                    else
                    {


                        EditorGUILayout.LabelField("Looking at " + CurrentlyFocusingDialogue.name);

                        if (ApplyToItem == null || ApplyToItem.Count != Items.transform.childCount)
                        {
                            ApplyToItem = new List<bool>();
                            for (int i = 0; i < Items.transform.childCount; i++)
                            {
                                /*bool check = true;

                                foreach (ItemOpinion itemCheck in  CurrentlyFocusingDialogue.ItemOpinions)
                                    if (itemCheck.Item.gameObject== Items.transform.GetChild(i).gameObject)
                                        if (itemCheck.TargetDialogueNode!=null)
                                            check = false;
                                if (check)*/
                                ApplyToItem.Add(false);
                            }
                        }

                        for (int i = 0; i < Items.transform.childCount; i++)
                        {
                            bool check = true;

                            foreach (ItemOpinion itemCheck in CurrentlyFocusingDialogue.ItemOpinions)
                            {

                                if (itemCheck.Item.gameObject == Items.transform.GetChild(i).gameObject)
                                    if (itemCheck.TargetDialogueNode != null)
                                        check = false;
                            }
                            if (check)
                                ApplyToItem[i] = EditorGUILayout.Toggle(Items.transform.GetChild(i).name, ApplyToItem[i]);

                        }

                    bool tempcheck = false;

                        foreach (bool checking in ApplyToItem)
                    {
                        if (checking)
                            tempcheck = true;
                    }

                    if (!tempcheck)
                        canMake = false;

                    }
            }


                numberOfLines = EditorGUILayout.IntField("Number of Lines", numberOfLines);

            if (numberOfLines < 0)
                numberOfLines = 0;
            if (DiologueToCreate == null)
            {
                DiologueToCreate = new List<string>();
            }
            while (DiologueToCreate.Count < numberOfLines && numberOfLines != 0)
            {
                DiologueToCreate.Add("");
            }

            for (int i = 0; i < numberOfLines; i++)
            {
                if (i % 2 == 1)
                    EditorGUILayout.LabelField("Sarahs Line");
                else
                    EditorGUILayout.LabelField(Character.ToString() + " Line");

                DiologueToCreate[i] = EditorGUILayout.TextField(DiologueToCreate[i]);
                
            if (DiologueToCreate[i] == "")
                canMake = false;
            }
            if (numberOfLines == 0)
                canMake = false;
                   


            if (canMake && AssetDatabase.FindAssets(initialDesignation, CheckingFiles).Length != 0)
            {
                if (initialDesignation!="")
                EditorGUILayout.LabelField(PlaceIn + initialDesignation + ".asset already exists");
                canMake = false;
            }


            EditorGUI.BeginDisabledGroup(canMake == false);
            if (GUILayout.Button("Create new Nodes"))
            {
                ScriptableObject NewDialogue;
                DialogueNode thisDialogue;
                DialogueNode LastDialogue = null;
                for (int i = 0; i < numberOfLines; i += 2)
                {
                    string Assetname = initialDesignation;
                    if (i != 0)
                    {
                        Assetname += ((i) / 2).ToString();
                    }
                    NewDialogue = ScriptableObject.CreateInstance<DialogueNode>();


                  


                    Debug.Log(PlaceIn + Assetname + ".asset");

                    AssetDatabase.CreateAsset(NewDialogue, PlaceIn + Assetname + ".asset");

                    EditorUtility.SetDirty(NewDialogue);

                    
                    thisDialogue = AssetDatabase.LoadAssetAtPath<DialogueNode>(PlaceIn + Assetname + ".asset");
                    if (i == 0 &&Itemquestion)
                    {
                            for (int j = 0; j< Items.transform.childCount; j++)
                            {
                                

                                if (ApplyToItem[j])
                                {
                                
                                    ItemOpinion holdItem = new ItemOpinion();
                                    holdItem.Item = Items.transform.GetChild(j).GetComponent<Items>();
                                    holdItem.TargetDialogueNode = thisDialogue;
                                    CurrentlyFocusingDialogue.ItemOpinions.Add(holdItem);
                                }
                            
                        }
                    }
                    thisDialogue.introText = DiologueToCreate[i];

                    if (LastDialogue != null)
                    {
                        LastDialogue.optionsText = new List<string>();
                        LastDialogue.optionsText.Add(DiologueToCreate[i - 1]);
                        LastDialogue.targetDialogueNodes = new List<DialogueNode>();
                        LastDialogue.targetDialogueNodes.Add(thisDialogue);


                    }

                    LastDialogue = thisDialogue;

                    if (i >= numberOfLines - 2)
                    {

                        thisDialogue.optionsText = new List<string>();
                        LastDialogue.targetDialogueNodes = new List<DialogueNode>();

                        if (i >= numberOfLines - 1)
                            thisDialogue.optionsText.Add("Continue");
                        else
                        {
                            LastDialogue.optionsText.Add(DiologueToCreate[i + 1]);
                        }

                    }



                }

                LastDialogue.targetDialogueNodes = new List<DialogueNode>();
                LastDialogue.targetDialogueNodes.Add(Dismissal);
                LastDialogue.modifiesQueryLimit = true;
                AssetDatabase.Refresh();
                numberOfLines = 0;
                DiologueToCreate = new List<string>();
            }

            EditorGUI.EndDisabledGroup();
        }
        }
    }
