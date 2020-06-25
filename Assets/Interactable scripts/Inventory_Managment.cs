using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Managment : MonoBehaviour {

     public GameObject[] Inventory;

     public List<string> KeyRing;

    public List<string> Previously_Opened_Lock;

    public List<string> Previously_Solved_Puzzles;


    public int Number_Of_Slots=5;
    public GameObject Inventory_Prefab_Object;
    
    public Sprite Frame;
    public Sprite Empty_Image;

    public float Margin_For_Inventory_Slots=10;
    
    int Number_filled_slots;
    
    public float RightMargin;
    public float LeftMargin;

    public Interactable ItemHold;

    public GameObject TurnOffObject;
    public Text InfoOut;

    


    void Start()
    {
        

        Number_filled_slots = 0;
        Inventory = new GameObject[Number_Of_Slots];
        KeyRing = new List<string>();


        float WidthAdjust = Inventory_Prefab_Object.GetComponent<RectTransform>().rect.width;
        float DistanceBetween = Margin_For_Inventory_Slots + ((Screen.width- RightMargin- LeftMargin) / Number_Of_Slots)- WidthAdjust/2;


        for (int i = 0; i < Number_Of_Slots; i++)
        {
            GameObject HoldingSpot= Instantiate(Inventory_Prefab_Object,this.transform) as GameObject;
            HoldingSpot.transform.position = new Vector3(this.transform.position.x-(DistanceBetween*(i- Number_Of_Slots/2))-RightMargin+LeftMargin , this.transform.position.y, this.transform.position.z);

            Image holdImage;
            HoldingSpot.GetComponent<Inventory_Slot>().MyImage.sprite = Empty_Image;
            holdImage = HoldingSpot.transform.GetChild(0).GetComponent<Image>();
            holdImage.sprite = Frame;

            Inventory[i]=HoldingSpot;
        }
    }

    public bool AddInventory(Interactable ObjectInput)
    {
        if (Number_filled_slots < Number_Of_Slots)
        {

            Inventory[Number_filled_slots].GetComponent<Inventory_Slot>().ItemName = ObjectInput.name;
            Inventory[Number_filled_slots].GetComponent<Image>().sprite= ObjectInput.Display_Sprite;

            Inventory[Number_filled_slots].GetComponent<Inventory_Slot>().MyItem = ObjectInput.GetComponent<Items>();

            Debug.Log(Inventory[Number_filled_slots].GetComponent<Inventory_Slot>().ItemName);
            Number_filled_slots++;
            return true;
        }
        return false;
    }


    public void AddInventoryInView()
    {
        if (Number_filled_slots < Number_Of_Slots)
        {
            Inventory_Slot ThisSlot = Inventory[Number_filled_slots].GetComponent<Inventory_Slot>();
            ThisSlot.ItemName = ItemHold.name;
            ThisSlot.MyImage.sprite = ItemHold.Display_Sprite;
           ThisSlot.MyItem = ItemHold.GetComponent<Items>();
            ItemHold.GetComponent<Items>().pickup(FindObjectOfType<GameMananger>().Current_Room);
            
            Number_filled_slots++;
            
        }


        TurnOffObject.SetActive(false);
        FindObjectOfType<GameMananger>().ToggleCursor(false);
        if (InfoOut != null)
            InfoOut.text = "";

    }

    public bool CheckIfLockShouldOpen(string name)
    {
        int Return_value = GetLocationInArray(name);
       
        if (Return_value == -1 && Previously_Opened_Lock.IndexOf(name) == -1 && Previously_Solved_Puzzles.IndexOf(name) == -1)
        {
            return (KeyRing.IndexOf(name)!=-1);
        }
            return true;
    }


    public bool ItemIsInInventory(Items CheckingFor)
    {
        for (int i = 0; i < Number_filled_slots; i++)
        {
            if (Inventory[i].GetComponent<Inventory_Slot>().MyItem == CheckingFor)
                return true;
        }
        return false;
    }

    public int GetLocationInArray(string name)
    {
        for (int i=0;i< Number_filled_slots; i++)
        {
            if (Inventory[i].GetComponent<Inventory_Slot>().ItemName == name)
                return i;
        }
        return -1;
    }

    public void RemoveInventory(GameObject Me)
    {
       // Items ItemAdded = Me.GetComponent<Inventory_Slot>().MyItem;

       // ItemAdded.drop(FindObjectOfType<GameMananger>().Current_Room);


        RemoveInventoryByLocation(GetLocationInArray(Me.GetComponent<Inventory_Slot>().ItemName));

    }

    public bool RemoveInventoryByLocation(int WhichOne)
    {
       // Debug.Log(Number_filled_slots);


        if (Number_filled_slots > WhichOne)
        {
            //Debug.Log(Inventory[WhichOne].GetComponent<Inventory_Slot>().ItemName);
            for (int i = 1; i < Number_filled_slots; i++)
            {
                if (i > WhichOne)
                {
                    //Debug.Log(Inventory[i].GetComponent<Inventory_Slot>().MyItem.GetComponent<Item_Click>().Display_Sprite);
                    Inventory[i - 1].GetComponent<Inventory_Slot>().ItemName = Inventory[i].GetComponent<Inventory_Slot>().ItemName;
                    Inventory[i - 1].GetComponent<Inventory_Slot>().MyItem = Inventory[i].GetComponent<Inventory_Slot>().MyItem;
                    Inventory[i - 1].GetComponent<Inventory_Slot>().MyImage.sprite = Inventory[i].GetComponent<Inventory_Slot>().MyImage.sprite;
                }
            }
            Number_filled_slots--;
            Inventory[Number_filled_slots].GetComponent<Inventory_Slot>().MyImage.sprite   = Empty_Image;
            Inventory[Number_filled_slots].GetComponent<Inventory_Slot>().ItemName = "";
            Inventory[Number_filled_slots].GetComponent<Inventory_Slot>().MyItem = null;



            return true;
        }
        return false;
    }

   
    public void AddKey(string name)
    {
        
            KeyRing.Add(name);
            
    }

  
    public void RecordLock(string name)
    {

        Previously_Opened_Lock.Add(name);

    }
  public void RecordPuzzle(string name)
    {

        Previously_Solved_Puzzles.Add(name);

    }


    public bool Contains(string Name)
    {
        foreach(GameObject checking in Inventory)
        {
            if (checking.GetComponent<Inventory_Slot>().ItemName == ItemHold.name)
                return true;
        }
        return true;
    }

}

