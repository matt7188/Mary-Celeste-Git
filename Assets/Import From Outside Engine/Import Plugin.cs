
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


public class ImportPlugin : EditorWindow
{

    string FilePath;
    string MaterialsFilePath;
    string MeshFilePath;
    string PrefabFilePath;
    string TextureFilePath;
    string ImportFilePath;

    GameObject PrefabImport;
    Material SetMaterial;

    string MaterialName;
    string Albedo;
    string Metallic;
    string NormalMap;
    string HeightMap;
    string Occlusion;
    string DetailMask;

    bool displayOrganize;
    bool ExistingMaterial;


    Rect buttonRect;

    [MenuItem("Window/Import Plugin Window")]


    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ImportPlugin));
    }

    protected void OnEnable()
    {
       
    }

    void OnGUI()
    {
        displayOrganize = true;

        GUILayout.Label("Main Folder:", EditorStyles.boldLabel);
        FilePath = EditorGUILayout.TextField("Folder Path", FilePath);
        if (GUILayout.Button("Locate Folder"))
        {
            FilePath = EditorUtility.OpenFolderPanel("Choose A Path", "", "");
            MaterialsFilePath="";
            MeshFilePath = "";
            PrefabFilePath = "";
            TextureFilePath = "";
            ImportFilePath = "";
        }

        if (!Directory.Exists(FilePath))
        {

            GUILayout.Label("Path does not exist, create it?");
            if (GUILayout.Button("Create Folder"))
            {
                Directory.CreateDirectory(FilePath);
                AssetDatabase.Refresh();
            }

        }
        else
        {

            FilePath= FilePath.Replace(Application.dataPath, "Assets");

            GUILayout.Label("Materials Folder:", EditorStyles.boldLabel);
            if (MaterialsFilePath == "")
            {
                MaterialsFilePath = FilePath + "/Materials";
                MaterialsFilePath = MaterialsFilePath.Replace(Application.dataPath,"Assets/");

            }
            MaterialsFilePath = EditorGUILayout.TextField("Folder", MaterialsFilePath);

            if (!Directory.Exists(MaterialsFilePath))
            {
                displayOrganize = false;
                GUILayout.Label("No Materials Folder. Create new or designate:");
                if (GUILayout.Button("Locate Folder"))
                {
                    MaterialsFilePath = EditorUtility.OpenFolderPanel("Choose A Path", FilePath, "");

                }
                if (GUILayout.Button("Create Folder"))
                {
                    Directory.CreateDirectory(MaterialsFilePath);
                    AssetDatabase.Refresh();
                }

            }
            GUILayout.Label("Mesh Folder:", EditorStyles.boldLabel);
            if (MeshFilePath == "")
                MeshFilePath = FilePath + "/Mesh";

            MeshFilePath = EditorGUILayout.TextField("Folder", MeshFilePath);

            if (!Directory.Exists(MeshFilePath))
            {
                displayOrganize = false;
                GUILayout.Label("No Mesh Folder. Create new or designate:");
                if (GUILayout.Button("Locate Folder"))
                {
                    MeshFilePath = EditorUtility.OpenFolderPanel("Choose A Path", FilePath, "");

                }
                if (GUILayout.Button("Create Folder"))
                {
                    Directory.CreateDirectory(MeshFilePath);
                    AssetDatabase.Refresh();
                }

            }
            GUILayout.Label("Prefab Folder:", EditorStyles.boldLabel);
            if (PrefabFilePath == "")
                PrefabFilePath = FilePath + "/Prefab";

            PrefabFilePath = EditorGUILayout.TextField("Folder", PrefabFilePath);

            if (!Directory.Exists(PrefabFilePath))
            {
                displayOrganize = false;
                GUILayout.Label("No Prefab Folder. Create new or designate:");
                if (GUILayout.Button("Locate Folder"))
                {
                    PrefabFilePath = EditorUtility.OpenFolderPanel("Choose A Path", FilePath, "");

                }
                if (GUILayout.Button("Create Folder"))
                {
                    Directory.CreateDirectory(PrefabFilePath);
                    AssetDatabase.Refresh();
                }

            }
            GUILayout.Label("Texture Folder:", EditorStyles.boldLabel);
            if (TextureFilePath == "")
                TextureFilePath = FilePath + "/Texture";

            TextureFilePath = EditorGUILayout.TextField("Folder", TextureFilePath);

            if (!Directory.Exists(TextureFilePath))
            {
                displayOrganize = false;
                GUILayout.Label("No Texture Folder. Create new or designate:");
                if (GUILayout.Button("Locate Folder"))
                {
                    TextureFilePath = EditorUtility.OpenFolderPanel("Choose A Path", FilePath, "");

                }
                if (GUILayout.Button("Create Folder"))
                {
                    Directory.CreateDirectory(TextureFilePath);
                    AssetDatabase.Refresh();
                }

            }

            GUILayout.Label("Import Folder:", EditorStyles.boldLabel);
            if (ImportFilePath == "")
                ImportFilePath = FilePath + "/Import";

            ImportFilePath = EditorGUILayout.TextField("Folder", ImportFilePath);

            if (!Directory.Exists(ImportFilePath))
            {
                displayOrganize = false;
                GUILayout.Label("No Import Folder. Create new or designate:");
                if (GUILayout.Button("Locate Folder"))
                {
                    ImportFilePath = EditorUtility.OpenFolderPanel("Choose A Path", FilePath, "");

                }
                if (GUILayout.Button("Create Folder"))
                {
                    Directory.CreateDirectory(ImportFilePath);
                    AssetDatabase.Refresh();
                }

            }
        }


        if (displayOrganize)
        {
            GUILayout.Label("Naming Conventions:", EditorStyles.boldLabel);
            GUILayout.Label("Write down the naming conventions to look for. Use * fill in, and N/A or blank for if it is not present.");


            EditorGUI.BeginChangeCheck();
            MaterialName = EditorGUILayout.TextField("Object and Material Name", MaterialName);
            PrefabImport = (GameObject)EditorGUILayout.ObjectField("Object:", (GameObject)PrefabImport, typeof(GameObject), true);

            if (EditorGUI.EndChangeCheck()||( PrefabImport == null && MaterialName!=""))
            {
                var info = new DirectoryInfo(ImportFilePath);
                var fileInfo = info.GetFiles();
                foreach (FileInfo file in fileInfo)
                {
                    if (AssetDatabase.GetMainAssetTypeAtPath(ImportFilePath + "/" + file.Name) == typeof(GameObject))
                    {
                        if (Path.GetFileNameWithoutExtension(file.Name).Contains(MaterialName))
                        {
                            PrefabImport = (GameObject)AssetDatabase.LoadAssetAtPath(ImportFilePath + "/"+file.Name, typeof(GameObject));
                        }

                    }
                }
            }
            
            if (PrefabImport.transform.childCount > 0)
                GUILayout.Label("More then one Object in prefab", EditorStyles.boldLabel);


            ExistingMaterial = EditorGUILayout.Toggle("Using an existing Material", ExistingMaterial);

            if (!ExistingMaterial)
            {
                Albedo = EditorGUILayout.TextField("Albedo", Albedo);
                Metallic = EditorGUILayout.TextField("Metallic", Metallic);
                NormalMap = EditorGUILayout.TextField("NormalMap", NormalMap);
                HeightMap = EditorGUILayout.TextField("HeightMap", HeightMap);
                Occlusion = EditorGUILayout.TextField("Occlusion", Occlusion);
                DetailMask = EditorGUILayout.TextField("DetailMask", DetailMask);
            if (GUILayout.Button("Organize Import"))
            {
                
                bool[] Found = new bool[7] ;

                for (int i=0;i<7; i++)
                    Found[i] = false;

                List<string> AlreadyAssigned = new List<string>();

                var info = new DirectoryInfo(ImportFilePath);
                var fileInfo = info.GetFiles();


                    if(PrefabImport!= null)
                ImportPopup.MaterialName = PrefabImport.name;
                    else
                        ImportPopup.MaterialName = MaterialName;


                    foreach (FileInfo file in fileInfo)
                {
                    if (file.Extension != ".meta" && Path.GetFileNameWithoutExtension(file.Name).Contains(MaterialName))
                    {
                        string[] HoldStart = { Albedo, "" };

                        if (Albedo != "" && Albedo != "N/A")
                        {

                            if (Albedo.Contains("*"))
                            {
                                HoldStart = Albedo.Split('*');
                            }

                            if (!Found[0] && Path.GetFileNameWithoutExtension(file.Name).Contains(HoldStart[0]) &&
                                Path.GetFileNameWithoutExtension(file.Name).Contains(HoldStart[1]))
                            {
                                //Debug.Log(Path.GetFileNameWithoutExtension(file.Name) + " Is the Albedo");
                                //AlreadyAssigned.Add(Path.GetFileNameWithoutExtension(file.Name));
                                Found[0] = true;
                                ImportPopup.Albedo = file.Name;
                            }
                        }
                        if (Metallic != "" && Metallic != "N/A")
                        {


                            HoldStart = new string[] { Metallic, "" };
                            if (Metallic.Contains("*"))
                            {
                                HoldStart = Metallic.Split('*');
                            }
                            if (!Found[1] && Path.GetFileNameWithoutExtension(file.Name).Contains(HoldStart[0]) &&
                                Path.GetFileNameWithoutExtension(file.Name).Contains(HoldStart[1]) &&
                                !AlreadyAssigned.Contains(Path.GetFileNameWithoutExtension(file.Name)))
                            {
                                //Debug.Log(Path.GetFileNameWithoutExtension(file.Name) + " Is the Metallic");
                                //AlreadyAssigned.Add(Path.GetFileNameWithoutExtension(file.Name));
                                Found[1] = true;
                                ImportPopup.Metallic = file.Name;
                            }

                        }

                    
                        if (NormalMap != "" && NormalMap != "N/A")
                        {
                            HoldStart = new string[] { NormalMap, "" };
                        if (NormalMap.Contains("*"))
                        {
                            HoldStart = NormalMap.Split('*');
                        }
                        if (!Found[3] && Path.GetFileNameWithoutExtension(file.Name).Contains(HoldStart[0]) &&
                            Path.GetFileNameWithoutExtension(file.Name).Contains(HoldStart[1]) &&
                                !AlreadyAssigned.Contains(Path.GetFileNameWithoutExtension(file.Name)))
                                {
                                    //Debug.Log(Path.GetFileNameWithoutExtension(file.Name) + " Is the Normal Map");
                                //AlreadyAssigned.Add(Path.GetFileNameWithoutExtension(file.Name));
                                Found[3] = true;
                                ImportPopup.NormalMap = file.Name;
                            }
                        }
                        if (HeightMap != "" && HeightMap != "N/A")
                        {
                            HoldStart = new string[] { HeightMap, "" };
                        if (HeightMap.Contains("*"))
                        {
                            HoldStart = HeightMap.Split('*');
                        }
                        if (!Found[4] && Path.GetFileNameWithoutExtension(file.Name).Contains(HoldStart[0]) &&
                            Path.GetFileNameWithoutExtension(file.Name).Contains(HoldStart[1]) &&
                                !AlreadyAssigned.Contains(Path.GetFileNameWithoutExtension(file.Name)))
                                    {
                                        //Debug.Log(Path.GetFileNameWithoutExtension(file.Name) + " Is the Height Map");
                                //AlreadyAssigned.Add(Path.GetFileNameWithoutExtension(file.Name));

                                Found[4] = true;
                                ImportPopup.HeightMap = file.Name;
                            }
                        }
                        if (Occlusion != "" && Occlusion != "N/A")
                        {
                            HoldStart = new string[] { Occlusion, "" };
                        if (Occlusion.Contains("*"))
                        {
                            HoldStart = Occlusion.Split('*');
                        }
                        if (!Found[5] && Path.GetFileNameWithoutExtension(file.Name).Contains(HoldStart[0]) &&
                            Path.GetFileNameWithoutExtension(file.Name).Contains(HoldStart[1]) &&
                                !AlreadyAssigned.Contains(Path.GetFileNameWithoutExtension(file.Name)))
                                        {
                                            //Debug.Log(Path.GetFileNameWithoutExtension(file.Name) + " Is the Occlusion");
                               // AlreadyAssigned.Add(Path.GetFileNameWithoutExtension(file.Name));

                                Found[5] = true;
                                ImportPopup.Occlusion = file.Name;
                            }
                        }
                        if (DetailMask != "" && DetailMask != "N/A")
                        {
                            HoldStart = new string[] { DetailMask, "" };
                            if (DetailMask.Contains("*"))
                            {
                                HoldStart = DetailMask.Split('*');
                            }
                            if (!Found[6]&&Path.GetFileNameWithoutExtension(file.Name).Contains(HoldStart[0]) &&
                                Path.GetFileNameWithoutExtension(file.Name).Contains(HoldStart[1]) &&
                                !AlreadyAssigned.Contains(Path.GetFileNameWithoutExtension(file.Name)))
                                            {
                                                //Debug.Log(Path.GetFileNameWithoutExtension(file.Name) + " Is the Detail Mask");
                                //AlreadyAssigned.Add(Path.GetFileNameWithoutExtension(file.Name));
                                Found[6] = true;
                                ImportPopup.DetailMask = file.Name;
                            }
                        }

                       

                    }

                }
                ImportPopup.MaterialsFilePath = MaterialsFilePath;
                ImportPopup.ImportFilePath = ImportFilePath;
                ImportPopup.FilePath = FilePath;
                ImportPopup.MeshFilePath = MeshFilePath;
                ImportPopup.PrefabFilePath = PrefabFilePath;
                ImportPopup.TextureFilePath = TextureFilePath;
                ImportPopup.PrefabImport = PrefabImport;

                PopupWindow.Show(buttonRect, new ImportPopup());
                        if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();


                    if (MaterialName != "")
                    {
                         info = new DirectoryInfo(ImportFilePath);
                         fileInfo = info.GetFiles();
                        foreach (FileInfo file in fileInfo)
                        {
                            if (file.Extension == ".FBX")
                            {
                                if (Path.GetFileNameWithoutExtension(file.Name).Contains(MaterialName))
                                {
                                    PrefabImport = (GameObject)AssetDatabase.LoadAssetAtPath(ImportFilePath + "/" + file.Name, typeof(GameObject));
                                }

                            }
                        }
                    }

                }
            }
            else
            {
                SetMaterial = (Material)EditorGUILayout.ObjectField("Material:", (Material)SetMaterial, typeof(Material), true);

                if (GUILayout.Button("Attach Imported FBX"))
                {
                    GameObject NewPrefab = Object.Instantiate(PrefabImport);


                    if (NewPrefab.transform.childCount > 0)
                    {
                        foreach (Transform Child in NewPrefab.transform)
                        {
                            MeshRenderer rend = Child.GetComponent<MeshRenderer>();
                            
                            rend.material = SetMaterial;
                        }
                    }
                    else
                    {

                        MeshRenderer rend = NewPrefab.GetComponent<MeshRenderer>();
                        rend.material = SetMaterial;

                    }
                    

                        if (File.Exists(PrefabFilePath + "/" + PrefabImport.name + ".prefab"))
                    {
                        PrefabUtility.SaveAsPrefabAsset( NewPrefab,PrefabFilePath + "/" + PrefabImport.name + "1" + ".prefab");
                    }
                    else
                        PrefabUtility.SaveAsPrefabAsset(NewPrefab,PrefabFilePath + "/" + PrefabImport.name + ".prefab");

                    AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(PrefabImport), MeshFilePath + "/" + PrefabImport.name);
                    if (MaterialName != "")
                    {
                        var info = new DirectoryInfo(ImportFilePath);
                        var fileInfo = info.GetFiles();
                        foreach (FileInfo file in fileInfo)
                        {
                            if (file.Extension == ".FBX")
                            {
                                if (Path.GetFileNameWithoutExtension(file.Name).Contains(MaterialName))
                                {
                                    PrefabImport = (GameObject)AssetDatabase.LoadAssetAtPath(ImportFilePath + "/" + file.Name, typeof(GameObject));
                                }

                            }
                        }
                    }
                }
            }

            /*
        if (GUILayout.Button("Import", GUILayout.Width(200)))
                    {
                        PopupWindow.Show(buttonRect, new ImportPopup());
                    }
                    if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
        }
        */
        }
    }


}

#endif

