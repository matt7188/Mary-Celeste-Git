using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour {


    public enum Mode { Normal, Hard, Murder, MainMenu }

    public Mode WhichOne;
    
    public Image Blackout;

    void OnMouseDown()
    {
        StartCoroutine(StartNewGame(WhichOne));
    }
    public void OnClick()
    {
        StartCoroutine(StartNewGame(WhichOne));
    }

    public IEnumerator StartNewGame(Mode WhichOne)
    {
        switch (WhichOne) {
            case Mode.Normal:

        var tempcolor = Color.black;
        tempcolor.a = 0;
        Blackout.color = tempcolor;

        Blackout.gameObject.SetActive(true);
        float j = 0;

        while (Blackout.color.a < 1)
        {
            j++;

            tempcolor = Blackout.color;
            tempcolor.a = j / 20;
            Blackout.color = tempcolor;

            yield return new WaitForSeconds(0.01f);

        }
                FindObjectOfType<FullGameMananger>().loadIntoScene();
                break;
            case Mode.MainMenu:
                SceneManager.LoadScene("Main Menu");
                break;


        }
    }

}
