using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class that contains the audio source of a button click
public class AudioManager : MonoBehaviour
{
    //audio source containing the button audio clip
    private AudioSource buttonClickAudio;

    private AudioSource pieceClickAudio;

    private AudioSource levelCompletedAudio;

    //singleton instance of this class
    [HideInInspector] public static AudioManager instance;

    [SerializeField]
    private GameObject pieceClickAudioObject;

    [SerializeField]
    private GameObject levelCompletedAudioObject;

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

        CheckAudioClipException(buttonClickAudio, "buttonClick");

        pieceClickAudio = pieceClickAudioObject.GetComponent<AudioSource>();

        CheckAudioClipException(pieceClickAudio, "pieceClick");

        levelCompletedAudio = levelCompletedAudioObject.GetComponent<AudioSource>();

        CheckAudioClipException(levelCompletedAudio, "levelCompletedAudio");

        //this object is not removed when changing scenes, in order for background music to continue
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayButtonClickSound()
    {
        buttonClickAudio.Play();
    }

    public void PlayPieceDroppedSound()
    {
        pieceClickAudio.Play();
    }

    public void PlayLevelCompletedSound()
    {
        levelCompletedAudio.Play();
    }

    public void StopLevelCompletedSound()
    {
        levelCompletedAudio.Stop();
    }

    public void DestroyAudioManagerInstance()
    {
        if (instance != null)
            Destroy(gameObject);
    }

    public void CheckAudioClipException(AudioSource audioSource, string message)
    {
        if (audioSource == null)
            ExceptionHandler.instance.NullReferenceException("audio component does not exist from " + message);

        else if (audioSource.clip == null)
            ExceptionHandler.instance.NullReferenceException("audio clip does not exist from " + message);
    }
}
