using UnityEngine;
using System.Collections;

/// <summary>
/// A basic controller showing how to use the PageCurler system.
/// You should replace this component with your own control logic.
/// </summary>
public class DemoController : MonoBehaviour 
{
    public bool automatic = true;   //in automatic mode we'll play a looping page turning animation
    bool canFlip = true;    //in manual mode, we only listen to events when we can flip the page

    PageCurl curler;    //we store here the reference to the PageCurl component

    void Awake()
    {
        curler = GetComponent<PageCurl>();

        if (automatic)
        {
            //Start the first page flip
            curler.Flip();
        }
    }
	
	void Update () 
    {
        if (automatic)
        {
            //In automatic mode, we pause and resume the animation when the user presses the space key
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Pause and resume by changing the animation speed
                if (GetComponent<Animation>().isPlaying)
                    GetComponent<Animation>()["PageFlip"].speed = GetComponent<Animation>()["PageFlip"].speed <= 0f ? 1f : 0f;
            }
        }
        else
        {
            //In manual mode, we flip the page when the user presses the space key
            if (canFlip && Input.GetKeyDown(KeyCode.Space))
            {
                curler.Flip();  //You can also call Flip(true) to flip the page backwards
                canFlip = false;
            }
        }
	}

    //Animation event called when the page flip motion has ended
    public void OnPageFlip()
    {
        if (automatic)
            curler.Flip();  //we restart the animation again
        else
            canFlip = true; //we can now listen to the user again
    }
}
