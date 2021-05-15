using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // listen vars
    public int ListenForPause = -1;
    public int ListenForSelection = -1;
    public bool Paused = false;
    // object refs
    public GameObject PMCamera;
    public GameObject GameController;
    public GameObject GameCamera;
    // Start is called before the first frame update
    void Start()
    {
        // get references to objects we need to modify
        PMCamera = GameObject.Find("PMCamera");
        GameController = GameObject.Find("GameController");
        GameCamera = GameObject.Find("GameCamera");
    }

    // Update is called once per frame
    void Update()
    {
        // if we are not paused and recieve a pause event, we pause the game. move camera and stuff
        if(!Paused && ListenForPause == 1)
        {
            // disable game camera
            // enable pause menu camera
            PMCamera.GetComponent<Camera>().enabled = true;
            GameCamera.GetComponent<Camera>().enabled = false;
            // unlock mouse
            Cursor.lockState = CursorLockMode.None;
            // enable paused var
            Paused = true;
        }

        // If we are paused, we listen for the input from MouseHoverPause
        if (Paused && ListenForSelection != -1)
        {
            switch (ListenForSelection)
            {
                // Resume
                case 0:
                    // tell game controller to resume
                    GameController.GetComponent<GameController>().PauseListener = 0;
                    break;
                // Main Menu
                case 1:
                    // tell gamecontroller to  exit to main menu
                    GameController.GetComponent<GameController>().PauseListener = 1;
                    break;

                default:
                    break;
            }
            // set paused to not paused, reset listener vars
            Paused = false;
            ListenForPause = -1;
            ListenForSelection = -1;
            // disable pause menu camera and enable game camera
            GameCamera.GetComponent<Camera>().enabled = true;
            PMCamera.GetComponent<Camera>().enabled = false;
        }
    }
}
