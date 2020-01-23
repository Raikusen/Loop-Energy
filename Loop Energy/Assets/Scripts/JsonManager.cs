using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;

//claass that reads the information of the JSON files present on this game
public class JsonManager : MonoBehaviour
{
    private string readFilePath;
    private string textFileName = "textData.json";
    private string levelFileName = "levelData.json";

    //string information of the json file loaded
    private string jsonTextContents;

    private string jsonLevelContents;

    //the data from the text json file, that can be found on the json text data file loaded
    [HideInInspector]
    public JsonData textData;

    //the data from the level json file, that can be found on the json level data file loaded
    [HideInInspector]
    public JsonData levelData;

    //the language being currently used on the game
    [HideInInspector]
    public string currentTextLanguage = "English";

    //singleton instance of this class
    [HideInInspector] public static JsonManager instance;

    //temporary string used to store the text of a game button
    private string tempTextData;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;

        //load text language file and levels data file
        LoadTextJSONFileData();
        LoadLevelJSONFileData();

        //this object is not destroyed so it can be used on the game scene
        DontDestroyOnLoad(instance);
    }

    //loading the information present on the language text JSON file
    private void LoadJSONTextFile(string filePath)
    {
        //using Unity Web Request to load JSON file information from android
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            www.SendWebRequest();
            while (!www.isDone)
            {
            }
            jsonTextContents = www.downloadHandler.text;
        }
        else
        {
            jsonTextContents = System.IO.File.ReadAllText(filePath);
        }
    }

    private void LoadJSONLevelFile(string filePath)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            www.SendWebRequest();
            while (!www.isDone)
            {
            }
            jsonLevelContents = www.downloadHandler.text;
        }
        else
        {
            jsonLevelContents = System.IO.File.ReadAllText(filePath);
        }
    }

    public void LoadTextJSONFileData()
    {
        //loading text file data
        readFilePath = Application.streamingAssetsPath + "/" + textFileName;

        LoadJSONTextFile(readFilePath);

        //passing that information as a JsonData object 
        if (jsonTextContents != null)
            textData = JsonMapper.ToObject(jsonTextContents);
    }

    public void LoadLevelJSONFileData()
    {
        //loading text file data
        readFilePath = Application.streamingAssetsPath + "/" + levelFileName;

        LoadJSONLevelFile(readFilePath);

        //passing that information as a JsonData object 
        if (jsonLevelContents != null)
            levelData = JsonMapper.ToObject(jsonLevelContents);
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
            tempTextData = textData[currentLanguage][buttonData.buttonName].ToString();

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