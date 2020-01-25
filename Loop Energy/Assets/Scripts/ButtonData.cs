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

    //needed for stage level buttons number
    [HideInInspector]
    public bool refreshButtonText = false;

    //the number of a level button
    [HideInInspector]
    public int buttonLevel = 0;
}