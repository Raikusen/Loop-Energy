using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class that contains the audio source of a button click
public class AudioManager : MonoBehaviour
{
    //audio source containing the button audio clip
    private AudioSource buttonClickAudio;

    // Start is called before the first frame update
    void Start()
    {
        buttonClickAudio = GetComponent<AudioSource>();

        if (buttonClickAudio == null)
            ExceptionHandler.instance.NullReferenceException("button audio component does not exist");

        else if(buttonClickAudio.clip == null)
            ExceptionHandler.instance.NullReferenceException("button audio clip does not exist");
    }

    public void PlayButtonClickSound()
    {
        buttonClickAudio.Play();
    }
}
