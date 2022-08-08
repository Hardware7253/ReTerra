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
        FindObjectOfType<SliderUpdateValue>().onSetUiScale += SetUiScale;

        myRTransform = gameObject.GetComponent<RectTransform>();
        ShowText(false);
    }

    void SetUiScale()
    {
        myRTransform.localScale = new Vector2(MainBalancing.uiScale, MainBalancing.uiScale);
    }
    
    void ShowText(bool showText)
    {
        gameObject.GetComponent<Text>().enabled = showText;
    }
}
