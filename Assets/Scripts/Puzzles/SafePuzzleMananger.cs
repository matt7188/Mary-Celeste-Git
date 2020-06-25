using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafePuzzleMananger : MonoBehaviour
{
    public GameObject[] TurnOn;

    public SafePuzzle[] AllTumblers;
    public GameObject[] Arrows;
    int lookingAt=0;

    public GameObject SafeDoor;

    public bool Open = false;


    public void Start()
    {
        foreach (GameObject Switching in TurnOn)
        {
            Debug.Log(Switching.name);
            Switching.SetActive(false);
        }
    }

    public void SetUp()
    {
        foreach (SafePuzzle SwitchPosition in AllTumblers)
        {
            if (SwitchPosition != AllTumblers[lookingAt])
                SwitchPosition.Active = false;
        }

        foreach (GameObject SwitchPosition in Arrows)
        {
            if (SwitchPosition != Arrows[lookingAt])
                SwitchPosition.gameObject.SetActive(false);
        }


        AllTumblers[lookingAt].Active = true;
        Arrows[lookingAt].gameObject.SetActive(true);
    }
        public void SwitchTumbler(bool Right)
    {
        //Debug.Log("Hit");
        if (Right)
            lookingAt++;
        else
            lookingAt--;

        if (lookingAt >= AllTumblers.Length)
            lookingAt = 0;
        if (lookingAt < 0)
            lookingAt = AllTumblers.Length-1;

        foreach (SafePuzzle SwitchPosition in AllTumblers)
            {
                if (SwitchPosition != AllTumblers[lookingAt])
                    SwitchPosition.Active=false;
            }

            foreach (GameObject SwitchPosition in Arrows)
            {
                if (SwitchPosition != Arrows[lookingAt])
                    SwitchPosition.gameObject.SetActive(false);
            }


        AllTumblers[lookingAt].Active = true;
        Arrows[lookingAt].gameObject.SetActive(true);


    }


    private void Update()
    {
        bool result = true;
        foreach (SafePuzzle Check in AllTumblers)
        {
            if (!Check.Correct)
            {
                result = false;
                break;
            }
        }
        
       
        if (result&& !Open)
        {
            StartCoroutine(OpenSafe());
        }


    }
    public TextFx.TextFxTextMeshPro ActText;

    IEnumerator OpenSafe()
    {

        foreach (GameObject Switching in TurnOn)
        {
            Switching.SetActive(true);
        }

        int test = 0;
        Open = true;
        Debug.Log("Made it");
        SafeDoor.GetComponent<Collider>().enabled = false;
        while (test<100)
            {
            test++;
                SafeDoor.transform.Rotate(new Vector3(0, 0, -1));
                yield return new WaitForSeconds(.01f);
            }


    }
}
