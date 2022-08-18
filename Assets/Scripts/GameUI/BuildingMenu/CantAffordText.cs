using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CantAffordText : MonoBehaviour
{
    RectTransform myRTransform;

    // Start is called before the first frame update
    void Start()
    {   
        FindObjectOfType<MainBalancing>().onSetCFText += ShowText;

        myRTransform = gameObject.GetComponent<RectTransform>();
        ShowText(false);
    }

    void ShowText(bool showText)
    {
        gameObject.GetComponent<Text>().enabled = showText;
    }
}
