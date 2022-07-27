using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TurnsScript : MonoBehaviour
{
    int turn = 0;
    float uiScale = 5;

    int fontSize = 14;

    public event Action<int> onTurnUpdate;
    RectTransform myRTransform;
    Vector2 referenceSize = new Vector2(60, 30);

    // Start is called before the first frame update
    void Start()
    {   
        FindObjectOfType<MainBalancing>().onSetUiScale += SetUiScale;

        myRTransform = gameObject.GetComponent<RectTransform>();
        SetScalePos();
        SetTurnText();
    }

    void SetUiScale(int scale)
    {
        uiScale = scale;
    }

    // Set the scale and position of the turns button
    void SetScalePos()
    {
        Vector2 sizePostScale = new Vector2(referenceSize.x * (uiScale / 2), referenceSize.y * (uiScale / 2)); // Calulate the new image size after it has been scaled

        gameObject.GetComponentInChildren<Text>().fontSize = fontSize; // Set font size
        myRTransform.sizeDelta = referenceSize; // Set button size
        myRTransform.localScale = new Vector2(uiScale, uiScale); // Set button scale
        myRTransform.anchoredPosition = new Vector2(-sizePostScale.x, sizePostScale.y); // Set button position
    }

    // Set turn text
    void SetTurnText()
    {
        gameObject.GetComponentInChildren<Text>().text = "Turn: " + turn + "\n" + "Advance?";
    }

    // Increment turn
    public void IncrementTurn()
    {
        turn++;
        onTurnUpdate?.Invoke(turn);
        SetTurnText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
