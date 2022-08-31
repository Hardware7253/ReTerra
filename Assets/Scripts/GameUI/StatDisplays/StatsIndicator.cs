using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsIndicator : MonoBehaviour
{   
    string global = "Showing global stats";
    string tile = "Showing tile stats";

    Text myText;

    void Start()
    {
        myText = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        SetText(!TilesHandler.tileIsClicked);
    }

    void SetText(bool isGlobal)
    {
        if (isGlobal)
        {
           myText.text = global;
           return;
        }
        myText.text = tile;
    }
}
