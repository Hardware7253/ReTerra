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
        "Orchard",            // id 10
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
        "Nuclear Plant",      // id 32
        "33",                 // id 33
        "34",                 // id 34
        "35",                 // id 35
        "36",                 // id 36
        "37",                 // id 37
        "38",                 // id 38
        "39",                 // id 39
        "40"                  // id 40
    };

    // In depth info for each tile
    string[] tileInfo = new string[]
    {
        "People live in the town, make them happy and the population will grow faster. The more people there are the more currency you'll earn per year",           // id 0
        "Makes people happy, and has great potential for power generation",                                                                                         // id 1
        "Full of rare resources below ground, and high winds above ground due to high elevation",                                                                   // id 2
        "A desolate waste land, makes people unhappy",                                                                                                              // id 3
        "Can grow into grass and have a high chance to spread to adjacent tiles",                                                                                   // id 4
        "Can grow into grass and have a high chance to spread to adjacent tiles",                                                                                   // id 5
        "Can grow into grass and have a high chance to spread to adjacent tiles",                                                                                   // id 6
        "Lush green fields. Makes people happy, and has great soil",                                                                                                // id 7
        "A small ammount of bushes and trees for people to enjoy, makes people happy",                                                                              // id 8
        "A great forest with many tall trees, makes people happy",                                                                                                  // id 9
        "Full of plants and fruit trees, provides great improvements to hapiness",                                                                                  // id 10
        "11",                                                                                                                                                       // id 11
        "12",                                                                                                                                                       // id 12
        "13",                                                                                                                                                       // id 13
        "14",                                                                                                                                                       // id 14
        "15",                                                                                                                                                       // id 15
        "16",                                                                                                                                                       // id 16
        "17",                                                                                                                                                       // id 17
        "Harvesting the power of the currents, makes people slightly unhappy",                                                                                      // id 18
        "Harvesting the power of the currents, makes people slightly unhappy",                                                                                      // id 19
        "Harvesting the power of the currents, makes people slightly unhappy",                                                                                      // id 20
        "21",                                                                                                                                                       // id 21
        "Harvesting the power of the winds, makes people slightly unhappy",                                                                                         // id 22
        "Harvesting the power of the winds, makes people slightly unhappy",                                                                                         // id 23
        "Harvesting the power of the winds, makes people slightly unhappy",                                                                                         // id 24
        "25",                                                                                                                                                       // id 25
        "Harvesting resources from below, vibrations and noise make people unhappy",                                                                                // id 26
        "Harvesting resources from deep below, vibrations and noise make people very unhappy",                                                                      // id 27
        "28",                                                                                                                                                       // id 28
        "Burning coal to run the turbines, releases fumes which make people unhappy",                                                                               // id 29
        "Burning coal to run the turbines, releases fumes which make people unhappy",                                                                               // id 30
        "31",                                                                                                                                                       // id 31
        "Uranium-235 undergoing fission to run the turbines, great for power and environment if managed correctly. Threat of catastrophe makes people unhappy",     // id 32
        "33",                                                                                                                                                       // id 33
        "34",                                                                                                                                                       // id 34
        "35",                                                                                                                                                       // id 35
        "36",                                                                                                                                                       // id 36
        "37",                                                                                                                                                       // id 37
        "38",                                                                                                                                                       // id 38
        "39",                                                                                                                                                       // id 39
        "40"                                                                                                                                                        // id 40
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
        {29 , 4, 32, -1, -1, -1}, // Barren Plains (id3)
        {-1, -1, -1, -1, -1, -1}, // Seeds (id4)
        {-1, -1, -1, -1, -1, -1}, // Seeds (Growing1) (id5)
        {-1, -1, -1, -1, -1, -1}, // Seeds (Growing2) (id6)
        {8 , 10, -1, -1, -1, -1}, // Plains (id7)
        {9 , -1, -1, -1, -1, -1}, // Park (id8)
        {-1, -1, -1, -1, -1, -1}, // Forest (id9)
        {-1, -1, -1, -1, -1, -1}, // Orchard (id10)
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
        {-1, -1, -1, -1, -1, -1}, // Nuclear Plant (id32)
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
    //   $,    c,   p,   e    
        {0,    0,   0,   0},    // Town
        {0,    0,   0,   1},    // Water
        {0,    0,   0,   0},    // Rocks
        {0,    0,   0,  -1},    // Barren Plains
        {-5,   0,   0,   0},    // Seeds
        {0,    0,   0,   0},    // Seeds (Growing1)
        {0,    0,   0,   0},    // Seeds (Growing2)
        {0,    0,   0,   1},    // Plains
        {-10,  0,   0,   4},    // Park
        {-13,  0,   0,  12},    // Forest
        {-30,  0,   0,  9 },    // Orchard
        {0,    0,   0,   0},    // 11
        {0,    0,   0,   0},    // 12
        {0,    0,   0,   0},    // 13
        {0,    0,   0,   0},    // 14
        {0,    0,   0,   0},    // 15
        {0,    0,   0,   0},    // 16
        {0,    0,   0,   0},    // 17
        {-12,  0,   3,  -1},    // Hydro Turbine Mk.1
        {-10,  0,   8,  -1},    // Hydro Turbine Mk.2
        {-17,  0,  18,  -1},    // Hydro Turbine Mk.3
        {0,    0,   0,   0},    // 21
        {-10,  0,   2,  -1},    // Wind Turbine Mk.1
        {-12,  0,   8,  -1},    // Wind Turbine Mk.2
        {-15,  0,  14,  -1},    // Wind Turbine Mk.3
        {0,    0,   0,   0},    // 25
        {-10,  4,  -4,  -4},    // Mine Mk.1
        {-15, 12, -14,  -10},    // Mine Mk.2
        {0,    0,   0,   0},    // 28
        {-20, -4,  20,  -8},    // Coal Plant Mk.1
        {-30, -6,  45, -18},    // Coal Plant Mk.2
        {0,    0,   0,   0},    // 31
        {-85,  0,  50,   0},    // Nuclear plant
        {0,    0,   0,   0},    // 33
        {0,    0,   0,   0},    // 34
        {0,    0,   0,   0},    // 35
        {0,    0,   0,   0},    // 36
        {0,    0,   0,   0},    // 37
        {0,    0,   0,   0},    // 38
        {0,    0,   0,   0},    // 39
        {0,    0,   0,   0}     // 40
    };

    // Effect each tile has on town hapiness
    int[] tileHapiness = new int[]
    {
        0,       // Town
        1,       // Water
        0,       // Rocks
        -1,      // Barren Plains
        0,       // Seeds
        0,       // Seeds (Growing1)
        0,       // Seeds (Growing2)
        2,       // Plains
        4,       // Park
        6,       // Forest
        12,      // Orchard
        0,       // 11
        0,       // 12
        0,       // 13
        0,       // 14
        0,       // 15
        0,       // 16
        0,       // 17
        -1,      // Hydro Turbine Mk.1
        -1,      // Hydro Turbine Mk.2
        -1,      // Hydro Turbine Mk.3
        0,       // 21
        -1,      // Wind Turbine Mk.1
        -1,      // Wind Turbine Mk.2
        -1,      // Wind Turbine Mk.3
        0,       // 25
        -4,      // Mine Mk.1
        -8,      // Mine Mk.2
        0,       // 28
        -4,      // Coal Plant Mk.1
        -8,     // Coal Plant Mk.2
        0,       // 31
        -4,      // Nuclear plant
        0,       // 33
        0,       // 34
        0,       // 35
        0,       // 36
        0,       // 37
        0,       // 38
        0,       // 39
        0        // 40
    };


    // Array of tiles that could potentially require markers
    int[] idsReqMarkers = new int[]
    {
        0,  // Town
        26, // Mine Mk.1
        27, // Mine Ml.2
        29, // Coal Plant Mk.1
        30, // Coal Plant Mk.2
    };

    // Each element in the array shows the consumption of all tiles on a stat
    // E.g. If element 1 was -18 there would be 18 units of coal consumed across all tiles
    //                                  $, c, p, e
    int[] tilesConsumption = new int[] {0, 0, 0, 0};


    public static int turn;
    public static int uiScale = 5;

    float powerPerPerson = 0.02f; // Power consumed per person per turn
    float currencyPerPerson = 0.001f; // Currency made per person per turn

    public static int seedGrowChancePerTurn = 80; // Percentage chance of a seed growing per turn
    public static int seedSpreadChancePerAdjacentTile = 80; // Percentage chance of a seed spreading to an adjacent tile per turn

    public static int[] buttonIds = new int[] {-1, -1, -1, -1, -1, -1}; // Array of tile ids corresponding to each building menu botton

    int sButton;
    int pButton;

    bool sHover;
    bool pHover;


    // Global stats
    int gCurrency = 12;
    int gCoal = 0, gReqCoal = 0;
    int gPower = 0, gReqPower = 0;
    int gEnvironment = 0;
    int gPopulation = 0;
    float gHapiness = 0f;

    // Amount the respective global stat is multiplied by to find the change that should be applied to the hapiness
    int minHapiness = 0;
    int maxHapiness = 0;

    int startingPopulation = 20; // The lowest possible population

    // Value that the hapiness has to be less than inorder for the population to decrease
    float populationHapinessDecreaseThreshold = 0.42f;

    // Town stats
    int currencyMake = 0;
    int powerConsume = 0;

    public static int score = 0;    

    public event Action<int, string, bool, int> onSetBuildText; // Event responsible for updating text, and image visibility of build menu buttons
    public event Action<bool, bool> onSetBuildOpen; // Opens / closes the build menu, and enables / disables scroll rect
    public event Action<int, int, int, bool, Color> onSetStatText; // Event responsible for setting text of stats displays
    public event Action<int> onBuildButtonPressed; // Event gives TilesHandler tile to build on the isometric map
    public event Action resetClickedTile; // Does the equivelent of re clicking a tile after the buidling menu has been hovered. Otherwise the incorrect stats show in the stats display
    public event Action onResetMarker; // Remove all markers on screen
    public event Action<Vector3Int, int> onPlaceMarker; // PLace marker at given position
    public event Action<bool> onSetCFText; // Sets the cant afford text on/off
    public event Action<float> onSendHapiness; // Gives the hapiness value to other scripts. This is used to seth the sprite for the population display
    public event Action<string, string> onSetinfo; // Set info for info screen

    Color defaultColor = Colours.colorsArray[18]; // Default color for stats displays text (gray)
    Color positiveColor = Colours.colorsArray[19]; // Positive color for stats display text, used when showing a preview for what effects a tile has (green)
    Color negativeColor = Colours.colorsArray[20]; // Negative color for stats display text, used when showing a preview for what effects a tile has (red)


    void Awake()
    {
        turn = 0;
        score = 0;
        ResetMasterStats();
    }
    
    void Start()
    {   
        FindObjectOfType<TilesHandler>().onTileSelect += UpdateButtons;
        FindObjectOfType<TilesHandler>().onTileSend += TileStatsImpact;
        FindObjectOfType<TilesHandler>().onResetMStats += ResetMasterStats;
        FindObjectOfType<ButtonDetector>().hoveredButtons += SetHoveredBMButton;

        gPopulation = startingPopulation;
        SelStats(-1, true);
        FindMaxMinHapiness();

        MapIntFloat(0, -10, 10, 0f, 1f);

        // Hallo, pls redo hapiness based entirely of of the tile hapiness. Then  just multiply that by some ammount of power to get final hapiness
    }

    void FindMaxMinHapiness()
    {
        for (int i = 0; i < tileHapiness.Length; i++)
        {
            if (tileHapiness[i] > maxHapiness)
                maxHapiness = tileHapiness[i];

            if (tileHapiness[i] < minHapiness)
                minHapiness = tileHapiness[i];
        }
        maxHapiness *= TilesHandler.width * TilesHandler.height;
        minHapiness *= TilesHandler.width * TilesHandler.height;
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
        // gPopulation = 0; Dont reset population, as population is added onto every turn, rather then calculated from scratch
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

            // Update the hovered building menu button, because the tile the button represents can change after a tile has been built
            // Even though the button being hovered is the same, the text is different, that's why it has to be updated
            SetHoveredBMButton(button, true, true);
            return;
        }
        
        // If the tile cannot be built play the error sound
        FindObjectOfType<AudioManager>().PlaySound("Error");
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
            CalcPeopleUsedPower();
            CalcHapiness();
            onSendHapiness?.Invoke(gHapiness);


            CalcPopulation();
            CalcCurrencyMake();
            AddScore();

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
                    
                    if (selId != idsReqMarkers[0])
                    {   
                        int idMarkerType = CalcMarker(selId); // Calculate what type of marker the selected id needs
                        if (idMarkerType == 0 || idMarkerType == 1) // Marker types that aren't 0 or 1 are invalid
                            onPlaceMarker?.Invoke(new Vector3Int(x, y, 0), idMarkerType); // Place idMarkerType at x, y position on tilemap
                    }
                    
                    // Town markers are calculated seperately from different markers so they do not take priority over something more important
                    if (selId == idsReqMarkers[0])
                        townPos = new Vector3Int(x, y, 0);
                } 
            }
        }

        // If there is not enough excess power for the town place a power marker at the town position
        if (gPower - gReqPower < powerConsume)
            onPlaceMarker?.Invoke(townPos, 1); // Place power marker at town position
    }


    // Checks if the id could require markers
    bool IdReqMarker(int id)
    {   
        bool idReqMarker = false;
        for (int i = 0; i < idsReqMarkers.Length; i++)
        {
            if (id == idsReqMarkers[i])
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

            onSetStatText?.Invoke(i, stat, 0, false, color);
        }
        
        onSetinfo?.Invoke(tileNames[id], tileInfo[id]);
    }

    // Calculate town hapiness
    void CalcHapiness()
    {
        int hapiness = 0;

        // Go through tile grid
        // Add tile hapiness effects to hapiness
        for (int x = 0; x < TilesHandler.width; x++)
        {
            for (int y = 0; y < TilesHandler.height; y++)
            {
                hapiness += tileHapiness[TilesHandler.tileGrid[x, y]];
            }
        }

        gHapiness = MapIntFloat(hapiness, minHapiness, maxHapiness, 0f, 1f);

        // Add environment and power affects to hapiness
        int environment;
        int power;
        int excessPower = gPower - gReqPower - powerConsume;

        // Eliminate 0 and negative numers
        if (gEnvironment > 0)
            environment = gEnvironment;
        else
            environment = 1;

        if (excessPower > 0)
            power = excessPower;
        else
            power = 1;

        // Calculate hapiness mod
        float hapinessMod = Math.Abs((environment * power)) / 1000f;

        // Make hapiness mod into a suitable number for percentage increases / decreases
        if (gEnvironment < 0 && excessPower < 0)
            hapinessMod = 1 - hapinessMod;
        else
            hapinessMod += 1;

        gHapiness *= hapinessMod;
    }

    // Maps an integer number to a float value
    float MapIntFloat(int num, int minInVal, int maxInVal, float minOutVal, float maxOutVal)
    {
        num += Math.Abs(minInVal);

        int dInValues = maxInVal - minInVal;
        float dOutValues = maxOutVal - minOutVal;

        float floatValPerInt = dOutValues / dInValues;
        return num * floatValPerInt;
    }
    // Calculate town population
    void CalcPopulation()
    {   
        // If people are unhappy reduce the population
        if (gHapiness < populationHapinessDecreaseThreshold)
        {
            gPopulation -= (int)Mathf.Ceil(UnityEngine.Random.Range(gHapiness * turn / 8, gHapiness * turn));
        }

        // If people are happy increase the population
        else
        {
            gPopulation += (int)Mathf.Ceil(UnityEngine.Random.Range(gHapiness * turn / 8, gHapiness * turn));
        }
        
        if (gPopulation < startingPopulation)
            gPopulation = startingPopulation;
    }

    // Calculate the ammount of power that is needed for the current population
    void CalcPeopleUsedPower()
    {
        powerConsume = (int)Mathf.Ceil(gPopulation * powerPerPerson); // Multiply population by power used per person, round up to the nearest int
    }


    void CalcCurrencyMake()
    {
        currencyMake = (int)Mathf.Ceil(gPopulation * currencyPerPerson);
        if (currencyMake <= 0)
            currencyMake = 1;
        
        gCurrency += currencyMake;
    }

    // Add to the score
    void AddScore()
    {
        score += ((gPopulation * gEnvironment) + (gPopulation * (gPower - gReqPower - powerConsume))) / 128;
    }
}
