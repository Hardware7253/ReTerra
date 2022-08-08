using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuScript : MonoBehaviour
{
    float uiScaleL = (float)MainBalancing.uiScale;

    Vector2 sizePostScale;
    Vector2 openedPosition;
    Vector2 closedPosition;

    bool[] buttonsActive;

    [SerializeField]
    GameObject[] buttons;

    [SerializeField]
    GameObject buttonsContainer;

    int yPerButton = 30; // How much y value of the buttons container is assigned to each button

    RectTransform myRTransform;

    public static bool buildingMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to events
        FindObjectOfType<SliderUpdateValue>().onSetUiScale += SetUiScale;
        FindObjectOfType<MainBalancing>().onSetBuildText += SetButtonText;
        FindObjectOfType<MainBalancing>().onSetBuildOpen += MenuOpen;

        myRTransform = gameObject.GetComponent<RectTransform>();

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
        SetSize();
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
        sButton.GetComponentInChildren<Text>().text = text;
    }

    // Closes / opens the building menu by moving it off / on screen
    void MenuOpen(bool isOpen, bool isScrolling)
    {
        SetScalePos();
        if (isOpen)
        {
            myRTransform.anchoredPosition = openedPosition; // Open menu
            buildingMenuOpen = true;
            return;
        }
        buildingMenuOpen = false;
        myRTransform.anchoredPosition = closedPosition; // Close menu
    }

    // Sets the appropriate size of the button container for how many buttons there are
    void SetSize()
    {
        // Detect how many buttons there are
        int buttons = 0;
        for (int i = 0; i < MainBalancing.buttonIds.Length; i++)
        {   
            if (MainBalancing.buttonIds[i] >= 0)
                buttons = i + 1;
                
            if (MainBalancing.buttonIds[i] < 0)
                i = MainBalancing.buttonIds.Length;
        }

        // Set size according to buttons     x stays constant
        myRTransform.sizeDelta = new Vector3(myRTransform.sizeDelta.x, yPerButton * buttons);
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
