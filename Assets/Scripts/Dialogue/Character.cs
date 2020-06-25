using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "DialogueSystem/Character", order = 1)]
public class Character : ScriptableObject
{
    //public enum Room
    //{
    //    Deck, CaptainsQuarters, Hallway, Galley, Bilge,
    //    SeamenQuarters, Hold, MatesQuarters, TwistHallway, SafeRoom
    //}
    public RoomName CurrentRoom;
    public int Trust;
    public int Fear;

    //public Room tenMinsAgoLocation;
    //public Room hourAgoLocation;
    //public Room firstMurderLocation;
    //public Room startLocation;
}
