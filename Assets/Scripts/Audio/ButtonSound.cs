using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// https://answers.unity.com/questions/954777/ui-button-play-sound-when-highlighted.html
// Code by AlwaysSunny
public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    public void OnPointerEnter( PointerEventData ped )
    {
        //FindObjectOfType<AudioManager>().PlaySound("UIHover"); 
    }

    public void OnPointerDown( PointerEventData ped )
    {
        FindObjectOfType<AudioManager>().PlaySound("UIClick"); 
    }
}
