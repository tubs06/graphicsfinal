using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // listener
    public int ChoiceListener = -1;
    public GameObject GameController;
    public GameObject MMCamera;
    // loaded game var.
    public static bool GameLoaded = false;
    // Start is called before the first frame update
    void Start()
    {
        // get refs to some objects we need
        GameController = GameObject.Find("GameController");
        MMCamera = GameObject.Find("MMCamera");
        // disable game and pause menu cameras
        GameObject.Find("GameCamera").GetComponent<Camera>().enabled = false;
        GameObject.Find("PMCamera").GetComponent<Camera>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // child script has triggered
        if (!GameLoaded && ChoiceListener != -1)
        {
            switch (ChoiceListener)
            {
                case 0:
                    // hand control to game controller to start solar system demo
                    GameController.GetComponent<GameController>().DemoID = 0;
                    GameObject.Find("PaneLight").GetComponent<Light>().enabled = false;
                    break;
                case 1:
                    // hand control to game controller to start collision demo
                    GameController.GetComponent<GameController>().DemoID = 1;
                    GameObject.Find("PaneLight").GetComponent<Light>().enabled = false;
                    break;
                case 2:

                    break;
                default:
                    break;
            }
            // reset listener, set loaded game to true so script sleeps
            ChoiceListener = -1;
            GameLoaded = true;
        }
    }
}
