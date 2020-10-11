using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Camera Scaler to adjust how the screen looks everytime the 
/// </summary>
public class CameraScaler : MonoBehaviour
{

    private BoardSetup _board;
    public float cameraOffset;
    public float screenWidth;
    public float screenHeight;
    public float padding = 2;
    private Camera cam;
    // Use this for initialization
    void Start() {
        _board = FindObjectOfType<BoardSetup>();
        cam = GetComponent<Camera>();
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }
    
    /// <summary>
    /// Manages camera positioning when generating boards
    /// </summary>
    /// <param name="height">Board Height</param>
    /// <param name="width">Board Width</param>
    public void RepositionCamera(float height, float width) {
        Vector3 _tempPos = new Vector3((width-1) / 2, (height-1) / 2, cameraOffset);
        transform.position = _tempPos;
        //Camera.main.orthographicSize = (board.width >= board.height) ? (board.width / 2 + padding) / aspectRatio : board.height / 2 + padding;

        if (cam != null) {
            if (_board.width >= _board.height) {
                cam.orthographicSize = height + padding / 2;
            } else {
                cam.orthographicSize = width / 2 +  (padding % 7);
            }
        }

    }
}

