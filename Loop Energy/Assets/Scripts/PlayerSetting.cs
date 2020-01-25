using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class responsible for adding information to Unity PlayerPrefs
public class PlayerSetting : MonoBehaviour
{
    //tags for key strings

    //language keys
    public const string LANGUAGE_KEY = "language";

    //stage keys
    public const string CURRENT_STAGE_KEY = "currentStage";
    public const string CURRENT_PLAYING_STAGE_KEY = "currentPlayingStage";
    public const string STAGES_COMPLETED_KEY = "stagesCompleted";

    //level keys
    public const string CURRENT_LEVEL_FROM_LAST_STAGE_UNLOCKED_KEY = "currentLevel";
    public const string CURRENT_STAGE_LEVEL_SELECTED_KEY = "currentStageLevelSelected";
    public const string TOTAL_LEVELS_COMPLETED_KEY = "levelsCompleted";
    public const string CURRENT_LEVEL_PAGE_KEY = "currentStageLevelsPage";

    //camera keys
    public const string CAMERA_BACKGROUND_COLOR = "cameraBackgroundColor";

    void Start()
    {
        //for testing purposes, after deleate imediatly comment function and restart application
        //USE WITH CAUTION
        //PlayerPrefs.DeleteAll();

        //creating keys information to PlayerPrefs if needed

        CheckLanguageKey();

        CheckCurrentStageKey();
        CheckCurrentStageLevelKey();
        CheckStagesCompletedKey();

        CheckPlayableStageKey();

        CheckCurrentLevelKey();
        CheckTotalLevelsCompletedKey();

        CheckStageLevelsPage();
    }

    //the language currently selected on the playing device
    private void CheckLanguageKey()
    {
        //if there is not a language defined on PlayerPrefs, add a language key
        //which is the English language by default
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
            StartMenuNavigator.instance.ChangeTextLanguageMainMenu();
        }
    }

    //the last stage that was playable
    //used for last level played
    private void CheckPlayableStageKey()
    {
        CheckIntegerKey(CURRENT_PLAYING_STAGE_KEY, 0);
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

    //the level than a player can play, from the last stage he unlocked
    //when a level is completed for the first time, current level will be incremented
    //when a stage is completed for the first time, current level will be set to one
    private void CheckCurrentLevelKey()
    {
        CheckIntegerKey(CURRENT_LEVEL_FROM_LAST_STAGE_UNLOCKED_KEY, 1);
    }

    //the level being currently selected on the selected stage
    private void CheckCurrentStageLevelKey()
    {
        CheckIntegerKey(CURRENT_STAGE_LEVEL_SELECTED_KEY, 0);
    }

    //the amount of levels completed from the entire pool of stage levels,
    //when a level is completed for the first time increment it by one
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

    //the current color of the camera background
    private void CheckCameraBackgroundColorKey()
    {
        if (!PlayerPrefs.HasKey(CAMERA_BACKGROUND_COLOR))
        {
            PlayerPrefs.SetString(CAMERA_BACKGROUND_COLOR, "blue");
            PlayerPrefs.Save();
        }
    }
}