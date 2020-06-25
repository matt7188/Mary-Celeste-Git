using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Click : Interactable
{
    protected override void ShowInformation()
    {
        Item_Click[] allItems = FindObjectsOfType<Item_Click>();

        foreach (Item_Click Itemswitch in allItems)
        {

            ChangeLayersRecursively(Itemswitch.transform, 0);
        }

        ChangeLayersRecursively(transform, 9);

        if (Display_Object == null)
        {
            Diologue_Box[] hold = Resources.FindObjectsOfTypeAll(typeof(Diologue_Box)) as Diologue_Box[];
            foreach (Diologue_Box check in hold)
            {
                if (check.gameObject.tag == "Item View")
                    Display_Object = check.gameObject;
            }
        }

        if (Play_On_Click != null)
        {
            Play_On_Click.Play();
        }
        Display_Object.SetActive(true);
        Diologue_Box Send_Info = Display_Object.GetComponent<Diologue_Box>();
        Items ThisItem = GetComponent<Items>();


        ThisItem.DescriptionCheck();

        if (ThisItem.ItemCam == null)
        {
            ThisItem.ItemCam = GameObject.FindObjectOfType<Rotate_Around_Point>().gameObject;
        }

        ThisItem.ItemCam.transform.position = this.transform.position; /////spin around an object inside the itam

        GameMananger GM = FindObjectOfType<GameMananger>();

        GM.ToggleCursor(true);
        GM.TimePass(2);
        Send_Info.ItemDescription(ThisItem);
        GM.Notes.addItem(ThisItem);
        
    }

    protected override bool OnPickup()
    {


        Item_Click[] allItems = FindObjectsOfType<Item_Click>();

        foreach(Item_Click Itemswitch in allItems)
        {

            ChangeLayersRecursively(Itemswitch.transform, 0);
        }

        ChangeLayersRecursively(transform, 9);

        if (Display_Object == null)
        {
            Diologue_Box[] hold = Resources.FindObjectsOfTypeAll(typeof(Diologue_Box))as Diologue_Box[];
            foreach (Diologue_Box check in hold) {
                if (check.gameObject.tag== "Item View")
                    Display_Object = check.gameObject;
            }
        }

        Display_Object.SetActive(true);
        Diologue_Box Send_Info = Display_Object.GetComponent<Diologue_Box>();
        Items ThisItem = GetComponent<Items>();


        ThisItem.DescriptionCheck();

        if (ThisItem.ItemCam == null)
        {
            ThisItem.ItemCam = GameObject.FindObjectOfType<Rotate_Around_Point>().gameObject;
        }

        ThisItem.ItemCam.transform.position = this.transform.position; /////spin around an object inside the itam

        GameMananger GM = FindObjectOfType<GameMananger>();

        GM.ToggleCursor(true);
        GM.TimePass(2);
        Send_Info.ItemDescription(ThisItem);
        GM.Notes.addItem(ThisItem);


        Inventory_Managment IM = FindObjectOfType<Inventory_Managment>();

        IM.ItemHold = this;

        return true;
    }
void ChangeLayersRecursively(Transform trans, int num)
{
        trans.gameObject.layer = num;
    foreach (Transform child in trans)
    {
        ChangeLayersRecursively(child, num);
    }
}


}
