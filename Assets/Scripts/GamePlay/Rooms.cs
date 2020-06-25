 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum RoomName { CaptainsQ, Deck, Hall, Hold, MatesQ, SeaQ, Galley, Bilge, Person};

public class Rooms : MonoBehaviour {
    
        public bool Active;

        public RoomName type;
        public List<Items> placed;
        public LocationsForItems[] ItemLocations;
        public Rooms[] Door;
        public string description;

    GameMananger GM;


    private void Start()
    {
    }

    public bool OceanLocation()
    {
        switch (type)
        {
            case global::RoomName.Bilge:
                return false;
            case global::RoomName.CaptainsQ:
                return true;
            case global::RoomName.Deck:
                return true;
            case global::RoomName.Galley:
                return true;
            case global::RoomName.Hall:
                return false;
            case global::RoomName.Hold:
                return false;
            case global::RoomName.MatesQ:
                return true;
            case global::RoomName.SeaQ:
                return true;
        }

        return true;
    }

    public bool MoveIntoPlace(Items Place)
    {
        bool assigned=false;

        if (placed.Count < ItemLocations.Length && CanPlace(Place.Size))
        {
            placed.Add(Place);

            for (int i = Place.Size; i <= 5; i++)
            {
                foreach (LocationsForItems check in ItemLocations)
                {
                    if (check.AssignedItem == null && check.size == i)
                    {
                        check.AssignedItem = Place;
                        Place.transform.position = check.transform.position;
                        assigned = true;
                        Place.CurrentlyIn = this;

                        if (GM==null)
                            GM = GameObject.FindObjectOfType<GameMananger>();



                        foreach (cakeslice.Outline OultineCheck in Place.GetComponent<Item_Click>().OutlineHold)
                        {
                            OultineCheck.CurrentRoom = Place.CurrentlyIn;
                            if (GM.Current_Room == Place.CurrentlyIn)
                                OultineCheck.enabled = true;
                            else
                                OultineCheck.enabled = false;

                        }

                        break;
                    }
                }


                if (assigned)
                    break;
            }
        }
        return assigned;

    }

    public void RemoveItem(Items Removing)
    {
        foreach (LocationsForItems check in ItemLocations)
        {
            if (check.AssignedItem == Removing)
            {
                check.AssignedItem = null;
            }
        }
        placed.Remove(Removing);
    }

    public bool CanPlace(int SizeRequest)
    {
        foreach (LocationsForItems Check in ItemLocations)
            if (Check.size >= SizeRequest)
                return true;
        return false;
    }

    public string RoomName()
    {
        switch(type)
        {
            case global::RoomName.CaptainsQ:
                return "Captians Quarters";
            case global::RoomName.MatesQ:
                return "First and Second Mates Quarters";
            case global::RoomName.SeaQ:
                return "General Seamens Quarters";
        }
            return type.ToString();
    }
}
