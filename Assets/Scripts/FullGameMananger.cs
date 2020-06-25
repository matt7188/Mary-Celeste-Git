using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FullGameMananger : MonoBehaviour
{
    public enum TurnPageTo { NewGame,HardGame,ChooseGame,Options,QuitGame}
    public Introduction intro;

    public bool RunTutorial;

    public bool[] MurdersAcomplished;

    // Use this for initialization
    void Awake()
    {
        FullGameMananger[] objs = GameObject.FindObjectsOfType<FullGameMananger>();
        RunTutorial = true;

        if (objs.Length > 1)
        {
            intro.Run = false;
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);


    }

    public void TurnPage(TurnPageTo thisPage, bool book)
    {
        //if (book)
       // {
            switch (thisPage)
            {
                case TurnPageTo.NewGame:
                    FindObjectOfType<PageCurl>().Flip();
                    break;
                case TurnPageTo.Options:
                    FindObjectOfType<PageCurl>().Flip();
                    break;
                case TurnPageTo.QuitGame:
                    Application.Quit();
                    Debug.Log("would exit game");
                    break;
            }
     //   }

        /*else
     //   {
            switch (thisPage)
            {
                case TurnPageTo.NewGame:
                    SceneManager.LoadScene("Twist Adventure Scene");
                    SceneManager.LoadScene("SafeRoom", LoadSceneMode.Additive);
                    SceneManager.LoadScene("Twist Hallway", LoadSceneMode.Additive);
                    break;
                case TurnPageTo.Options:
                    FindObjectOfType<PageCurl>().Flip();
                    break;
                case TurnPageTo.QuitGame:
                    Application.Quit();
                    Debug.Log("would exit game");
                    break;
            }
   }     */


    }



    public void loadIntoScene()
    {
        SceneManager.LoadScene("Loading", LoadSceneMode.Additive);

        StartCoroutine(SceneLoad());
    }

    public IEnumerator SceneLoad()
    {
        AsyncOperation asyncLoadLevel;

        yield return new WaitForSeconds(0.01f);


        asyncLoadLevel = SceneManager.LoadSceneAsync("Captians Quarters", LoadSceneMode.Additive);
        while (!asyncLoadLevel.isDone)
            yield return new WaitForSeconds(0.3f);

        asyncLoadLevel = SceneManager.UnloadSceneAsync("Main Menu");
        while (!asyncLoadLevel.isDone)
            yield return new WaitForSeconds(0.3f);
        asyncLoadLevel= SceneManager.LoadSceneAsync("Adventure Manager Additive Scene", LoadSceneMode.Additive);

        while (!asyncLoadLevel.isDone)
            yield return new WaitForSeconds(0.01f);

 yield return new WaitForSeconds(2f);

        asyncLoadLevel = SceneManager.UnloadSceneAsync("Loading");
        while (!asyncLoadLevel.isDone)
            yield return new WaitForSeconds(0.3f);


        FindObjectOfType<GameMananger>().ToggleCursor(true);

        GameObject blackout= GameObject.FindGameObjectsWithTag("Black Out Transition")[0];


     RawImage Blackout= blackout.GetComponent<RawImage>();
    var tempcolor = Color.black;
        tempcolor.a = 1;
        Blackout.color = tempcolor;

        Blackout.gameObject.SetActive(true);
        float j = 40;

        while (Blackout.color.a > 0)
        {
            j--;

            tempcolor = Blackout.color;
            tempcolor.a = j / 40;
            Blackout.color = tempcolor;
            

            yield return new WaitForSeconds(0.01f);

        }

        FindObjectOfType<GameMananger>().ToggleCursor(false);

    }

}
