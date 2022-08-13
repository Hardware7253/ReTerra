using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;


// Generates terrain, builds tiles, player interactions with tiles
public class MenuTilesHandler : MonoBehaviour
{

    // The ids of the tiles that are naturally generated
    // In order of height bottom to top
    int[] terrainTiles = new int[]
    {   
        1,  // Water
        2,  // Rocks
        7,  // Plains
        7,  // Plains
        8,  // Park
        9,  // Forest
        24, // Wind Turbine Mk.3
    };

    // Map width and height
    public static readonly int width = 40;
    public static readonly int height = 40;

    private float magnification = 10f; // Perlin noise magnification recommend width * 0.25

    // Array stores index of each tile, effectively the text version of what is shown on screen
    public static int[,] tileGrid = new int[width, height];

    [SerializeField]
    private TileBase[] tiles;

    [SerializeField]
    Tilemap tilemap;

    void Start()
    {   
        // Randomly offsets the perlin noise
        int offSetX = UnityEngine.Random.Range(0, 99999 * width);
        int offSetY = UnityEngine.Random.Range(0, 99999 * height);

        GenerateTileGrid(offSetX, offSetY);
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
                tileGrid[x, y] = index;
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
        return terrainTiles[Map(fIndex, terrainTiles.Length, 1f)]; // Map float value to integers to be used in index array
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
        return maxOutVal - 1; // Return maxOutVal if the for loop fails
    }

    // Places tiles according to tileGrid array
    void CreateTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int id = tileGrid[x, y];

                PlaceTile(new Vector2Int(x, y), id, false);
            }
        }
    }

    // Places tile corresponding to id at pos on the isometric grid
    void PlaceTile(Vector2Int pos, int id, bool updateTileGrid)
    {   
        if (updateTileGrid)
            tileGrid[pos.x, pos.y] = id;

        tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), tiles[id]);
    }
} // Class
