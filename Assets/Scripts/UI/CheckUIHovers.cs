using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CheckUIHovers : MonoBehaviour
{
    bool bMHover;
    bool tHover;

    public event Action<bool> gameUIHovered;

    // Start is called before the first frame update
    void Start()
    {
        //FindObjectOfType<BMImageScale>().buildingMenuHovered += HoverBuildingMenu;
        //FindObjectOfType<TurnsScript>().turnsHovered += HoverTurns;
    }

    private void Update()
    {
        SetGameUIHover();
    }

    void HoverBuildingMenu(bool hovered)
    {
        bMHover = hovered;
    }

    void HoverTurns(bool hovered)
    {
        tHover = hovered;
    }


    // Gets input from each individual piece of gameUI then makes them all into a master variable. Which describes if any of the game ui is being hovered
    void SetGameUIHover()
    {
        if (bMHover || tHover) // Something is being hovered
            gameUIHovered?.Invoke(true);
        if (!bMHover && !tHover) // Nothing is beign hovered
            gameUIHovered?.Invoke(false);
    }
}
