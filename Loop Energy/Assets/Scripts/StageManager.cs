using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class that holds the amount of levels that each stage has
public class StageManager : MonoBehaviour
{
    //singleton instance of this class
    [HideInInspector] public static StageManager instance;

    //the amount of total levels on each staage
    private int stage1Levels = 4;
    private int stage2Levels = 5;
    private int stage3Levels = 2;

    //the amount of stages the gaame haas
    private int totalGameStages = 3;

    //an array containing the levels that each stage has
    private int[] levelsPerStageArray;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    void Start()
    {
        levelsPerStageArray = new int[totalGameStages];

        levelsPerStageArray[0] = stage1Levels;
        levelsPerStageArray[1] = stage2Levels;
        levelsPerStageArray[2] = stage3Levels;
    }

    public int GetTotalLevelsFromStage(int stage)
    {
        if (stage < 0 || (levelsPerStageArray.Length + 1) <= stage)
            Debug.LogError("stage number or arraay length are incorrect. " + stage);

        return levelsPerStageArray[(stage - 1)];   
    }

    public bool checkIfStageIsCompleted(int stage)
    {
        if (stage < 0 || (levelsPerStageArray.Length + 1) <= stage)
            Debug.LogError("stage number or arraay length are incorrect. " + stage);

        int completedStages = PlayerPrefs.GetInt(PlayerSetting.STAGES_COMPLETED_KEY);

        if (completedStages >= stage)
            return true;

        else return false;
    }
}