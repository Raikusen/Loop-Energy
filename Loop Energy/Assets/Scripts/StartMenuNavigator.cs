using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    //language menu buttons
    [SerializeField]
    private Button englishLanguageButton;

    [SerializeField]
    private Button portugueseLanguageButton;

    //button to return to the previous menu
    [SerializeField]
    private Button backButton;

    //button to restart last level played
    [SerializeField]
    private Button continueButton;

    //stage buttons
    [SerializeField]
    private Button stage_1_Button;
    [SerializeField]
    private Button stage_2_Button;
    [SerializeField]
    private Button stage_3_Button;

    //level buttons
    [SerializeField]
    private Button firstLevelButton;

    [SerializeField]
    private Button secondLevelButton;

    [SerializeField]
    private Button thirdLevelButton;

    [SerializeField]
    private Button fourthLevelButton;

    [SerializeField]
    private Button previousStageLevelsButton;

    [SerializeField]
    private Button nextStageLevelsButton;

    //arry containing the four level buttons
    private Button[] levelButtonArray;

    //progress report on the complete levels and stages
    [SerializeField]
    private Text completedLevelsText;

    [SerializeField]
    private Text completedStagesText;

    private ButtonData tempButtonData;

    //if there are stage level buttons being shown
    private bool stageLevelsBeingShown = false;

    //the stage currently being selected on the menu
    private int currentStageSelected = 0;

    private int totalLevelButtonsOnPage = 0;

    //singleton instance of this class
    [HideInInspector] public static StartMenuNavigator instance;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    void Start()
    {
        levelButtonArray = new Button[4];

        levelButtonArray[0] = firstLevelButton;
        levelButtonArray[1] = secondLevelButton;
        levelButtonArray[2] = thirdLevelButton;
        levelButtonArray[3] = fourthLevelButton;

        totalLevelButtonsOnPage = levelButtonArray.Length;

        CheckActivationForContinueButton(true);
            
    }

    //check if back button returns to stage menu or main menu
    public void CheckBackButtonBehaviour()
    {
        //stage menu
        if (stageLevelsBeingShown == true)
            ChangeToStageMenuButtons();

        //main menu
        else ChangeToMainMenuButtons();
    }

    public void ChangeToLanguageMenu()
    {
        ActivationForBackButton(true);

        ActivationForStartingMenuButtons(false);
        ActivationForLanguageButtons(true);
    }

    public void ChangeToMainMenuButtons()
    {
        ActivationForStartingMenuButtons(true);
        ActivationForLanguageButtons(false);
        ActivationForStageButtons(false);
        ActiationForProgressText(false);

        ActivationForBackButton(false);
    }

    public void ChangeToStageMenuButtons()
    {
        SetStageLevelsBeingShown(false);

        //stage level buttons are disabled
        if (firstLevelButton.gameObject.activeSelf == true)
            ActivationForLevelButtons(currentStageSelected);
            
        ActivationForStartingMenuButtons(false);
        ActivationForStageButtons(true);

        ActivationForBackButton(true);
    }

    public void ChangeToLevelButtons()
    {
        ActivationForStageButtons(false);

        ActivationForBackButton(true);
    }

    //changing the level page of a stage screen
    private void ChangeStageLevels(int currentLevelPage)
    {
        SetStageLevelsBeingShown(false);

        //deactivating previous stage level butons
        if (firstLevelButton.gameObject.activeSelf == true)
            ActivationForLevelButtons(currentStageSelected);

        //updating the currentLevelPage
        PlayerPrefs.SetInt(PlayerSetting.CURRENT_LEVEL_PAGE_KEY, currentLevelPage);
        PlayerPrefs.Save();

        SetStageLevelsBeingShown(true);

        //updating the stage level buttons to be shown
        ActivationForLevelButtons(currentStageSelected);
    }

    public void ActivationForStartingMenuButtons(bool value)
    {
        playButton.gameObject.SetActive(value);
        languageButton.gameObject.SetActive(value);
        progressButton.gameObject.SetActive(value);

        CheckActivationForContinueButton(value);

        if (value == true)
        {
            CheckButtonTextLanguage(playButton);
            CheckButtonTextLanguage(languageButton);
            CheckButtonTextLanguage(progressButton);

            if(continueButton.gameObject.activeSelf == true)
                CheckButtonTextLanguage(continueButton);
        }
    }

    public void CheckActivationForContinueButton(bool value)
    {
        //if the player played at least one level, button can be shown
        if (PlayerPrefs.GetInt(PlayerSetting.CURRENT_PLAYING_STAGE_KEY) > 0)
            continueButton.gameObject.SetActive(value);
        
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
        //there is an increment to load the stages the player has completed
        int stagesCompleted = PlayerPrefs.GetInt(PlayerSetting.STAGES_COMPLETED_KEY) + 1;

        if (stagesCompleted >= 1)
            stage_1_Button.gameObject.SetActive(value);

        if (stagesCompleted >= 2)
            stage_2_Button.gameObject.SetActive(value);

        if (stagesCompleted >= 3)
            stage_3_Button.gameObject.SetActive(value);

        if (value == true)
        {
            //when activate stage buttons, start at the first page
            PlayerPrefs.SetInt(PlayerSetting.CURRENT_LEVEL_PAGE_KEY, 1);
            PlayerPrefs.Save();

            if (stagesCompleted >= 1)
                CheckButtonTextLanguage(stage_1_Button);

            if (stagesCompleted >= 2)
                CheckButtonTextLanguage(stage_2_Button);

            if (stagesCompleted >= 3)
                CheckButtonTextLanguage(stage_3_Button);
        }
    }

    //activating the level buttons for the stage given
    public void ActivationForLevelButtons(int stage)
    {
        //disabling the stage buttons
        ActivationForStageButtons(false);

        //current stage selected is equal to the stage recieved
        currentStageSelected = stage;

        //if old and current stage are different, go to the first page
        int oldStageSelected = PlayerPrefs.GetInt(PlayerSetting.CURRENT_STAGE_KEY);

        if(currentStageSelected != oldStageSelected)
            PlayerPrefs.SetInt(PlayerSetting.CURRENT_LEVEL_PAGE_KEY, 1);
       
        //updating the currentStage key
        PlayerPrefs.SetInt(PlayerSetting.CURRENT_STAGE_KEY, currentStageSelected);
        PlayerPrefs.Save();

        int totalStageLevels = StageManager.instance.GetTotalLevelsFromStage(stage);
        int currentLevelPage = PlayerPrefs.GetInt(PlayerSetting.CURRENT_LEVEL_PAGE_KEY);

        bool value = stageLevelsBeingShown;

        if (totalStageLevels <= 0)
        {
            Debug.LogError("Amount of leves on stage " + stage + "is " + totalStageLevels);
            return;
        }

        //the amount level stage buttons that will be revealed
        int levelButtonsToShow = 0;

        //if total levels of a stage are equal or less than 4, show buttons equal to the total levels
        if (totalStageLevels <= totalLevelButtonsOnPage)
            levelButtonsToShow = totalStageLevels;

        else
        {
            //quotient and remainder between the total game levels and the current level page
            int levelButtonQuotient = totalStageLevels / (currentLevelPage * totalLevelButtonsOnPage);
            int levelButtonReminder = totalStageLevels % (currentLevelPage * totalLevelButtonsOnPage);

            //if there are more or equal levels than the currentLevelPage * 4, show 4 buttons
            if(levelButtonQuotient > 0)
                levelButtonsToShow = totalLevelButtonsOnPage;
            

            //else show the remainder level buttons available
            else if(levelButtonQuotient == 0 && levelButtonReminder > 0)
                 levelButtonsToShow = levelButtonReminder % 4;
                
        }

        bool stageIsCompleted = StageManager.instance.CheckIfStageIsCompleted(stage);

        int currentLevelOnStage = PlayerPrefs.GetInt(PlayerSetting.CURRENT_LEVEL_FROM_LAST_STAGE_UNLOCKED_KEY);

        bool breakLevelPositioning;

        int i = 0;

        string tempString;

        //showing the corresponding buttons of a stage on the current level page
        while (i < levelButtonsToShow)
        {
            //see if there are still more buttons to be created or not
            if (((i + 1) + (currentLevelPage - 1) * totalLevelButtonsOnPage) <= currentLevelOnStage)
                breakLevelPositioning = true;

            else breakLevelPositioning = false;

            //if stage is completed, the levels of a stage are still shown, even if the last played level number
            //is different of what is expected
            if (breakLevelPositioning == false && stageIsCompleted == false)
                break;

            levelButtonArray[i].gameObject.SetActive(value);

            //change level buttons number text
            if(value == true)
            {
                tempButtonData = levelButtonArray[i].GetComponent<ButtonData>();

                //refreshing the text button, for deleating the previous number on the level button text
                tempButtonData.refreshButtonText = true;

                tempButtonData.buttonLevel = ((currentLevelPage - 1) * totalLevelButtonsOnPage) + (i + 1);

                //change button language
                CheckButtonTextLanguage(levelButtonArray[i]);

                tempString = " " + (((currentLevelPage - 1) * totalLevelButtonsOnPage) + (i + 1));
                 
                //adding the level number on a level button text
                levelButtonArray[i].GetComponentInChildren<Text>().text += tempString;
            }

            i++;

        }

        //showing next and previous stage buttons if needed
        if (value == true)
        {

            if (totalStageLevels > (currentLevelPage * totalLevelButtonsOnPage) &&
                (currentLevelOnStage > (currentLevelPage * totalLevelButtonsOnPage) || stageIsCompleted == true))
            {
                nextStageLevelsButton.gameObject.SetActive(value);
                CheckButtonTextLanguage(nextStageLevelsButton);
            }

            if (currentLevelPage > 1 && ((currentLevelPage * totalLevelButtonsOnPage) - 4) > 0)
            {
                previousStageLevelsButton.gameObject.SetActive(value);
                CheckButtonTextLanguage(previousStageLevelsButton);
            }
        }

        else if(value == false)
        {
            nextStageLevelsButton.gameObject.SetActive(value);
            previousStageLevelsButton.gameObject.SetActive(value);
        }
    }

    public void SetStageLevelsBeingShown(bool value)
    {
        stageLevelsBeingShown = value;
    }

    public void ActivationForBackButton(bool value)
    {
        backButton.gameObject.SetActive(value);

        if(value == true)
            CheckButtonTextLanguage(backButton);
    }

    public void ChangeTextLanguage()
    {
        CheckButtonTextLanguage(englishLanguageButton);
        CheckButtonTextLanguage(portugueseLanguageButton);
        CheckButtonTextLanguage(backButton);
    }

    public void ChangeTextLanguageMainMenu()
    {
        CheckButtonTextLanguage(playButton);
        CheckButtonTextLanguage(languageButton);
        CheckButtonTextLanguage(progressButton);
    }

    //changing game's text language when pressing a language button
    public void LanguageButtonPressed()
    {

        GameObject currentButton = EventSystem.current.currentSelectedGameObject;

        ButtonData tempData = currentButton.GetComponent<ButtonData>();

        string buttonName = tempData.buttonName;

        //if the game's current language is not equal to the current language of this button
        if (buttonName != null && !(string.Equals(buttonName,
                JsonManager.instance.currentTextLanguage)))
        {
            //check if this object has a buttonName defined
            ExceptionHandler.instance.StringNullOrWhiteException(buttonName,
            transform.name + " has an invalid text.");

            //change the game's current language
            JsonManager.instance.currentTextLanguage = buttonName;
            ChangeTextLanguage();

            //save the current language on the Unity PlayerPrefs for future acess
            if (PlayerPrefs.HasKey(PlayerSetting.LANGUAGE_KEY))
            {
                PlayerPrefs.SetString(PlayerSetting.LANGUAGE_KEY, buttonName);
                PlayerPrefs.Save();
            }
        }

    }

    //function for changing the text language of a button
    private void CheckButtonTextLanguage(Button button)
    {
        tempButtonData = button.GetComponent<ButtonData>();

        if(tempButtonData != null)
            JsonManager.instance.CheckButtonTextLanguageJSON(button, tempButtonData);

    }

    private void ActiationForProgressText(bool value)
    {
        completedStagesText.gameObject.SetActive(value);
        completedLevelsText.gameObject.SetActive(value);
    }

    //function showing the completed stages and completed levels on the player save data
    public void ShowProgress()
    {
        ActivationForStartingMenuButtons(false);
        ActivationForBackButton(true);
        ActiationForProgressText(true);

        string currentLanguage = JsonManager.instance.currentTextLanguage;

        string tempStringMeessage;

        completedStagesText.text = JsonManager.instance.textData[currentLanguage]["Completed Stages"].ToString();
        completedLevelsText.text = JsonManager.instance.textData[currentLanguage]["Completed Levels"].ToString();

        tempStringMeessage = ": " + PlayerPrefs.GetInt(PlayerSetting.STAGES_COMPLETED_KEY);
        completedStagesText.text += tempStringMeessage;

        tempStringMeessage = ": " + PlayerPrefs.GetInt(PlayerSetting.TOTAL_LEVELS_COMPLETED_KEY);
        completedLevelsText.text += tempStringMeessage;
    }

    //return to the previous page of the level stage buttons
    public void PreviousStageLevelsPage()
    {
        int currentLevelPage = PlayerPrefs.GetInt(PlayerSetting.CURRENT_LEVEL_PAGE_KEY);
        currentLevelPage--;

        ChangeStageLevels(currentLevelPage);
    }

    //go to the next page of the level stage buttons
    public void NextStageLevelsPage()
    {
        int currentLevelPage = PlayerPrefs.GetInt(PlayerSetting.CURRENT_LEVEL_PAGE_KEY);
        currentLevelPage++;

        ChangeStageLevels(currentLevelPage);
    }

    public void ContinueLastGame()
    {
        int lastStagePlayed = PlayerPrefs.GetInt(PlayerSetting.CURRENT_PLAYING_STAGE_KEY);
        int lastLevelPlayed = PlayerPrefs.GetInt(PlayerSetting.CURRENT_STAGE_LEVEL_SELECTED_KEY);

        if (lastStagePlayed > 0 && lastStagePlayed <= StageManager.instance.GetGameTotalStages() &&
            lastLevelPlayed > 0 && lastLevelPlayed <= StageManager.instance.GetTotalLevelsFromStage(lastStagePlayed))
        {
            PlayerPrefs.SetInt(PlayerSetting.CURRENT_STAGE_KEY, lastStagePlayed);
            PlayerPrefs.Save();

            SceneManager.LoadScene("GameScene");
        }

        else Debug.LogError("Cannot continue last game played with stage and level " + lastStagePlayed
            + ' ' + lastLevelPlayed);
    }

    //loading the game scene
    public void LoadGameScene()
    {
        GameObject currentButton = EventSystem.current.currentSelectedGameObject;

        ButtonData tempData = currentButton.GetComponent<ButtonData>();

        PlayerPrefs.SetInt(PlayerSetting.CURRENT_STAGE_LEVEL_SELECTED_KEY, tempData.buttonLevel);
        PlayerPrefs.Save();

        SceneManager.LoadScene("GameScene");
    }
}