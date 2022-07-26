﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;


// Generates terrain, builds tiles, player interactions with tiles
public class TilesHandlerBackup : MonoBehaviour
{
    // Map width and height
    private static readonly int width = 7;
    private static readonly int height = 7;

    private float magnification = 1.8f; // Perlin noise magnification reccomend 1.8

    // Array stores index of each tile, effectively the text version of what is shown on screen
    public int[,] tileGrid = new int[height, width];

    [SerializeField]
    private TileBase[] tiles;
    int GeneratedTilesAmmount = 3; // How many items in the tiles array will be used as natural generation tiles

    [SerializeField]
    Tilemap tilemap;

    public event Action<int> OnTilePressed;

    Color defaultTileColour = new Color(1, 1, 1, 1);
    Color hoverTileColour = new Color(0.95f, 0.95f, 0.95f, 1);
    Color clickTileColour = new Color(0.8f, 0.95f, 0.8f, 1);

    Vector3Int clickedTile = new Vector3Int(width, height, 0);

    bool gameUIHovered;


    void Start()
    {
        FindObjectOfType<CheckUIHovers>().gameUIHovered += SetGameUIHover;
        FindObjectOfType<BMController>().onBuildButtonPressed += AddTileToBuild;
        FindObjectOfType<MainBalancing>().onNewTurn += NewTurn;

        // Randomly offsets the perlin noise
        int offSetX = UnityEngine.Random.Range(0, 99999);
        int offSetY = UnityEngine.Random.Range(0, 99999);

        GenerateTileGrid(offSetX, offSetY);
        CreateTiles();

    }

    void Update()
    {
        TileMouseInteractions();
    }

    void SetGameUIHover(bool isHovered)
    {
        gameUIHovered = isHovered;
    }



    // Handles interactions between the user and the tiles
    // Updates tile color when it is hovered or clicked on
    // Stores postions of selected tiles and last clicked tiles
    void TileMouseInteractions()
    {
        Vector3Int sTilePos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        sTilePos.z = 0;

        bool tilesHovered = sTilePos.x >= 0 && sTilePos.x < width && sTilePos.y >= 0 && sTilePos.y < height;

        // Un clicks a tile if escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
            clickedTile = new Vector3Int(width, height, 0);

        // Resets all tile colours and checks for clicked tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // If the current tile is clicked set its color to the click color
                if (new Vector3Int(x, y, 0) == clickedTile)
                    tilemap.SetColor(clickedTile, clickTileColour);


                // If the current tile is not clicked set its color to the default color
                if (new Vector3Int(x, y, 0) != clickedTile)
                    tilemap.SetColor(new Vector3Int(x, y, 0), defaultTileColour);
            }
        }

        // Changes tile hover color and detects when a tile is clicked
        if (tilesHovered && sTilePos != clickedTile)
        {
            if (!gameUIHovered)
            {
                // Changes hovered tile color
                tilemap.SetColor(sTilePos, hoverTileColour);

                if (Input.GetMouseButtonDown(0))
                {
                    clickedTile = sTilePos;
                    OnTilePressed?.Invoke(tileGrid[sTilePos.y, sTilePos.x]); // Gets index of the selected tile and sends it to other scripts when the left mouse button is pressed
                }

            }


        }

    }


    // Adds tile to build to the tileGrid array
    void AddTileToBuild(int id)
    {
        tileGrid[clickedTile.y, clickedTile.x] = id;
        CreateTiles();
    }

    // Makes the array that stores the index of each tile
    void GenerateTileGrid(int offSetX, int offSetY)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Add index to each x and y position of tileGrid
                int index = GetIndex(x, y, offSetX, offSetY);
                tileGrid[y, x] = index;
            }
        }
    }

    // Gets index of given x and y coordinates
    int GetIndex(int x, int y, int offSetX, int offSetY)
    {
        // Perlin noise only uses float values between 0.0 and 1.0 hence the x / width and y / height
        float xCoord = (float)x / width * magnification + offSetX;
        float yCoord = (float)y / height * magnification + offSetY;

        float fIndex = Mathf.PerlinNoise(xCoord, yCoord); // Gets value of specific perlin coordinate
        return Map(fIndex, GeneratedTilesAmmount, 1f); // Map float value to integers to be used in index array
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

    // Places tiles according to tileGrid array
    void CreateTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = tileGrid[y, x];

                tilemap.SetTile(new Vector3Int(x, y, 0), tiles[index]);
            }
        }
    }

    public void NewTurn()
    {
        UpdateSeeds();
    }

    // Update seeds
    // Grow seeds and spread seeds
    void UpdateSeeds()
    {
        // Check all positions in tileGrid for seeds
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int tileId = tileGrid[y, x];

                if (tileId == 9 || tileId == 10 || tileId == 11) // Check for seed
                {
                    GrowSeed(tileId, x, y);

                    if (tileId != 11)
                        SpreadSeed(tileId, x, y);
                }
            }
        }
    }

    // Grows seeds
    void GrowSeed(int id, int x, int y)
    {
        if (UnityEngine.Random.Range(1, 3) == 1) // 50% chance to grow seed each time the function is called
        {
            if (tileGrid[y, x] == 11) // Grow final seed growth phase to grass
                tileGrid[y, x] = 3;

            if (tileGrid[y, x] < 11 && tileGrid[y, x] >= 9) // Grow seed to next growth phase
                tileGrid[y, x] = id + 1;
        }

        CreateTiles();
    }

    // Spreads seeds
    void SpreadSeed(int id, int x, int y)
    {
        Vector2Int[] adjacentTiles = new Vector2Int[4];

        // Gets adjacent tiles to the input tile
        adjacentTiles[0] = new Vector2Int(x--, y); // Left
        adjacentTiles[1] = new Vector2Int(x++, y); // Right
        adjacentTiles[2] = new Vector2Int(x, y++); // Up
        adjacentTiles[3] = new Vector2Int(x, y--); // Down

        for (int i = 0; i < 4; i++)
        {
            Vector2Int sTile = adjacentTiles[i];

            if ((sTile.x < width && sTile.x >= 0 && sTile.y < height && sTile.y >= 0)) // Only spread to adjacent tile if it is inside the tileGrid
            {
                if ((UnityEngine.Random.Range(1, 3) == 1) && tileGrid[sTile.y, sTile.x] == 1) // 50% chance of adjacent tile being spread to barrenPlains
                {
                    tileGrid[sTile.y, sTile.x] = 9;
                }
            }
        }
        CreateTiles();
    }

    // tommorow switch x and y for all mentions of tilegrid, make a event in mainbalancing for seed growth and spread chance

} // Class
