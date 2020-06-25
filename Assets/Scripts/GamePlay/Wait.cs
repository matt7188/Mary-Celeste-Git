using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : MonoBehaviour {

    public GameMananger Game;

    public void OnClick()
    {
        Game.TimePass(1);
    }
    }
