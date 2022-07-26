using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenuScript : MonoBehaviour
{
    float uiScale = 5;

    int fontSize = 14;

    int statsPerButton = 3;

    [SerializeField]
    GameObject[] buttons;
    
    RectTransform myRTransform;

    // Start is called before the first frame update
    void Start()
    {
        myRTransform = gameObject.GetComponent<RectTransform>();
        SetScalePos();

        SetButtonText(5, 420, "Coal plant");
    }

    // Set the scale and position of the turns button
    void SetScalePos()
    {
        Vector2 refSize = myRTransform.sizeDelta;
        Vector2 sizePostScale = new Vector2(refSize.x * (uiScale / 2), refSize.y * (uiScale / 2)); // Calulate the new image size after it has been scaled

        myRTransform.localScale = new Vector2(uiScale, uiScale); // Set image scale
        myRTransform.anchoredPosition = new Vector2(sizePostScale.x, -sizePostScale.y); // Set image position
    }


    // Sets all text inside the given button, this is the main text and text on each stat display
    void SetButtonText(int button, int stats, string text)
    {
        GameObject sButton = buttons[button];
        Component[] texts;

        texts = sButton.GetComponentsInChildren<Text>(); // Add Text component of each gameObject at or below sButton position in hierarchy to the array
        texts[0].GetComponent<Text>().text = text; // Set the text

        string statsString = stats.ToString(); // Convert the stats int into a string
        for (int i = 0; i < statsPerButton; i++)
        {
            string statText = statsString[i].ToString(); // Use i to index the stats string and convert the given char back into a string. This is done because each character is a value for a display.
            texts[i + 1].GetComponent<Text>().text = statText; // Set the text
        }      
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
