using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextScript : MonoBehaviour
{
    [SerializeField]
    Slider slider;

    [SerializeField]
    Text sliderText;


    // Start is called before the first frame update
    void Start()
    {   
        // Make text the same as the slider value on startup
        sliderText.text = slider.value.ToString();

        // Whenever the slider value is changed update the text with the new value
        slider.onValueChanged.AddListener((value) => 
        {   
            //FindObjectOfType<AudioManager>().PlaySound("UIHover"); 
            sliderText.text = value.ToString();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
