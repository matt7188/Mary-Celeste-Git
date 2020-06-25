using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    FullGameMananger FGM;

    public TextFx.TextFxTextMeshPro ActText;
    public GameObject ActSwitch;

    // Start is called before the first frame update
    void Start()
    {
        bool ActTwoCheck = false;

        if (FGM==null)
            FGM = GameObject.FindObjectOfType<FullGameMananger>();


        foreach (bool check in FGM.MurdersAcomplished)
            if (check)
                ActTwoCheck = true;
        if (ActTwoCheck)
            ActText.AnimationManager.PlayAnimation();
        else
            ActSwitch.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
