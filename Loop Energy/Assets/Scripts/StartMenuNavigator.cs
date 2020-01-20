using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//class responsible for the navigation of the menu buttons
public class StartMenuNavigator : MonoBehaviour
{
    //buttons present on the main menu
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button languageButton;

    [SerializeField]
    private Button progressButton;

    [SerializeField]
    private Button englishLanguageButton;

    [SerializeField]
    private Button portugueseLanguageButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Button stage_1_Button;
    [SerializeField]
    private Button stage_2_Button;
    [SerializeField]
    private Button stage_3_Button;

    private ButtonData tempButtonData;

    //string information for a button, that is retrieved on the textData.json file
    private string tempTextData;

    //singleton instance of this class
    [HideInInspector] public static StartMenuNavigator instance;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    public void ChangeToLanguageMenu()
    {
        ActivationForStartingMenuButtons(false);
        ActivationForLanguageButtons(true);
        ActivationForStageButtons(false);

        ActivationForBackButton(true);
    }

    public void ChangeToMainMenuButtons()
    {
        ActivationForStartingMenuButtons(true);
        ActivationForLanguageButtons(false);
        ActivationForStageButtons(false);

        ActivationForBackButton(false);
    }

    public void ChangeToStageMenuButtons()
    {
        ActivationForStartingMenuButtons(false);
        ActivationForLanguageButtons(false);
        ActivationForStageButtons(true);

        ActivationForBackButton(true);
    }

    public void ActivationForStartingMenuButtons(bool value)
    {
        playButton.gameObject.SetActive(value);
        languageButton.gameObject.SetActive(value);
        progressButton.gameObject.SetActive(value);

        if(value == true)
        {
            CheckButtonTextLanguage(playButton);
            CheckButtonTextLanguage(languageButton);
            CheckButtonTextLanguage(progressButton);
        }
    }

    public void ActivationForLanguageButtons(bool value)
    {
        englishLanguageButton.gameObject.SetActive(value);
        portugueseLanguageButton.gameObject.SetActive(value);

        if (value == true)
        {
            CheckButtonTextLanguage(englishLanguageButton);
            CheckButtonTextLanguage(portugueseLanguageButton);
        }
    }

    public void ActivationForStageButtons(bool value)
    {
        stage_1_Button.gameObject.SetActive(value);
        stage_2_Button.gameObject.SetActive(value);
        stage_3_Button.gameObject.SetActive(value);

        if (value == true)
        {
            CheckButtonTextLanguage(stage_1_Button);
            CheckButtonTextLanguage(stage_2_Button);
            CheckButtonTextLanguage(stage_3_Button);
        }
    }

    public void ActivationForBackButton(bool value)
    {
        backButton.gameObject.SetActive(value);

        if(value == true)
            CheckButtonTextLanguage(backButton);
    }

    public void ChangeTextLanguage(string language)
    {
        CheckButtonTextLanguage(englishLanguageButton);
        CheckButtonTextLanguage(portugueseLanguageButton);
        CheckButtonTextLanguage(backButton);
    }

    public void ChangeTextLanguageMainMenu(string language)
    {
        CheckButtonTextLanguage(playButton);
        CheckButtonTextLanguage(languageButton);
        CheckButtonTextLanguage(progressButton);
    }

    //function for changing the text language of a button
    public void CheckButtonTextLanguage(Button button)
    {
        tempButtonData = button.GetComponent<ButtonData>();

        if(tempButtonData != null)
        {
            //if button exists and the button text language is not the same as the current game language,
            //change the button text language
            if(!(string.Equals(tempButtonData.buttonTextLanguage,
                JsonManager.instance.currentTextLanguage)))
            {
                string currentLanguage = JsonManager.instance.currentTextLanguage;

                //see if currentTextLanguage string is valid
                ExceptionHandler.instance.StringNullOrWhiteException(currentLanguage,
                "there is not a game language defined.");

                //updating the button text language to be equal to the game's current language
                tempButtonData.buttonTextLanguage = currentLanguage;

                //getting the string of the language information for the button recieved on this function
                tempTextData = JsonManager.instance.itemData[currentLanguage][tempButtonData.buttonName].ToString();

                //checking if the text JSON file has informaation about the given button
                ExceptionHandler.instance.StringNullOrWhiteException(tempTextData,
                "there is not information defined in the text JSON file, for the button " + tempButtonData.buttonName);

                //changing the button text, according to the textData.json file information
                button.GetComponentInChildren<Text>().text = tempTextData;
            }
             
        }
    }
}