using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select_person : MonoBehaviour {

    public GameObject TemplateButton;

    public People[] AllPeople;

    public void Start()
    {
        for(int i=0;i< AllPeople.Length;i++)
        {
            GameObject newButton = Instantiate(TemplateButton) as GameObject;
            newButton.transform.SetParent(transform, false);
            newButton.transform.position= new Vector3(TemplateButton.transform.position.x, TemplateButton.transform.position.y - i * 29.0F, TemplateButton.transform.position.z);
            newButton.GetComponentInChildren<Text>().text = AllPeople[i].name;
            newButton.GetComponent<Accuse_person>().MyPerson= AllPeople[i];
        }
        Destroy(TemplateButton);
        this.gameObject.SetActive(false);
    }

        public void CreateButtons()
    {


    }

}
