using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MouseHover : MonoBehaviour
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
            case "Start Solar System Demo":
                // tell main menu to load solar system
                gameObject.GetComponentInParent<MainMenu>().ChoiceListener = 0;
                break;
            case "Start Collision Demo":
                // tell main menu to load collision demo
                gameObject.GetComponentInParent<MainMenu>().ChoiceListener = 1;
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
