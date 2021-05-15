using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // control variables
    public int PauseListener = -1;
    public int DemoID = -1;
    public bool GameLoaded = false;
    public bool GamePlaying = false;
    // references
    public GameObject LoadedGame;
    public GameObject GameCamera;
    public GameObject MMCamera;
    public GameObject PauseMenu;
    public GameObject SolarSystem;
    public GameObject CollDemo;
    public GameObject CollDemoLight;
    public GameObject SolarLights;
    // Start is called before the first frame update
    void Start()
    {
        // get references to objects well need
        PauseMenu = GameObject.Find("PauseMenuObject");
        SolarSystem = GameObject.Find("SolarSystemDemoObject");
        GameCamera = GameObject.Find("GameCamera");
        MMCamera = GameObject.Find("MMCamera");
        CollDemo = GameObject.Find("CollDemoObject");
        CollDemoLight = GameObject.Find("DirectionalLight");
        SolarLights = GameObject.Find("Lights");
    }

    // Update is called once per frame
    void Update()
    {
        if(GameLoaded == false && DemoID != -1)
        {
            // start game
            switch (DemoID)
            {
                case 0:
                    // add solar system script to solar system object
                    LoadedGame = SolarSystem;
                    LoadedGame.AddComponent<SolarSystem>();
                    // activate solar lights, deactivate collision lights
                    SolarLights.SetActive(true);
                    CollDemoLight.SetActive(false);

                    break;
                case 1:
                    // add colldemo object to colldemo object
                    LoadedGame = CollDemo;
                    LoadedGame.AddComponent<CollDemo>();
                    // deactivate solar lights, activate collision lights
                    SolarLights.SetActive(false);
                    CollDemoLight.SetActive(true);

                    break;
                default:
                    break;
            }
            // set loaded and playing vars true
            GameLoaded = true;
            GamePlaying = true;
            // disable main menu camera
            GameCamera.GetComponent<Camera>().enabled = true;
            MMCamera.GetComponent<Camera>().enabled = false;
            // enable movement script
            GameCamera.GetComponent<Camera>().transform.position = new Vector3(30, 20, 0);
            GameCamera.AddComponent<PlayerControl>();
            // lock cursor and stuff
            Cursor.lockState = CursorLockMode.Locked;
        }
        // game is loaded, handle the pause events and stuff
        if (GameLoaded)
        {
            // put game in pause state
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // if we are playing
                if(GamePlaying == true)
                {
                    GameObject.Find("PaneLight").GetComponent<Light>().enabled = true;
                    GamePlaying = false;
                    // stop player movement, and simulation on the solar system.
                    GameCamera.GetComponent<PlayerControl>().Activated = false;
                    if(DemoID == 0)
                    {
                        LoadedGame.GetComponent<SolarSystem>().Simulating = false;
                    }
                    else
                    {
                        LoadedGame.GetComponent<CollDemo>().Simulating = false;
                    }

                    // switch to pause menu
                    PauseMenu.GetComponent<PauseMenu>().ListenForPause = 1;
                }
            }
        }

        // we are paused
        if (!GamePlaying)
        {
            // and we have some input from the pause menu
            if (PauseListener != -1)
            {
                // handle input
                switch (PauseListener)
                {
                        // resume game button pressed
                    case 0:
                        // enable player movement and simulation
                        GameCamera.GetComponent<PlayerControl>().Activated = true; 
                        if (DemoID == 0)
                        {
                            LoadedGame.GetComponent<SolarSystem>().Simulating = true;
                        }
                        else
                        {
                            LoadedGame.GetComponent<CollDemo>().Simulating = true;
                        }
                        // lock mouse
                        Cursor.lockState = CursorLockMode.Locked;
                        break;
                        // main menu button pressed
                    case 1:
                        // destroy current demo objects
                        if(DemoID == 0) 
                        {
                            Destroy(LoadedGame.GetComponent<SolarSystem>());
                        }
                        else
                        {
                            Destroy(LoadedGame.GetComponent<CollDemo>());
                        }
                        // destroy player control script
                        Destroy(GameCamera.GetComponent<PlayerControl>());
                        // set gamecontrol vars to default
                        GameLoaded = false;
                        GamePlaying = false;
                        PauseListener = -1;
                        DemoID = -1;
                        // enable main menu camera, disable game camera
                        MMCamera.GetComponent<Camera>().enabled = true;
                        GameCamera.GetComponent<Camera>().enabled = false;
                        // tell main menu to take control
                        MainMenu.GameLoaded = false;
                        return;
                }
                // set gameplaying to true, we have resumed
                GamePlaying = true;
                // reset pause listener
                PauseListener = -1;
                // disable menu light
                GameObject.Find("PaneLight").GetComponent<Light>().enabled = false;
            }
        }
    }
}
