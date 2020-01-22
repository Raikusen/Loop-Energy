using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class responsible for changing the camera background
public class BackgroundManager : MonoBehaviour
{
    [SerializeField]
    private Camera GameCamera;

    private Color cameraBackgroundColor;

    // Start is called before the first frame update
    void Start()
    {
        if(GameCamera == null)
            ExceptionHandler.instance.NullReferenceException("there is not a camera associated to the Background Manager.");
    }

    public void ChangeBackgroundColorMainMenu()
    {
        GameCamera.backgroundColor = Color.blue;

        PlayerPrefs.SetString(PlayerSetting.CAMERA_BACKGROUND_COLOR, "blue");
        PlayerPrefs.Save();
    }

    public void ChangeBackgroundColorStage1()
    {
        GameCamera.backgroundColor = Color.yellow;

        PlayerPrefs.SetString(PlayerSetting.CAMERA_BACKGROUND_COLOR, "yellow");
        PlayerPrefs.Save();
    }

    public void ChangeBackgroundColorStage2()
    {
        GameCamera.backgroundColor = Color.green;

        PlayerPrefs.SetString(PlayerSetting.CAMERA_BACKGROUND_COLOR, "green");
        PlayerPrefs.Save();
    }

    public void ChangeBackgroundColorStage3()
    {
        GameCamera.backgroundColor = Color.red;

        PlayerPrefs.SetString(PlayerSetting.CAMERA_BACKGROUND_COLOR, "red");
        PlayerPrefs.Save();
    }
}