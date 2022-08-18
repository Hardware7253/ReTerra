using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextApplier : MonoBehaviour
{
    [SerializeField]
    private int fontIndexer, fontSizeIndexer; 
    
    private void Start()
    {
        gameObject.GetComponent<Text>().font = TextSettings.fonts[fontIndexer];
        gameObject.GetComponent<Text>().fontSize = TextSettings.fontSizes[fontSizeIndexer];
    }
}
