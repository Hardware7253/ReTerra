using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnsScriptOld : MonoBehaviour
{
    RectTransform textTransform;
    RectTransform buttonTransform;

    [SerializeField]
    GameObject text;

    [SerializeField]
    GameObject button;

    Vector2 baseScale = new Vector2(150, 40);
    Vector2 turnsHoverBox;
    int basePad = 4;
    int baseFontSize = 32;

    int turn = -1;

    bool largeScreen;

    public event Action<bool> turnsHovered;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<CameraScript>().onResChange += SetTurnsUI;
        FindObjectOfType<CameraScript>().onChangeScreenType += SetScreenType;

        textTransform = text.GetComponent<RectTransform>();
        buttonTransform = button.GetComponent<RectTransform>();
        updateTurnsText();
    }

    void SetScreenType(bool isLargeScreen)
    {
        largeScreen = isLargeScreen;
    }

    // Update is called once per frame
    void Update()
    {
        checkTurnsHover();
    }


    // Set size and positions of the turns UI and scales it slightly with the screen size
    void SetTurnsUI()
    {
        Vector2 scale = baseScale;

        int pad = basePad;
        int fontSize = baseFontSize;
        if (largeScreen) // Make turns UI bigger for larger screens
        {
            scale = new Vector2(baseScale.x * 2, baseScale.y * 2);
            pad = basePad * 2;
            fontSize = baseFontSize * 2;
        }

        turnsHoverBox = new Vector2(scale.x + pad * 2, scale.y * 2 + pad * 4);

        // Set Scales
        textTransform.sizeDelta = scale;
        buttonTransform.sizeDelta = scale;

        // Set positions
        buttonTransform.anchoredPosition = new Vector2((-scale.x / 2) - pad, (scale.y / 2) + pad);
        textTransform.anchoredPosition = new Vector2(buttonTransform.anchoredPosition.x, buttonTransform.anchoredPosition.y + scale.y + (pad * 2));

        // Set font sizes
        text.GetComponentInChildren<Text>().fontSize = fontSize;
        button.GetComponentInChildren<Text>().fontSize = fontSize;
    }

    // Increments turn text
    public void updateTurnsText()
    {
        turn++;
        text.GetComponentInChildren<Text>().text = "Turn " + turn;
    }

    // Check if the turns (button or text) are hovered
    public void checkTurnsHover()
    {
        if (Input.mousePosition.x > (Screen.width - turnsHoverBox.x) && Input.mousePosition.y < turnsHoverBox.y)
            turnsHovered?.Invoke(true);
        if (!(Input.mousePosition.x > (Screen.width - turnsHoverBox.x) && Input.mousePosition.y < turnsHoverBox.y))
            turnsHovered?.Invoke(false);
    }


}
