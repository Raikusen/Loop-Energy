using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class responsible for adding information to Unity PlayerPrefs
public class PlayerSetting : MonoBehaviour
{
    //tags for key strings
    public const string LANGUAGE_KEY = "language";
    public const string CURRENT_STAGE_KEY = "currentStage";
    public const string STAGES_COMPLETED_KEY = "stagesCompleted";
    public const string CURRENT_LEVEL_KEY = "currentLevel";
    public const string TOTAL_LEVELS_COMPLETED_KEY = "levelsCompleted";
    public const string CURRENT_LEVEL_PAGE_KEY = "currentStageLevelsPage";
    public const string CAMERA_BACKGROUND_COLOR = "cameraBackgroundColor";

    void Start()
    {
        CheckLanguageKey();

        CheckCurrentStageKey();
        CheckStagesCompletedKey();

        CheckCurrentLevelKey();
        CheckTotalLevelsCompletedKey();

        CheckStageLevelsPage();
    }

    private void CheckLanguageKey()
    {
        //if there is not a language defined on PlayerPrefs, add a language key
        //which is the English language
        if (!PlayerPrefs.HasKey(LANGUAGE_KEY))
        {
            PlayerPrefs.SetString(LANGUAGE_KEY, "English");
            PlayerPrefs.Save();
        }

        //if there is already a language defined on PlayerPrefs, load the game's current language
        //and update the main menu buttons text
        else if (PlayerPrefs.HasKey(LANGUAGE_KEY))
        {
            //checking if language string exists on the Unity PlayerPrefs
            ExceptionHandler.instance.StringNullOrWhiteException(PlayerPrefs.GetString(LANGUAGE_KEY),
            "PlayerPrefs langauge text");

            JsonManager.instance.currentTextLanguage = PlayerPrefs.GetString(LANGUAGE_KEY);
            StartMenuNavigator.instance.ChangeTextLanguageMainMenu(JsonManager.instance.currentTextLanguage);
        }
    }

    //when a stage is completed, integer value is incremented
    //unless currentStage is the last one available
    private void CheckCurrentStageKey()
    {
        CheckIntegerKey(CURRENT_STAGE_KEY, 1);
    }

    //when a stage is completed, integer value is incremented
    private void CheckStagesCompletedKey()
    {
        CheckIntegerKey(STAGES_COMPLETED_KEY, 0);
    }

    //when a stage is completed for the first time, current level will be set to one
    //when a level is completed for the first time, current level will be incremented
    private void CheckCurrentLevelKey()
    {
        CheckIntegerKey(CURRENT_LEVEL_KEY, 1);
    }

    //the amount of levels completed,
    //when a level is completed for the first time increment integer by one
    private void CheckTotalLevelsCompletedKey()
    {
        CheckIntegerKey(TOTAL_LEVELS_COMPLETED_KEY, 0);
    }

    //the current page with the buttons of a stage, each page can have 4 levels
    //the next page shows the next 4 levels of a stage, if they are available 
    private void CheckStageLevelsPage()
    {
        //if application is reset, page starts at 1 again
        PlayerPrefs.SetInt(CURRENT_LEVEL_PAGE_KEY, 1);
    }

    private void CheckIntegerKey(string key, int value)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }
    }

    private void CheckCameraBackgroundColorKey()
    {
        if (!PlayerPrefs.HasKey(CAMERA_BACKGROUND_COLOR))
        {
            PlayerPrefs.SetString(CAMERA_BACKGROUND_COLOR, "blue");
            PlayerPrefs.Save();
        }
    }
}