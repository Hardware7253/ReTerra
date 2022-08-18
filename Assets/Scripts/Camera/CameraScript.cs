using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    float cameraSpeed = 1.2f;

    int ppusIndexer = 1;
    int[] ppus = new int[] {32, 64, 128};

    float[] camXLimits = new float[2] {-4, 4};
    float[] camYLimits = new float[2] {-0.5f, 4};
    void LateUpdate()
    {   
        // Return if pauseMenu is open
        // Because camera should not be able to move when the game is paused
        if (EscapeLevelHandler.pauseMenuOpen)
            return;

        // Move camera with WASD
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * cameraSpeed;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * cameraSpeed;

        Vector3 move = new Vector3(x, y, 0);

        transform.Translate(move, Space.Self);

        
        CamerRestriction();
        
    }

    // Restrict camera movement
    void CamerRestriction()
    {
        Vector3 pos = gameObject.transform.position;
        float outPosX = pos.x;
        float outPosY = pos.y;

        bool shouldUpdate = false;

        // Camera restriction for negative x
        if (pos.x < camXLimits[0])
        {
            outPosX = camXLimits[0];
            shouldUpdate = true;
        }

        // Camera restriction for positive x
        if (pos.x > camXLimits[1])
        {
            outPosX = camXLimits[1];
            shouldUpdate = true;
        }

        // Camera restriction for negative y
        if (pos.y < camYLimits[0])
        {
            outPosY = camYLimits[0];
            shouldUpdate = true;
        }

        // Camera restriction for positive y
        if (pos.y > camYLimits[1])
        {
            outPosY = camYLimits[1];
            shouldUpdate = true;
        }

        if (shouldUpdate)
            gameObject.transform.position = new Vector3(outPosX, outPosY, -1);
    }
}
