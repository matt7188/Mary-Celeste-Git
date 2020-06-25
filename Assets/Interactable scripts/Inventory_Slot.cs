using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Inventory_Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Items MyItem;
    public Text Text;
    public Image MyImage;

    public string ItemName;

    bool HoverOver;

    void Start()
    {
        ItemName = "";
        Text.text = "";
        MyItem = null;
       /* EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { EnterEvent((PointerEventData)data); });
        trigger.triggers.Add(entry);*/
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Text.text = ItemName;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Text.text = "";
    }

    public void Onclick()
    {
        if (MyItem != null)
        {
            if (MyItem.drop(FindObjectOfType<GameMananger>().Current_Room))
                FindObjectOfType<Inventory_Managment>().RemoveInventory(this.gameObject);
            else
                Debug.Log("Nope");
        }
    }
}
