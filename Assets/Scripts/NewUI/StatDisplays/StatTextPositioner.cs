using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTextPositioner : MonoBehaviour
{
    RectTransform myRTransform;
    // Start is called before the first frame update
    void Start()
    {
        myRTransform = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        myRTransform.anchoredPosition = new Vector2(myRTransform.sizeDelta.x / 2, 0);
    }
}
