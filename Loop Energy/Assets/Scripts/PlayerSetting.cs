using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class responsible for adding information to Unity PlayerPrefs
public class PlayerSetting : MonoBehaviour
{

    void Start()
    {
        //if there is not a language defined on PlayerPrefs, add a language key
        //which is the English language
        if (!PlayerPrefs.HasKey("language"))
        {
            PlayerPrefs.SetString("language", "English");
            PlayerPrefs.Save();
        }

        //if there is already a language defined on PlayerPrefs, load the game's current language
        //and update the main menu buttons text
        else
        {
            //checking if language string exists on the Unity PlayerPrefs
            ExceptionHandler.instance.StringNullOrWhiteException(PlayerPrefs.GetString("language"),
            "PlayerPrefs langauge text");

            JsonManager.instance.currentTextLanguage = PlayerPrefs.GetString("language");
            StartMenuNavigator.instance.changeTextLanguageMainMenu(JsonManager.instance.currentTextLanguage);
        }
    }
}