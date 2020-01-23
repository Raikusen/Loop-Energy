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

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;

        LoadBackgroundColorCamera();
    }

    public void ReturnToMainMenu()
    {
        //clean game UI buttons
        ActivateGameMenuButtons(false);
        gameMenuButton.gameObject.SetActive(false);

        //destroy instances that were passed by the previous scene
        JsonManager.instance.DestroyJSONInstance();
        AudioManager.instance.DestroyAudioManagerInstance();

        gameCamera.backgroundColor = Color.blue;

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
        previousLevelButton.gameObject.SetActive(value);
        nextLevelButton.gameObject.SetActive(value);
        quitButton.gameObject.SetActive(value);
        resumeLevelButton.gameObject.SetActive(value);
        restartLevelButton.gameObject.SetActive(value);

        if (value == true)
        {
            CheckButtonTextLanguage(previousLevelButton);
            CheckButtonTextLanguage(nextLevelButton);
            CheckButtonTextLanguage(quitButton);
            CheckButtonTextLanguage(resumeLevelButton);
            CheckButtonTextLanguage(restartLevelButton);
        }

        else CheckButtonTextLanguage(gameMenuButton);
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

        GameManager.instance.SetGameIsPlayable(true);

        ActivateLevelCompletedText(true);
        ActivateGameMenuButtons(true);

        GameManager.instance.SetGameIsPlayable(false);
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