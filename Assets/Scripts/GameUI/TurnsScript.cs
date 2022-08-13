using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnsScript : MonoBehaviour
{
    float uiScaleL = (float)MainBalancing.uiScale;

    int fontSize = 14;

    RectTransform myRTransform;
    Vector2 referenceSize = new Vector2(60, 30);

    public event Action onNewTurn;

    // Start is called before the first frame update
    void Start()
    {   
        FindObjectOfType<SliderUpdateValue>().onSetUiScale += SetUiScale;

        myRTransform = gameObject.GetComponent<RectTransform>();
        SetScalePos();
        SetTurnText();
    }

    // Called whenever uiScale is changed
    void SetUiScale()
    {
        uiScaleL = (float)MainBalancing.uiScale;
        
        SetScalePos();
        SetTurnText();
    }

    // Set the scale and position of the turns button
    void SetScalePos()
    {
        Vector2 sizePostScale = new Vector2(referenceSize.x * (uiScaleL / 2), referenceSize.y * (uiScaleL / 2)); // Calulate the new image size after it has been scaled

        gameObject.GetComponentInChildren<Text>().fontSize = fontSize; // Set font size
        myRTransform.sizeDelta = referenceSize; // Set button size
        myRTransform.localScale = new Vector2(uiScaleL, uiScaleL); // Set button scale
        myRTransform.anchoredPosition = new Vector2(-sizePostScale.x, sizePostScale.y); // Set button position
    }

    // Set turn text
    void SetTurnText()
    {
        gameObject.GetComponentInChildren<Text>().text = "Year: " + MainBalancing.turn + "\n" + "Advance?";
    }

    // Increment turn
    public void IncrementTurn()
    {
        MainBalancing.turn++;
        onNewTurn?.Invoke();
        SetTurnText();
        FindObjectOfType<AudioManager>().PlaySound("UIClick");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            IncrementTurn();
        }
    }
}
