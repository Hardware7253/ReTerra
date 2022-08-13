using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraScript : MonoBehaviour
{   
    float cameraSpeed = 0.4f;

    Vector3 startingPosition = new Vector3(-7, 8, -10);

    int dx = 0;
    int dy = 0;

    void Start()
    {
        transform.position = startingPosition;
        StartCoroutine(ChangeCameraMovement(10));
    }

    void LateUpdate()
    {   
        // Move camera with WASD
        float x = dx * Time.deltaTime * cameraSpeed;
        float y = dy * Time.deltaTime * cameraSpeed;

        Vector3 move = new Vector3(x, y, 0);

        transform.Translate(move, Space.Self);
    }

    IEnumerator ChangeCameraMovement(float seconds)
    {
        while (true)
        {   
            // Go up
            dx = 0;
            dy = 1;
            yield return new WaitForSeconds(seconds);

            // Go right
            dx = 1;
            dy = 0;
            yield return new WaitForSeconds(seconds * 3);

            // Go down
            dx = 0;
            dy = -1;
            yield return new WaitForSeconds(seconds);

            // Go left
            dx = -1;
            dy = 0;
            yield return new WaitForSeconds(seconds * 3);
        }
    }
}
