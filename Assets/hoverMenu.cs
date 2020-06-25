using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverMenu : MonoBehaviour
{


    public TextFx.TextFxTextMeshPro Showing;

    

    void OnMouseEnter()
    {
        
        Showing.AnimationManager.PlayAnimation(0, 0);
    }
    void OnMouseExit()
    {
        Showing.AnimationManager.ContinuePastBreak();
    }
}
