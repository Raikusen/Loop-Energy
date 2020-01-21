using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class that contains the audio source of a button click
public class AudioManager : MonoBehaviour
{
    //audio source containing the button audio clip
    private AudioSource buttonClickAudio;

    //singleton instance of this class
    [HideInInspector] public static AudioManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        buttonClickAudio = GetComponent<AudioSource>();

        if (buttonClickAudio == null)
            ExceptionHandler.instance.NullReferenceException("button audio component does not exist");

        else if(buttonClickAudio.clip == null)
            ExceptionHandler.instance.NullReferenceException("button audio clip does not exist");

        //this object is not removed when changing scenes, in order for background music to continue
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayButtonClickSound()
    {
        buttonClickAudio.Play();
    }
}
