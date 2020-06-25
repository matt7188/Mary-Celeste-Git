using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdventureGameMananger : MonoBehaviour {

    public Inventory_Managment inventoryInLevel;
    public GameObject Player;

    void Awake()
    {
        /* if (!i)
         {
             i = this;
             DontDestroyOnLoad(gameObject);
         }
         else
             Destroy(gameObject);*/
    }

    // Use this for initialization
    void Start() {


    }

    // Update is called once per frame
    void Update() {
        if (inventoryInLevel == null)
        {
            inventoryInLevel = FindObjectOfType<Inventory_Managment>();
        }   
    }

    public void LoadSection(string SceneName)
    {
        StartCoroutine(LoadSectionIE(SceneName));
    }
    public void UnloadSection(string SceneName)
    {
        StartCoroutine(UnLoadSectionIE(SceneName));
    }

    IEnumerator LoadSectionIE(string SceneName)
    {
        if(SceneManager.GetSceneByName(SceneName).isLoaded)
        {
            yield break;
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    IEnumerator UnLoadSectionIE(string SceneName)
    {
        if (!SceneManager.GetSceneByName(SceneName).isLoaded)
        {
            yield break;
        }
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(SceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }
}
