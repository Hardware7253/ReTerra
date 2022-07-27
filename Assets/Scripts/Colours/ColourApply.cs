using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourApply : MonoBehaviour
{
    // Script used to apply colours from Colours.cs to ui
    
    [SerializeField]
    bool isText;

    [SerializeField]
    int colorIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (isText)
        {   
            gameObject.GetComponent<Text>().color = Colours.colorsArray[colorIndex];
            return;
        }
        gameObject.GetComponent<Image>().color = Colours.colorsArray[colorIndex];
    }
}
