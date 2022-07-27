using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;              

public class MainBalancing : MonoBehaviour
{
    int turn;
    int uiScale = 5;

    float powerPerPerson = 0.003f; // Power consumed per person
    float currencyPerPerson = 0.001f; // Currency make per person

    int tileGrowChancePerTurn = 50; // Percentage chance of a seed growing per turn
    int seedSpreadChancePerAdjacentTile = 50; // Percentage chance of a seed spreading to an adjacent tile per turn

    int[] buttonIds; // Array of tile ids corresponding to each building menu botton

    int sButton;
    int pButton;

    bool sHover;
    bool pHover;


    int turnsVeryUnhappy = 0;


    // Global stats
    int gCurrency = 12;
    int gCoal = 0, gReqCoal = 0;
    int gPower = 0, gReqPower = 0;
    int gEnvironment = 0;
    int gPopulation = 0;
    float gHapiness = 0f;

    // Town stats
    int currencyMake = 0;
    int powerConsume = 0;
    int startPopulation = 200; // Not the actual starting population number, but this does have an affect on the starting population

    int minPopulation = 20;

    public event Action<int, int> onSendBalanceInfo; // Event responsible for sending balancing info to other scripts
    public event Action onNewTurn; // Called whenever there is a new turn so all scripts can keep track

    public event Action<int, string, bool, int> onSetBuildText; // Event responsible for updating text, and image visibility of build menu buttons
    public event Action<bool, bool> onSetBuildOpen; // Opens / closes the build menu, and enables / disables scroll rect
    public event Action<int, int, int, bool, Color> onSetStatText; // Event responsible for setting text of stats displays
    public event Action<int> onBuildButtonPressed; // Event gives TilesHandler tile to build on the isometric map
    public event Action resetClickedTile; // Does the equivelent of re clicking a tile after the buidling menu has been hovered. Otherwise the incorrect stats show in the stats display
    public event Action<int, int, int> onSendMarker; // Each time this is called markers are made
    public event Action onResetMarker; // Remove all markers on screen
    public event Action<bool> onSetCFText; // Sets the cant afford text on/off
    public event Action<int> onSetUiScale; // Gives other scripts the uiScale

    Color defaultColor = new Color(0.1960784f, 0.1960784f, 0.1960784f, 1); // Default color for stats displays (gray)
    Color positiveColor = new Color(0.4039216f, 0.7215686f, 0.5764706f, 1); // Positive color for stats display, used when showing a preview for what effects a tile has (green)
    Color negativeColor = new Color(0.8588235f, 0.4313726f, 0.4313726f, 1); // Negative color for stats display, used when showing a preview for what effects a tile has (red)


    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<TilesHandler>().onTileSelect += UpdateButtons;
        FindObjectOfType<TilesHandler>().onTileSend += TileStatsImpact;
        FindObjectOfType<TilesHandler>().onResetMStats += ResetMasterStats;
        FindObjectOfType<ButtonDetector>().hoveredButtons += SetHoveredBMButton;

        SelStats(-1, true);

        /*
            Future me.
            The town should be part of the stats display because it is too confusing at the moment.
            It should only visibly be part of the stats display though. So it doesn't take power priority over something
            that actaully needs it.

            THANK YOU ME YOU ARE AWESOME. HOT DAMN MY KEYBOARD SOUNDS AWESOME RIGHT NOW. I JUST WANT TO KEEP ON TYPING.
            BUT I CANNOT. CAPS MY BAD. JK SHIFT. OK I AM STOPPING NOW. GOOD BYE.
        */
    }


    public void IncrementTurn()
    {
        turn++;
        onNewTurn?.Invoke();
        // Deselect tile
    }

    void SetHoveredBMButton(int button, bool isHovered)
    {
        sButton = button;
        sHover = isHovered;

        // Condition is only met if the button or hover changes
        // This is so the stat displays aren't spam updated, that would block them being updated by other pieces of code
        if (sButton != pButton || sHover != pHover)
        {   
            int selId = buttonIds[button];

            // Turn on / off can't afford text
            SetCAffordText(selId, isHovered);

            if (isHovered)
                SelStats(selId, true);

            if (!isHovered)
                resetClickedTile?.Invoke();
            
        }

        pButton = sButton;
        pHover = sHover;
    }


    // Set selected tile id, this can be set by the clicked on tile or the hovered text in the build menu
    void SelStats(int id, bool notTilePressed)
    {   
        if (id > -1)
        {   
            // Sets stats accordingly if a tile is clicked or a building option is hovered
            SetStatsForBuildTiles(id, notTilePressed);
            return;
        }

        // Set master stats displays (when a tile isn't selected)
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


    // Reset stats that are calculated each turn
    // So stats don't accumulate
    void ResetMasterStats()
    {
        gCoal = 0;
        gReqCoal = 0;
        gPower = 0;
        gReqPower = 0;
        gEnvironment = 0;
        gPopulation = 0;
        gHapiness = 0;
    }

    // Gets called when a building button is pressed
    // Builds tiles if there is enough currecny
    public void BuildButtonPressed(int button)
    {   
        int selId = buttonIds[button];

        // Only builds tile if there is enough currency to do so
        if(CheckTileAffordable(selId))
        {
            gCurrency += TileStats(selId, true)[0];

            SelStats(buttonIds[button], false); // Set the stats displays to show the stats of the new tile
            onBuildButtonPressed?.Invoke(selId); // Place new tile
        }
        
    }

    // Returns true if the given tile id is affordable with the players current ammount of currency
    bool CheckTileAffordable(int id)
    {   
        int idCurrencyEffect = TileStats(id, true)[0];
        return (gCurrency + idCurrencyEffect) >= 0;
    }

    // Sets the can't afford text on/off
    void SetCAffordText(int id, bool buttonHovered)
    {
        onSetCFText?.Invoke(!CheckTileAffordable(id) && buttonHovered); // Turns on if the tile hovered is affordable and a building button is being hovered
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
        // 2D array makes better
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
    {   string text;

        // Each string corresponds to a tile id
        string[] tileNames = new string[]
        {
            "Town",               "Water",     "Rocks",             "Barren Plains",      "Seeds",
            "Seeds",              "Seeds",     "Plains",            "Park",               "Forest",
            "10",                 "11",        "12",                "13",                 "14",
            "15",                 "16",        "17",                "Hydro Turbine Mk.1", "Hydro Turbine Mk.2",
            "Hydro Turbine Mk.3", "21",        "Wind Turbine Mk.1", "Wind Turbine Mk.2",  "Wind Turbine Mk.3",
            "25",                 "Mine Mk.1", "Mine Mk.2",         "28",                 "Coal Plant Mk.1",
            "Coal Plant Mk.2",    "31",        "32",                "33",                 "34",
            "35",                 "36",        "37",                "38",                 "39",
            "40",                 "41"
        };

        text = null;
        if (id < tileNames.Length)
            text = tileNames[id];
        return text;
    }


    // Holds information for what the effects are of tiles on stats
    // E.g. Hydro plant gives 3 power per turn, and costs 12 currency
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

        /*
        Old shit delete later
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
                currency = -10;
                coal = 0;
                power = 0;
                environment = 4;
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
                environment = -1;
                break;

            case 20: // Hydro Turbine, building
                currency = -12;
                coal = 0;
                power = 3;
                environment = -1;
                break;

            case 23: // Coal Plant, building
                currency = -20;
                coal = -2;
                power = 8;
                environment = -6;
                break;

            case 24: // Mines, building
                currency = -10;
                coal = 4;
                power = -4;
                environment = -3;
                break;

            case 29: // Town, building
                currency = currencyMake;
                coal = 0;
                power = powerConsume * -1;
                environment = 0;
                break;

            default:
                currency = 0;
                coal = 0;
                power = 0;
                environment = 0;
                break;
        }
        */

        /*  
            2D array defines stats for each tile
            Each sub array represent a tile id, the id is the sub arrays position in the master array
            Each element in the sub array represents a stat impact
            Element 0 of the sub array is the currency impact
            Element 1 of the sub array is the coal impact
            Element 2 of the sub array is the power impact
            Element 3 of the sub array is the environment impact
        */
        int[,] tileStatsArray = new int[,]
        {
            {0, 0, 0, 0}, // Town
            {0, 0, 0, 1}, // Water
            {0, 0, 0, 0}, // Rocks
            {0, 0, 0 -1}, // Barren Plains
            {-5, 0, 0, 0}, // Seeds
            {0, 0, 0, 0}, // Seeds (Growing1)
            {0, 0, 0, 0}, // Seeds (Growing2)
            {0, 0, 0, 1}, // Plains
            {-10, 0, 0, 4}, // Park
            {-13, 0, 0, 10}, // Forest
            {0, 0, 0, 0}, // 10
            {0, 0, 0, 0}, // 11
            {0, 0, 0, 0}, // 12
            {0, 0, 0, 0}, // 13
            {0, 0, 0, 0}, // 14
            {0, 0, 0, 0}, // 15
            {0, 0, 0, 0}, // 16
            {0, 0, 0, 0}, // 17
            {-12, 0, 3, -1}, // Hydro Turbine Mk.1
            {-15, 0, 8, -2}, // Hydro Turbine Mk.2
            {-17, 0, 16, -4}, // Hydro Turbine Mk.3
            {0, 0, 0, 0}, // 21
            {-10, 0, 2, -1}, // Wind Turbine Mk.1
            {-12, 0, 8, -2}, // Wind Turbine Mk.2
            {-15, 0, 14, -3}, // Wind Turbine Mk.3
            {0, 0, 0, 0}, // 25
            {-10, 4, -4, -3}, // Mine Mk.1
            {-23, 12, -14, -6}, // Mine Mk.2
            {0, 0, 0, 0}, // 28
            {-20, -2, 8, -6}, // Coal Plant Mk.1
            {-30, -4, 18, -12}, // Coal Plant Mk.2
            {0, 0, 0, 0}, // 31
            {0, 0, 0, 0}, // 32
            {0, 0, 0, 0}, // 33
            {0, 0, 0, 0}, // 34
            {0, 0, 0, 0}, // 35
            {0, 0, 0, 0}, // 36
            {0, 0, 0, 0}, // 37
            {0, 0, 0, 0}, // 38
            {0, 0, 0, 0}, // 39
            {0, 0, 0, 0}, // 40
        };

        switch (id)
        {
            case 0: // Town, Building
                currency = currencyMake;
                coal = 0;
                power = powerConsume * -1;
                environment = 0;
                break;

            case 1: // Water, Non Building
                currency = 0;
                coal = 0;
                power = 0;
                environment = 1;
                break;

            case 2: // Rocks, Non Building
                currency = 0;
                coal = 0;
                power = 0;
                environment = 0;
                break;

            case 3: // Barren PLains, Non Building
                currency = 0;
                coal = 0;
                power = 0;
                environment = -1;
                break;

            case 4: // Seeds, Building
                currency = -5;
                coal = 0;
                power = 0;
                environment = 0;
                break;

            case 5: // Seeds (Growing1), Building
                currency = 0;
                coal = 0;
                power = 0;
                environment = 0;
                break;

            case 6: // Seeds (Growing2), Building
                currency = 0;
                coal = 0;
                power = 0;
                environment = 0;
                break;

            case 7: // Plains, Building
                currency = 0;
                coal = 0;
                power = 0;
                environment = 1;
                break;

            case 8: // Park, Building
                currency = -10;
                coal = 0;
                power = 0;
                environment = 4;
                break;

            case 9: // Forest, Building
                currency = -20;
                coal = 0;
                power = 0;
                environment = 10;
                break;

            case 18: // Hydro Turbine Mk.1, Building
                currency = -12;
                coal = 0;
                power = 3;
                environment = -1;
                break;

            case 19: // Hydro Turbine Mk.2, Building
                currency = -20;
                coal = 0;
                power = 8;
                environment = -2;
                break;
            
            case 20: // Hydro Turbine Mk.3, Building
                currency = -30;
                coal = 0;
                power = 16;
                environment = -4;
                break;
            
            case 22: // Wind Turbine Mk.1, Building
                currency = -10;
                coal = 0;
                power = 2;
                environment = -1;
                break;
            
            case 23: // Wind Turbine Mk.2, Building
                currency = -20;
                coal = 0;
                power = 7;
                environment = -2;
                break;
            
            case 24: // Wind Turbine Mk.3, Building
                currency = -40;
                coal = 0;
                power = 15;
                environment = -3;
                break;
            
            case 26: // Mine Mk.1, Building
                currency = -10;
                coal = 4;
                power = -4;
                environment = -3;
                break;
            
            case 27: // Mine Mk.2, Building
                currency = -20;
                coal = 12;
                power = -14;
                environment = -6;
                break;
            
            case 29: // Coal Plant Mk.1, Building
                currency = -20;
                coal = -2;
                power = 8;
                environment = -6;
                break;

            case 30: // Coal Plant Mk.2, Building
                currency = -30;
                coal = -4;
                power = 18;
                environment = -12;
                break;

            default:
                currency = 0;
                coal = 0;
                power = 0;
                environment = 0;
                break;
        }

        statsArray = tileStatsArray[id];

        // Town stats aren't in the array
        if (id == 0)
            statsArray = new int[] {currencyMake, 0, powerConsume * -1, 0};

        if (!includeCurrency)
            statsArray = new int[] {0, coal, power, environment};

        return statsArray;
    }

    // Gets the difference a given tile Id makes on the master stats
    // Updates the master stats display if shouldUpdate is true
    void TileStatsImpact(int id, bool shouldUpdate)
    {   
        // Id 29 (town) affects stats differently so it is skipped
        if (id == 29)
            return;

        int[] stats;
        stats = TileStats(id, false);

        int lCoal = stats[1];
        int lPower = stats[2];
        int lEnvironment = stats[3];

        gCoal += lCoal;
        gPower += lPower;
        gEnvironment += lEnvironment;


        // If coal or power is less than 0 negate the effect on the made display, and add the affect to the required display (the part after the slash)
        if (lCoal < 0)
        {
            int lCoalUns = Math.Abs(lCoal);

            gCoal += lCoalUns;
            gReqCoal += lCoalUns;
        }


        if (lPower < 0)
        {
            int lPowerUns = Math.Abs(lPower);

            gPower += lPowerUns;
            gReqPower += lPowerUns;
        }


        if (shouldUpdate)
        {   
            DoMarkers();
            CalcHapiness();
            CalcPopulation();
            CalcPeopleUsedPower();
            CalcCurrencyMake();
            SetMasterStats();
        }

    }

    // Updates markers
    void DoMarkers()
    {   
        /*  
            2D array for tiles that could potentially require markers
            Each sub array represents a tile
            Element 0 of the sub array is the tile id
            Element 1 of the sub array is the required markers (this is calculated so they have a default of 0)
            Element 2 of the sub array is the marker type (0 = coal marker, 1 = power marker)
            Element 3 of the sub array is the stat type the tile produces (0 = currency, 1 = coal, 2 = power, 3 = environment)
        */
        int[,] idsReqMarkers = new int[5, 4]
        {
            {0,  0, 1, 0}, // Town
            {26, 0, 1, 1}, // Mine Mk.1
            {27, 0, 1, 1}, // Mine Ml.2
            {29, 0, 0, 2}, // Coal Plant Mk.1
            {30, 0, 0, 2}  // Coal Plant Mk.2
        };

        // Gets dimensions of the above array
        Vector2Int idsRMDimensions = new Vector2Int(idsReqMarkers.GetLength(0), idsReqMarkers.GetLength(1));

        onResetMarker?.Invoke();
        for (int i = 0; i < idsRMDimensions.x; i++)
        {   
            idsReqMarkers[i, 1] = CalcMarkers(idsReqMarkers[i, 0])[idsReqMarkers[i, 2]]; // Get required markers for id at i position, put required markers into position 1 of the 2nd array
            onSendMarker?.Invoke(idsReqMarkers[i, 0], idsReqMarkers[i, 1], idsReqMarkers[i, 2]); // Set markers for id at i position in array

            // Negate impact of id from master stats
            int impact = idsReqMarkers[i, 1] * TileStats(idsReqMarkers[i, 0], false)[idsReqMarkers[i, 3]];
            switch (idsReqMarkers[i, 3]) // Switch makes sure the right stat is impacted
            {
                case 1:
                    gPower -= impact;
                    break;

                case 2:
                    gCoal -= impact;
                    break;

                default:
                    break;
            }
        }

        SetMasterStats();
    }

    // Calculates how many markers are needed for a certain tile id
    // Markers show the player that they do not have enough of a certain resource to properly use a building
    int[] CalcMarkers(int id)
    {   
        int[] idStats = TileStats(id, false);

        int coalConsumedPerTile = Math.Abs(idStats[1]);
        int powerConsumedPerTile = Math.Abs(idStats[2]);


        // Calculate markers needed
        int cCounters = (int)Mathf.Ceil((gReqCoal - gCoal) /  coalConsumedPerTile);
        int pCounters = (int)Mathf.Ceil((gReqPower - gPower) / powerConsumedPerTile);

        int[] counters = new int[] {cCounters, pCounters};

        // Remove any values less than 0
        for (int i = 0; i < counters.Length; i++)
        {
            if (counters[i] < 0)
                counters[i] = 0;
        }

        // [0] coal markers, [1] power markers
        return counters;
    }


    // Sets the stats displays
    void SetStatsForBuildTiles(int id, bool includeCurrency)
    {   
        if (id == 29)
            includeCurrency = true;

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

    // Calculate town hapiness
    void CalcHapiness()
    {
        int hapiness = 0;
        int excessPower = gPower - gReqPower;

        hapiness += (int)Mathf.Ceil(gEnvironment * 1.75f);
        hapiness += (int)Mathf.Ceil(excessPower * 1.65f);

        // Hapiness cannot be less that 0, also increment turns very unhappy
        if (hapiness < 0)
        {
            hapiness = 0;
            turnsVeryUnhappy++;
        }

        // Reset turns that the people have been very unhappy
        if (hapiness > 0)
        {
            turnsVeryUnhappy = 0;
        }  

        gHapiness = MapIntFloat(100, hapiness); 

        if (gHapiness > 1)
            gHapiness = 1;
    }

    // Maps an integer value to a float value
    float MapIntFloat(int divide, int val)
    {   
        float outVal = (float)val / divide;
        return outVal;
    }

    // Calculate town population
    void CalcPopulation()
    {   
        int negPopulationImpact = 0;
        float hapiness = gHapiness;
        if (gHapiness == 0)
            hapiness = 0.01f;

        // Calculate negative population impact if the townspeople are very unhappy
        if (turnsVeryUnhappy > 0)
        {   
            negPopulationImpact = (int)Mathf.Ceil((turnsVeryUnhappy * turn) * -3);
        }
        
        // Calculate population
        gPopulation += (int)Mathf.Ceil((startPopulation * turn) * (hapiness * 2)) + negPopulationImpact;

        // Make sure population does not go below minimum population
        if (gPopulation < minPopulation)
        {
            gPopulation = minPopulation;
        }
    }

    // Calculate the ammount of power that is needed for the current population
    void CalcPeopleUsedPower()
    {
        powerConsume = (int)Mathf.Ceil(gPopulation * powerPerPerson); // Multiply population by power used per person, round up to the nearest int
    }


    void CalcCurrencyMake()
    {
        float hapiness = gHapiness;
        if (gHapiness == 0)
            hapiness = 0.01f;

        currencyMake = (int)Mathf.Ceil((gPopulation * currencyPerPerson) * hapiness);
        gCurrency += 0;
    }


    // Update is called once per frame
    void Update()
    {
        // events like this that only need to be invoked at the start will not work
        // Because the subscriber will not be able to subscribe intime using FindObjectOfType
        onSendBalanceInfo?.Invoke(tileGrowChancePerTurn, seedSpreadChancePerAdjacentTile);
        //onSetUiScale?.Invoke(uiScale); // Only call when uiScale is updated
    }
}
