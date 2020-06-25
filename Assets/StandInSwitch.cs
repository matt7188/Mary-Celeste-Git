using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StandInSwitch : MonoBehaviour
{
    void OnMouseDown()
    {


        SceneManager.LoadScene("Twist Adventure Scene");
        SceneManager.LoadScene("SafeRoom", LoadSceneMode.Additive);
        SceneManager.LoadScene("Twist Hallway", LoadSceneMode.Additive);
        
    }
}
