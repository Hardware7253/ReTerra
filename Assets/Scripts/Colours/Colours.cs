using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colours : MonoBehaviour
{   
    // Script that stores colours for ui
    // So the ui can easily be changed
    public static Color32[] colorsArray = new Color32[]
    {
        new Color32(255, 255, 255, 255), // Default colour                         0

        new Color32(33, 37, 41, 128),    // Main menu background                   1
        new Color32(255, 255, 255, 255), // Main menu text                         2
        new Color32(33, 37, 41, 255),    // Main menu button background            3
        new Color32(255, 255, 255, 255), // Main menu button text                  4

        new Color32(33, 37, 41, 128),    // Pause menu background                  5
        new Color32(255, 255, 255, 255), // Pause menu text                        6
        new Color32(33, 37, 41, 255),    // Pause menu button background           7
        new Color32(255, 255, 255, 255), // Pause menu button text                 8

        new Color32(33, 37, 41, 255),    // Ingame button background               9
        new Color32(33, 37, 41, 255),    // Ingame ui background 1                 10
        new Color32(255, 255, 255, 255), // Ingame ui background 2                 11

        new Color32(255, 255, 255, 255), // Unassigned                             12
        new Color32(255, 255, 255, 255), // Unassigned                             13
        new Color32(255, 255, 255, 255), // Unassigned                             14
        new Color32(255, 255, 255, 255), // Unassigned                             15
        new Color32(255, 255, 255, 255), // Unassigned                             16
        new Color32(255, 255, 255, 255), // Unassigned                             17

        new Color32(255, 255, 255, 255), // Default ingame text                    18
        new Color32(103, 183, 146, 255), // Stats display text positive colour     19
        new Color32(218, 110, 110, 255), // Stats display text negative colour     20

        //new Color32(168, 168, 168, 255), // Game background bad envrionment        21
        //new Color32(173, 222, 178, 255), // Game background good envrionment       22
    };


    /*
    //Vector4 PauseMenuColour = new Vector4(127, 127, 128, 128);

    // Maps integer to float value
    float MapIntToFloat(int input, int maxInput, float maxOutPut)
    {
        float floatValuePerInt = maxOutPut / maxInput;
        return input * floatValuePerInt;
    }
    */
}
