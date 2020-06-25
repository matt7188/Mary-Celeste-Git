
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;


public class Interactable : MonoBehaviour {
    
    public enum TypeOfInteraction
    {
        Pickup,  
        // General Item collection. Comes with a Picture for the inventory screen
        Lock,
        // If you have the keys and Items in your inventory, then you can Active the lock
        Key,
        // Items that are needed for locks but do not take a space in your inventory
        Door,
        // Transportation Elements, may have a key
        Information,
        // NPC or Text
        People,
        // An interactive Gui
        Puzzle
        // An interactive Gui
    }

    public TypeOfInteraction Type_of_Interaction;

    protected Inventory_Managment Inventory_Mananger;
    protected AdventureGameMananger Game_Mananger;
    public cakeslice.Outline [] OutlineHold ;


    public AudioSource Play_On_Click;

    // Pickup
    public string Name;

    public Sprite Display_Sprite;

    //Lock
    public string [] Required_Objects;

    //Door
    public string Scene_Load;
    public string Scene_Unload;

    //Information   //People
    public GameObject Display_Object;

    //Puzzle
    //public Puzzle Puzzle_Object;

    void Start()
    {
        Inventory_Mananger = GameObject.FindObjectOfType<Inventory_Managment>();
        Game_Mananger = GameObject.FindObjectOfType<AdventureGameMananger>();
        
        OutlineHold = GetComponents<cakeslice.Outline>();

        if (OutlineHold.Length == 0)
            OutlineHold = GetComponentsInChildren<cakeslice.Outline>();
        if(OutlineHold.Length == 0)
            Debug.Log(gameObject.name+ " Does Not Have Outline!");
        
    }

protected virtual bool OnPickup()
    {
        if (Play_On_Click != null)
        {
            Play_On_Click.Play();
        }

        if (Inventory_Mananger.AddInventory(this))
        {
            Destroy(this.gameObject);

            return true;
        }
        else
            return false;

    }
    protected virtual void OnKeyPickup()
    {
        if (Play_On_Click != null)
        {
            Play_On_Click.Play();
        }
        Inventory_Mananger.AddKey(Name);
        Destroy(this.gameObject);

    }
    protected virtual void OpenDoor()
    {
        if (Play_On_Click != null)
        {
            Play_On_Click.Play();
        }
        SceneManager.LoadScene(Scene_Load, LoadSceneMode.Additive);

        for(int i=0;i<SceneManager.sceneCount;i++)
            if (SceneManager.GetSceneAt(i).name== Scene_Unload)
                SceneManager.UnloadSceneAsync(Scene_Unload);

    }
    protected virtual void OnUnlock()
    {
        if (Play_On_Click != null)
        {
            Play_On_Click.Play();
        }
        Destroy(this.gameObject);
        Inventory_Mananger.RecordLock(this.name);
    }

    protected virtual void ShowInformation()
    {
        if (Play_On_Click != null)
        {
            Play_On_Click.Play();
        }
        Display_Object.SetActive(true);
    }
    
    public bool clickOn ()
    {
        if (Play_On_Click != null)
        {
            Play_On_Click.Play();
        }
        bool HaveAll=true;

        switch (Type_of_Interaction)
        {
            case TypeOfInteraction.Pickup:
                return OnPickup();

            case TypeOfInteraction.Key:
                OnKeyPickup();
                return true;
            case TypeOfInteraction.Lock:
                foreach (string hold in Required_Objects)
                {
                    if (!Inventory_Mananger.CheckIfLockShouldOpen(hold))
                        HaveAll = false;

                }
                
                if ( HaveAll)
                {
                   OnUnlock();
                }
                

                return true;
            case TypeOfInteraction.Door:
                foreach (string hold in Required_Objects)
                {
                    if (!Inventory_Mananger.CheckIfLockShouldOpen(hold))
                    {
                        HaveAll = false;
                    }

                }

                if (HaveAll)
                {
                    OpenDoor();
                }
                return true;
            case TypeOfInteraction.Information:
                ShowInformation();
                return true;

            case TypeOfInteraction.People:
                ShowInformation();
                return true;

            case TypeOfInteraction.Puzzle:
                ShowInformation();
                return true;

        }
        return false;
    }
}


#if UNITY_EDITOR
// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.
[CustomEditor(typeof(Interactable), true)]
[CanEditMultipleObjects]
public class Interactable_Editor : Editor
{
    public SerializedProperty
       Type_of_Interaction_Prop,
       Name_Prop,
       Display_Sprite_Prop,
        Required_Objects_Prop,
    Display_Gui_Prop,
        Puzzle_That_Will_Be_Showed_Prop,
        Inventory_Mananger_Prop,
    Play_On_Click_Prop,
    Load_Scene_Prop,
    Unload_Scene_Prop;

    void OnEnable()
    {
        // Setup the SerializedProperties
        Type_of_Interaction_Prop = serializedObject.FindProperty("Type_of_Interaction");
        Name_Prop = serializedObject.FindProperty("Name");
        Display_Sprite_Prop = serializedObject.FindProperty("Display_Sprite");
        Required_Objects_Prop = serializedObject.FindProperty("Required_Objects");
        Display_Gui_Prop = serializedObject.FindProperty("Display_Object");
        Play_On_Click_Prop = serializedObject.FindProperty("Play_On_Click");
        Load_Scene_Prop = serializedObject.FindProperty("Scene_Load");
        Unload_Scene_Prop = serializedObject.FindProperty("Scene_Unload");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
                EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(Type_of_Interaction_Prop);

        Interactable.TypeOfInteraction st = (Interactable.TypeOfInteraction)Type_of_Interaction_Prop.enumValueIndex;
        

        switch (st)
        {
            case Interactable.TypeOfInteraction.Pickup:
                EditorGUILayout.PropertyField(Name_Prop, new GUIContent("Name"));
                EditorGUILayout.PropertyField(Display_Gui_Prop, new GUIContent("Display_Object"));
                EditorGUILayout.PropertyField(Display_Sprite_Prop, new GUIContent("Display_Sprite"));
                break;

            case Interactable.TypeOfInteraction.Lock:
                EditorGUILayout.PropertyField(Required_Objects_Prop, true);
                break;
            case Interactable.TypeOfInteraction.Key:
                EditorGUILayout.PropertyField(Name_Prop, new GUIContent("Name"));
                EditorGUILayout.PropertyField(Display_Sprite_Prop, new GUIContent("Display_Sprite"));
                break;
            case Interactable.TypeOfInteraction.Door:
                EditorGUILayout.PropertyField(Required_Objects_Prop, true);
                EditorGUILayout.PropertyField(Load_Scene_Prop, new GUIContent("New Scene"));
                EditorGUILayout.PropertyField(Unload_Scene_Prop, new GUIContent("Old Scene"));
                break;
            case Interactable.TypeOfInteraction.Information:
            case Interactable.TypeOfInteraction.People:
                EditorGUILayout.PropertyField(Display_Gui_Prop, new GUIContent("Display_Object"));
                break;

            case Interactable.TypeOfInteraction.Puzzle:
                EditorGUILayout.PropertyField(Display_Gui_Prop, new GUIContent("Display_Object"));
                break;

        }

        EditorGUILayout.PropertyField(Play_On_Click_Prop, new GUIContent("Play_On_Click"));

        serializedObject.ApplyModifiedProperties();
    }
}

#endif