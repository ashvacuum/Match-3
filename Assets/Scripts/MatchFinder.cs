using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchFinder : MonoBehaviour
{

    public List<GameObject> currentMatches = new List<GameObject>();
    private BoardSetup _board;

    private void Start() {
        _board = FindObjectOfType<BoardSetup>();
    }
    public void FindAllMatches() {
        StartCoroutine(FindAllMatchesCo());
    }

    private void AddToListAndMatch(GameObject _dot) {
        if (!currentMatches.Contains(_dot)) {
            currentMatches.Add(_dot);
        }
        _dot.GetComponent<Tile>().isMatched = true;
    }

    private void GetNearbyPieces(GameObject _dot1, GameObject _dot2, GameObject _dot3) {
        AddToListAndMatch(_dot1);
        AddToListAndMatch(_dot2);
        AddToListAndMatch(_dot3);
    }

    private IEnumerator FindAllMatchesCo() {
        //yield return new WaitForSeconds(.2f);
        yield return null;
        for (int i = 0; i < _board.width; i++) {
            for (int j = 0; j < _board.height; j++) {
                GameObject _currentDot = _board.tilePieces[i, j];
                if (_currentDot != null) {
                    Tile currentDot_C = _currentDot.GetComponent<Tile>();
                    if (i > 0 && i < _board.width - 1) {
                        GameObject leftDot = _board.tilePieces[i - 1, j];
                        GameObject rightDot = _board.tilePieces[i + 1, j];
                        if (leftDot != null && rightDot != null) {
                            Tile leftDot_C = leftDot.GetComponent<Tile>();
                            Tile rightDot_C = rightDot.GetComponent<Tile>();
                            if (leftDot.tag == _currentDot.tag && rightDot.tag == _currentDot.tag) {
                                GetNearbyPieces(leftDot, _currentDot, rightDot);
                            }
                        }
                    }
                    if (j > 0 && j < _board.height - 1) {
                        GameObject downDot = _board.tilePieces[i, j - 1];
                        GameObject upDot = _board.tilePieces[i, j + 1];
                        if (upDot != null && downDot != null) {
                            Tile upDot_C = upDot.GetComponent<Tile>();
                            Tile downDot_C = downDot.GetComponent<Tile>();
                            if (downDot.tag == _currentDot.tag && upDot.tag == _currentDot.tag) {
                                GetNearbyPieces(upDot, _currentDot, downDot);
                            }
                        }
                    }
                }
            }
        }
    }


}
