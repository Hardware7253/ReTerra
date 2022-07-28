using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EscapeLevelHandler : MonoBehaviour
{   

    [SerializeField]
    GameObject pauseMenu, mainMenu, optionsMenu;
    public static bool pauseMenuOpen = false;
    bool optionsMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        // Handles the opening and closing of the pause menu
        // There are multiple things controlled by the escape key so there need to be checks to make sure the esacape key doesn't activate/deactivate multiple things at once
        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            // Open Pause menu
            if (BuildingMenuScript.buildingMenuOpen == false && TilesHandler.tileIsClicked == false && pauseMenuOpen == false)
            {   
                pauseMenu.SetActive(true);
                pauseMenuOpen = true;
                return;
            }

            if (pauseMenuOpen == true)
            {   
                // Close options menu
                if (optionsMenuOpen == true)
                {
                    optionsMenu.SetActive(false);
                    mainMenu.SetActive(true);
                    optionsMenuOpen = false;
                    return;
                }

                // Close pause menu
                optionsMenu.SetActive(false);
                mainMenu.SetActive(true);
                pauseMenu.SetActive(false);
                pauseMenuOpen = false;
            }
        }
    }

    // Functions to update variables with button presses
    // Because these buttons do the same thing as pressing escape
    public void CloseMenu()
    {
        pauseMenuOpen = false;
    }

    public void OpenOptions()
    {
        optionsMenuOpen = true;
    }

    public void CloseOptions()
    {
        optionsMenuOpen = false;
    }
}

