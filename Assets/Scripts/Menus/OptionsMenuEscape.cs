using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Needed for main menu only
public class OptionsMenuEscape : MonoBehaviour
{
    public GameObject OptionsMenu;
    public GameObject MainMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionsMenu.SetActive(false);
            MainMenu.SetActive(true);
        }
    }
}
