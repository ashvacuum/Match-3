using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {
    public int width;
    public int height;
}

public class BoardSetup : MonoBehaviour {

    #region Public Variables
    /// <summary>
    /// How wide you cant your Match 3 board to be, min 4, max 10
    /// </summary>
    [Range(4, 10)]
    public int width;

    /// <summary>
    /// How tall you cant your Match 3 board to be, min 4, max 10
    /// </summary>
    [Range(4, 10)]
    public int height;

    /// <summary>
    /// Number of Unique Pieces per board min 3 max 5
    /// </summary>
    [Range(3, 5)]
    public int tilePieceCount;

    [Tooltip("All Color Variations of Tiles")]
    public GameObject[] tilePrefabs;

    [Tooltip("Prefab for the background tile")]
    public GameObject containerPrefab;

    public static Board board;


    public GameObject[,] boardContainers { get; private set; }

    public GameObject[,] tilePieces { get; private set; }
    #endregion

    #region Private variables
    private MatchFinder _finder;

    private System.Random _randomGenerator = new System.Random();
    #endregion
    private void Start() {
        _finder = FindObjectOfType<MatchFinder>();
    }

    [ContextMenu("Begin Game")]    
    public void BeginGame() {
        RemoveAll();
        if(width % 2 != 0 || height %2 != 0) {
            Debug.Log("You must set your width/height to an even number");
        }
        SetupBoard();        
    }

    /// <summary>
    /// Creates Match 3 board according to width and height
    /// </summary>
    public void SetupBoard() {
        board = new Board { height = height, width = width };
        boardContainers = new GameObject[width, height];
        tilePieces = new GameObject[width, height];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Vector2 Location = new Vector2(i, j);
                boardContainers[i,j] = Instantiate(containerPrefab, Location, Quaternion.identity);
            }
        }
        SetupPieces();
    }

    private void SetupPieces() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                tilePieces[i, j] = GetRandomTile(boardContainers[i, j].transform, i, j);
                Tile tile = tilePieces[i, j].GetComponent<Tile>();
                tile.column = i;
                tile.column = j;
            }
        }
    }

    private GameObject GetRandomTile(Transform parent, int x, int y) {
        //int num = randomGenerator.Next(0, tilePieceCount);        
        int num = _randomGenerator.Next(0, tilePieceCount);
        int maxIterations = 0;
        while (MatchesAt(x, y, tilePrefabs[num]) && maxIterations < 100) {
            //num = UnityEngine.Random.Range(0, tilePieceCount);
            num = _randomGenerator.Next(0, tilePieceCount);
            Debug.Log("match found");
            maxIterations++;
        }
        maxIterations = 0;         
        return Instantiate(tilePrefabs[num], parent);
    }

    public void DestroyMatches() {
        //How many elements are i nthe matrched pieces from find matches
        /* USELESS CODE
        if (_finder.currentMatches.Count >= 4) {
            CheckToMakeBombs();
        }*/
        _finder.currentMatches.Clear();

        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (tilePieces[i, j] != null) {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        StartCoroutine(DecreaseRowCo2());
    }

    private bool MatchesAt(int _col, int _row, GameObject _piece) {
        if (_col > 1 && _row > 1) {
            if (tilePieces[_col - 1, _row] != null && tilePieces[_col - 2, _row] != null) {
                if (tilePieces[_col - 1, _row].tag == _piece.tag && tilePieces[_col - 2, _row].tag == _piece.tag) {
                    return true;
                }
            }
            if (tilePieces[_col, _row - 1] != null && tilePieces[_col, _row - 2] != null) {
                if (tilePieces[_col, _row - 1].tag == _piece.tag && tilePieces[_col, _row - 2].tag == _piece.tag) {
                    return true;
                }
            }
        } else if (_col <= 1 || _row <= 1) {
            if (_row > 1) {
                if (tilePieces[_col, _row - 1] != null && tilePieces[_col, _row - 2] != null) {
                    if (tilePieces[_col, _row - 1].tag == _piece.tag && tilePieces[_col, _row - 2].tag == _piece.tag) {
                        return true;
                    }
                }
            }
            if (_col > 1) {
                if (tilePieces[_col - 1, _row] != null && tilePieces[_col - 2, _row] != null) {
                    if (tilePieces[_col - 1, _row].tag == _piece.tag && tilePieces[_col - 2, _row].tag == _piece.tag) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void RemoveAll() {
        RemoveItem(tilePieces);
        RemoveItem(boardContainers);
    }

    private void RemoveItem(GameObject[,] items) {
        if (items == null) return;
        foreach(GameObject g in items) {
            if (g != null)
                Destroy(g);
        }
        items = null;
    }
}
