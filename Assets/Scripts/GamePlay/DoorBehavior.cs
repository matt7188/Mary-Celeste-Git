using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class DoorBehavior : Interactable
{

    DoorTransition CalledTransition;

   // Rooms StartingRoom;
    Rooms EndingRoom;

    Rooms[] allRooms; 
    void Start()
    {
        Type_of_Interaction = TypeOfInteraction.Door;
        Inventory_Mananger = GameObject.FindObjectOfType<Inventory_Managment>();
        Game_Mananger = GameObject.FindObjectOfType<AdventureGameMananger>();
        allRooms = FindObjectsOfType<Rooms>();
        CalledTransition = FindObjectOfType<DoorTransition>();


        /*switch (Scene_Unload)
        {
            case "Captians Quarters":
                StartingRoom = allRooms[FindRoom(global::Name.CaptainsQ)];
                break;
            case "Hallway":
                StartingRoom = allRooms[FindRoom(global::Name.Hall)];
                break;
            case "Deck":
                StartingRoom = allRooms[FindRoom(global::Name.Deck)];
                break;
            case "Galley":
                StartingRoom = allRooms[FindRoom(global::Name.Galley)];
                break;
            case "Bilge":
                StartingRoom = allRooms[FindRoom(global::Name.Bilge)];
                break;
            case "Hold":
                StartingRoom = allRooms[FindRoom(global::Name.Hold)];
                break;
            case "Mates Quarters":
                StartingRoom = allRooms[FindRoom(global::Name.MatesQ)];
                break;
            case "Seamen Quarters":
                StartingRoom = allRooms[FindRoom(global::Name.SeaQ)];
                break;

        }*/

        switch (Scene_Load)
        {
            case "Captians Quarters":
                EndingRoom = allRooms[FindRoom(global::RoomName.CaptainsQ)];
                break;
            case "Hallway":
                EndingRoom = allRooms[FindRoom(global::RoomName.Hall)];
                break;
            case "Deck":
                EndingRoom = allRooms[FindRoom(global::RoomName.Deck)];
                break;
            case "Galley":
                EndingRoom = allRooms[FindRoom(global::RoomName.Galley)];
                break;
            case "Bilge":
                EndingRoom = allRooms[FindRoom(global::RoomName.Bilge)];
                break;
            case "Hold":
                EndingRoom = allRooms[FindRoom(global::RoomName.Hold)];
                break;
            case "Mates Quarters":
                EndingRoom = allRooms[FindRoom(global::RoomName.MatesQ)];
                break;
            case "Seamen Quarters":
                EndingRoom = allRooms[FindRoom(global::RoomName.SeaQ)];
                break;

        }

        
    }

    int FindRoom(RoomName Lookingfor)
    {
        for (int i=0;i< allRooms.Length;i++)
            if (allRooms[i].type== Lookingfor)
            {
                //Debug.Log(i);
                return i;
            }
        return -1;
    }

    protected override void OpenDoor()
    {
        GameMananger Gm = FindObjectOfType<GameMananger>();

       // GameObject Player=GameObject.FindGameObjectsWithTag("Player")[0];

        CalledTransition.TransitionOpen(this.gameObject, Scene_Load, Scene_Unload,  EndingRoom);

        // Debug.Log(EndingRoom.name);


        Gm.Notes.Rooms_checked[Gm.RoomIndex(EndingRoom)] = true;

        Gm.ChangeRoom(EndingRoom);
        Gm.TimePass(1);
        Gm.InfoCheck("");
    }

    public Rooms endingRoomCheck()
    { return EndingRoom; }


    }


