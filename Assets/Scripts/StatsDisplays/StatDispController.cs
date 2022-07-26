using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatDispController : MonoBehaviour
{

    [SerializeField]
    private GameObject[] statDisplays;

    [SerializeField]
    private GameObject[] statTexts;

    int offset;

    // Input parameters are base sizes (the size for large screens will be double these values)
    int bFontSize = 32;
    Vector2Int bTextBoxScale = new Vector2Int(100, 50);

    int bPad = 2;
    Vector2Int bIconScale = new Vector2Int(64, 64);


    int fontSize;
    Vector2Int textBoxScale;

    int pad = 2;
    Vector2Int iconScale;

    bool largeScreen;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<CameraScript>().onResChange += ScaleParameters;
        FindObjectOfType<CameraScript>().onChangeScreenType += SetScreenType;
        FindObjectOfType<MainBalancing>().setStatDisplay += SetStatsText;

        SetStatsText(0, 0, 10);
        SetStatsText(1, 1, 10);
        SetStatsText(2, 2, 10);
    }

    void SetScreenType(bool isLargeScreen)
    {
        largeScreen = isLargeScreen;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Scale size of the stat displays with larger screens
    void ScaleParameters()
    {
        fontSize = bFontSize;
        textBoxScale = bTextBoxScale;
        pad = bPad;
        iconScale = bIconScale;

        if (largeScreen) // Detect large screen size
        {
            fontSize = bFontSize * 2;
            textBoxScale = new Vector2Int(bTextBoxScale.x * 2, bTextBoxScale.y * 2);
            pad = bPad * 2;
            iconScale = new Vector2Int(bIconScale.x * 2, bIconScale.y * 2);
        }

        offset = iconScale.y + pad;
        setStatsPositions();
    }

    // Set scales and positions of stats displays
    void setStatsPositions()
    {
        Vector2Int basePos = new Vector2Int(iconScale.x / 2 + pad, iconScale.y / 2 + pad);
        for (int i = 0; i < statDisplays.Length; i++)
        {
            int lOffset = offset * i;
            Vector2Int pos = new Vector2Int(basePos.x , basePos.y + lOffset);

            RectTransform imageTransform = statDisplays[i].GetComponent<RectTransform>();
            imageTransform.anchoredPosition = pos; // Set position
            imageTransform.sizeDelta = iconScale; // Set image scale

            RectTransform textTransform = statTexts[i].GetComponent<RectTransform>();
            textTransform.anchoredPosition = new Vector2Int(textBoxScale.x / 2, 0); // Set text box pos
            textTransform.sizeDelta = textBoxScale; // Set text box size
            statTexts[i].GetComponent<Text>().fontSize = fontSize; // Set font size
        }
    }

    // Sets the text of a given stats display
    void SetStatsText(int display, int supply, int demand)
    {
        statDisplays[display].GetComponentInChildren<Text>().text = supply.ToString() + " / " + demand.ToString();
    }
}
