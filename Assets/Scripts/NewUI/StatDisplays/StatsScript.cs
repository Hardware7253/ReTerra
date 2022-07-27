using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScript : MonoBehaviour
{
    float uiScaleL = (float)MainBalancing.uiScale;
    float pad = 2;

    int keyPresses = 0;

    int statImageSize = 16; // Reference Width and Height in pixels of all stats images
    Vector2 currencyImageSize = new Vector2(32, 16); // Reference Width and Height in pixels of currency image


    bool statMenuOpen = true;

    [SerializeField]
    GameObject[] statsDisplays;

    [SerializeField]
    GameObject currencyDisplay;

    [SerializeField]
    Sprite[] populationImages;

    Vector2[] displayPositions;

    // Start is called before the first frame update
    void Start()
    {   
        FindObjectOfType<PauseMenuScript>().onSetUiScale += SetUiScale;
        FindObjectOfType<MainBalancing>().onSetStatText += SetStatText;
        FindObjectOfType<MainBalancing>().onSendHapiness += SetHapiness;

        displayPositions = new Vector2[statsDisplays.Length + 2];
        ShowStats(statMenuOpen);
        SetHapiness(0f);
    }

    // Called whenever uiScale is changed
    void SetUiScale()
    {
        uiScaleL = (float)MainBalancing.uiScale;
        
        ShowStats(statMenuOpen);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            keyPresses++;
            ToggleStats();
        }
    }

    // Toggles the stats whenever it is called
    void ToggleStats()
    {
        if ((keyPresses % 2) == 0)
        {
            ShowStats(true);
            return;
        }
        ShowStats(false);
    }


    // Container for all things relating to the scaling and positioning of the stats displays
    void ShowStats(bool active)
    {
        SetStats(active);

        // Turns the stats displays on/off
        void SetStats(bool statsActive)
        {
            SetPositions();

            if (!statsActive)
            {
                // Disable every stats display apart from currency
                for (int i = 0; i < statsDisplays.Length; i++)
                {
                    statsDisplays[i].GetComponent<Image>().enabled = false;
                    statsDisplays[i].GetComponentInChildren<Text>().enabled = false;
                }

                currencyDisplay.GetComponent<RectTransform>().anchoredPosition = displayPositions[statsDisplays.Length + 1]; // Set currency image position to bottom
            }
        }

        // Set displays to their scaled positions
        void SetPositions()
        {
            CalculatePositions();
            RectTransform cTransfrom = currencyDisplay.GetComponent<RectTransform>();

            for (int i = 0; i < statsDisplays.Length; i++)
            {
                statsDisplays[i].GetComponent<Image>().enabled = true;
                statsDisplays[i].GetComponentInChildren<Text>().enabled = true;

                statsDisplays[i].GetComponent<RectTransform>().localScale = new Vector2(uiScaleL, uiScaleL); // Set image scale
                statsDisplays[i].GetComponent<RectTransform>().anchoredPosition = displayPositions[i]; // Set image position
            }
            cTransfrom.localScale = new Vector2(uiScaleL, uiScaleL); // Set currency image scale
            cTransfrom.anchoredPosition = displayPositions[statsDisplays.Length]; // Set currency image position to top
        }

        // Calculates positions for the scaled images to go to
        void CalculatePositions()
        {
            // Calculates size of stats images after they have been scaled
            float imageSizePostScale = statImageSize * (uiScaleL / 2);
            Vector2 currencyImageSizePostScale = new Vector2(currencyImageSize.x * (uiScaleL / 2), currencyImageSize.y * (uiScaleL / 2));

            Vector2 bottomPosition = new Vector2(imageSizePostScale + pad, imageSizePostScale + pad); // The position of the bottom most image
            for(int i = 0; i < statsDisplays.Length; i++)
                displayPositions[i] = new Vector2(bottomPosition.x, bottomPosition.y + ((imageSizePostScale * 2 + pad * 2) * i)); // Increases bottomPos by postImageScale + pad * 2 each iteration

            displayPositions[statsDisplays.Length + 1] = new Vector2(currencyImageSizePostScale.x + pad, currencyImageSizePostScale.y + pad); // currency position when it's at the bottom
            displayPositions[statsDisplays.Length] = new Vector2(displayPositions[statsDisplays.Length + 1].x, displayPositions[statsDisplays.Length - 1].y + (imageSizePostScale) + currencyImageSizePostScale.y + (pad * 2)); // Currency position when it's at the top
        }
    }

    // Set text of stats displays
    // Make changing of stats more smooth at some point
    void SetStatText(int display, int supply, int demand, bool isSupDem, Color color)
    {   
        if (display >= statsDisplays.Length)
        {
            Text cText = currencyDisplay.GetComponentInChildren<Text>();

            cText.text = " " + supply;
            cText.color = color;
            return;
        }


        Text sText = statsDisplays[display].GetComponentInChildren<Text>();

        sText.text = " " + supply + "/" + demand;

        if (!isSupDem)
            sText.text = " " + supply; // Ignore demand if the display mode is not supply/demand
        sText.color = color;

    }

    // Sets which sprite is used for the population display
    // This visually shows population hapiness to the player
    void SetHapiness(float hapiness)
    {   
        int spriteIndex = Map(hapiness, populationImages.Length, 1f);
        statsDisplays[0].GetComponent<Image>().sprite = populationImages[spriteIndex];
    }

    // Maps float to integer value E.g. If input = 0.21, maxOutVal = 1, maxInVal = 1f then the output will be 1
    // This is because (input <= 0.5) = 0 and (input > 0.5) = 1, each integer is mapped to a specific section of the float
    int Map(float input, int maxOutVal, float maxInVal)
    {
        float fValPerItem = maxInVal / maxOutVal;

        for (int i = 1; i <= maxOutVal; i++)
        {
            if (input <= fValPerItem * i) // Test to see if the the input value matches with a section of the float value
            {
                return i - 1; // Subtract by 1 to get the proper index, the for loop can't start at 0 because i has to be used to multiply
            }
        }
        return maxOutVal; // Return maxOutVal if the for loop fails
    }

}
