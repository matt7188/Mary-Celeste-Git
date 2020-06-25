using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkThroughSceneChange : MonoBehaviour
{
    public enum TwistScenes {Hall, Safe, Hold }

    public TwistScenes MovingTo;
    public TwistScenes[] Removing;

    public void SwitchScenesPostDoor()
    {
        switch(MovingTo)
        {
            case TwistScenes.Hall:
                SceneManager.LoadSceneAsync("Twist Hallway",LoadSceneMode.Additive);
                break;
            case TwistScenes.Safe:
                SceneManager.LoadSceneAsync("SafeRoom", LoadSceneMode.Additive);
                break;
            case TwistScenes.Hold:
                SceneManager.LoadSceneAsync("Twist Hold", LoadSceneMode.Additive);
                break;
        }
        StartCoroutine(MovePosition());
    }

    IEnumerator MovePosition()
    {
        LandingPoint [] GoingTo= FindObjectsOfType<LandingPoint>();
        bool foundIt = false;

        while (!foundIt)
        {
            yield return new WaitForSeconds(.01f);
           GoingTo = FindObjectsOfType<LandingPoint>();

            if (GoingTo.Length != 0)
                foreach (LandingPoint testing in GoingTo)
                {
                    if (testing.MyScene == MovingTo)
                    {
                     FPPlayerController FP= FindObjectOfType<FPPlayerController>();
                        FP.transform.position = testing.transform.position;
                        foundIt = true;
                    }
                }


        }
    }
    }
