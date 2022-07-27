using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MarkerTilesHandler : MonoBehaviour
{
    [SerializeField]
    TileBase[] markers;

    [SerializeField]
    Tilemap tilemap;


    // Must be the same as TilesHandler width and height
    int lWidth = TilesHandler.width;
    int lHeight = TilesHandler.height;

    // Start is called before the first frame update
    void Start()
    {   
        FindObjectOfType<MainBalancing>().onResetMarker += ResetMarkers;
        FindObjectOfType<MainBalancing>().onPlaceMarker += PlaceMarker;
    }

    // Places a marker at a given position
    // Marker 0 = coalMarker
    // Maerker 1 = powerMarker
    void PlaceMarker(Vector3Int position, int marker)
    {   
        tilemap.SetTile(position, markers[marker]);
    }

    // Removes all the markers on screen
    void ResetMarkers()
    {   
        for (int x = 0; x < lWidth; x++)
        {
            for (int y = 0; y < lHeight; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), null);
            }
        }
    }
}
