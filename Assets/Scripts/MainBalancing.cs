using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MainBalancing : MonoBehaviour
{
    int turn;

    int tileGrowChancePerTurn = 50; // Percentage chance of a seed growing per turn
    int seedSpreadChancePerAdjacentTile = 50; // Percentage chance of a seed spreading to an adjacent tile per turn

    int[] buttonIds; // Array of tile ids corresponding to each building menu botton

    int sButton;
    int pButton;

    bool sHover;
    bool pHover;

    // Global stats
    int gCurrency = 0;
    int gCoal = 0, gReqCoal = 0;
    int gPower = 0, gReqPower = 0;
    int gEnvironment = 0;
    int gPopulation = 0;
    int gHapiness = 0;

    public event Action<int, int> onSendBalanceInfo; // Event responsible for sending balancing info to other scripts
    public event Action onNewTurn; // Called whenever there is a new turn so all scripts can keep track

    public event Action<int, string, bool, int> onSetBuildText; // Event responsible for updating text, and image visibility of build menu buttons
    public event Action<bool, bool> onSetBuildOpen; // Opens / closes the build menu, and enables / disables scroll rect
    public event Action<int, int, int, bool, Color> onSetStatText; // Event responsible for setting text of stats displays
    public event Action<int> onBuildButtonPressed; // Event gives TilesHandler tile to build on the isometric map
    public event Action resetClickedTile; // Does the equivelent of re clicking a tile after the buidling menu has been hovered. Otherwise the incorrect stats show in the stats display

    Color defaultColor = new Color(0.1960784f, 0.1960784f, 0.1960784f, 1); // Default color for stats displays (gray)
    Color positiveColor = new Color(0.4039216f, 0.7215686f, 0.5764706f, 1); // Positive color for stats display, used when showing a preview for what effects a tile has (green)
    Color negativeColor = new Color(0.8588235f, 0.4313726f, 0.4313726f, 1); // Negative color for stats display, used when showing a preview for what effects a tile has (red)

    public void IncrementTurn()
    {
        turn++;
        onNewTurn?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<TilesHandler>().onTileSelect += UpdateButtons;
        FindObjectOfType<TilesHandler>().onTileSend += TileStatsImpact;
        FindObjectOfType<TilesHandler>().onResetMStats += ResetMasterStats;

        FindObjectOfType<ButtonDetector>().hoveredButtons += SetHoveredBMButton;
        SelStats(-1, true);
    }

    void SetHoveredBMButton(int button, bool isHovered)
    {
        sButton = button;
        sHover = isHovered;

        // If condition is only met if the button or hover change
        // This is so the stat displays aren't spammed updated, that would block them being updated by other pieces of code
        if (sButton != pButton || sHover != pHover)
        {
            if (isHovered)
            {
                SelStats(buttonIds[button], true);
            }

            if (!isHovered)
            {
                resetClickedTile?.Invoke();
            }
        }

        pButton = sButton;
        pHover = sHover;
    }


    // Set selected tile id, this can be set by the clicked on tile or the hovered text in the build menu
    void SelStats(int id, bool notTilePressed)
    {
        if (id > -1)
        {
            SetStatsForBuildTiles(id, notTilePressed);
            return;
        }

        SetMasterStats();

    }

    // Sets the master stats
    // The master stats are all the stats combined of every tile, and other calculated stats such as hapiness and currency
    void SetMasterStats()
    {
        onSetStatText.Invoke(0, gPopulation, 0, false, defaultColor);
        onSetStatText.Invoke(1, gEnvironment, 0, false, defaultColor);
        onSetStatText.Invoke(2, gPower, gReqPower, true, defaultColor);
        onSetStatText.Invoke(3, gCoal, gReqCoal, true, defaultColor);
        onSetStatText.Invoke(4, gCurrency, 0, false, defaultColor);
    }


    // Reset the master stats
    // So stats don't accumulate (because the stats are updated at the start of each turn)
    void ResetMasterStats()
    {
        gCurrency = 0;
        gCoal = 0;
        gReqCoal = 0;
        gPower = 0;
        gReqPower = 0;
        gEnvironment = 0;
        gPopulation = 0;
        gHapiness = 0;
    }

    // Gets called when a building button is pressed
    public void BuildButtonPressed(int button)
    {
        SelStats(buttonIds[button], false);
        onBuildButtonPressed?.Invoke(buttonIds[button]);
    }

    // Update text and visibility of each building menu button based on the selected tile id
    void UpdateButtons(int id)
    {
        bool buttonActive = true;
        int[] buildIds = SetButtonsId(id);
        for (int i = 0; i < buildIds.Length; i++)
        {
            string bText = IdToString(buildIds[i]);

            // If there is not text the button should be deactivated
            if (bText == null)
                buttonActive = false;

            onSetBuildText?.Invoke(i, bText, buttonActive, buildIds[i]);
        }


        // Closes the building menu if all of the buttons have no text
        onSetBuildOpen?.Invoke(!CheckArrayConstant(-1, buildIds), ArrayLEActive(buildIds, -1, 2));
        SelStats(id, false);
    }

    // Checks if an array is >= activeNeeded number
    // Null num is a number that should be treated as nothing in the array
    bool ArrayLEActive(int[] array, int nullNum, int activeNeeded)
    {
        int activeButtons = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] > nullNum)
                activeButtons++;
        }

        if (activeButtons > activeNeeded)
            return true;
        return false;

    }

    // Returns true if a given int array only stores the value constant
    bool CheckArrayConstant(int constant, int[] array)
    {
        bool returnBool = true;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != constant)
                returnBool = false;
        }
        return returnBool;
    }

    // Look up table for tiles that can be built on selected tile
    // E.g. Index {1, 3, 5} can be built on index 2
    int[] SetButtonsId(int id)
    {
        switch (id)
        {
            case 0: // Water
                buttonIds = new int[] { 20, -1, -1, -1, -1, -1 }; // Hydro turbine
                break;

            case 1: // Barren Plains
                buttonIds = new int[] { 23, 9, -1, -1, -1, -1 }; // Coal plant, Seeds
                break;

            case 2: // Rocks
                buttonIds = new int[] { 24, 19, -1, -1, -1, -1 }; // Mines, Wind turbine
                break;

            case 3: // Plains
                buttonIds = new int[] { 8, -1, -1, -1, -1, -1 }; // Trees
                break;

            default:
                buttonIds = new int[] { -1, -1, -1, -1, -1, -1 };
                break;
        }
        return buttonIds;
    }

    // Gets the text for a button from an ID
    string IdToString(int id)
    {
        string text;
        switch (id)
        {
            case 0: // Water
                text = "Water";
                break;

            case 1: // Barren Plains
                text = "Barren Plains";
                break;

            case 2: // Rocks
                text = "Rocks";
                break;

            case 3: // Plains
                text = "Plains";
                break;

            case 8: // Trees
                text = "Trees";
                break;

            case 9: // Seeds
                text = "Seeds";
                break;

            case 19: // Wind Turbine
                text = "Wind Turbine";
                break;

            case 20: // Hydro Turbine
                text = "Hydro Turbine";
                break;

            case 23: // Coal Plant
                text = "Coal Plant";
                break;

            case 24: // Mines
                text = "Mines";
                break;


            default:
                text = null;
                break;
        }
        return text;
    }

    int[] TileStats(int id, bool includeCurrency)
    {
        // These variables describe the relative affect on their corresponding stats per turn
        // E.g. each water tile provides an additional 1 hapiness per turn
        // Currency only describes the effect a tile has on the curreny once. This is in the turn a new building tile is placed
        int currency;
        int coal;
        int power;
        int environment;

        int[] statsArray;

        switch (id)
        {
            case 0: // Water, non building
                currency = 0;
                coal = 0;
                power = 0;
                environment = 1;
                break;

            case 1: // Barren Plains, non building
                currency = 0;
                coal = 0;
                power = 0;
                environment = -1;
                break;

            case 2: // Rocks, non building
                currency = 0;
                coal = 0;
                power = 0;
                environment = 0;
                break;

            case 3: // Plains, building
                currency = 0;
                coal = 0;
                power = 0;
                environment = 1;
                break;

            case 8: // Trees, building
                currency = -5;
                coal = 0;
                power = 0;
                environment = 2;
                break;

            case 9: // Seeds, building
                currency = -5;
                coal = 0;
                power = 0;
                environment = 0;
                break;

            case 19: // Wind Turbine, building
                currency = -10;
                coal = 0;
                power = 2;
                environment = 0;
                break;

            case 20: // Hydro Turbine, building
                currency = -12;
                coal = 0;
                power = 3;
                environment = 0;
                break;

            case 23: // Coal Plant, building
                currency = -20;
                coal = -2;
                power = 8;
                environment = -4;
                break;

            case 24: // Mines, building
                currency = -10;
                coal = 4;
                power = 0;
                environment = -2;
                break;


            default:
                currency = 0;
                coal = 0;
                power = 0;
                environment = 0;
                break;
        }

        statsArray = new int[] {currency, coal, power, environment};
        if (!includeCurrency)
            statsArray = new int[] {0, coal, power, environment};

        return statsArray;
    }

    // Gets the difference a given tile Id makes on the master stats
    // Updates the master stats display if shouldUpdate is true
    void TileStatsImpact(int id, bool shouldUpdate)
    {
        int[] stats;
        stats = TileStats(id, false);

        gCoal += stats[1];
        gPower += stats[2];
        gEnvironment += stats[3];

        if (shouldUpdate)
        {
            // Calculations for power, hapiness, etc should go here
            SetMasterStats();
        }

    }


    // Holds information for what the effects are of tiles on stats
    // E.g. Hydro plant gives 3 power per turn, and costs 12 currency
    void SetStatsForBuildTiles(int id, bool includeCurrency)
    {
        int[] stats = TileStats(id, includeCurrency);

        int currency= stats[0];
        int coal = stats[1];
        int power = stats[2];
        int environment = stats[3];


        // Add impact stats to an array, the positions in the array correspond to the position of the display they are to be displayed in
        // There is a 0 at the start because hapiness and population are calculated independently, they don't only rely on tiles
        int[] tileStatsImpact = new int[] {0, environment, power, coal, currency};
        if (!includeCurrency)
            tileStatsImpact = new int[] {0, environment, power, coal, 0};

        for (int i = 0; i < tileStatsImpact.Length; i++)
        {
            int stat = tileStatsImpact[i];
            Color color;

            // Change colors of display depending on if the displayed number is negative, 0, or positive
            // This makes it easier to understand
            color = defaultColor;
            if ( stat > 0 )
                color = positiveColor;
            if ( stat < 0)
                color = negativeColor;

            onSetStatText.Invoke(i, stat, 0, false, color);
        }
    }


    // Update is called once per frame
    void Update()
    {
        // events like this that only need to be invoked at the start will not work
        // Because the subscriber will not be able to subscribe intime using FindObjectOfType
        onSendBalanceInfo?.Invoke(tileGrowChancePerTurn, seedSpreadChancePerAdjacentTile);
    }
}
