using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsScript : MonoBehaviour
{
    float uiScale = 5;
    float pad = 2;

    int keyPresses = 0;

    int statImageSize = 16; // Reference Width and Height in pixels of all stats images
    Vector2 currencyImageSize = new Vector2(32, 16); // Reference Width and Height in pixels of currency image


    bool statMenuOpen = true;

    [SerializeField]
    GameObject[] statsDisplays;

    [SerializeField]
    GameObject currencyDisplay;

    Vector2[] displayPositions;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<MainBalancing>().onSetStatText += SetStatText;

        displayPositions = new Vector2[statsDisplays.Length + 2];
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

                statsDisplays[i].GetComponent<RectTransform>().localScale = new Vector2(uiScale, uiScale); // Set image scale
                statsDisplays[i].GetComponent<RectTransform>().anchoredPosition = displayPositions[i]; // Set image position
            }
            cTransfrom.localScale = new Vector2(uiScale, uiScale); // Set currency image scale
            cTransfrom.anchoredPosition = displayPositions[statsDisplays.Length]; // Set currency image position to top
        }

        // Calculates positions for the scaled images to go to
        void CalculatePositions()
        {
            // Calculates size of stats images after they have been scaled
            float imageSizePostScale = statImageSize * (uiScale / 2);
            Vector2 currencyImageSizePostScale = new Vector2(currencyImageSize.x * (uiScale / 2), currencyImageSize.y * (uiScale / 2));

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

}
