using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public enum Language
        {   
            Korean,English
        }
    [SerializeField]
    private Language language = Language.Korean;
    public TMP_Text[] UItext;
    void Start()
    {
        if(language.ToString() == "English"){
            UItext[0].text="Which building did you just pass?";
            UItext[0].fontSize=0.24f;
            UItext[1].text="";
            UItext[2].text="Please select the type of the passed building!";
            UItext[2].fontSize=0.17f;
            UItext[3].text="Cafe";
            UItext[4].text="fruit store";
            UItext[5].text="Flower";
            UItext[6].text="Pizza";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
