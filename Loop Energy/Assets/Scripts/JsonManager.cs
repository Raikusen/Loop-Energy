using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

//claass that reads the information of the JSON files present on this game
public class JsonManager : MonoBehaviour
{
    private string textFilePath;
    private string textFileName = "textData.json";

    //string information of the json file loaded
    private string jsonContents;

    //the data from the text json file, that can be found on the json text data file loaded
    [HideInInspector]
    public JsonData textData;

    //the language being currently used on the game
    [HideInInspector]
    public string currentTextLanguage = "English";

    //singleton instance of this class
    [HideInInspector] public static JsonManager instance;

    private string tempTextData;

    public bool reloadTextFile = false;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;

        LoadTextJSONFileData();

        DontDestroyOnLoad(instance);
    }

    //loading the information present on the language text JSON file
    private void LoadJSONFile(string filePath)
    {

        //reading the information present on the json file
        jsonContents = System.IO.File.ReadAllText(filePath);
    }

    public void LoadTextJSONFileData()
    {
        //loading text file data
        textFilePath = Application.streamingAssetsPath + "/" + textFileName;

        LoadJSONFile(textFilePath);

        //passing that information as a JsonData object 
        if (jsonContents != null)
            textData = JsonMapper.ToObject(jsonContents);
    }

    public void CheckButtonTextLanguageJSON(Button button, ButtonData buttonData)
    {
        //if button exists and the button text language is not the same as the current game language,
        //change the button text language
        if (!(string.Equals(buttonData.buttonTextLanguage,
            JsonManager.instance.currentTextLanguage)) || buttonData.refreshButtonText == true)
        {
            string currentLanguage = JsonManager.instance.currentTextLanguage;

            //see if currentTextLanguage string is valid
            ExceptionHandler.instance.StringNullOrWhiteException(currentLanguage,
            "there is not a game language defined.");

            //updating the button text language to be equal to the game's current language
            buttonData.buttonTextLanguage = currentLanguage;

            //getting the string of the language information for the button recieved on this function
            tempTextData = JsonManager.instance.textData[currentLanguage][buttonData.buttonName].ToString();

            //checking if the text JSON file has informaation about the given button
            ExceptionHandler.instance.StringNullOrWhiteException(tempTextData,
            "there is not information defined in the text JSON file, for the button " + buttonData.buttonName);

            //changing the button text, according to the textData.json file information
            button.GetComponentInChildren<Text>().text = tempTextData;

            //hack for refreshing stage level buttons
            if (buttonData.refreshButtonText == true)
                buttonData.refreshButtonText = false;
        }
    }

    public void DestroyJSONInstance()
    {
        if (instance != null)
            Destroy(gameObject);
    }
}