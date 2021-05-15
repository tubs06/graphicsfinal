using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MouseHoverPause : MonoBehaviour
{
    // start color as white
    void Start()
    {
        gameObject.GetComponent<TextMeshPro>().color = Color.white;
    }

    // when mouse enters change color
    private void OnMouseEnter()
    {
        gameObject.GetComponent<TextMeshPro>().color = Color.blue;
    }

    // when mouse leaves, reset color
    private void OnMouseExit()
    {
        gameObject.GetComponent<TextMeshPro>().color = Color.white;
    }

    private void OnMouseDown()
    {
        // choice has been made, MainMenu object now handles executing selected option, unless exit is pressed, then call Application.Quit();.
        switch (gameObject.GetComponent<TextMeshPro>().text)
        {
            case "Resume":
                // tell pausemenu to resume game
                transform.parent.GetComponent<PauseMenu>().ListenForSelection = 0;
                break;
            case "Main Menu":
                // tell pausemenu to go to mainmenu
                transform.parent.GetComponent<PauseMenu>().ListenForSelection = 1;
                break;
            case "Quit":
                // close game
                Application.Quit();
                break;
            default:
                break;
        }
    }
}
