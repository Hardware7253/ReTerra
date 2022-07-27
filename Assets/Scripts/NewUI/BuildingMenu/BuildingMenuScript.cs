using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuScript : MonoBehaviour
{
    float uiScaleL = (float)MainBalancing.uiScale;

    int fontSize = 12;

    Vector2 sizePostScale;
    Vector2 openedPosition;
    Vector2 closedPosition;

    bool[] buttonsActive;

    [SerializeField]
    GameObject[] buttons;

    [SerializeField]
    GameObject buttonsContainer;

    float buttonsContainerInitY;

    RectTransform myRTransform;

    public event Action<Vector2> onSendProperties;

    public static bool buildingMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to events
        FindObjectOfType<PauseMenuScript>().onSetUiScale += SetUiScale;
        FindObjectOfType<MainBalancing>().onSetBuildText += SetButtonText;
        FindObjectOfType<MainBalancing>().onSetBuildOpen += MenuOpen;

        myRTransform = gameObject.GetComponent<RectTransform>();

        // Get initial y value of the button container, this is the top of the scroll rect
        buttonsContainerInitY = buttonsContainer.GetComponent<RectTransform>().anchoredPosition.y;

        // Scale and place the building menu and hide it
        SetScalePos();
        MenuOpen(false, false);
    }

    // Called whenever uiScale is changed
    void SetUiScale()
    {
        uiScaleL = (float)MainBalancing.uiScale;
        
        SetScalePos();
        MenuOpen(false, false);
    }

    // Set the scale and position of the turns button
    void SetScalePos()
    {
        Vector2 refSize = myRTransform.sizeDelta;
        sizePostScale = new Vector2(refSize.x * (uiScaleL / 2), refSize.y * (uiScaleL / 2)); // Calulate the new image size after it has been scaled

        myRTransform.localScale = new Vector2(uiScaleL, uiScaleL); // Set image scale

        // Set image positions for when the menu is open or closed
        openedPosition = new Vector2(sizePostScale.x, -sizePostScale.y);
        closedPosition = new Vector2(-sizePostScale.x, -sizePostScale.y);
    }


    // Sets text inside the given button
    public void SetButtonText(int button, string text, bool buttonActive, int id)
    {
        GameObject sButton = buttons[button];

        // Enable / disable the button
        sButton.GetComponent<Image>().enabled = buttonActive;
        sButton.GetComponent<Button>().interactable = buttonActive;

        // Set the text
        sButton.GetComponentInChildren<Text>().fontSize = fontSize;
        sButton.GetComponentInChildren<Text>().text = text;

        buttonsContainer.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, buttonsContainerInitY); // Reset scroll position to the top
    }

    // Closes / opens the building menu by moving it off / on screen
    void MenuOpen(bool isOpen, bool isScrolling)
    {
        // Set the scrolling on / off
        gameObject.GetComponent<ScrollRect>().enabled = isScrolling;

        if (isOpen)
        {
            myRTransform.anchoredPosition = openedPosition; // Open menu
            SendProperties();
            buildingMenuOpen = true;
            return;
        }
        SendProperties();
        buildingMenuOpen = false;
        myRTransform.anchoredPosition = closedPosition; // Close menu
    }

    // Send properties for can't afford text
    // x and y are for container position
    void SendProperties()
    {
        Vector2 properties = myRTransform.anchoredPosition;

        onSendProperties?.Invoke(properties);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuOpen(false, false);
        }
    }
}
