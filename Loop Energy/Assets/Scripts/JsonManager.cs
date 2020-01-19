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

    //the data that can be found on the json file loaded
    [HideInInspector]
    public JsonData itemData;

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

        LoadTextJSONFile();
    }

    //loading the information present on the language text JSON file
    private void LoadTextJSONFile()
    {
        textFilePath = Application.streamingAssetsPath + "/" + textFileName;

        //reading the information present on the json file
        jsonContents = System.IO.File.ReadAllText(textFilePath);

        //passing that information as a JsonData object 
        itemData = JsonMapper.ToObject(jsonContents);
    }
}