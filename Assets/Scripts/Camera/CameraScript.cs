using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    float cameraSpeed = 1.2f;

    float pSize;
    Vector2 pRes;

    public event Action onSizeChange;
    public event Action onResChange;
    public event Action<bool> onChangeScreenType;

    void LateUpdate()
    {

        // Move camera with WASD
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * cameraSpeed;
        float y = Input.GetAxis("Vertical") * Time.deltaTime * cameraSpeed;

        Vector3 move = new Vector3(x, y, 0);

        transform.Translate(move, Space.Self);
    }

    private void Update()
    {
        // Detects if the cameras size has changed
        if (Camera.main.orthographicSize != pSize)
        {
            pSize = Camera.main.orthographicSize;
            onSizeChange?.Invoke();
        }

        // Detects if the screen resolution has changed
        if (new Vector2(Screen.width, Screen.height) != pRes)
        {
            pRes = new Vector2(Screen.width, Screen.height);
            SetScreenType();
            onResChange?.Invoke();
        }
    }

    void SetScreenType()
    {
        bool largeScreen = false;
        if (Screen.width >= 1280 && Screen.height >= 720) // Detect large screen size
        {
            largeScreen = true;
        }

        onChangeScreenType?.Invoke(largeScreen);

    }

}
