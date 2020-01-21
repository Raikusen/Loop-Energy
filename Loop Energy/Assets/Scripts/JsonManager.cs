using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;

        //loading text file data
        textFilePath = Application.streamingAssetsPath + "/" + textFileName;

        LoadJSONFile(textFilePath);

        //passing that information as a JsonData object 
        if (jsonContents != null)
            textData = JsonMapper.ToObject(jsonContents);
    }

    //loading the information present on the language text JSON file
    private void LoadJSONFile(string filePath)
    {

        //reading the information present on the json file
        jsonContents = System.IO.File.ReadAllText(filePath);
    }
}