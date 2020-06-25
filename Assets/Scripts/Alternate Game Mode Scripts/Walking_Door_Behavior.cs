using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking_Door_Behavior : Interactable
{
    static bool SomeoneOpening;

    bool Currentlymoving;
    bool Opened;

    GameObject Rotating;
    void Start()
    {
        Type_of_Interaction = TypeOfInteraction.Door;
        Rotating = this.transform.parent.gameObject;
        SomeoneOpening = false;
        Currentlymoving = false;
        Opened = false;

        if (Inventory_Mananger == null) 
        Inventory_Mananger = GameObject.FindObjectOfType<Inventory_Managment>();
    }

    protected override void OpenDoor()
    {

        if (!Opened)
        StartCoroutine(rotateDoor(true));
    }


    void Update()
    {
        if(SomeoneOpening && !Currentlymoving&& Opened)
            StartCoroutine(rotateDoor(false));

    }

    IEnumerator rotateDoor(bool direction)
    {
        Opened = direction;
        int i = 85;
        Currentlymoving = true;
        SomeoneOpening = true;

        WalkThroughSceneChange Moving= GetComponent<WalkThroughSceneChange>();

        if (Moving!=null)
        {
            Moving.SwitchScenesPostDoor();
        }
        while (i > 0)
        {
            if (direction)
                Rotating.transform.Rotate(Vector3.forward, 1);
            else
                Rotating.transform.Rotate(Vector3.forward, -1);

            yield return new WaitForSeconds(0.02f);
            i--;
        }
        SomeoneOpening = false;
        Currentlymoving = false;

    }
}

