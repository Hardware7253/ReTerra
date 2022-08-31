using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSettings : MonoBehaviour
{
    public static Font[] fonts = new Font[3];

    private void Awake()
    {
        fonts[0] = Resources.Load("Fonts/Inconsolata/Inconsolata-Light") as Font;       // Menus font
        fonts[1] = Resources.Load("Fonts/Inconsolata/Inconsolata-Light") as Font;       // Game font
        fonts[2] = Resources.Load("Fonts/Inconsolata/Inconsolata-SemiBold") as Font;    // Title font
    }
    

    public static int[] fontSizes = new int[]
    {
        180,    // 0 Title
        60,     // 1 Body
        40,     // 2 Extra 1
        30,     // 3 Extra 2

        12,     // 4 Stats font size    
        12,     // 5 Turns font size
        10,     // 6 Building menu font size
        8,      // 7 Can't afford text font size
        14,     // 8 Info menu title
        8,      // 9 Info menu body

        6,      // 10 stats indicator font size

        180     // 11 Special
    };
}
