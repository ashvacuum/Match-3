using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Dropdown height;
    public Dropdown width;
    public Dropdown pieces;
    public GameObject panel;

    private BoardSetup _setup;

    private void Start() {
        _setup = FindObjectOfType<BoardSetup>();
    }

    public void StartGame() {
        string s_width;
        string s_height;
        string s_piece;

        s_width = width.itemText.text;
        s_height = height.itemText.text;
        s_piece = pieces.itemText.text;
        
        try {
            _setup.width = int.Parse(s_width);
            _setup.height = int.Parse(s_height);
            _setup.tilePieceCount = int.Parse(s_piece);
        }
        catch (FormatException) {
            Debug.Log($"Unable to parse, reverting to default settings");
            _setup.width = 4;
            _setup.height = 4;
            _setup.tilePieceCount = 3;
        }
        panel.SetActive(false);
        _setup.BeginGame();
    }

    public void RestartGame() {
        SceneManager.LoadScene(0);
    }
}
