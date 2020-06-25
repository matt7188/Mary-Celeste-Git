using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnHoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    public Text theText;
    public GameObject Whiteline;
    // Start is called before the first frame update
    void Start()
    {
        //theText = GetComponent<Text>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        //theText.text = "-" + theText.text + "-";
        Whiteline.SetActive(true);
        Whiteline.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 12, this.transform.position.z);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        /*if (theText.text[0] = '-')
        {
            theText.text = theText.text.Remove(0, 1);
            theText.text = theText.text.Remove(theText.text.Length - 1, 1);
        }*/
        Whiteline.SetActive(false);
    }
}
