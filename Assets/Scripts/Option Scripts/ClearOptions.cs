using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearOptions : MonoBehaviour
{
   
    public void ExitOptions()
    {
        SceneManager.UnloadSceneAsync("Option Menu");
    }
}
