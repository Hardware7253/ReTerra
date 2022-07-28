using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMainMenuMusic : MonoBehaviour
{
    public void FadeMusicIn(bool fadeIn)
    {   
        if (fadeIn)
        {
            //AudioManager.FindObjectOfType<Animator>().SetTrigger("fadeIn");
            return;
        }
        //AudioManager.FindObjectOfType<Animator>().SetTrigger("fadeOut");
    }
}
