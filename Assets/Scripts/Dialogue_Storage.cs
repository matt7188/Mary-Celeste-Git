using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Storage : MonoBehaviour {

    public string [] CaptianWords;
   
    public string[] SophiaWords;
    
    public string[] AlbertWords;
   
    public string[] AndrewWords;
   
    public string[] EdwardWords;
    
    public string[] VolkertWords;
    
    public string[] BozWords;
    public string[] ArianWords;
    public string[] GottliebWords;
     public People Captian;public People Sophia; public People Albert; public People Andrew;
    public People Edward;public People Volkert;
    public People Boz;
    public People Arian;public People Gottlieb;

    People MurderRef;


    public int largestLength()
    {
        int[] AllLength = { CaptianWords.Length, SophiaWords.Length, AlbertWords.Length, AndrewWords.Length, EdwardWords.Length, VolkertWords.Length, BozWords.Length, ArianWords.Length, GottliebWords.Length };

        int hold = 2;

        for (int i = 0; i < 9; i++)
            if (AllLength[i] > hold)
                hold = AllLength[i];
        return hold;
    }


        public void CheckMurdersForFilter(People Murder)
    {
        MurderRef = Murder;

            FilterWordsAtStart(CaptianWords, (Murder.MyName!= "Captian Benjamin Spooner Briggs"));
       
            FilterWordsAtStart(SophiaWords, (Murder.MyName != "Sophia Matilda Briggs"));
        
            FilterWordsAtStart(AlbertWords, (Murder.MyName != "Albert G. Richardson"));
      
            FilterWordsAtStart(AndrewWords, (Murder.MyName != "Andrew Gilling"));
       
            FilterWordsAtStart(EdwardWords, (Murder.MyName != "Edward William Head"));

            FilterWordsAtStart(VolkertWords, (Murder.MyName != "Volkert Lorenzen"));
       
            FilterWordsAtStart(BozWords, (Murder.MyName != "Boz Lorenzen"));
        
            FilterWordsAtStart(ArianWords, (Murder.MyName != "Arian Martens"));
        
            FilterWordsAtStart(GottliebWords, (Murder.MyName != "Gottlieb Goodschaad"));

            FilterWordsWithRefrence(CaptianWords, Captian);
            FilterWordsWithRefrence(SophiaWords, Sophia);
        FilterWordsWithRefrence(AlbertWords, Albert);
        FilterWordsWithRefrence(AndrewWords, Andrew);
        FilterWordsWithRefrence(EdwardWords, Edward);
        FilterWordsWithRefrence(VolkertWords, Volkert);
        FilterWordsWithRefrence(BozWords, Boz);
        FilterWordsWithRefrence(ArianWords, Arian);
        FilterWordsWithRefrence(GottliebWords, Gottlieb);


        Captian.Diologue= CaptianWords;
    
    Sophia.Diologue = SophiaWords;
    Albert.Diologue = AlbertWords;
   Andrew.Diologue = AndrewWords;
        
    Edward.Diologue = EdwardWords;
        
    Volkert.Diologue = VolkertWords;
       
     Boz.Diologue =  BozWords;
      
    Arian.Diologue =   ArianWords;
       
    Gottlieb.Diologue =  GottliebWords;

    }

    void FilterWordsAtStart(string [] Checking, bool Murder)
    {
        for (int i = 0; i < Checking.Length; i++)
        {
            if (Checking[i].Contains("*"))
            {
                string[] Splitup = Checking[i].Split('*');
                Checking[i] = Splitup[0] + Cipher.Offset(Splitup[1], GameMananger.cipherNum) + Splitup[2];
            }

            if (Checking[i].Contains("1"))
            {
                string[] Splitup = Checking[i].Split('1');
                Checking[i] = Splitup[0] + MurderRef.MyName + Splitup[1];
            }

           
        }

    }



    void FilterWordsWithRefrence(string[] Checking, People personRefrence)
    {
        for (int i = 0; i < Checking.Length; i++)
        {
            if (Checking[i].Contains("2"))
            {
                string[] Splitup = Checking[i].Split('2');
                Checking[i] = Splitup[0] + personRefrence.Alabi.RoomName() + Splitup[1];
            }
        }

        for (int i = 0; i < Checking.Length; i++)
        {
            if (Checking[i].Contains("3"))
            {
                string[] Splitup = Checking[i].Split('3');
                Checking[i] = Splitup[0] + FindObjectOfType<GameMananger>().AssignAlabis(personRefrence) + Splitup[1];
            }
        }
    }

    }
