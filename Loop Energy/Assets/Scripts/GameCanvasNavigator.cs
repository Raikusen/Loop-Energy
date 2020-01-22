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

    void Awake()
    {
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
        gameMenuButton.gameObject.SetActive(!value);

        gameMenuPanel.gameObject.SetActive(value);

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

    //review function
    private void LoadBackgroundColorCamera()
    {
        if (gameCamera == null)
            ExceptionHandler.instance.NullReferenceException("there is not a camera associated to the Background Manager.");

        string cameraColor = PlayerPrefs.GetString("cameraBackgroundColor");

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