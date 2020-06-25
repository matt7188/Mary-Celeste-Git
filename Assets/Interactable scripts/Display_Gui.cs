using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display_Gui : MonoBehaviour {

public    string Subtitle_Text;
    public GameObject Player_To_Be_Disabled;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        foreach (MonoBehaviour script in Player_To_Be_Disabled.GetComponents<MonoBehaviour>())
        {
            script.enabled = true;
        }

        //Player_To_Be_Disabled.gameObject.SetActive(true);
    }

    void OnEnable()
    {
        foreach (MonoBehaviour script in Player_To_Be_Disabled.GetComponents<MonoBehaviour>())
        {
            script.enabled = false;
        }

    }

}
