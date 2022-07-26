using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

// Todo
// Fix clicking on tiles through UI
public class BMController : MonoBehaviour
{
    int buttonPad = 4;
    int buttonHeight = 64;
    int buttonsAmmount;
    int fontSize = 32;

    int[] buttonsId;
    Vector2[] buttonPositions;
    Vector2 buttonSize;

    bool largeScreen;

    [SerializeField]
    private GameObject[] buttons;

    public event Action<int> onBuildButtonPressed;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe functions to events
        FindObjectOfType<CameraScript>().onResChange += SetButtonPosScale;
        FindObjectOfType<CameraScript>().onChangeScreenType += SetScreenType;
        FindObjectOfType<TilesHandler>().OnTilePressed += SetButtonsId;

        // Setting size of arrays to the ammount of buttons
        buttonsAmmount = buttons.Length;

        buttonsId = new int[buttonsAmmount];
        buttonsId = new int[] { -1, -1, -1, -1 };
        buttonPositions = new Vector2[buttonsAmmount];
    }

    void SetScreenType(bool isLargeScreen)
    {
        largeScreen = isLargeScreen;
    }

    // Gets the tile to build corresponding to the button pressed
    // E.g. If button 0 is pressed it will return a value of 21
    public void BuildTileCorrespondingButton(int buttonId)
    {
        onBuildButtonPressed?.Invoke(buttonsId[buttonId]);
    }


    // Calculates and sets positions and sizes for buttons so that they are evenly spaced and distrubiuted inside the building menu bar
    void SetButtonPosScale()
    {
        float buttonsXOffset = Screen.width / buttonsAmmount;
        buttonSize = new Vector2(buttonsXOffset - buttonPad / 2, buttonHeight); // Calculate size

        // Scales button size for a larger screen
        if (largeScreen)
            buttonSize.y = buttonHeight * 2;

        for (int i = 0; i < buttonsAmmount; i++)
        {
            // Calculate positions
            buttonPositions[i] = new Vector2(buttonsXOffset * (i + 1), 0);
            buttonPositions[i].x -= buttonsXOffset / 2;

            // Set position and size
            buttons[i].GetComponent<RectTransform>().anchoredPosition = buttonPositions[i];
            buttons[i].GetComponent<RectTransform>().sizeDelta = buttonSize;
        }
    }


    // Toggles button visibility and updates thier text
    void placeButtons()
    {
        string buttonText;
        int fSize = fontSize;
        if (largeScreen) // Detect large sreen
            fSize = fontSize * 2;
        for (int i = 0; i < buttonsAmmount; i++)
        {
            buttonText = IdToString(buttonsId[i]);
            
            buttons[i].GetComponentInChildren<Text>().text = buttonText; // Set button text
            buttons[i].GetComponentInChildren<Text>().fontSize = fSize; // Set button text font size

            // Buttons are interactable unless they have no text
            buttons[i].GetComponentInChildren<Button>().interactable = true;
            if (buttonText == null)
                buttons[i].GetComponentInChildren<Button>().interactable = false;
        }
    }

    // Look up table for tiles that can be built on selected tile
    // E.g. Index {1, 3, 5} can be built on index 2
    void SetButtonsId(int id)
    {
        switch (id)
        {
            case 0: // Water
                buttonsId = new int[] { 20, -1, -1, -1 }; // Hydro turbine
                break;

            case 1: // Barren Plains
                buttonsId = new int[] { 9, 23, -1, -1 }; // Seeds, Coal plant
                break;

            case 2: // Rocks
                buttonsId = new int[] { 19, 24, -1, -1 }; // Mine
                break;

            case 3: // Plains
                buttonsId = new int[] { 8, -1, -1, -1 }; // Trees
                break;


            default:
                buttonsId = new int[] { -1 , -1, -1, -1 };
                break;
        }
        placeButtons(); // Place buttons because their text might change if a different tile is selected
    }

    // Gets the text for a button from an ID
    string IdToString(int id)
    {
        string text;
        switch (id)
        {
            case 0: // Water
                text = "Water";
                break;

            case 1: // Barren Plains
                text = "Barren Plains";
                break;

            case 2: // Rocks
                text = "Rocks";
                break;

            case 3: // Plains
                text = "Plains";
                break;

            case 8: // Trees
                text = "Trees";
                break;

            case 9: // Seeds
                text = "Seeds";
                break;

            case 19: // Wind Turbine
                text = "Wind Turbine";
                break;

            case 20: // Hydro Turbine
                text = "Hydro Turbine";
                break;

            case 23: // Coal Plant
                text = "Coal Plant";
                break;

            case 24: // Mines
                text = "Mines";
                break;


            default:
                text = null;
                break;
        }
        return text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
