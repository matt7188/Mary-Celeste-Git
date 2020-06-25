using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Clue, Useful, Killing, Environment }
public class Items : MonoBehaviour {

    public GameObject ItemCam;
    public ItemType gettype;
    public new string name;
    public bool used;
    public bool held;
    public People heldby;
    public Rooms CurrentlyIn;

    public string description;
    public string InjuryDescription;
    public string UsedDescription;
    public bool Seen=false;
    public Rooms Home;
    public int Size;
    public int Value=0;

    public bool Captian;
    public bool Sophia;
    public bool Albert;
    public bool Andrew;
    public bool Edward;
    public bool Volkert;
    public bool Boz;
    public bool Arian;
    public bool Gottlieb;


    public void DescriptionCheck()
    {
        
            if (description.Contains("*"))
            {

                string[] Splitup = description.Split('*');

                string CipherSet = "";

                switch (FindObjectOfType<GameMananger>().cipherType)
                {
                    case Cipher.CipherType.Mono:
                        CipherSet = Cipher.Mono(Splitup[1], GameMananger.cipherLetters);
                        break;
                    case Cipher.CipherType.Poly:
                        CipherSet = Cipher.Poly(Splitup[1], GameMananger.cipherLetters);
                        break;
                    case Cipher.CipherType.Shift:
                        CipherSet = Cipher.Offset(Splitup[1], GameMananger.cipherNum);
                        break;
                }

                description = Splitup[0] + CipherSet + Splitup[2];
            if (!Seen)
        {
            Seen = true;
                Notebook.CiphersStored += CipherSet + "\n";
            }

        }
    }

    void Update()
    {
        //if (used)
          //  this.gameObject.transform.position = new Vector3(Mathf.Sin(Time.time * 5), transform.position.y, transform.position.z);

    }

    public void pickup(Rooms CurrentRoom)
    {
        CurrentRoom.RemoveItem(this);
        CurrentlyIn = null;
        held = true;
       // this.GetComponent<Renderer>().enabled = false;
        this.transform.position = new Vector3(-1000, -1000, -1000);
    }
    
    public bool drop(Rooms Newlocation)
    {
        if (Newlocation.MoveIntoPlace(this))
        {
            held = false;
            //this.GetComponent<Renderer>().enabled = true;
            CurrentlyIn = Newlocation;
            return true;
        }
        return false;
        
    }

}
