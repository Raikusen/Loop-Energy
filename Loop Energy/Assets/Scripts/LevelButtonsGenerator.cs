using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonsGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject PrefabButtonGameObject;

    public void CreateStageLevelButtons(int min, int max, int stage)
    {
        StartMenuNavigator.instance.ActivationForStageButtons(false);
        StartMenuNavigator.instance.ActivationForBackButton(true);
    }
}
