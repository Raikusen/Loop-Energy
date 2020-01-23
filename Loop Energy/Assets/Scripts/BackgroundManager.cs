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

        SaveColor("blue");
    }

    public void ChangeBackgroundColorStage1()
    {
        GameCamera.backgroundColor = Color.yellow;

        SaveColor("yellow");
    }

    public void ChangeBackgroundColorStage2()
    {
        GameCamera.backgroundColor = Color.green;

        SaveColor("green");
    }

    public void ChangeBackgroundColorStage3()
    {
        GameCamera.backgroundColor = Color.red;

        SaveColor("red");
    }

    public void SaveColor(string colorString)
    {
        PlayerPrefs.SetString(PlayerSetting.CAMERA_BACKGROUND_COLOR, colorString);
        PlayerPrefs.Save();
    }
}