using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoMenuScript : MonoBehaviour
{
    RectTransform myRTransform;
    
    Vector3 openPosition;
    bool menuOpen = false;

    // Used to scale the vertical size of the body text box based on how many words there are
    float wordsPerLine = 2f;
    int lineSize = 10;

    void Start()
    {
        FindObjectOfType<MainBalancing>().onSetinfo += SetText;
        myRTransform = gameObject.GetComponent<RectTransform>();
        OpenCloseMenu(menuOpen);
    }

    void Update()
    {   
        // Toggle the info menu when the key is pressed
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!menuOpen)
                menuOpen = true;
            else if (menuOpen)
                menuOpen = false;
        }

        if (TilesHandler.tileIsClicked && menuOpen)
            OpenCloseMenu(true);
        else
            OpenCloseMenu(false);
    }

    // Set open poisition according to the uiscale
    void SetPosScale()
    {
        myRTransform.localScale = new Vector3(MainBalancing.uiScale, MainBalancing.uiScale, 1);
        openPosition = new Vector3((myRTransform.sizeDelta.x * MainBalancing.uiScale) / -2, (myRTransform.sizeDelta.y * MainBalancing.uiScale) / -2, 0);
    }

    // Set title and body text of the info menu
    void SetText(string title, string body)
    {
        SetBodySize(body);

        Text[] texts = gameObject.GetComponentsInChildren<Text>();

        texts[0].text = title;
        texts[1].text = body;
    }

    // Open / close the menu
    void OpenCloseMenu(bool shouldOpen)
    {
        SetPosScale();
        if (shouldOpen)
            myRTransform.anchoredPosition = openPosition;

        if (!shouldOpen)
            myRTransform.anchoredPosition = new Vector3(openPosition.x * -2, openPosition.y, 0);
    }

    // Set body text vertical size
    void SetBodySize(string description)
    {
        int lines = (int)Math.Abs((float)GetWords(description) / (float)wordsPerLine);

        RectTransform bodyTransform = gameObject.GetComponentsInChildren<RectTransform>()[2];
        bodyTransform.sizeDelta = new Vector3(bodyTransform.sizeDelta.x, lines * lineSize, 1);
    }

    // Return number of words in a string
    int GetWords(string s)
    {
        char space = ' ';

        // Find number of spaces in the string
        int spaces = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == space)
                spaces++;
        }

        // Number of words = spaces + 1
        if (spaces > 0)
            return spaces + 1;
        return 0;
    }

}
