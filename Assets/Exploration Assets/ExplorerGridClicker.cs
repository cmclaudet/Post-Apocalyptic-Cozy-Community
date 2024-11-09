using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExplorerGridClicker : MonoBehaviour
{
    private Grid grid;
    private Tilemap tilemap;

    void Start()
    {
        grid = GetComponent<Grid>();
        tilemap = GetComponentInChildren<Tilemap>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;

            Vector3Int cellPosition = grid.WorldToCell(mouseWorldPosition);

            // Optionally, check if a tile exists
            TileBase clickedTile = tilemap.GetTile(cellPosition);
            if (clickedTile != null)
            {
                Debug.Log($"Clicked on cell: {cellPosition} with tile: {clickedTile.name}");
            }
            else
            {
                Debug.Log($"Clicked on empty cell: {cellPosition}");
            }
        }
    }
}
