using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    Slider uiScaleSlider;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Start()
    {   
        // Set the uiScale value on startup
        MainBalancing.uiScale = (int)uiScaleSlider.value;

        // Whenever the slider value is changed update the uiScale
        uiScaleSlider.onValueChanged.AddListener((value) => 
        {
            MainBalancing.uiScale = (int)value;
        });
    }
}
