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

    public void ReturnToMainMenu()
    {
        //clean game UI buttons
        ActivateGameMenuButtons(false);
        gameMenuButton.gameObject.SetActive(false);

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
    }

    public void ClickButtonSound()
    {
        AudioManager.instance.PlayButtonClickSound();
    }
}