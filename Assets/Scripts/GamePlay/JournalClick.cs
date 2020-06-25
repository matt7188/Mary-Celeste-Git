using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalClick : MonoBehaviour {

    public enum WhatToSay { Help, Where, Cipher}

    public WhatToSay whichPage;
    public Book_Flip Turning;

    void OnMouseDown()
    {
        Turning.TurnPage(whichPage);
    }
}
