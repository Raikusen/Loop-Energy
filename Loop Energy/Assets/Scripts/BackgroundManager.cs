using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void ChangeBackgroundColorStage1()
    {
        GameCamera.backgroundColor = Color.yellow;
    }

    public void ChangeBackgroundColorStage2()
    {
        GameCamera.backgroundColor = Color.green;
    }

    public void ChangeBackgroundColorStage3()
    {
        GameCamera.backgroundColor = Color.red;
    }

    //try to change
    public void setCameraBackgroundColor(float r, float g, float b)
    {
        cameraBackgroundColor.r = r;
        cameraBackgroundColor.g = g;
        cameraBackgroundColor.b = b;
        cameraBackgroundColor.a = 255;

        GameCamera.backgroundColor = cameraBackgroundColor;
    }
}
