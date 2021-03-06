﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using LitJson;

//class managing the game state of a playable level
public class GameManager : MonoBehaviour
{
    //singleton instance of this class
    [HideInInspector] public static GameManager instance;

    //division line to separate the puzzle pieces and the game menu button
    [SerializeField]
    private GameObject divisionLine;

    //the y limit where pieces cannot move further than the value stipulated
    [HideInInspector] public float limitYPiece;
    private float limitYCutoff = 0.5f;

    //the playable puzzle pieces avaialable on the level
    private List<PuzzlePiece> puzzlePiecesList;

    private int numberOfCorrectPieces = 0;

    //a list containing the correct solution for the level
    private List<string> rightSolutionList;

    private int totalNumberOfPieces;

    private int currentStage;

    private int currentStageLevel;

    //the playable pieces prefabs
    [SerializeField]
    private PuzzlePiece spherePrefab;

    [SerializeField]
    private PuzzlePiece rectanglePrefab;

    private bool gameIsPlayable = true;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        limitYPiece = divisionLine.transform.position.y - limitYCutoff;

        currentStageLevel = PlayerPrefs.GetInt(PlayerSetting.CURRENT_STAGE_LEVEL_SELECTED_KEY);

        if (currentStageLevel > 0)
            LoadStageLevel(currentStageLevel);
    }

    //loding the given level on the current stage selected
    public void LoadStageLevel(int level)
    {
        //get the current stage and update the current playing stage key
        currentStage = PlayerPrefs.GetInt(PlayerSetting.CURRENT_STAGE_KEY);

        PlayerPrefs.SetInt(PlayerSetting.CURRENT_PLAYING_STAGE_KEY, currentStage);
        PlayerPrefs.Save();

        SetGameIsPlayable(true);

        numberOfCorrectPieces = 0;

        //bad solution should have used object prefabing,
        //deleting old puzzle pieces when loading a new level
        if (puzzlePiecesList != null && puzzlePiecesList.Count != 0)
        {
            PuzzlePiece objectDestroyed;
            for (int i = 0; i < puzzlePiecesList.Count; i++)
            {
                objectDestroyed = puzzlePiecesList[i];
                Destroy(objectDestroyed.gameObject);
                puzzlePiecesList[i] = null;
            }

            puzzlePiecesList.Clear();
        }

        else puzzlePiecesList = new List<PuzzlePiece>();

        //deleating previous right solution 
        if (rightSolutionList != null && rightSolutionList.Count > 0)
            rightSolutionList.Clear();

        else rightSolutionList = new List<string>();

        string stageKey = "Stage " + currentStage;
        string levelKey = "Level " + currentStageLevel;

        if(JsonManager.instance.levelData[stageKey][levelKey] == null)
            throw new NullReferenceException("level key cannot be found on game manager.");

        //adding puzzle pieces to puzzlePiecesList
        else
        {
            totalNumberOfPieces = JsonManager.instance.levelData[stageKey][levelKey].Count - 1;

            int i;

            for (i = 0; i < totalNumberOfPieces; i++)
                CheckPrefabSpawn(JsonManager.instance.levelData[stageKey][levelKey][i]);

            //adding the corresponding piece types to the right solution
            for(i = 0; i < totalNumberOfPieces; i++)
            {
                string pieceMessage = JsonManager.instance.levelData[stageKey][levelKey][totalNumberOfPieces]["solution"][i].ToString();

                rightSolutionList.Add(pieceMessage);
            }

            //check which pieces are already on a correct position
            CheckCurrentSolution();
        }
    }

    //check which prefab is supposed to be instantiated
    private void CheckPrefabSpawn(JsonData jsonData)
    {
        if(jsonData["type"] == null)
            throw new NullReferenceException("prefab type cannot be found on game manager.");

        else
        {
            //getting transform positions
            double startPosX = (double)(int)jsonData["startX"];
            double startPosY = (int)jsonData["startY"];
            double incrementalPosX = (double)(int)jsonData["decimalAddX"] * 0.1;
            double incrementalPosY = (double)(int)jsonData["decimalAddY"] * 0.1;

            startPosX += incrementalPosX;
            startPosY += incrementalPosY;

            if (jsonData["type"].ToString() == "circle")
                SpawnCircle(startPosX, startPosY);

            else if (jsonData["type"].ToString() == "rectangle")
                SpawnRectangle(startPosX, startPosY);

        }
    }

    private void SpawnCircle(double startX, double startY)
    {
        SpawnPiece(startX, startY, spherePrefab, "circle");
    }

    private void SpawnRectangle(double startX, double startY)
    {
        SpawnPiece(startX, startY, rectanglePrefab, "rectangle");
    }

    private void SpawnPiece(double startX, double startY, PuzzlePiece puzzlePiecePrefab, string pieceType)
    {
        Vector2 pieceNewPosition = new Vector2((float)startX, (float)startY);

        PuzzlePiece tempPuzzlePiece;

        tempPuzzlePiece = Instantiate(puzzlePiecePrefab, pieceNewPosition, Quaternion.identity);

        tempPuzzlePiece.SetPieceType(pieceType);

        //set the index of the piece to the position it is located on the puzzle pieces list
        tempPuzzlePiece.puzzlePiecesIndex = puzzlePiecesList.Count;

        //adding the spawned piece to the puzzle pieces list
        puzzlePiecesList.Add(tempPuzzlePiece);
    }

    public bool GetGameIsPlayable()
    {
        return gameIsPlayable;
    }

    public void SetGameIsPlayable(bool value)
    {
        gameIsPlayable = value;
    }

    //check starting solution after loading the level
    //it is not checking if the level is currently completed already
    public void CheckCurrentSolution()
    {
        if(puzzlePiecesList.Count != rightSolutionList.Count)
            throw new InvalidOperationException("puzzle pieces list size is not the same as the solution list size");

        //the pieces are correct if the types order of the puzzle list, than were placed
        //like the order written on the level JSON file, is the same as the order of the right solution list 
        for (int i = 0; i < puzzlePiecesList.Count; i++)
        {
            //if piece is on a correct position, change it's sprite to the light one
            if (puzzlePiecesList[i].GetPieceType() == rightSolutionList[i])
            {
                numberOfCorrectPieces++;
                puzzlePiecesList[i].ChangeToLightSprite();
                puzzlePiecesList[i].SetPieceIsInCorrectPosition(true);
            }
                
        } 
    }

    //check the result of trading two puzzle pieces positions
    public void CheckTradePieces(PuzzlePiece pieceA, PuzzlePiece pieceB)
    {
        int oldIndexA = pieceA.puzzlePiecesIndex;
        int oldIndexB = pieceB.puzzlePiecesIndex;

        bool oldCorrectPosA = pieceA.GetPieceIsInCorrectPosition();
        bool oldCorrectPosB = pieceB.GetPieceIsInCorrectPosition();

        pieceA.puzzlePiecesIndex = oldIndexB;
        pieceB.puzzlePiecesIndex = oldIndexA;

        puzzlePiecesList[pieceA.puzzlePiecesIndex] = pieceA;
        puzzlePiecesList[pieceB.puzzlePiecesIndex] = pieceB;

        CheckExchangeOfPiecesPosition(pieceA, oldCorrectPosA);
        CheckExchangeOfPiecesPosition(pieceB, oldCorrectPosB);
    }

    //check the result of a piece moving from one position to a position with another piece on it
    private void CheckExchangeOfPiecesPosition(PuzzlePiece puzzlePiece, bool oldCorrectValue)
    {
        //piece moved from a correct to an incorrect position
        if (puzzlePiecesList[puzzlePiece.puzzlePiecesIndex].GetPieceType() != rightSolutionList[puzzlePiece.puzzlePiecesIndex] &&
           oldCorrectValue == true)
        {
            puzzlePiece.SetPieceIsInCorrectPosition(false);
            puzzlePiece.ChangeToNormalSprite();
            numberOfCorrectPieces--;
        }

        //piece moved from an incorrect to a correct position
        else if (puzzlePiecesList[puzzlePiece.puzzlePiecesIndex].GetPieceType() == rightSolutionList[puzzlePiece.puzzlePiecesIndex] &&
           oldCorrectValue == false)
        {
            puzzlePiece.SetPieceIsInCorrectPosition(true);
            puzzlePiece.ChangeToLightSprite();
            numberOfCorrectPieces++;
        }

        CheckLevelCompleted();
    }

    private void CheckLevelCompleted()
    {
        //level is completed if all pieces are on correct positions
        if (numberOfCorrectPieces == totalNumberOfPieces)
        {
            GameManager.instance.gameIsPlayable = false;

            //small delay after level is complete
            StartCoroutine(DelayTime(0.4f));
            
        }
    }

    private IEnumerator DelayTime(float time)
    {
        yield return new WaitForSeconds(time);
        GameCanvasNavigator.instance.CompletedLevel();
    }

    //reset the level to the starting pieces position
    public void ResetLevel()
    {
        AudioManager.instance.StopLevelCompletedSound();
        GameCanvasNavigator.instance.ActivateLevelCompletedText(false);

        currentStageLevel = PlayerPrefs.GetInt(PlayerSetting.CURRENT_STAGE_LEVEL_SELECTED_KEY);
        currentStage = PlayerPrefs.GetInt(PlayerSetting.CURRENT_STAGE_KEY);

        LoadStageLevel(currentStageLevel);
        GameCanvasNavigator.instance.ActivateGameMenuButtons(false);
    }
}