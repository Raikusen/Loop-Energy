using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//temp, needed for handling exceptions
using System;

public class ExceptionHandler : MonoBehaviour
{
    //singleton instance of this class
    [HideInInspector] public static ExceptionHandler instance;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    //if a determinated string is null or empty, throw a given message
    public void StringNullOrWhiteException(string checkString, string exceptionMessage)
    {
        if (string.IsNullOrWhiteSpace(checkString))
        {
            throw new ArgumentNullException(exceptionMessage);
        }       
    }

    //if a determinated object is null, throw a given message
    public void NullReferenceException(string message)
    {
        throw new NullReferenceException(message);
    }
}