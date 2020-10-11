using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousCol;
    public int previousRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;


    [Header("Swipe Adjustment")]
    public float swipeAngle = 0;
    public float swipeResist = 1f;

    [Header("Tile Movement")]
    public GameObject otherDot;
    private Vector2 tempPos;

    private Vector2 firstTouchPos = Vector2.zero;
    private Vector2 finalTouchPos = Vector2.zero;


    private BoardSetup _board;

    private MatchFinder _match;



    private void Start() {
        _match = FindObjectOfType<MatchFinder>();
        _board = FindObjectOfType<BoardSetup>();
    }

    private void Update() {
        //TileMotion();
    }

    private void TileMotion() {

        targetX = column;
        targetY = row;

        if ((Mathf.Abs(targetX - transform.position.x)) > .1) {
            //Move towards target
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, 0.6f);
            if (_board.tilePieces[column, row] != this.gameObject) {
                _board.tilePieces[column, row] = this.gameObject;
                //TODO FIX
                //findMatches.FindAllMatches();

            }


        } else {
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
        }

        if ((Mathf.Abs(targetY - transform.position.y)) > .1) {
            //Move towards target
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, 0.6f);
            if (_board.tilePieces[column, row] != this.gameObject) {
                _board.tilePieces[column, row] = this.gameObject;

                //TODO FIX
                //findMatches.FindAllMatches();
            }

        } else {
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = tempPos;
        }
    }

    void SwipeMove(Vector2 direction) {
        otherDot = _board.tilePieces[column + (int)direction.x, row + (int)direction.y];
        previousRow = row;
        previousCol = column;
        if (otherDot != null) {
            otherDot.GetComponent<Tile>().column += -1 * (int)direction.x;
            otherDot.GetComponent<Tile>().row += -1 * (int)direction.y;
            column += (int)direction.x;
            row += (int)direction.y;
            //StartCoroutine(CheckMoveCo());
        }
    }

    void Moves() {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < BoardSetup.board.width - 1) {
            //Right Swipe
            SwipeMove(Vector2.right);
        } else if (swipeAngle > 45 && swipeAngle <= 135 && row < BoardSetup.board.height - 1) {
            //Up Swipe
            SwipeMove(Vector2.up);
        } else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0) {
            //Left Swipe
            SwipeMove(Vector2.left);
        } else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0) {
            //Down
            SwipeMove(Vector2.down);
        } else {
            //board.currentState = GameState.move;
        }
    }

    private IEnumerator CheckMoveCo() {
        if (otherDot != null) {
            Tile dotTile = otherDot.GetComponent<Tile>();
            if (!isMatched && !dotTile.isMatched) {
                dotTile.row = row;
                dotTile.column = column;
                row = previousRow;
                column = previousCol;
                yield return new WaitForSeconds(0.5f);                
                //board.currentState = GameState.move;
            } else {/*
                if (endGameMgr != null) {
                    if (endGameMgr.reqs.gameType == GameType.Moves) {
                        endGameMgr.DecreaseCounterValue();
                    }
                }*/
                _board.DestroyMatches();
            }

            //otherDot = null;
        }
    }
}
