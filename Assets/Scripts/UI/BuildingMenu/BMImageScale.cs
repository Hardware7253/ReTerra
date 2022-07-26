using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BMImageScale : MonoBehaviour
{
    RectTransform myRTransform;

    int barWidth = 72;
    int bWidth;

    Vector3 barBasePos;

    bool buildingMenuOpen;

    public event Action<bool> buildingMenuActive;
    public event Action<bool> buildingMenuHovered;

    bool largeScreen;

    private void Start()
    {
        // Subscribe functions to events
        FindObjectOfType<CameraScript>().onResChange += SetBar;
        FindObjectOfType<CameraScript>().onChangeScreenType += SetScreenType;
        FindObjectOfType<TilesHandler>().OnTilePressed += TileClicked;

        myRTransform = GetComponent<RectTransform>();
    }

    void SetScreenType(bool isLargeScreen)
    {
        largeScreen = isLargeScreen;
    }

    // Calulate bar size and position everytime the screen resoultion changes
    public void SetBar()
    {
        bWidth = barWidth;
        if (largeScreen) // Make bar bigger for large screens
            bWidth = barWidth * 2;

        myRTransform.sizeDelta = new Vector2(Screen.width, bWidth);

        barBasePos = new Vector3(0, myRTransform.anchoredPosition.x - (bWidth / 2));

        buildingMenuOpen = false;
        PlaceBar();
    }

    // Shows the bar when a tile is clicked
    private void TileClicked(int _)
    {
        buildingMenuOpen = true;
        PlaceBar();
    }

    // Hide or show the bar based on isOnScreen
    private void PlaceBar()
    {
        if (buildingMenuOpen)
        {
            myRTransform.anchoredPosition = barBasePos;
            buildingMenuActive?.Invoke(true);
        }

        if (!buildingMenuOpen)
        {
            myRTransform.anchoredPosition = new Vector3(barBasePos.x, barBasePos.y + bWidth, barBasePos.z);
            buildingMenuActive?.Invoke(false);
        }
    }

    private void Update()
    {
        // Hide the building bar if escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            buildingMenuOpen = false;
            PlaceBar();
        }
            
        // Check if mouse is or isn't hovering the build menu
        if ((Input.mousePosition.y > Screen.height - bWidth) && buildingMenuOpen)
            buildingMenuHovered?.Invoke(true);
        if (!((Input.mousePosition.y > Screen.height - bWidth) && buildingMenuOpen))
            buildingMenuHovered?.Invoke(false);

    }

}
