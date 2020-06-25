using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeSafeDoor : MonoBehaviour
{
    public SafePuzzleMananger Checking;

   public  GameObject Door;

    // Update is called once per frame
    void Update()
    {
       if (Checking.Open)
        {
            Door.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
