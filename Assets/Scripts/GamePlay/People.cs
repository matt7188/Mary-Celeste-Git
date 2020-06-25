using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class People : MonoBehaviour {
    

    public string MyName;
    public string description;
    public GameObject MyPicture;
    public Font myHandwriting;

    public bool murder; // true= they are the killer
    private int OnPath;
    public bool alive; // alive or dead
    public Rooms Here; // where they are
    int Rest;// how long between moving
    public List<Command> MyNextCommand;

    public Rooms Alabi;
    public string AlabiOutput;
    public Rooms[] Route;
    public Rooms[] AllRooms;
   
    public string[] MurderNoObject; // What the killer does when no items are avalible
    public string [] Diologue;
    public Items[] held; // itams posesed by the person. strength used to determin if can be picked up
                         //Is Visiable for Diologue;
                         
    public bool drop;

    GameMananger GM;

    public int Stress;

    public int Trust;

    public List<RoomName> WhereIHaveBeen;

    public List<RoomName> SignificantLocations; //Start of game, each murder, at time of accusation

    public Opinions MyOpinion;

    public int MaxQueryLimit;
    public int CurrentQueryLimit;
    public DialogueNode DismissalNode;



    public void Start()
    {
        ResetQueryLimit();

        /* clue = new string[Everyone.numberOfClues];
         for (int i = 0; i < Everyone.numberOfClues; i++)
             clue[i] = Everyone.PopulateClues(MyName, i);

        MyNextCommand
        MyNextCommand = new Command();
        MyNextCommand.GoingTo = RoomName.Person;
        MyNextCommand.Persuing = null;
        MyNextCommand.TryingToFind = null;
        MyNextCommand.DropingItem = RoomName.Person;
 */

        MyNextCommand = new List<Command>();
        murder = false;
        alive = true;
        Rest = 0;
        OnPath = 0;

        gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        GM = FindObjectOfType<GameMananger>();

        foreach (Items pickup in held)
        {
            pickup.pickup(Here);

        }
        Stress = 0;
        RecordPast();


        if (Alabi == null)
        {
            Alabi = Here;
        }

        SignificantLocations.Add(Alabi.type);

        MyOpinion = GetComponent<Opinions>();

    }

    public void RecordPast()
    {
        WhereIHaveBeen.Add(Here.type);
    }
    public void Move() // move around
    {
        int hold = -1;

        if (!alive)
            return;

        if (murder)
        {
            if (Random.Range(0,30) < (Rest))
            {
                Rest = 10;

                hold = Random.Range(0, Here.Door.Length);

                Here = Here.Door[hold];

            }

        }

        else
        {

            if (MyNextCommand.Count != 0)
            {
                Debug.Log(MyName);
                if (MyNextCommand[0].DropingItem != RoomName.Person)
                {
                    if (MyNextCommand[0].DropingItem != Here.type)
                        MyNextCommand[0].GoingTo = MyNextCommand[0].DropingItem;
                    else
                        drop = true;
                }
                else
                if (MyNextCommand[0].ItemPickupOrDrop != null)
                {
                    if (MyNextCommand[0].ItemPickupOrDrop.held)
                    {
                        MyNextCommand[0].Persuing = MyNextCommand[0].ItemPickupOrDrop.heldby;
                    }
                    else
                        MyNextCommand[0].GoingTo = MyNextCommand[0].ItemPickupOrDrop.CurrentlyIn.type;

                }
                if (MyNextCommand[0].Persuing != null)
                {
                    MyNextCommand[0].GoingTo = MyNextCommand[0].Persuing.Here.type;
                }

                if (MyNextCommand[0].GoingTo != RoomName.Person)
                {
                    if (MyNextCommand[0].GoingTo == this.Here.type)
                    {
                        if (MyNextCommand[0].DropingItem != RoomName.Person)
                        {
                            foreach (Items check in held)
                            {
                                if (check == MyNextCommand[0].ItemPickupOrDrop)
                                    check.drop(Here);
                            }
                        }
                        else
                        if (MyNextCommand[0].ItemPickupOrDrop.CurrentlyIn == this.Here)
                        {
                            MyNextCommand[0].ItemPickupOrDrop.pickup(Here);
                        }
                        else


                            MyNextCommand.Remove(MyNextCommand[0]);
                    }

                    bool Moved = false;
                    foreach (Rooms DoorCheck in Here.Door)
                    {
                        if (DoorCheck.type == MyNextCommand[0].GoingTo)
                        {
                            Here = DoorCheck;
                            Moved = true;
                            MyNextCommand[0].GoingTo = RoomName.Person;
                        }
                    }
                    if (!Moved)
                    {
                        Here = Here.Door[0];
                    }
                }
            }
            else
            if (Random.Range(0, 30) < (Rest) && alive == true)
            {
                Rest = 0;
                //Console.WriteLine(facts.name);
                if (Route[OnPath] != null)
                    Here = Route[OnPath];
                else
                    Here = AllRooms[Random.Range(0, AllRooms.Length - 1)];

                OnPath++;
                if (OnPath >= Route.Length)
                    OnPath = 0;

            }
            
        }


        if(drop)
        {
            DropItem(0);
        }

        transform.position = GM.PlaceInRoom(Here);


        Rest++;
        
    }

    public void AddItem(Items IN)
    {
        Items[] hold = new Items[held.Length + 1];

        for (int i = 0; i < held.Length - 1; i++)
            hold[i] = held[i];

        hold[held.Length] = IN;
        held = hold;

    }
    public void DropItem(int i)
    {

        drop = false;
        if (held.Length != 0)
        {
            //Debug.Log(held[i].name);
            if (held[i].drop(Here))
            {

               // Items OUT = held[i];
                Items[] hold = new Items[held.Length-1];
                int k = 0;


                for (int j = 0; j < held.Length; j++)
                {
                    if (j != i)
                    {
                        hold[k] = held[j];
                        k++;
                    }
                }
                held = hold;

            }
        }
    }

    public void Kill(People Killer) // die
    {
        alive = false;
        

        description = Killer.MurderNoObject[Random.Range(0, Killer.MurderNoObject.Length)];
        

    }
    public void Kill(Items killed) // die
    {
        alive = false;

        killed.used=true;

        description = killed.InjuryDescription;
    }

    public void ResetQueryLimit()
    {
        CurrentQueryLimit = MaxQueryLimit;
    }
}

