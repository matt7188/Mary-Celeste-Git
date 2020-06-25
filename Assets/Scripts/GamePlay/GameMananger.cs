using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMananger : MonoBehaviour
{

    public bool Twist;


    [Header("Visual Elements")]
    public GameObject EscapeMenu;
    public RayCastFromCenter CenterImgae;
    public Text InfoOutput;
    public TextFx.TextFxTextMeshProUGUI RoomTitle;
    public Text LookingInfoOutput;

    public GameObject ClockMin;
    public GameObject ClockHour;

    [Header("Wold State")]
    public People[] Crew;
    public MurderProgression[] AllMurders;
    public Rooms[] RoomsOnShip;
    public GameObject ItemsMaster;
    public List<Items> ItemsOnShip;
    public Notebook Notes;
    public Cipher.CipherType cipherType;
    public static int cipherNum;
    public static char [] cipherLetters;


    [Header("Control State")]
    public int num_people;
    public bool death;
    Rooms[] before;
    public int Is_murder = 0;
   public People Murder;
    public MurderProgression CurrentMurderProgression;

    int[] NumInRoomLeft;

    int min;
    int hour;
    [Header("Current State")]
    public int lives = 3;
    bool [] clues_found;
    public People talk_to = null;

    public bool reset = false;

    public bool listen = false;

    public Rooms Current_Room;

    public string infoout = "";

    public bool Killable;

    int numberOfCluesFound;
    int numberOfCluesInExistance;
    List<string> CluesFound;

    cakeslice.Outline[] AllOutlines;


    public void Start()
    {
        FullGameMananger[] objs = GameObject.FindObjectsOfType<FullGameMananger>();

        if (objs.Length != 1)
        {
            GameObject.FindGameObjectWithTag("Black Out Transition").SetActive(false);
        }

        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

       // if (sceneName != "Captians Quarters")
        //{
        //    SceneManager.LoadScene("Captians Quarters", LoadSceneMode.Additive);
       // }


        //ToggleCursor();
        RayCastFromCenter.Clickable = true;
        BasicMove.Lock = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        AllOutlines = GameObject.FindObjectsOfType<cakeslice.Outline>();


        if (Twist)
        {

        }
        else
        {
            NumInRoomLeft = new int[RoomsOnShip.Length];

            ///////////////////////////People/////////////////////////////////////

            num_people = Crew.Length;


            for (int i = 0; i < num_people; i++)
            {
                //Person[i] = Select.RandPers(new int[0]);
                //Person[i].Here = Room[5];
            }

            do
                Is_murder = 2;//Random.Range(0,num_people - 1);
            while (Is_murder == 1);

            Crew[Is_murder].murder = true;
            Murder = Crew[Is_murder];
            CurrentMurderProgression = AllMurders[Is_murder];

            // MurderRoom = RoomsOnShip[4];//Random.Range(0, RoomsOnShip.Length)];
            clues_found = new bool[Murder.MurderNoObject.Length];
        

            for (int j = 0; j < num_people; j++)
            {
                
                if (Crew[j].Alabi == null)
                {
                    do
                    {
                        Crew[j].Alabi = RoomsOnShip[Random.Range(0, RoomsOnShip.Length)];
                    } while (Crew[j].Alabi.type != RoomName.MatesQ && Crew[j].Alabi.type != RoomName.SeaQ && Crew[j].Alabi.type != RoomName.Galley && Crew[j].Alabi.type != RoomName.Deck);
                }
            }


            ////////////////////////////Items/////////////////////////////

            cipherNum = Random.Range(1, 26);

            cipherLetters = Cipher.GenerateMono();
            

            ItemsOnShip = new List<Items>();

            foreach (Transform ItemsToKeep in ItemsMaster.transform)
            {
                Items MyItems = ItemsToKeep.GetComponent<Items>();
                if (MyItems.gettype == ItemType.Useful)
                {
                    ItemsOnShip.Add(MyItems);
                }
                else
                {
                    ItemsToKeep.position = this.transform.position;
                    switch (Is_murder)
                    {
                        case 0:
                            if (MyItems.Captian)
                                ItemsOnShip.Add(MyItems);
                            break;
                        case 1:
                            if (MyItems.Sophia)
                                ItemsOnShip.Add(MyItems);
                            break;
                        case 2:
                            if (MyItems.Albert)
                                ItemsOnShip.Add(MyItems);
                            break;
                        case 3:
                            if (MyItems.Andrew)
                                ItemsOnShip.Add(MyItems);
                            break;
                        case 4:
                            if (MyItems.Edward)
                                ItemsOnShip.Add(MyItems);
                            break;
                        case 5:
                            if (MyItems.Volkert)
                                ItemsOnShip.Add(MyItems);
                            break;
                        case 6:
                            if (MyItems.Boz)
                                ItemsOnShip.Add(MyItems);
                            break;
                        case 7:
                            if (MyItems.Arian)
                                ItemsOnShip.Add(MyItems);
                            break;
                        case 8:
                            if (MyItems.Gottlieb)
                                ItemsOnShip.Add(MyItems);
                            break;

                    }
                }
            }

            foreach (Items Check in ItemsOnShip)
            {
                if (!Check.held)
                {
                    if (Check.gettype == ItemType.Useful)
                    {
                       // Debug.Log(Check.name);
                    }
                    else
                    if (Check.Home != null)
                        Check.Home.MoveIntoPlace(Check);
                    else
                    {
                        int WhichOne;
                        do
                        {
                            WhichOne = Random.Range(0, RoomsOnShip.Length);
                        } while (!RoomsOnShip[WhichOne].CanPlace(Check.Size));

                        RoomsOnShip[WhichOne].MoveIntoPlace(Check);
                    }

                }
            }

            ////////////////////////////Starting State/////////////////////////////

           // GameObject.FindObjectOfType<Lighting_mananger>().GM = this;
           // GameObject.FindObjectOfType<Lighting_mananger>().LightingCheck();

            CluesFound = new List<string>();

            before = new Rooms[num_people];
            death = false;
            Notes.InitializeNotebook(Crew, RoomsOnShip);
            Notes.Rooms_checked[RoomIndex(Current_Room)] = true;

            RoomTitle.text = "Captians Quarters";

            for (int a = 0; a < RoomsOnShip.Length; a++)
            {
                NumInRoomLeft[a] = HowMenyInRoom(RoomsOnShip[a]);
            }

            hour = 9;
            min = 0;

            InfoOutput.text = "";

            Debug.Log("The murder is: " + Murder);

            //Debug.Log("The murder room is: " + MurderRoom);

            // Clock.text = TimeIs();
            numberOfCluesFound = 0;
            numberOfCluesInExistance = RoomsOnShip.Length + num_people + ItemsOnShip.Count + 2;


            ChangeHands();
            StartCoroutine(AfterSetup());


            foreach (cakeslice.Outline Switch in AllOutlines)
            {
                if (Switch.CurrentRoom == Current_Room)
                {
                    Switch.enabled = true;
                }
                else
                    Switch.enabled = false;

            }

            Killable = false;
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeMenu.SetActive(!EscapeMenu.activeSelf);
            ToggleCursor(EscapeMenu.activeSelf);
        }
        /*
        if (Input.GetKeyDown(KeyCode.K))
        {
            Killable = true;
        }

       if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("Twist Hallway", LoadSceneMode.Additive);
            for (int r = 0; r < SceneManager.sceneCount; r++)
                if (SceneManager.GetSceneAt(r).name != "Lighting")
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(r).name);
                }

            //FindObjectOfType<BasicMove>().transform.position = new Vector3(9.1f, 0.11f,-5f);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            SceneManager.LoadScene("Twist Hold", LoadSceneMode.Additive);
            for (int r = 0; r < SceneManager.sceneCount; r++)
                if (SceneManager.GetSceneAt(r).name != "Lighting")
                {
                    SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(r).name);
                }

            //FindObjectOfType<BasicMove>().transform.position = new Vector3(9.1f, 0.11f,-5f);
        }

        //RoomTitle.AnimationManager.PlayAnimation(1f, 0);
        */
    }


    ///////////////////////////////////////GAMEPLAY///////////////////////
    void LateUpdate()
    {

        if (LookingInfoOutput != null) { 
            LookingInfoOutput.text = "";

           GameObject Looking = CenterImgae.CurrentlyLookingAt();
            if (Looking != null)
            {
                Items ItemCheck = Looking.GetComponent<Items>();
                if (ItemCheck != null)
                {
                    LookingInfoOutput.text = ItemCheck.name;
                }
                else
                {
                    People PeopleCheck = Looking.GetComponent<People>();
                    if (PeopleCheck != null)
                    {
                        LookingInfoOutput.text = PeopleCheck.name;
                    }
                    else
                    {
                        DoorBehavior DoorCheck = Looking.GetComponent<DoorBehavior>();
                        if (DoorCheck != null)
                        {
                            LookingInfoOutput.text = DoorCheck.endingRoomCheck().RoomName();
                        }
                    }
                }
            }
        }
    }

        public void TimePass(int HowMuchTime)
    {

        if (Twist)
            return;




        for (int TimeCheck = 0; TimeCheck< HowMuchTime; TimeCheck++) {

            

            if (CurrentMurderProgression!=null)
            {
               foreach (MurderProgression.Step CheckFor in CurrentMurderProgression.Steps )
                {
                    if (CheckFor.TimeOrClues && CheckFor.TimeNeeded<= min+(hour*60)-540)
                    {
                        Debug.Log(CheckFor.TimeNeeded+ "<="+ (min + (hour * 60)-540));
                    }
                    if (!CheckFor.TimeOrClues && CheckFor.TimeNeeded <= numberOfCluesFound)
                    {
                        Debug.Log("hit clue");
                    }
                }
               

        ///////////////////////////////WORK HERE!????????????????


            }


            for (int a= 0; a< RoomsOnShip.Length;a++)
            {
                NumInRoomLeft[a] = HowMenyInRoom(RoomsOnShip[a]);
            }

            int num_items = 0;


            for (int j = 0; j < num_people; j++)
                before[j] = Crew[j].Here;

            min++;
            if (min > 59)
            {
                hour++;
                min = 0;
            }
            if (hour > 12)
                hour = 1;

            for (int j = 0; j < num_people; j++)
            {
                Crew[j].RecordPast();
                if (talk_to != Crew[j])
                {
                        Crew[j].Move();
                }
            }
            num_items = Crew[Is_murder].held.Length + Crew[Is_murder].Here.placed.Count;

            if (HowMenyInRoom(Murder.Here) == 2)
                foreach (People check in Crew)
                {
                    if (Killable&&Murder != check && Murder.Here == check.Here && death != true && check.alive) //&& num_itams != 0
                    {
                        if (check.Stress == 2)
                        {
                            List<Items> avalible = new List<Items>();
                            Debug.Log("MURDER " + check.MyName + Murder.Here.RoomName());
                            int Clue;
                            int antiLoop = 0;
                            do
                            {
                                Clue = Random.Range(0, clues_found.Length);
                                antiLoop++;
                            }
                            while (clues_found[Clue] && antiLoop < 20);



                            for (int j = 0; j < Murder.held.Length; j++)
                                if (Murder.held[j].gettype == ItemType.Killing)
                                    avalible.Add(Murder.held[j]);

                            for (int j = 0; j < Murder.Here.placed.Count; j++)
                                if (Murder.Here.placed[j].gettype == ItemType.Killing)
                                    avalible.Add(Murder.Here.placed[j]);


                            if (avalible.Count != 0)
                                check.Kill(avalible[Random.Range(0, num_items)]);
                            else
                                check.Kill(Murder);

                            Instantiate(FindObjectOfType<Blood_Objects>().BloodPrefabs[Random.Range(0, 7)], new Vector3(check.transform.position.x, check.transform.position.y - .259f, check.transform.position.z), Quaternion.identity);

                            Crew[Is_murder].Move();
                            Crew[Is_murder].Move();
                            Crew[Is_murder].Move();
                            Crew[Is_murder].Move();

                            before[Is_murder] = null;
                        }
                        else {
                            check.Stress++;
                            Debug.Log("Attack " + check.MyName + Murder.Here.RoomName()+ check.Stress);
                            
                                }
                    }
                }


            
        }
        death = false;

        ChangeHands();

        if (NumberAlive() == 0)
            Debug.Log("YOU DIED");

    }
    public string TimeIs() { return string.Concat(hour.ToString(), " : ", min.ToString().PadLeft(2, '0')); }
    public string List_People()
    {
        bool first = true, any = false, more = false;
        string r = "\nYou see ";
        string hold = "";
        for (int i = 0; i < num_people; i++)
            if (Crew[i].Here == Current_Room && Crew[i].alive == true)
                if (first == true)
                {
                    any = true;
                    first = false;
                    hold = Crew[i].name;
                }
                else
                {
                    more = true;
                    r += hold + ", ";
                    hold = Crew[i].name;
                }

        if (any == false)
            return "";
        if (more == true)
            r += " and ";
        r += hold;

        r += " in the room with you.";
        return r;
    }
    public int RoomIndex(Rooms IN)
    {
        int i = 0;
        while (i < 12 && IN != RoomsOnShip[i]) { i++; }
        return i;
    }
    public int PeopleIndex(People IN)
    {
        int i = 0;
        while (i < num_people && IN != Crew[i]) { i++; }
        return i;
    }
    private string Attacked()
    {
        string Out = "The room suddenly went dark. What apears to be a masked figure approches you.";


        switch (lives)
        {
            case 3:
                Out += " Fortunatly, you scream out before they got closed enough to do what you can only fear is the worst. Fearing discovery, they run from the room, leaving you behind. While you survived this time, you are not sure If you will be so lucky next time.";
                break;
            case 2:
                Out += " Before you can scream, they cover your mouth with some sort of rag. Out of desperation you swing your fist at where you think there face is. They duck out of the way, but loosen there grip just enough for you to push them away. Taking advantage of this temporary withdrawal you rush to the power switch on the wall. Once the lights come on, you turn, but the assailant is no longer in the room. You have a feeling you wont survive the next incident.";
                break;
            case 1:
                int num_alive = 0;
                for (int i = 0; i < num_people; i++)
                {
                    if (Crew[i].alive == true)
                        num_alive++;
                }
                //var death = new EndGame(("The room sudenly goes dark. as you turn around, you are tackled, hands fastend around your throat. You strugle to the best of your ability, but your vision slowly dims, until finally, with a last heave, you succumb to death\n" + Person[Is_murder].facts.name + " stands up from your body, taking the key from your pocket, and placing the gun from the dining room in your hand." +
                //    "\n\n  Number of Survivors: " + num_alive + "\n  Clues found: " + clues_found + "\n  You did not survive\n  The Murderer was:" + Person[Is_murder].facts.name), 2);
               // death.ShowDialog();
                break;

        }
        lives--;

        return Out;
    }
    public void Pickup(People Person)
    {
        //int Action = Random.Range(0, 1000);
        /*
        if (Person.held.Length != 0)
        {
            if (((Action * Person.facts.strength) / (Person.held.Length)) <= 100)
            {
                Person.DropItam(Person.Here.placed[);
                Person.Here.placed
            }
        }*/
    }

    public void ToggleCursor(bool Set)
    {
        CenterImgae.ShowingImage = !Set;

        RayCastFromCenter.Clickable = !Set;
        RayCastFromCenter.right = false;
        BasicMove.Lock = Set;
        Cursor.visible = Set;
        ToggleCursorCaption(!Set);



    }

    public void ToggleCursorCaption(bool Set)
    {
     GameObject grandChild;
        grandChild = CenterImgae.transform.GetChild(0).gameObject;
       grandChild.SetActive(Set);
        
    }


    public int HowMenyInRoom( Rooms WhichRoom)
    {
        int Output = 0;
        if (Current_Room == WhichRoom)
            Output++;
        foreach (People check in Crew)
        {
            if (WhichRoom == check.Here && check.alive)
                Output++;


        }
        return Output;
    }

    public Vector3 PlaceInRoom(Rooms whichRoom)
    {
        int hold = RoomIndex(whichRoom);

        NumInRoomLeft[hold]--;
        switch (whichRoom.type)
        {

            case RoomName.CaptainsQ:
                return new Vector3(whichRoom.transform.position.x + NumInRoomLeft[hold] - HowMenyInRoom(whichRoom)+3, whichRoom.transform.position.y+.5f, whichRoom.transform.position.z + 2);

            case RoomName.Hall:
            case RoomName.SeaQ:
            case RoomName.Galley:
            
            case RoomName.Hold:
                return new Vector3(whichRoom.transform.position.x + NumInRoomLeft[hold]- HowMenyInRoom(whichRoom)+2, whichRoom.transform.position.y + .3f, whichRoom.transform.position.z + 4);

            case RoomName.MatesQ:
                return new Vector3(whichRoom.transform.position.x + NumInRoomLeft[hold] - HowMenyInRoom(whichRoom), whichRoom.transform.position.y + .3f, whichRoom.transform.position.z - 3);

            case RoomName.Bilge:
                return new Vector3(whichRoom.transform.position.x +4, whichRoom.transform.position.y+1, whichRoom.transform.position.z + NumInRoomLeft[hold] - HowMenyInRoom(whichRoom));
            case RoomName.Deck:
                return new Vector3(whichRoom.transform.position.x - 4, whichRoom.transform.position.y + .5f, whichRoom.transform.position.z + NumInRoomLeft[hold] - HowMenyInRoom(whichRoom));

        }


        return new Vector3(0, 0, 0);

    }
    public void InfoCheck(string ExtraInfo)
    {
        string Output="";

        bool checkDead = false;
        int PeopleInRoom = 0;

        foreach (People dead in Crew)
        {
            if (dead.Here== Current_Room)
            {
                PeopleInRoom++;
                if (dead.alive == false)
                {
                    checkDead = true;
                }
            }
        }

        if (checkDead)
        {
            Output += "There is evidence of a dead body here. ";
            Notes.Rooms_checked[RoomIndex(Current_Room)] = true;
        }



        


        InfoOutput.text = Output;
    }
    IEnumerator AfterSetup()
    {
        yield return new WaitForFixedUpdate();

        foreach(People move in Crew)
            move.transform.position = this.PlaceInRoom(move.Here);


        foreach(Items Check in ItemsOnShip)

        foreach (cakeslice.Outline OultineCheck in Check.GetComponent<Item_Click>().OutlineHold)
            OultineCheck.CurrentRoom = Check.CurrentlyIn;


        foreach (cakeslice.Outline Switch in AllOutlines)
        {
            if (Switch != null)
            {

                if (Switch.CurrentRoom == Current_Room)
                {
                    Switch.enabled = true;
                }
                else
                    Switch.enabled = false;
            }
        }


    }
    public string AssignAlabis(People Setting)
    {
        string output = "";

        bool WithPerson = false;
        foreach (People Compare in Crew)
        {

            if (Compare != Setting && Setting.Alabi == Compare.Alabi && !Compare.murder)
            {
                if (!WithPerson)
                    output += "";
                else
                    output += ",9";

                output += " " + Compare.MyName;

                WithPerson = true;
            }
        }
        string[] Splitup = output.Split('9');
        output = "";

        for (int i = 0; i < Splitup.Length; i++)
        {
            output += Splitup[i];
            if (i + 2 == Splitup.Length)
                output += " and";
        }


        return output;
    }

    public void AddClue(string Name, int value=1)
    {
        if (!CluesFound.Contains(Name))
        {
            CluesFound.Add(Name);
            numberOfCluesFound+=value;
           
        }
    }

    public void ChangeHands()
    {

        ClockMin.transform.eulerAngles = new Vector3(0, 0, min*-6);
        ClockHour.transform.eulerAngles = new Vector3(0, 0, 45f+((hour-9)*-30f)+( min* -(30f/60f)));
    }
    public string CluesFoundOutput() {
        return numberOfCluesFound+"/"
        + numberOfCluesInExistance;
    }
    public int NumberAlive()
    {
        int Out = 0;
        foreach(People check in Crew)
            if (check.alive&& !check.murder)
            {
                Out++;
            }
        return Out;
    }

    public void ChangeRoom(Rooms NewRoom)
    {

        foreach (cakeslice.Outline Switch in AllOutlines)
        {
            if (Switch != null) { 

            if (Switch.CurrentRoom == NewRoom)
            {
                Switch.enabled = true;
            }
            else
                Switch.enabled = false;
            }
        }

        Current_Room = NewRoom;
        RoomTitle.text = NewRoom.RoomName();
        RoomTitle.AnimationManager.PlayAnimation(1f, 0);
    }
}

