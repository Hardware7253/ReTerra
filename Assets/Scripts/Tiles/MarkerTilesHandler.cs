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

    Vector3Int[] markerPositions;
    int markerPosIndex = 0;


    // Must be the same as TilesHandler width and height
    int width = 7;
    int height = 7;

    // Start is called before the first frame update
    void Start()
    {   
        FindObjectOfType<MainBalancing>().onResetMarker += ResetMarkers;
        FindObjectOfType<TilesHandler>().onPlaceMarker += PlaceMarker;
        
        markerPositions = new Vector3Int[width * height];
    }

    // Places a marker at a given position
    // Marker 0 = coalMarker
    // Maerker 1 = powerMarker
    void PlaceMarker(Vector3Int position, int marker)
    {   
        markerPositions[markerPosIndex] = position;
        tilemap.SetTile(position, markers[marker]);

        markerPosIndex++;
    }

    // Removes all the markers on screen
    void ResetMarkers()
    {   
        int length = markerPosIndex;
        for (int i = 0; i < length; i++)
        {
            tilemap.SetTile(markerPositions[i], null);

            markerPosIndex--;
        }
    }
}
