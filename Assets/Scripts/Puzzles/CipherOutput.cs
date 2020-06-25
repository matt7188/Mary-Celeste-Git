using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CipherOutput : MonoBehaviour
{
    public Text CiphersVisual;
    public Text Origional;
    public Text Solution;
    public Text Output;

    public Text Title;

    public GameObject Input;
    public GameObject[] ComparisonsObj;
    public Text[] Comparisons;

    int CurrentChange;

    Text Mine;

    Cipher.CipherType LookingAt = Cipher.CipherType.Shift;

    public void Init()
    {
        
        switch (LookingAt) {
            case Cipher.CipherType.Shift:
        CiphersVisual.text = Notebook.CiphersStored;
                Solution.gameObject.SetActive(true);
                Solution.text = Origional.text;
                Title.text = "Caesar Cipher";
        Output.text = CiphersVisual.text;
        CurrentChange = 0;
                Input.gameObject.SetActive(false);
                break;
            case Cipher.CipherType.Mono:
                CiphersVisual.text = Notebook.CiphersStored;
                Solution.gameObject.SetActive(false);
                Title.text = "Lookup Cipher";
                Output.text = CiphersVisual.text;
                CurrentChange = 0;
                Input.gameObject.SetActive(true);
                break;
            case Cipher.CipherType.Poly:
                CiphersVisual.text = Notebook.CiphersStored;
                Solution.gameObject.SetActive(false);
                Title.text = "Vigenere Cipher";
                Output.text = CiphersVisual.text;
                CurrentChange = 0;
                Input.gameObject.SetActive(true);
                break;
        }

        if (Input.activeSelf)
        {
            for (int i = 0; i< 26; i++) {
                ComparisonsObj[i].transform.position = new Vector3(Input.transform.position.x, Input.transform.position.y + i * -14.3f, Input.transform.position.z);
                //Comparisons[i] = ComparisonsObj[i].GetComponentInChildren<Text>();
            }
        }

    }


    public void change(bool Up)
    {
        if (LookingAt == Cipher.CipherType.Shift)
        {
            if (Up)
                CurrentChange++;
            else
                CurrentChange--;

            Output.text = Cipher.Offset(CiphersVisual.text, CurrentChange);
            Solution.text = Cipher.Offset(Origional.text, CurrentChange);
        }
    }

    public void changeTyping(int WhoAmI)
    {
        string OutputString = "";
        for (int Hold = 0; Hold < Comparisons.Length; Hold++)
        {
            if (Comparisons[Hold].text != "")
            {
                OutputString += Comparisons[Hold].text;
            }
            else
                OutputString += " ";
        }

        //Debug.Log(OutputString);
        if(LookingAt==Cipher.CipherType.Mono)
            Output.text = Cipher.Mono(CiphersVisual.text, OutputString.ToCharArray());
        if (LookingAt == Cipher.CipherType.Poly)
            Output.text = Cipher.Poly(CiphersVisual.text, OutputString.ToCharArray());

        if (WhoAmI < ComparisonsObj.Length)
        {
            ComparisonsObj[WhoAmI].GetComponent<InputField>().ActivateInputField();
            ComparisonsObj[WhoAmI].GetComponent<InputField>().Select();
        }
        else
        {
            ComparisonsObj[0].GetComponent<InputField>().ActivateInputField();
            ComparisonsObj[0].GetComponent<InputField>().Select();

        }
    }
        public void SwitchType(bool Right)
    {
        if (Right)
        {
            switch (LookingAt)
            {
                case Cipher.CipherType.Mono:
                    LookingAt = Cipher.CipherType.Shift;
                    break;
                case Cipher.CipherType.Poly:
                    LookingAt = Cipher.CipherType.Mono;
                    break;
                case Cipher.CipherType.Shift:
                    LookingAt = Cipher.CipherType.Poly;
                    break;
            }
        }
        switch (LookingAt)
        {
            case Cipher.CipherType.Mono:
                LookingAt = Cipher.CipherType.Shift;
                break;
            case Cipher.CipherType.Poly:
                LookingAt = Cipher.CipherType.Mono;
                break;
            case Cipher.CipherType.Shift:
                LookingAt = Cipher.CipherType.Poly;
                break;
        }

        Init();

    }

}
