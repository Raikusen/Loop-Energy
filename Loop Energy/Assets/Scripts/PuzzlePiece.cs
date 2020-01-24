using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class implementing the behaviour of a game piece
public class PuzzlePiece : MonoBehaviour
{

    //if the piece object is being hold
    private bool pieceBeingHold = false;

    //position of the piece while it is not moving
    [HideInInspector]
    public Vector2 currentStationaryPosition;

    //the touch object made by the player input on mobile
    private Touch playerInputTouch;

    //the position where the player is touching or clicking currently
    private Vector2 touchPosition;

    //eaasing of the piece while being moved
    private int speedEasing = 100;

    //if a piece is currently moving
    private static bool moving = false;

    //index of piece on the puzzle pieces list
    [HideInInspector]
    public int puzzlePiecesIndex;

    //if the piece is colliding with another piece or not
    private bool colliding = false;

    //Vector2 variable to be used in update function
    private Vector2 tempPos;

    //if collisions can be triggered or not
    private bool triggerCollision = false;

    //getting the spriteRender of the game object, for changing layer order
    private SpriteRenderer spriteRenderer;

    //images relative to the state the piece is currently in
    [SerializeField]
    private Sprite normalPieceSprite;

    [SerializeField]
    private Sprite lightPieceSprite;

    [SerializeField]
    private Sprite pressedPieceSprite;

    //maximum Y value the piece can achieve
    private float limitYPiece;

    //the piece prefb type
    private string pieceType;

    private bool pieceIsInCorrectPosition = false;

    void Awake()
    {
        currentStationaryPosition = transform.position;

        spriteRenderer = GetComponent<SpriteRenderer>();

        limitYPiece = GameManager.instance.limitYPiece;
    }

    void Update()
    {
        //if the game is not playble, stop input events on the pieces
        if (GameManager.instance.GetGameIsPlayable() == false)
            return;

        //if the player made a click on the mobile device
        if (SystemInfo.deviceType == DeviceType.Handheld && (moving == false || pieceBeingHold == true))
        {
            playerInputTouch = Input.GetTouch(0);
            //get the touch position related to the world space
            touchPosition = Camera.main.ScreenToWorldPoint(playerInputTouch.position);

            switch (playerInputTouch.phase)
            {
                //player touched on a piece, that one will began to move
                case TouchPhase.Began:
                    if (GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPosition))
                        SetPieceMovement(true);
                    break;

                //if the player is moving his finger
                case TouchPhase.Moved:
                    if (GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPosition) &&
                        transform.position.y <= GameManager.instance.limitYPiece)
                        MovePieceByInput();
                    break;

                 //when the plyer releases his finger
                case TouchPhase.Ended:
                    ReturnObjectToOldStationaryPosition();
                    break;
            }
        }

        //if a left mouse click was made
        else if(SystemInfo.deviceType == DeviceType.Desktop && Input.GetMouseButton(0) 
            && (moving == false || pieceBeingHold == true))
        {
            tempPos.x = Input.mousePosition.x;
            tempPos.y = Input.mousePosition.y;

            //get the touch position related to the world space
            touchPosition = Camera.main.ScreenToWorldPoint(tempPos);

            //if piece is being clicked, set piece movement to true
            if (pieceBeingHold == false &&
                GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPosition))
                SetPieceMovement(true);
            
            //moving the piece while clicking on it
            else if(moving == true && pieceBeingHold == true)
                MovePieceByInput();

        }

        //when the mouse button is releaased stop moving the piece
        else if(SystemInfo.deviceType == DeviceType.Desktop && !Input.GetMouseButton(0)
            && moving == true && pieceBeingHold == true && colliding == false)
            ReturnObjectToOldStationaryPosition();
    }

    //verify is a piece can change positions with another, when both are on collision
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(pieceBeingHold == true)
        {
            //colliding flag to true since a collision was detected
            if(colliding == false)
                colliding = true;

            //if a click or input is being made while two pieces are colliding
            if(!Input.GetMouseButton(0) || playerInputTouch.phase == TouchPhase.Ended)
            {
                Vector2 grabbedPiecePosition = currentStationaryPosition;
                Vector2 collidingPiecePosition = collision.gameObject.transform.position;

                //graabbed piece changes position
                transform.position = collidingPiecePosition;
                currentStationaryPosition = collidingPiecePosition;

                //colliding piece changes position
                PuzzlePiece tempPuzzlePiece = collision.gameObject.GetComponent<PuzzlePiece>();
                collision.gameObject.transform.position = grabbedPiecePosition;
                tempPuzzlePiece.currentStationaryPosition = grabbedPiecePosition;

                //check result of trading the position of the two pieces
                GameManager.instance.CheckTradePieces(this, tempPuzzlePiece);

                AudioManager.instance.PlayPieceDroppedSound();

                SetPieceMovement(false);
            }
        }
   
    }

    //piece is no longer colliding
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (colliding == true)
            colliding = false;
    }

    private void SetPieceMovement(bool value)
    {
        pieceBeingHold = value;
        moving = value;

        //if the piece is moving, increase the layer order for the grabbed piece appearing 
        //first on the screen when colliding with other pieces
        if (value == true)
        {
            spriteRenderer.sortingOrder = 2;
            ChangeToPressedSprite();
        }

        else
        {
            spriteRenderer.sortingOrder = 1;

            if(pieceIsInCorrectPosition == false)
                ChangeToNormalSprite();

            else ChangeToLightSprite();
        }
    }

    //moving a piece based on the input or touch position
    private void MovePieceByInput()
    {
        tempPos = transform.position;

        transform.position = Vector2.Lerp(transform.position, touchPosition, speedEasing * Time.deltaTime);

        //if piece y position is greater than the YLimit, return to previous frame position 
        if (transform.position.y > GameManager.instance.limitYPiece)
            transform.position = tempPos;
    }

    //object returns to the position it was before moving
    private void ReturnObjectToOldStationaryPosition()
    {
        SetPieceMovement(false);
        transform.position = currentStationaryPosition;
    }

    //changing the puzzle piece sprite image, according to the game events
    public void ChangeToNormalSprite()
    {
        spriteRenderer.sprite = normalPieceSprite;
    }

    public void ChangeToLightSprite()
    {
        spriteRenderer.sprite = lightPieceSprite;
    }

    public void ChangeToPressedSprite()
    {
        spriteRenderer.sprite = pressedPieceSprite;
    }

    public static bool GetPiecesAreMoving()
    {
        return moving;
    }

    public string GetPieceType()
    {
        return pieceType;
    }

    public void SetPieceType(string type)
    {
        pieceType = type;
    }

    public bool GetPieceIsInCorrectPosition()
    {
        return pieceIsInCorrectPosition;
    }

    public void SetPieceIsInCorrectPosition(bool value)
    {
        pieceIsInCorrectPosition = value;
    }
}