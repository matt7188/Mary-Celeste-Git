using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour {

    public enum whichAmI { Return, Options, Exit}

    public whichAmI IAmA;
    public GameObject EscapeMenu;

    public void OnClick()
    {
        switch (IAmA)
        {
            case whichAmI.Return:
                EscapeMenu.SetActive(!EscapeMenu.activeSelf);
                GameObject.FindObjectOfType<GameMananger>().ToggleCursor(EscapeMenu.activeSelf);
                break;
            case whichAmI.Options:
                SceneManager.LoadSceneAsync("Option Menu", LoadSceneMode.Additive);
                break;
            case whichAmI.Exit:
                Application.Quit();
                break;

        }
    }

}
