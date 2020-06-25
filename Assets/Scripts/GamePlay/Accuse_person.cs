using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Accuse_person : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameMananger GM;
    public People MyPerson;
    public GameObject Display_Object;
    new Image renderer;

    public Image ScoreScreen;
    public Text ScoreOutput;

    void Start()
    {
        renderer = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        renderer.color = new Color(renderer.color.r, renderer.color.g,renderer.color.b,.7f);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);


    }

    public void OnClick()
    {
        if (MyPerson.murder)
        {
            ScoreOutput.text = "Number Still Alive: ";
            ScoreOutput.text += GM.NumberAlive().ToString();
            ScoreOutput.text += "\n\n\nClues Found:" +
                 GM.CluesFoundOutput();

            //this.GetComponent<Image>().color = Color.green;

            ScoreScreen.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("WRONG");
            Display_Object.SetActive(false);

            GM.ToggleCursor(false);
            GM.TimePass(1);
        }

    }
}
