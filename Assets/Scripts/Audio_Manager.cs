using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour {

    public AudioSource music;
    public float Musiclevels;

    public AudioSource Ambiance;
    public float AmbianceLevels;

    public AudioSource[] RandomNoises;

    public AudioClip CaptainsQ, Deck, Hall, Hold, MatesQ, SeaQ, Galley, Bilge;

    void Start () {
        music.Play();
        music.volume = Musiclevels;
        Ambiance.Play();
        Ambiance.volume = AmbianceLevels;
        StartCoroutine(RandomSounds());

    }
	
	public void SwitchAmbiance(Rooms CurrentRoom)
    {
        AudioClip hold=null;

        switch (CurrentRoom.type)
        {
            case RoomName.Bilge:
                hold = Bilge;
                break;
            case RoomName.CaptainsQ:
                hold = CaptainsQ;
                break;
            case RoomName.Deck:
                hold = Deck;
                break;
            case RoomName.Galley:
                hold = Galley;
                break;
            case RoomName.Hall:
                hold = Hall;
                break;
            case RoomName.Hold:
                hold = Hold;
                break;
            case RoomName.MatesQ:
                hold = MatesQ;
                break;
            case RoomName.SeaQ:
                hold = SeaQ;
                break;

        }
        if (hold!=Ambiance.clip)
        {
            Ambiance.clip = hold;
            Ambiance.Play();

        }

    }

    IEnumerator RandomSounds()
    {

        while (true)
        {
            int CurrentNoise;

            do
            {
                CurrentNoise = Random.Range(0, RandomNoises.Length);
                yield return new WaitForSeconds(.1f);
            }
            while (RandomNoises[CurrentNoise].isPlaying);

            RandomNoises[CurrentNoise].Play();

            yield return new WaitForSeconds(Random.Range(1f,3.5f));
        }

    }

 }
