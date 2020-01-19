using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//class that has interface information about a game button
public class ButtonData : MonoBehaviour
{
    //name of the button, whose string is equal to one of the language options
    //present on the textData.json file
    public string buttonName;

    //the starting language of the game, when playing for the first time is english
    [HideInInspector]
    public string buttonTextLanguage = "English";

    //button component of the game objects using this script
    private Button classButton;

    void Awake()
    {
        classButton = GetComponent<Button>();
    }

    //changing game's text language
    public void LanguageButtonPressed()
    {
        //if the game's current language is not equal to the one selected on a button
        if (!(string.Equals(buttonName,
                JsonManager.instance.currentTextLanguage)))
        {
            //check if this object has a buttonName defined
            ExceptionHandler.instance.StringNullOrWhiteException(buttonName,
            transform.name + " has an invalid text.");

            //change the game's current language
            JsonManager.instance.currentTextLanguage = buttonName;
            StartMenuNavigator.instance.changeTextLanguage(buttonName);

            //save the current language on the Unity PlayerPrefs for future acess
            if (PlayerPrefs.HasKey("language"))
            {
                PlayerPrefs.SetString("language", buttonName);
                PlayerPrefs.Save();
            }
        }
            
    }
}