using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class responsible for changing the camera background
public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    private Camera gameCamera;

    private Color cameraBackgroundColor;

    void Start()
    {
        if(gameCamera == null)
            ExceptionHandler.instance.NullReferenceException("there is not a camera associated to the Background Manager.");
    }

    public void ChangeBackgroundColorMainMenu()
    {
        gameCamera.backgroundColor = Color.blue;

        SaveColor("blue");
    }

    public void ChangeBackgroundColorStage1()
    {
        gameCamera.backgroundColor = Color.yellow;

        SaveColor("yellow");
    }

    public void ChangeBackgroundColorStage2()
    {
        gameCamera.backgroundColor = Color.green;

        SaveColor("green");
    }

    public void ChangeBackgroundColorStage3()
    {
        gameCamera.backgroundColor = Color.red;

        SaveColor("red");
    }

    //change camera background according to stage number
    public void CheckChangeOnBackgroundCameraOnStage()
    {
        int stage = PlayerPrefs.GetInt(PlayerSetting.CURRENT_STAGE_KEY);

        switch(stage)
        {
            case 1:
                ChangeBackgroundColorStage1();
                break;
            case 2:
                ChangeBackgroundColorStage2();
                break;
            case 3:
                ChangeBackgroundColorStage3();
                break;
        }
    }

    //save cmeraa color key, for game scene knowing which camera to load,
    //when that scene was loaded by Continue button
    public void CheckChangeOnBackgroundCameraOnContinue()
    {
        int stage = PlayerPrefs.GetInt(PlayerSetting.CURRENT_STAGE_KEY);

        switch (stage)
        {
            case 1:
                SaveColor("yellow");
                break;
            case 2:
                SaveColor("green");
                break;
            case 3:
                SaveColor("red");
                break;
        }
    }

    public void SaveColor(string colorString)
    {
        PlayerPrefs.SetString(PlayerSetting.CAMERA_BACKGROUND_COLOR, colorString);
        PlayerPrefs.Save();
    }
}