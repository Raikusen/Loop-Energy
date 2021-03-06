﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class that holds the amount of levels that each stage has
public class StageManager : MonoBehaviour
{
    //singleton instance of this class
    [HideInInspector] public static StageManager instance;

    //the amount of total levels on each stage
    private int stage1Levels = 4;
    private int stage2Levels = 5;
    private int stage3Levels = 2;

    //the amount of stages the game has
    private int totalGameStages = 3;

    //an array containing the levels that each stage has
    private int[] levelsPerStageArray;

    private int totalGamelevels = 0;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    void Start()
    {
        //assign to an array the amount of levels each stage has
        levelsPerStageArray = new int[totalGameStages];

        levelsPerStageArray[0] = stage1Levels;
        levelsPerStageArray[1] = stage2Levels;
        levelsPerStageArray[2] = stage3Levels;

        for (int i = 0; i < levelsPerStageArray.Length; i++)
            totalGamelevels += levelsPerStageArray[i];

        //do not destroy this object 
        DontDestroyOnLoad(this.gameObject);
    }

    //the amount of levels a stage has
    public int GetTotalLevelsFromStage(int stage)
    {
        if (stage < 0 || (totalGamelevels + 1) <= stage)
            Debug.LogError("stage number is incorrect. " + stage);

        return levelsPerStageArray[(stage - 1)];   
    }

    //get the level number from a stage, corresponding from the total game levels
    public int GetStageLevelNubmerFromTotalLevels(int stage, int level)
    {
        //the level number related to all stage levels
        int levelSum = 0;

        if (stage < 0 || stage > levelsPerStageArray.Length)
            Debug.LogError("stage number is incorrect. " + stage);

        else if (level < 0 || levelsPerStageArray[(stage - 1)] < level)
            Debug.LogError("level number is incorrect. " + level);

        else
        {
            int i;
            int stageCount = stage - 1;

            //if stage of the level is greater than one, add the previous stages total levels
            for (i = 0; i < stageCount; i++)
                levelSum += levelsPerStageArray[i];

            levelSum += level;
        }

        return levelSum;
    }

    //check if a stage is already completed
    public bool CheckIfStageIsCompleted(int stage)
    {
        if (stage < 0 || (levelsPerStageArray.Length + 1) <= stage)
            Debug.LogError("stage number or arraay length are incorrect. " + stage);

        int completedStages = PlayerPrefs.GetInt(PlayerSetting.STAGES_COMPLETED_KEY);

        if (completedStages >= stage)
            return true;

        else return false;
    }

    public int GetGameTotalLevels()
    {
        return totalGamelevels;
    }

    public int GetGameTotalStages()
    {
        return totalGameStages;
    }

    public void DestroyStageManagerInstance()
    {
        if (instance != null)
            Destroy(gameObject);
    }
}