using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diary : MonoBehaviour
{
    public Texture[] Pages;
    public Material[] PagesMat;

    public Vector3[] Slots;

    public SafePuzzleMananger SafeAnswer;

    public Book diaryChange;


    // Start is called before the first frame update
    void Start()
    {
         int WhichOne= Random.Range(0, Slots.Length);

        SafeAnswer.AllTumblers[2].FinalNumber = Slots[WhichOne].x;
        SafeAnswer.AllTumblers[0].FinalNumber = Slots[WhichOne].y;
        SafeAnswer.AllTumblers[1].FinalNumber = Slots[WhichOne].z;

        diaryChange.AlternatePage = Pages[WhichOne];
        diaryChange.AlternatePageMat = PagesMat[WhichOne];

    }
    
}
