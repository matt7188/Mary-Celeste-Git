
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ImportPopup : PopupWindowContent
    {

    public static string FilePath;
    public static string MaterialsFilePath;
    public static string MeshFilePath;
    public static string PrefabFilePath;
    public static string TextureFilePath;
    public static string ImportFilePath;

    public static GameObject PrefabImport;


    public static string MaterialName;
    public static string Albedo;
    public static string Metallic;
    public static string NormalMap;
    public static string HeightMap;
    public static string Occlusion;
    public static string DetailMask;

    
    public override Vector2 GetWindowSize()
        {
            return new Vector2(400, 250);
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.Label("Check Correct Configuration", EditorStyles.boldLabel);
        MaterialName = EditorGUILayout.TextField("Object and Material Name", MaterialName);
        Albedo = EditorGUILayout.TextField("Albedo", Albedo);
        Metallic = EditorGUILayout.TextField("Metallic", Metallic);
        NormalMap = EditorGUILayout.TextField("NormalMap", NormalMap);
        HeightMap = EditorGUILayout.TextField("HeightMap", HeightMap);
        Occlusion = EditorGUILayout.TextField("Occlusion", Occlusion);
        DetailMask = EditorGUILayout.TextField("DetailMask", DetailMask);

        if (GUILayout.Button("Create Prefab", GUILayout.Width(200)))
        {


            AssetDatabase.MoveAsset(ImportFilePath + "/" + Albedo, TextureFilePath + "/" + Albedo);
            AssetDatabase.MoveAsset(ImportFilePath + "/" + Metallic, TextureFilePath + "/" + Metallic);
            AssetDatabase.MoveAsset(ImportFilePath + "/" + NormalMap, TextureFilePath + "/" + NormalMap);
            AssetDatabase.MoveAsset(ImportFilePath + "/" + HeightMap, TextureFilePath + "/" + HeightMap);
            AssetDatabase.MoveAsset(ImportFilePath + "/" + Occlusion, TextureFilePath + "/" + Occlusion);
            AssetDatabase.MoveAsset(ImportFilePath + "/" + DetailMask, TextureFilePath + "/" + DetailMask);


            var NewMaterial = new Material(Shader.Find("Standard"));
            
            NewMaterial.mainTexture=(Texture2D)AssetDatabase.LoadAssetAtPath(TextureFilePath + "/" + Albedo, typeof(Texture2D));
            NewMaterial.SetTexture("_MettallicGlossMap", (Texture2D)AssetDatabase.LoadAssetAtPath(TextureFilePath + "/" + Metallic, typeof(Texture2D)));
            NewMaterial.SetTexture("_BumpMap", (Texture2D)AssetDatabase.LoadAssetAtPath(TextureFilePath + "/" + NormalMap, typeof(Texture2D)));
            NewMaterial.SetTexture("_ParallaxMap", (Texture2D)AssetDatabase.LoadAssetAtPath(TextureFilePath + "/" + HeightMap, typeof(Texture2D)));
            NewMaterial.SetTexture("_Occlusion", (Texture2D)AssetDatabase.LoadAssetAtPath(TextureFilePath + "/" + Occlusion, typeof(Texture2D)));
            NewMaterial.SetTexture("_DetailMask", (Texture2D)AssetDatabase.LoadAssetAtPath(TextureFilePath + "/" + DetailMask, typeof(Texture2D)));
            
            Debug.Log(MaterialsFilePath + "/" + MaterialName + ".mat");

            AssetDatabase.CreateAsset(NewMaterial, MaterialsFilePath+"/"+ MaterialName+".mat");

            GameObject NewPrefab= Object.Instantiate(PrefabImport);

            if (NewPrefab.transform.childCount > 0)
            {
                foreach (Transform Child in NewPrefab.transform)
                {
                    MeshRenderer rend = Child.GetComponent<MeshRenderer>();

                    rend.material = (Material)AssetDatabase.LoadAssetAtPath(MaterialsFilePath + "/" + MaterialName + ".mat", typeof(Material));

                }
            }
            else
            {


                MeshRenderer rend = NewPrefab.GetComponent<MeshRenderer>();
                rend.material = (Material)AssetDatabase.LoadAssetAtPath(MaterialsFilePath + "/" + MaterialName + ".mat", typeof(Material));
            }
            PrefabUtility.SaveAsPrefabAsset(NewPrefab,PrefabFilePath + "/" + MaterialName + ".prefab");

            AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(PrefabImport), MeshFilePath + "/" + PrefabImport.name);

            editorWindow.Close();
        }
        if (GUILayout.Button("Close", GUILayout.Width(200)))
        {
            editorWindow.Close();
        }

    }

        public override void OnOpen()
        {
            Debug.Log("Popup opened: " + this);
        }

        public override void OnClose()
        {
            Debug.Log("Popup closed: " + this);
        }
}
#endif