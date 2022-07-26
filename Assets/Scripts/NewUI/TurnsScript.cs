using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnsScript : MonoBehaviour
{
    float uiScale = 5;

    int fontSize = 14;

    RectTransform myRTransform;
    Vector2 referenceSize = new Vector2(60, 30);

    // Start is called before the first frame update
    void Start()
    {
        myRTransform = gameObject.GetComponent<RectTransform>();
        SetScalePos();
        SetTurnText(1000);
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

    void SetTurnText(int turn)
    {
        gameObject.GetComponentInChildren<Text>().text = "Turn: " + turn + "\n" + "Advance?";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
