using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;              

public class MainBalancing : MonoBehaviour
{
    // Represents names for each tile id
    string[] tileNames = new string[]
    {
        "Town",               // id 0
        "Water",              // id 1
        "Rocks",              // id 2
        "Barren Plains",      // id 3
        "Seeds",              // id 4
        "Seeds",              // id 5
        "Seeds",              // id 6
        "Plains",             // id 7
        "Park",               // id 8
        "Forest",             // id 9
        "10",                 // id 10
        "11",                 // id 11
        "12",                 // id 12
        "13",                 // id 13
        "14",                 // id 14
        "15",                 // id 15
        "16",                 // id 16
        "17",                 // id 17
        "Hydro Turbine Mk.1", // id 18
        "Hydro Turbine Mk.2", // id 19
        "Hydro Turbine Mk.3", // id 20
        "21",                 // id 21
        "Wind Turbine Mk.1",  // id 22
        "Wind Turbine Mk.2",  // id 23
        "Wind Turbine Mk.3",  // id 24
        "25",                 // id 25
        "Mine Mk.1",          // id 26
        "Mine Mk.2",          // id 27
        "28",                 // id 28
        "Coal Plant Mk.1",    // id 29
        "Coal Plant Mk.2",    // id 30
        "31",                 // id 31
        "32",                 // id 32
        "33",                 // id 33
        "34",                 // id 34
        "35",                 // id 35
        "36",                 // id 36
        "37",                 // id 37
        "38",                 // id 38
        "39",                 // id 39
        "40"                  // id 40
    };


    /*
        2D array that defines what tiles can be built on a given tile
        Each sub array represents a tile (the id corresponds to the index of the array)
        Each item within that sub array represents the id's that can be built on that tile
        -1 is the no buildable tile id

    */
    int[,] buildableTiles = new int[,]
    {
        {-1, -1, -1, -1, -1, -1}, // Town (id0)
        {18, -1, -1, -1, -1, -1}, // Water (id1)
        {22, 26, -1, -1, -1, -1}, // Rocks(id2)
        {29 , 4, -1, -1, -1, -1}, // Barren Plains (id3)
        {-1, -1, -1, -1, -1, -1}, // Seeds (id4)
        {-1, -1, -1, -1, -1, -1}, // Seeds (Growing1) (id5)
        {-1, -1, -1, -1, -1, -1}, // Seeds (Growing2) (id6)
        {8 , -1, -1, -1, -1, -1}, // Plains (id7)
        {9 , -1, -1, -1, -1, -1}, // Park (id8)
        {-1, -1, -1, -1, -1, -1}, // Forest (id9)
        {-1, -1, -1, -1, -1, -1}, // 10
        {-1, -1, -1, -1, -1, -1}, // 11
        {-1, -1, -1, -1, -1, -1}, // 12
        {-1, -1, -1, -1, -1, -1}, // 13
        {-1, -1, -1, -1, -1, -1}, // 14
        {-1, -1, -1, -1, -1, -1}, // 15
        {-1, -1, -1, -1, -1, -1}, // 16
        {-1, -1, -1, -1, -1, -1}, // 17
        {19, -1, -1, -1, -1, -1}, // Hydro Turbine Mk.1 (id18)
        {20, -1, -1, -1, -1, -1}, // Hydro Turbine Mk.2 (id19)
        {-1, -1, -1, -1, -1, -1}, // Hydro Turbine Mk.3 (id20)
        {-1, -1, -1, -1, -1, -1}, // 21
        {23, -1, -1, -1, -1, -1}, // Wind Turbine Mk.1 (id22)
        {24, -1, -1, -1, -1, -1}, // Wind Turbine Mk.2 (id23)
        {-1, -1, -1, -1, -1, -1}, // Wind Turbine Mk.3 (id24)
        {-1, -1, -1, -1, -1, -1}, // 25
        {27, -1, -1, -1, -1, -1}, // Mine Mk.1 (id26)
        {-1, -1, -1, -1, -1, -1}, // Mine Mk.2 (id27)
        {-1, -1, -1, -1, -1, -1}, // 28
        {30, -1, -1, -1, -1, -1}, // Coal Plant Mk.1 (id29)
        {-1, -1, -1, -1, -1, -1}, // Coal Plant Mk.2 (id30)
        {-1, -1, -1, -1, -1, -1}, // 31
        {-1, -1, -1, -1, -1, -1}, // 32
        {-1, -1, -1, -1, -1, -1}, // 33
        {-1, -1, -1, -1, -1, -1}, // 34
        {-1, -1, -1, -1, -1, -1}, // 35
        {-1, -1, -1, -1, -1, -1}, // 36
        {-1, -1, -1, -1, -1, -1}, // 37
        {-1, -1, -1, -1, -1, -1}, // 38
        {-1, -1, -1, -1, -1, -1}, // 39
        {-1, -1, -1, -1, -1, -1}  // 40
    };


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
    //   $, c, p, e    
        {0, 0, 0, 0},       // Town
        {0, 0, 0, 1},       // Water
        {0, 0, 0, 0},       // Rocks
        {0, 0, 0, -1},      // Barren Plains
        {-5, 0, 0, 0},      // Seeds
        {0, 0, 0, 0},       // Seeds (Growing1)
        {0, 0, 0, 0},       // Seeds (Growing2)
        {0, 0, 0, 1},       // Plains
        {-10, 0, 0, 4},     // Park
        {-13, 0, 0, 10},    // Forest
        {0, 0, 0, 0},       // 10
        {0, 0, 0, 0},       // 11
        {0, 0, 0, 0},       // 12
        {0, 0, 0, 0},       // 13
        {0, 0, 0, 0},       // 14
        {0, 0, 0, 0},       // 15
        {0, 0, 0, 0},       // 16
        {0, 0, 0, 0},       // 17
        {-12, 0, 3, -1},    // Hydro Turbine Mk.1
        {-15, 0, 8, -2},    // Hydro Turbine Mk.2
        {-17, 0, 16, -4},   // Hydro Turbine Mk.3
        {0, 0, 0, 0},       // 21
        {-10, 0, 2, -1},    // Wind Turbine Mk.1
        {-12, 0, 8, -2},    // Wind Turbine Mk.2
        {-15, 0, 14, -3},   // Wind Turbine Mk.3
        {0, 0, 0, 0},       // 25
        {-10, 4, -4, -3},   // Mine Mk.1
        {-23, 12, -14, -6}, // Mine Mk.2
        {0, 0, 0, 0},       // 28
        {-20, -2, 8, -6},   // Coal Plant Mk.1
        {-30, -4, 18, -12}, // Coal Plant Mk.2
        {0, 0, 0, 0},       // 31
        {0, 0, 0, 0},       // 32
        {0, 0, 0, 0},       // 33
        {0, 0, 0, 0},       // 34
        {0, 0, 0, 0},       // 35
        {0, 0, 0, 0},       // 36
        {0, 0, 0, 0},       // 37
        {0, 0, 0, 0},       // 38
        {0, 0, 0, 0},       // 39
        {0, 0, 0, 0}        // 40
    };


    /*  
        2D array for tiles that could potentially require markers
        Each sub array represents a tile
        Element 0 of the sub array is the tile id
        Element 1 of the sub array is the required markers (this is calculated so they have a default of 0)
        Element 2 of the sub array is the marker type (0 = coal marker, 1 = power marker)
        Element 3 of the sub array is the stat type the tile produces (0 = currency, 1 = coal, 2 = power, 3 = environment)
    */
    int[,] idsReqMarkers = new int[,]
    {
        {0,  0, 1, 0}, // Town
        {26, 0, 1, 1}, // Mine Mk.1
        {27, 0, 1, 1}, // Mine Ml.2
        {29, 0, 0, 2}, // Coal Plant Mk.1
        {30, 0, 0, 2}  // Coal Plant Mk.2
    };

    // Each element in the array shows the consumption of all tiles on a stat
    // E.g. If element 1 was -18 there would be 18 units of coal consumed across all tiles
    //                                $, c, p, e
    int[] tilesConsumption = new int[] {0, 0, 0, 0};


    int turn;
    public static int uiScale = 5;

    float powerPerPerson = 0.003f; // Power consumed per person
    float currencyPerPerson = 0.001f; // Currency make per person

    int tileGrowChancePerTurn = 50; // Percentage chance of a seed growing per turn
    int seedSpreadChancePerAdjacentTile = 50; // Percentage chance of a seed spreading to an adjacent tile per turn

    int[] buttonIds = new int[] {-1, -1, -1, -1, -1, -1}; // Array of tile ids corresponding to each building menu botton

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
    public event Action<Vector3Int, int> onPlaceMarker; // PLace marker
    public event Action<bool> onSetCFText; // Sets the cant afford text on/off
    public event Action<float> onSendHapiness; // Gives the hapiness value to other scripts. This is used to seth the sprite for the population display

    Color defaultColor = Colours.colorsArray[18]; // Default color for stats displays text (gray)
    Color positiveColor = Colours.colorsArray[19]; // Positive color for stats display text, used when showing a preview for what effects a tile has (green)
    Color negativeColor = Colours.colorsArray[20]; // Negative color for stats display text, used when showing a preview for what effects a tile has (red)


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

            Yo also the population and money calculations are broken
            Have fun.

            Markers busted
        */
    }


    public void IncrementTurn()
    {
        turn++; 
        onNewTurn?.Invoke();
    }

    void SetHoveredBMButton(int button, bool isHovered, bool changeCheckOverride)
    {
        sButton = button;
        sHover = isHovered;

        // Condition is only met if the button or hover changes
        // This is so the stat displays aren't spam updated, that would block them being updated by other pieces of code
        if (sButton != pButton || sHover != pHover || changeCheckOverride)
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
        
        // Add town power consume as a ghost stat (because it shouldn't be included in calculations) because it should be visible on the power required display
        onSetStatText.Invoke(2, gPower, gReqPower + powerConsume, true, defaultColor);
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

            // Update
            SetHoveredBMButton(button, true, true);
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

    // Get the ids each build button represents
    // The information is from the buildable tiles array
    int[] SetButtonsId(int id)
    {   
        for (int i = 0; i < buildableTiles.GetLength(1); i++)
        {   
            // Protection to make sure the index does not go outside the bounds of the buttonIds array (some values have a default of -1)
            if (id > 0)
                buttonIds[i] = buildableTiles[id, i];
        }
        
        return buttonIds;
    }

    // Gets the text for a button from an ID
    string IdToString(int id)
    {   string text = null;

        // Protection to make sure the index does not go outside the bounds of the tileNames array (some values have a default of -1)
        if (id > 0)
            text = tileNames[id];
        return text;
    }

    // Returns an array of the stats a given id has
    // E.g. Hydro plant gives 3 power per turn, and costs 12 currency
    // Output array: 0 = curreny, 1 = coal, 2 = power, 3 = environment
    int[] TileStats(int id, bool includeCurrency)
    {   
        int[] statsArray = new int[] {0, 0, 0, 0};

        // Sometimes -1 is used as a default, it shouldn't run the full function
        if (id < 0)
        {
            return statsArray;
        }

        
        for (int i = 0; i < tileStatsArray.GetLength(1); i++)
        {   
            statsArray[i] = tileStatsArray[id, i];
        }

        // Town stats aren't in the tileStatsArray, because some stats are not constant
        if (id == 0)
            statsArray = new int[] {currencyMake, 0, powerConsume * -1, 0};

        if (!includeCurrency)
            statsArray[0] = 0;

        return statsArray;
    }

    // Gets the difference a given tile Id makes on the master stats
    // Updates the master stats display if shouldUpdate is true
    void TileStatsImpact(int id, bool shouldUpdate)
    {   
        // Id 0 (Town) and lower should be skipped, because town works differently to other tiles
        if (id < 1)
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
            onSendHapiness?.Invoke(gHapiness);


            CalcPopulation();
            CalcPeopleUsedPower();
            CalcCurrencyMake();

            if (!BuildingMenuScript.buildingMenuOpen)
                SetMasterStats();
        }

    }


    void DoMarkers()
    {   
        // Reset
        onResetMarker?.Invoke();
        tilesConsumption = new int[] {0, 0, 0, 0};

        Vector3Int townPos = new Vector3Int(0, 0, 0);

        for (int x = 0; x < TilesHandler.width; x++)
        {
            for (int y = 0; y < TilesHandler.height; y++)
            {   
                int selId = TilesHandler.tileGrid[x, y];
                if (IdReqMarker(selId)) // Check if the selected id is an id in the idsReqMarkers array
                {   
                    
                    if (selId != idsReqMarkers[0, 0])
                    {   
                        int idMarkerType = CalcMarker(selId); // Calculate what type of marker the selected id needs
                        if (idMarkerType == 0 || idMarkerType == 1) // Marker types that aren't 0 or 1 are invalid
                            onPlaceMarker?.Invoke(new Vector3Int(x, y, 0), idMarkerType); // Place idMarkerType at x, y position on tilemap
                    }
                    
                    // Town markers are calculated seperately from different markers so they do not take priority over something more important
                    if (selId == idsReqMarkers[0, 0])
                        townPos = new Vector3Int(x, y, 0);
                } 
            }
        }

        // If there is not enough excess power for the town place a power marker at the town position
        if (gPower - gReqPower <= powerConsume)
            onPlaceMarker?.Invoke(townPos, 1); // Place power marker at town position
    }


    // Checks if the id could require markers
    bool IdReqMarker(int id)
    {   
        bool idReqMarker = false;
        for (int i = 0; i < idsReqMarkers.GetLength(0); i++)
        {
            if (id == idsReqMarkers[i, 0])
                idReqMarker = true;
        }
        return idReqMarker;
    }

    // Calculate the type of marker a given id needs based on the stats
    // markerType: -1 = no marker, 0 = coal marker, 1 = power marker
    // Adjusts stats accordingly
    int CalcMarker(int id)
    {   
        int[] globalStats = new int[4] {gCurrency, gCoal, gPower, gEnvironment};
        int[] globalStatsImpact = new int[4] {0, 0, 0, 0};
        int[] idStats = TileStats(id, false);
        int[] consProd = GetConsumptionProduction(id);

        int markerType = -1;

        // Calculate markers needed and stats impact, for coal and power only
        for (int i = 1; i < 3; i++)
        {   
            if (consProd[i] == -1)
            {   
                Debug.Log(id);
                Debug.Log(new Vector3(globalStats[i],  tilesConsumption[i],  idStats[i]));
                // Check if there is not enough of a stat produced globally to sustain the consumption of the given tile id
                if (globalStats[i] + tilesConsumption[i] + idStats[i] < 0)
                {   
                    markerType = i - 1;

                    // Because the consumption of a tile cannot be met, there will be no production
                    int pIndex = GetProductionIndex(consProd);
                    globalStatsImpact[pIndex] -= idStats[pIndex];
                }
                tilesConsumption[i] += idStats[i];
            }
        }

        gCurrency += globalStatsImpact[0];
        gCoal += globalStatsImpact[1];
        gPower += globalStatsImpact[2];
        gEnvironment += globalStatsImpact[3];

        return markerType;
    }


    // Gets the index in a consProd array (from function below) where a tile is producing a resource
    int GetProductionIndex(int[] consProd)
    {
        for (int i = 0; i < consProd.Length; i++)
        {
            if (consProd[i] > 0)
                return i;
        }
        return 0;
    }

    // Find out what type of stats the id consumes and produces
    // -1 is for consumption, 0 is for netural, 1 is for production
    int[] GetConsumptionProduction(int id)
    {
        int[] idStats = TileStats(id, false);
        int[] consProd = new int[4];

        for (int i = 0; i < idStats.Length; i++)
        {   
            // Neutral impact     
            consProd[i] = 0;

            // If stat has a positive impact it is production
            if (idStats[i] > 0)
            {
                consProd[i] = 1;
            }

            // If stat has a negative impact it is consumption
            if (idStats[i] < 0)
            {
                consProd[i] = -1;
            }  
        }
        return consProd;
    }


    // Sets the stats displays
    void SetStatsForBuildTiles(int id, bool includeCurrency)
    {   
        if (id == 0)
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
        gCurrency += currencyMake;
    }


    // Update is called once per frame
    void Update()
    {
        // events like this that only need to be invoked at the start will not work
        // Because the subscriber will not be able to subscribe intime using FindObjectOfType
        onSendBalanceInfo?.Invoke(tileGrowChancePerTurn, seedSpreadChancePerAdjacentTile);
    }
}
