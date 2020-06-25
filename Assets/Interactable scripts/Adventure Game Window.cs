
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

// NEED KEY MENU
// Set up other rooms (Audio)
// Add Prefab option to spawner
// add aditive scene mananger


public class AdventureGameWindow : EditorWindow
{
    Vector2 scrollPos;

    AdventureGameWindowSettings SaveState;

    private enum CurrentlyWorking
    {
        Scene_Managment,
        Audio_Managment,
        Inventory_Managment,
        Object_Spawner

    }

    CurrentlyWorking WorkingSpace;

    List<string> Scenes_Available;
    int Scenes_Index = 1;
    string ScenesName;
    bool DeleteScene;
    bool Lighting;
    static string pathToScript;

    string ObjectName = "Put The Name Of The Object Here";
    GameObject ObjectBuilderPrefab;
    GameObject Player;

    Inventory_Managment Inventory_Mananger;
     bool SetUpGroupEnabled;
    bool CustomScript;
    bool BuildFromPrefab;
    bool SimpleCollider;
    GameObject prefabForNewObject;
    GameObject holdNewObject;
    string ScriptName;

    string Name;
    Sprite Display_Sprite;
   List<string> Required_Objects;
    int Inventory_Index = 1;
    GameObject Display_Gui;
    GameObject Puzzle_That_Will_Be_Showed;



    Material NewMaterial;
    Mesh NewMesh;

    AudioSource PlayOnInteract;

    Interactable.TypeOfInteraction Type_Of_Object;


    private enum ColliderTypes
    {
        Box,
        Sphear,
        Capsule,
        Mesh,
        Terrain,
        Wheel
            
    }

    ColliderTypes WhichCollider;

    [MenuItem("Window/Adventure Game Window")]
    

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AdventureGameWindow));
    }

    protected void OnEnable()
    {
        if (Inventory_Mananger == null)
        {
            Inventory_Managment[] hold = FindObjectsOfType<Inventory_Managment>();

            if (hold.Length == 0)
            {
                Inventory_Mananger = (Inventory_Managment)AssetDatabase.LoadAssetAtPath("Assets/Interactable scripts/Prefabs/Inventory slots.prefab", typeof(Inventory_Managment));
            }
            else
            {
                Inventory_Mananger = hold[0];
            }
        }
        if (ObjectBuilderPrefab == null)
        {
            ObjectBuilderPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Interactable scripts/Prefabs/Interactive Prefab.prefab", typeof(GameObject));

        }
    }

    void OnGUI()
    {
        WorkingSpace = (CurrentlyWorking)EditorGUILayout.EnumPopup(WorkingSpace);

        EditorGUILayout.BeginVertical();
        scrollPos =
            EditorGUILayout.BeginScrollView(scrollPos);

        switch (WorkingSpace)
        {
            /// Scene Managment Section ///
            case CurrentlyWorking.Scene_Managment:


                ////////Should not be Hardcoded///////////

                if (GUILayout.Button("Open Basic Game"))
                {
                    for (int j = 0; j < SceneManager.sceneCount; j++)
                    {
                            EditorSceneManager.SaveScene(SceneManager.GetSceneAt(j));
                            EditorSceneManager.CloseScene(SceneManager.GetSceneAt(j), true);
                    }
                    EditorSceneManager.OpenScene("Assets/Scenes/Adventure Manager Additive Scene.unity");
                    if (Lighting)
                    EditorSceneManager.OpenScene("Assets/Scenes/Lighting.unity", OpenSceneMode.Additive);

                    EditorSceneManager.OpenScene("Assets/Scenes/Captians Quarters.unity", OpenSceneMode.Additive);
                }
                Lighting = EditorGUILayout.Toggle("With Lighting?", Lighting);
                    Player = (GameObject)EditorGUILayout.ObjectField("Player", (GameObject)Player, typeof(GameObject), true);

                if (Player == null && AdventureGameWindowSettings.PlayerSave != null)
                    Player = (GameObject)AssetDatabase.LoadAssetAtPath(AdventureGameWindowSettings.PlayerSave, typeof(GameObject));

                if (AdventureGameWindowSettings.PlayerSave != AssetDatabase.GetAssetPath(Player))
                    AdventureGameWindowSettings.PlayerSave = AssetDatabase.GetAssetPath(Player);


                if (GUILayout.Button("Add Game Managment Scene"))
                {

                    if (!EditorSceneManager.GetSceneByName("Adventure Manager Additive Scene").isLoaded)
                    {
                        EditorSceneManager.OpenScene("Assets/Interactable scripts/Additive Scene/Adventure Manager Additive Scene.unity", OpenSceneMode.Additive);
                    }
                    EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneByName("Adventure Manager Additive Scene"));
                    {
                        if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
                        {
                            GameObject HoldPlayer = Instantiate(Player);
                            HoldPlayer.tag = "Player";
                        }
                    }


                }

                SetUpGroupEnabled = EditorGUILayout.BeginToggleGroup("Manual Set Up Managment Scene", SetUpGroupEnabled);
                if (SetUpGroupEnabled)
                {
                    GUILayout.Label("If you choose to not use the level streaming, or need to make");
                    GUILayout.Label("your own custom prefabs, change the following:");
                    ObjectBuilderPrefab = (GameObject)EditorGUILayout.ObjectField("Interactive Prefab", (GameObject)ObjectBuilderPrefab, typeof(GameObject), true);
                    Inventory_Mananger = (Inventory_Managment)EditorGUILayout.ObjectField("Inventory Mananger", (Inventory_Managment)Inventory_Mananger, typeof(Inventory_Managment), true);
                }
                
                EditorGUILayout.EndToggleGroup();



                DeleteScene = EditorGUILayout.Toggle("Remove other scenes?", DeleteScene);

                GUILayout.Label("Number of scenes:");

                int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

               Scenes_Index = sceneCount;

                string[] scenes = new string[sceneCount];

                Scenes_Index = EditorGUILayout.IntField(Scenes_Index);

                if (Scenes_Index < 0)
                    Scenes_Index = 0;
                if (Scenes_Available == null)
                {
                    Scenes_Available = new List<string>();
                }
                while (Scenes_Available.Count < Scenes_Index && Scenes_Index != 0)
                {
                    Scenes_Available.Add("");
                }

                for (int i = 0; i < sceneCount; i++)
                {

                     Scenes_Available[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));

                    scenes[i] = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
                }

                
               
                for (int i = 0; i < Scenes_Index; i++)
                {
                    if (Scenes_Available[i] != "" && Scenes_Available[i] != "Adventure Manager Additive Scene" && Scenes_Available[i] != "Lighting")
                    {
                        Scenes_Available[i] = EditorGUILayout.TextField("Scene Name", Scenes_Available[i] as string);
                        GUILayout.Label(scenes[i]);
                        if (Scenes_Available[i] != System.IO.Path.GetFileNameWithoutExtension(scenes[i]))
                        {
                            Debug.Log("hi");////// chenge scene name here
                        }

                        if (GUILayout.Button("Load up " + Scenes_Available[i]))
                        {
                            if (DeleteScene) { 
                                for(int j = 0; j < SceneManager.sceneCount; j++) { 
                                if (SceneManager.GetSceneAt(j).name != "Adventure Manager Additive Scene" && SceneManager.GetSceneAt(j).name != "Lighting")
                                {
                                    EditorSceneManager.SaveScene(SceneManager.GetSceneAt(j));
                                    EditorSceneManager.CloseScene(SceneManager.GetSceneAt(j), true);
                                }
                            }
                        }
                            EditorSceneManager.OpenScene(scenes[i],OpenSceneMode.Additive);

                        }
                    }
                }
                
                /*
                ScenesName= EditorGUILayout.TextField("Scene Name", ScenesName as string);

                if (GUILayout.Button("Add Scene Called " + ScenesName))
                {
                    var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
                    EditorSceneManager.SaveScene(newScene);
                    var original = EditorBuildSettings.scenes;
                    var newSettings = new EditorBuildSettingsScene[original.Length + 1];
                    System.Array.Copy(original, newSettings, original.Length);
                    var sceneToAdd = new EditorBuildSettingsScene(ScenesName, true);
                    newSettings[newSettings.Length - 1] = sceneToAdd;
                    EditorBuildSettings.scenes = newSettings;
                }
                */

                break;



            /// Object Spawner Section ///


            case CurrentlyWorking.Object_Spawner:

                GUILayout.Label("Base Settings", EditorStyles.boldLabel);
                ObjectName = EditorGUILayout.TextField("Object Name", ObjectName);



                Type_Of_Object = (Interactable.TypeOfInteraction)EditorGUILayout.EnumPopup("Object to create:", Type_Of_Object);



                switch (Type_Of_Object)
                {
                    case Interactable.TypeOfInteraction.Pickup:
                        Name = EditorGUILayout.TextField("Display Name", Name);
                        Display_Sprite = (Sprite)EditorGUILayout.ObjectField("Sprite in Inventory", (Sprite)Display_Sprite, typeof(Sprite), true);
                        break;

                    case Interactable.TypeOfInteraction.Lock:
                        GUILayout.Label("Inventory Item Editor", EditorStyles.boldLabel);

                        Inventory_Index = EditorGUILayout.IntField(Inventory_Index);
                        if (Inventory_Index < 0)
                            Inventory_Index = 0;
                        if (Required_Objects == null)
                        {
                            Required_Objects = new List<string>();
                        }
                        while (Required_Objects.Count < Inventory_Index && Inventory_Index != 0)
                        {
                            Required_Objects.Add("");
                        }

                        for (int i = 0; i < Inventory_Index; i++)
                            Required_Objects[i] = EditorGUILayout.TextField("Item Name", Required_Objects[i] as string);

                        break;

                    case Interactable.TypeOfInteraction.Key:
                        Name = EditorGUILayout.TextField("Name", Name);
                        Display_Sprite = (Sprite)EditorGUILayout.ObjectField("Sprite in Inventory", (Sprite)Display_Sprite, typeof(Sprite), true);
                        break;
                    case Interactable.TypeOfInteraction.Door:
                        Name = EditorGUILayout.TextField("Name", Name);
                        Display_Sprite = (Sprite)EditorGUILayout.ObjectField("Sprite in Inventory", (Sprite)Display_Sprite, typeof(Sprite), true);
                        break;

                    case Interactable.TypeOfInteraction.Information:
                    case Interactable.TypeOfInteraction.People:
                        Display_Gui = (GameObject)EditorGUILayout.ObjectField("Gui That Will Be Displayed", (GameObject)Display_Gui, typeof(GameObject), true);
                        break;
                        
                }

                CustomScript = EditorGUILayout.Toggle("Add Custom Script", CustomScript);

                if (CustomScript)
                {
                    GUILayout.Label("This will make a new script in your home directory");
                }

                GUILayout.Label("Object Settings", EditorStyles.boldLabel);

                BuildFromPrefab = EditorGUILayout.Toggle("Build from Prefab:", BuildFromPrefab);
                if (BuildFromPrefab)
                {
                    prefabForNewObject= (GameObject)EditorGUILayout.ObjectField("Prefab:", (GameObject)prefabForNewObject, typeof(GameObject), true);

                }
                else
                {
                    NewMesh = (Mesh)EditorGUILayout.ObjectField("New Mesh", (Mesh)NewMesh, typeof(Mesh), true);
                    NewMaterial = (Material)EditorGUILayout.ObjectField("New Material", (Material)NewMaterial, typeof(Material), true);

                    SimpleCollider = EditorGUILayout.Toggle("Add Simple Collider", SimpleCollider);

                    if (SimpleCollider)
                    {
                        GUILayout.Label("Choose the Collider to add.");
                        GUILayout.Label("You will most likely need to adjust size and shape to match the object.");

                        WhichCollider = (ColliderTypes)EditorGUILayout.EnumPopup("Collider to create:", WhichCollider);
                    }
                }
                GUILayout.Label("Sound Settings", EditorStyles.boldLabel);

                PlayOnInteract = (AudioSource)EditorGUILayout.ObjectField("Audio on interact", (AudioSource)PlayOnInteract, typeof(AudioSource), true);


                string Warning = "";

                if (Player == null || ObjectBuilderPrefab == null || Inventory_Mananger == null)
                    Warning += "One of the required initialliing objects is not assigned. \n";

                if (ObjectName == "")
                    Warning += "The Object needs a name. \n";


                if (!EditorSceneManager.GetSceneByName("Adventure Manager Additive Scene").isLoaded)
                    Warning += "The Game Mananger Scene is not loaded. \n";

                switch (Type_Of_Object)
                {
                    case Interactable.TypeOfInteraction.Pickup:
                    case Interactable.TypeOfInteraction.Door:
                        if (Name == "")
                            Warning += "No Display Name is Given. \n";
                        if (Display_Sprite == null)
                            Warning += "No Display Sprite is Assigned. \n";
                        break;

                    case Interactable.TypeOfInteraction.Lock:
                        if (Required_Objects.Contains(""))
                            Warning += "Blank Object Required. \n";
                        break;

                    case Interactable.TypeOfInteraction.Information:
                    case Interactable.TypeOfInteraction.People:
                        if (Display_Gui == null)
                            Warning += "No Display Gui Assigned. \n";
                        break;
                       
                }

                if (BuildFromPrefab)
                {
                    if (prefabForNewObject == null)
                        Warning += "There is no Prefab Assigned. \n";
                }
                else
                {
                    if (NewMesh == null)
                        Warning += "There is no Mesh Assigned. \n";

                    if (NewMaterial == null)
                        Warning += "There is no Material Assigned. \n";


                    if (PlayOnInteract == null)
                        Warning += "There is no Audio for interactions. \n";
                }
                

                if (Warning != "")
                {
                    GUILayout.Label("Warning", EditorStyles.boldLabel);


                    GUILayout.Label(Warning);
                }






                if (holdNewObject == null && GUILayout.Button("Spawn New " + Type_Of_Object.ToString()))
                {
                    
                    if (BuildFromPrefab)
                    {

                        holdNewObject = Instantiate(prefabForNewObject, new Vector3(0, 0, 0), Quaternion.identity);
                        
                        holdNewObject.AddComponent<Interactable>();
                    }
                    else
                    {
                     holdNewObject = Instantiate(ObjectBuilderPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                   
                   
                    
                        if (SimpleCollider)
                        {
                            switch (WhichCollider)
                            {
                                case ColliderTypes.Box:
                                    holdNewObject.AddComponent<BoxCollider>();
                                    break;
                                case ColliderTypes.Capsule:
                                    holdNewObject.AddComponent<CapsuleCollider>();
                                    break;
                                case ColliderTypes.Mesh:
                                    holdNewObject.AddComponent<MeshCollider>();
                                    break;
                                case ColliderTypes.Sphear:
                                    holdNewObject.AddComponent<SphereCollider>();
                                    break;
                                case ColliderTypes.Terrain:
                                    holdNewObject.AddComponent<TerrainCollider>();
                                    break;
                                case ColliderTypes.Wheel:
                                    holdNewObject.AddComponent<WheelCollider>();
                                    break;

                            }
                        }


                        holdNewObject.GetComponent<Renderer>().material = NewMaterial;
                        holdNewObject.GetComponent<MeshFilter>().mesh = NewMesh;
                        NewMaterial = null;
                        NewMesh = null;
                    }
                    holdNewObject.name = ObjectName;
                    ObjectName = "Put The Name Of The Object Here";
                    if (CustomScript)
                    {
                        DestroyImmediate(holdNewObject.GetComponent<Interactable>());
                        ScriptName = CreateScriptComponent(holdNewObject);
                    }
                }
                    

                break;


        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        if (holdNewObject != null && !EditorApplication.isCompiling)
        {
            
           

            if (holdNewObject.GetComponent<Interactable>()==null)
            holdNewObject.AddComponent(Type.GetType(ScriptName));

            holdNewObject.GetComponent<Interactable>().Name = Name;

            holdNewObject.GetComponent<Interactable>().Display_Sprite = Display_Sprite;

            holdNewObject.GetComponent<Interactable>().Required_Objects = Required_Objects.ToArray();
            Required_Objects.Clear();
            holdNewObject.GetComponent<Interactable>().Display_Object = Display_Gui;
            Display_Gui = null;
            
            holdNewObject.GetComponent<Interactable>().Play_On_Click = PlayOnInteract;

            Type_Of_Object = Interactable.TypeOfInteraction.Pickup;
            holdNewObject = null;

        }


        }
  


    public string CreateScriptComponent(GameObject selected)
    {
        if (selected == null || selected.name.Length == 0)
        {
            Debug.Log("Selected object not Valid");
            return "Interactable";
        }
        else
        {
            // remove whitespace and minus
            string name = selected.name.Replace(" ", "_");
            name = name.Replace("-", "_");
            string copyPath = "Assets/" + name + ".cs";
            Debug.Log("Creating Classfile: " + copyPath);
            int Append = 0;
            if (File.Exists(copyPath) == true) { 
            do
            {
                string HoldName;
                Append++;
                HoldName = name + Append;
                copyPath = "Assets/" + HoldName + ".cs";
            }
            while (File.Exists(copyPath) == true);
            name = name + Append;
        }
            // do not overwrite
            using (StreamWriter outfile =
                    new StreamWriter(copyPath))
                {
                    outfile.WriteLine("using UnityEngine;");
                    outfile.WriteLine("using System.Collections;");
                    outfile.WriteLine("");
                    outfile.WriteLine("public class " + name + " : Interactable {");
                    outfile.WriteLine(" ");
                    outfile.WriteLine(" ");
                    outfile.WriteLine(" // Use this for initialization");
                    outfile.WriteLine(" void Start () {");
                outfile.WriteLine(" Inventory_Mananger = GameObject.FindObjectOfType<Inventory_Managment>();");
                outfile.WriteLine(" Game_Mananger = GameObject.FindObjectOfType<AdventureGameMananger>(); ");
                    outfile.WriteLine(" }");
                    outfile.WriteLine(" ");
                    outfile.WriteLine(" ");
                    outfile.WriteLine(" // Update is called once per frame");
                    outfile.WriteLine(" void Update () {");
                    outfile.WriteLine(" ");
                    outfile.WriteLine(" }");
                outfile.WriteLine("// protected override bool OnPickup() { }");
                outfile.WriteLine("// protected override void OnKeyPickup() { }");
                outfile.WriteLine("// protected override void OnUnlock() { }");
                outfile.WriteLine("// protected override void OpenDoor() { }");
                outfile.WriteLine("// protected override void ShowInformation() { }");
                outfile.WriteLine("// protected override void DisplayPuzzle() { }");
                outfile.WriteLine("//public override bool clickOn() { }"); 
    
        outfile.WriteLine("}");
                //File written
            }
           // //AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
           // AssetDatabase.ImportAsset(copyPath,ImportAssetOptions.ForceSynchronousImport);
            AssetDatabase.Refresh();
            //holdNewObject.AddComponent(Type.GetType(ScriptName));
            return name;
        }

    
}

}


[InitializeOnLoad]
public class AdventureGameWindowSettings
{
    private static string Player;
    static AdventureGameWindowSettings()
    {
        Load();
    }
    public static void Load()
    {
        Player = EditorPrefs.GetString("AdventureWindowPlayer", null);
    }
    public static void Save()
    {
        EditorPrefs.SetString("AdventureWindowPlayer", Player);
    }
    public static string PlayerSave
    {
        get { return Player; }
        set
        {
            if (Player != value)
            {
                Player = value;
                Save();
            }
        }
    }
}
#endif