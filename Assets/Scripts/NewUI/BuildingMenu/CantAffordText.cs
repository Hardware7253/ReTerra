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
        FindObjectOfType<BuildingMenuScript>().onSendProperties += SetTextPos;
        FindObjectOfType<MainBalancing>().onSetCFText += ShowText;

        myRTransform = gameObject.GetComponent<RectTransform>();

        ShowText(false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    // Set the position of the text just below the building menu buttons box
    void SetTextPos(Vector3 cTransformProperties)
    {   
        Vector2 mySize = myRTransform.sizeDelta;

        float yOffset = ((mySize.y / 2) * cTransformProperties.z);

        myRTransform.anchoredPosition = new Vector2(cTransformProperties.x, (cTransformProperties.y * 2) - yOffset);
        myRTransform.localScale = new Vector2(cTransformProperties.z, cTransformProperties.z);
    }

    void ShowText(bool showText)
    {
        gameObject.GetComponent<Text>().enabled = showText;
    }
}
