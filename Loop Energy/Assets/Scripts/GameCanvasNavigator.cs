using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//class responsible for navigating on the game menu
public class GameCanvasNavigator : MonoBehaviour
{
    //adding game menu buttons
    [SerializeField]
    private Button gameMenuButton;

    [SerializeField]
    private Button previousLevelButton;

    [SerializeField]
    private Button nextLevelButton;

    [SerializeField]
    private Button quitButton;

    [SerializeField]
    private Button resumeLevelButton;

    [SerializeField]
    private Button restartLevelButton;

    //panel to hide the game puzzle pieces
    [SerializeField]
    private GameObject gameMenuPanel;

    private ButtonData tempButtonData;

    [SerializeField]
    private Camera gameCamera;

    [SerializeField]
    private Text levelCompletedText;

    //singleton instance of this class
    [HideInInspector] public static GameCanvasNavigator instance;

    private int currentLevel;
    private int currentStage;
    private int currenStageLevelSelected;
    private int totalLevelsAvailable;
    private int totalLevelsCompleted;
    private int levelNumberFromTotalLevels;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;

        LoadBackgroundColorCamera();
    }

    void Start()
    {
        CalculateLevelNumberFromTotalLevels();
    }

    public void ReturnToMainMenu()
    {
        //clean game UI buttons
        ActivateGameMenuButtons(false);
        gameMenuButton.gameObject.SetActive(false);

        //destroy instances that were passed by the previous scene
        JsonManager.instance.DestroyJSONInstance();
        AudioManager.instance.DestroyAudioManagerInstance();
        StageManager.instance.DestroyStageManagerInstance();

        gameCamera.backgroundColor = Color.blue;

        gameMenuPanel.gameObject.SetActive(true);

        AudioManager.instance.StopLevelCompletedSound();

        //return to menu scene
        SceneManager.LoadScene("Menu");
    }

    public void ActivateGameMenuButtons(bool value)
    {
        //do not activate game menu buttons if a piece is moving
        if (value == true && (PuzzlePiece.GetPiecesAreMoving() == true ||
            GameManager.instance.GetGameIsPlayable() == false))
            return;

        //game and menu button are disabled if true
        GameManager.instance.SetGameIsPlayable(!value);

        gameMenuButton.gameObject.SetActive(!value);

        gameMenuPanel.gameObject.SetActive(value);

        //menu buttons are enabled if true
        quitButton.gameObject.SetActive(value);
        resumeLevelButton.gameObject.SetActive(value);
        restartLevelButton.gameObject.SetActive(value);

        if (value == true)
        {
            CheckButtonTextLanguage(quitButton);
            CheckButtonTextLanguage(resumeLevelButton);
            CheckButtonTextLanguage(restartLevelButton);
        }

        else CheckButtonTextLanguage(gameMenuButton);

        CheckPreviousButtonActivation(value);
        CheckNextButtonActivation(value);
    }

    public void ClickButtonSound()
    {
        AudioManager.instance.PlayButtonClickSound();
    }


    //function for changing the text language of a button
    private void CheckButtonTextLanguage(Button button)
    {
        tempButtonData = button.GetComponent<ButtonData>();

        if (tempButtonData != null)
            JsonManager.instance.CheckButtonTextLanguageJSON(button, tempButtonData);

    }

    public void ActivateLevelCompletedText(bool value)
    {
        levelCompletedText.gameObject.SetActive(value);

        if(value == true)
        {
            string currentLanguage = JsonManager.instance.currentTextLanguage;

            string levelMessage = JsonManager.instance.textData[currentLanguage]["Level Completed"].ToString();

            if (levelMessage != null)
                levelCompletedText.text = levelMessage;
        }

    }

    public void CompletedLevel()
    {
        //add part of stage

        //review part of previous and next level
        AudioManager.instance.PlayLevelCompletedSound();

        GameManager.instance.SetGameIsPlayable(true);

        ActivateLevelCompletedText(true);

        if (levelNumberFromTotalLevels > totalLevelsCompleted && totalLevelsCompleted < totalLevelsAvailable)
        {
            PlayerPrefs.SetInt(PlayerSetting.TOTAL_LEVELS_COMPLETED_KEY, levelNumberFromTotalLevels );
            PlayerPrefs.Save();

            bool stageIsCompleted = StageManager.instance.CheckIfStageIsCompleted(currentStage);

            //CHECK
            if(stageIsCompleted == false)
            {
                int currLevKeyValue = PlayerPrefs.GetInt(PlayerSetting.CURRENT_LEVEL_KEY);
                currLevKeyValue++;

                PlayerPrefs.SetInt(PlayerSetting.CURRENT_LEVEL_KEY, currLevKeyValue);
                

                int completedStages = PlayerPrefs.GetInt(PlayerSetting.STAGES_COMPLETED_KEY);

                if (completedStages < currentStage 
                    && currenStageLevelSelected == StageManager.instance.GetTotalLevelsFromStage(currentStage))
                {
                    Debug.Log("stage is completed");
                    PlayerPrefs.SetInt(PlayerSetting.STAGES_COMPLETED_KEY, completedStages + 1);
                    PlayerPrefs.SetInt(PlayerSetting.CURRENT_LEVEL_KEY, 1);
                }

                PlayerPrefs.Save();

            }
            

            totalLevelsCompleted = levelNumberFromTotalLevels;
        }

        ActivateGameMenuButtons(true);

        resumeLevelButton.gameObject.SetActive(false);

        GameManager.instance.SetGameIsPlayable(false);

    }

    private void CheckPreviousButtonActivation(bool value)
    {
        if (value == false)
        {
            previousLevelButton.gameObject.SetActive(false);
            return;
        }

        if (levelNumberFromTotalLevels > 1 && levelNumberFromTotalLevels <= totalLevelsAvailable)
        {
            previousLevelButton.gameObject.SetActive(true);
            CheckButtonTextLanguage(previousLevelButton);
        }
    }

    private void CheckNextButtonActivation(bool value)
    {
        if (value == false)
        {
            nextLevelButton.gameObject.SetActive(false);
            return;
        }

        if (levelNumberFromTotalLevels < totalLevelsAvailable && totalLevelsCompleted >= levelNumberFromTotalLevels)
        {
            nextLevelButton.gameObject.SetActive(true);
            CheckButtonTextLanguage(nextLevelButton); 
        }
    }

    public void GoToPreviousLevel()
    {
        int previousLevel = currenStageLevelSelected - 1;
        int stageSelected = currentStage;

        bool loadLevelSuccess = false;

        if(previousLevel > 0)
        {
            PlayerPrefs.SetInt(PlayerSetting.CURRENT_STAGE_LEVEL_SELECTED_KEY, previousLevel);
            PlayerPrefs.Save();

            loadLevelSuccess = true;
        }

        else if(previousLevel == 0 && stageSelected > 1)
        {
            stageSelected -= 1;

            int totalLevelsFromPreviouStage = StageManager.instance.GetTotalLevelsFromStage(stageSelected);

            PlayerPrefs.SetInt(PlayerSetting.CURRENT_STAGE_KEY, stageSelected);
            PlayerPrefs.SetInt(PlayerSetting.CURRENT_STAGE_LEVEL_SELECTED_KEY, totalLevelsFromPreviouStage);
            PlayerPrefs.Save();

            loadLevelSuccess = true;
        }

        if (loadLevelSuccess == true)
        {
            CalculateLevelNumberFromTotalLevels();
            AudioManager.instance.StopLevelCompletedSound();
            GameManager.instance.ResetLevel();
        }
    }

    public void GoToNextLevel()
    {
        int nextLevel = currenStageLevelSelected + 1;
        int stageSelected = currentStage;

        bool loadLevelSuccess = false;

        if (currentStage < StageManager.instance.GetGameTotalStages() &&
            nextLevel > StageManager.instance.GetTotalLevelsFromStage(currentStage))
        {
            stageSelected += 1;
            PlayerPrefs.SetInt(PlayerSetting.CURRENT_STAGE_KEY, stageSelected);
            PlayerPrefs.SetInt(PlayerSetting.CURRENT_STAGE_LEVEL_SELECTED_KEY, 1);

            bool stageIsComplete = StageManager.instance.CheckIfStageIsCompleted(stageSelected);

            if(stageIsComplete == false)
                PlayerPrefs.SetInt(PlayerSetting.CURRENT_LEVEL_KEY, 1);

            PlayerPrefs.Save();

            loadLevelSuccess = true;
        }

        else if (nextLevel > 0 && nextLevel <= totalLevelsAvailable)
        {
            PlayerPrefs.SetInt(PlayerSetting.CURRENT_STAGE_LEVEL_SELECTED_KEY, nextLevel);
            PlayerPrefs.Save();

            loadLevelSuccess = true;
        }

        if (loadLevelSuccess == true)
        {
            CalculateLevelNumberFromTotalLevels();
            AudioManager.instance.StopLevelCompletedSound();
            GameManager.instance.ResetLevel();
        }
    }

    private void CalculateLevelNumberFromTotalLevels()
    {
        currentLevel = PlayerPrefs.GetInt(PlayerSetting.CURRENT_LEVEL_KEY);
        currentStage = PlayerPrefs.GetInt(PlayerSetting.CURRENT_STAGE_KEY);
        currenStageLevelSelected = PlayerPrefs.GetInt(PlayerSetting.CURRENT_STAGE_LEVEL_SELECTED_KEY);
        totalLevelsAvailable = StageManager.instance.GetGameTotalLevels();
        totalLevelsCompleted = PlayerPrefs.GetInt(PlayerSetting.TOTAL_LEVELS_COMPLETED_KEY);

        levelNumberFromTotalLevels = StageManager.instance.GetStageLevelNubmerFromTotalLevels(currentStage,
            currenStageLevelSelected);
    }

    private void LoadBackgroundColorCamera()
    {
        if (gameCamera == null)
            ExceptionHandler.instance.NullReferenceException("there is not a camera associated to the Background Manager.");

        string cameraColor = PlayerPrefs.GetString(PlayerSetting.CAMERA_BACKGROUND_COLOR);

        if (cameraColor != null)
            switch(cameraColor)
            {
                case "blue":
                    gameCamera.backgroundColor = Color.blue;
                    break;
                case "yellow":
                    gameCamera.backgroundColor = Color.yellow;
                    break;
                case "green":
                    gameCamera.backgroundColor = Color.green;
                    break;
                case "red":
                    gameCamera.backgroundColor = Color.red;
                    break;
            }
                
    }

}