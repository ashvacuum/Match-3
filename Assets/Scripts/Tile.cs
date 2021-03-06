﻿using System.Collections;
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

    private float tileMovementSpeed = 0.1f;

    private void Start() {
        _match = FindObjectOfType<MatchFinder>();
        _board = FindObjectOfType<BoardSetup>();
        
    }

    private void OnMouseDown() {
        /*
        if (board.currentState == GameState.move) {
            
        }*/
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp() {
        finalTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalcAngle();
    }


    void CalcAngle() {
        if (Mathf.Abs(finalTouchPos.y - firstTouchPos.y) > swipeResist || Mathf.Abs(finalTouchPos.x - firstTouchPos.x) > swipeResist) {
            //board.currentState = GameState.wait;
            swipeAngle = Mathf.Atan2(finalTouchPos.y - firstTouchPos.y, finalTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI;
            Moves();
            //board.currentDot = this;
        } else {
            //board.currentState = GameState.move;
        }

    }

    private void Update() {
        TileMotion();
    }

    private void TileMotion() {

        targetX = column;
        targetY = row;

        if ((Mathf.Abs(targetX - transform.position.x)) > .1) {
            //Move towards target
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, tileMovementSpeed);
            if (_board.tilePieces[column, row] != this.gameObject) {
                _board.tilePieces[column, row] = this.gameObject;                
                _match.FindAllMatches();
            }
        } else {
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
        }

        if ((Mathf.Abs(targetY - transform.position.y)) > .1) {
            //Move towards target
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, tileMovementSpeed);
            if (_board.tilePieces[column, row] != this.gameObject) {
                _board.tilePieces[column, row] = this.gameObject;
                _match.FindAllMatches();
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
            //Debug.Log($"{column}, {row}, {otherDot.GetComponent<Tile>().column}, {otherDot.GetComponent<Tile>().row}");
            StartCoroutine(CheckMoveCo());
            
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

    private void OnDestroy() {
        //Debug.Log($"Destoryed {this.gameObject.name}");
    }


    private IEnumerator CheckMoveCo() {
        yield return new WaitForSeconds(0.3f);
        if (otherDot != null) {
            Tile dotTile = otherDot.GetComponent<Tile>();
            if (!isMatched && !dotTile.isMatched) {
                dotTile.row = row;
                dotTile.column = column;
                row = previousRow;
                column = previousCol;
                yield return null;
                //Debug.Log("No matches");
                //board.currentState = GameState.move;
            } else {/*
                if (endGameMgr != null) {
                    if (endGameMgr.reqs.gameType == GameType.Moves) {
                        endGameMgr.DecreaseCounterValue();
                    }
                }*/
                //Debug.Log("Matches Found");
                _board.DestroyMatches();
            }

            //otherDot = null;
        }
    }
}
