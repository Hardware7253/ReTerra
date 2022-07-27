using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField]
    Slider uiScaleSlider;

    public event Action onSetUiScale;
    
    // Start is called before the first frame update
    void Start()
    {
        // Set the uiScale value on startup
        MainBalancing.uiScale = (int)uiScaleSlider.value;

        // Whenever the slider value is changed update the uiScale
        uiScaleSlider.onValueChanged.AddListener((value) => 
        {
            MainBalancing.uiScale = (int)value;
            onSetUiScale?.Invoke();
        });
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Resume()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
