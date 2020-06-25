using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeSafe : MonoBehaviour
{
    public SafePuzzle ImLookingAt;

    float MyAnswer;

    // Start is called before the first frame update
    void Start()
    {
        MyAnswer = Random.Range(1, 100);
        transform.localEulerAngles = new Vector3(180 ,180+(MyAnswer * 360)/ 100, 3.945999f);
        ImLookingAt.FinalNumber = MyAnswer;
    }

    // Update is called once per frame
    void Update()
    {
       /* float AdjustValue = (ImLookingAt.transform.rotation.z) * 180;
        AdjustValue += ((MyAnswer * 360) / 100);
        if (AdjustValue < 0)
            AdjustValue += 360;

        AdjustValue = (ImLookingAt.transform.rotation.z)*180;
        Debug.Log(AdjustValue);
        // transform.localEulerAngles = new Vector3(180, AdjustValue , 3.945999f);
        */
        transform.Rotate(0, ImLookingAt.SendTolargeSafe,0 );
    }
}
