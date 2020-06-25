using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting_mananger : MonoBehaviour {

    public GameObject CaptainsQ, Deck, Hall, Hold, MatesQ, SeaQ, Galley, Bilge;


    public GameMananger GM;

    // Use this for initialization
    

    public void LightingCheck()
    {

         bool BCaptainsQ, BDeck, BHall, BHold, BMatesQ, BSeaQ, BGalley, BBilge;

        BCaptainsQ = BDeck = BHall = BHold = BMatesQ = BSeaQ = BGalley = BBilge = false;

        switch (GM.Current_Room.type)
        {
            case RoomName.CaptainsQ:
                BCaptainsQ = true;
                break;
            case RoomName.Deck:
                BDeck = true;
                break;
            case RoomName.Hall:
                BHall = true;
                break;
            case RoomName.Hold:
                BHold = true;
                break;
            case RoomName.MatesQ:
                BMatesQ = true;
                break;
            case RoomName.SeaQ:
                BSeaQ = true;
                break;
            case RoomName.Galley:
                BGalley = true;
                break;
            case RoomName.Bilge:
                BBilge = true;
                break;
        }

        CaptainsQ.SetActive(BCaptainsQ);
        Deck.SetActive(BDeck);
        Hall.SetActive(BHall);
        Hold.SetActive(BHold);
        MatesQ.SetActive(BMatesQ);
        SeaQ.SetActive(BSeaQ);
        Galley.SetActive(BGalley);
        Bilge.SetActive(BBilge);
        

    }


    }
