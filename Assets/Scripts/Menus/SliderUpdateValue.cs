using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdateValue : MonoBehaviour
{
    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private Slider uiScaleSlider;

    public event Action onSetUiScale;

    private void Start()
    {   
        // Set the slider values on startup
        uiScaleSlider.value = MainBalancing.uiScale;
        volumeSlider.value = AudioManager.volume;

        onSetUiScale?.Invoke();

        // Whenever the slider value is changed update the corresponding value
        // Events need to be called so code can run when values are changed
        // Otherwise the each script would have to check manually

        /*
            Whenever the slider value is changed update the corresponding value

            Events need to be called so code can run when values are changed,
            otherwise each script would have to check manually.
        */
        uiScaleSlider.onValueChanged.AddListener((value) => 
        {
            MainBalancing.uiScale = (int)value;
            onSetUiScale?.Invoke();
        });

        volumeSlider.onValueChanged.AddListener((value) => 
        {
            AudioManager.volume = (int)value;
        });
    }
}
