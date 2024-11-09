using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExplorerGridClicker : MonoBehaviour
{
    private Grid grid;
    private Tilemap tilemap;
    private ExplorerTileInfo tileInfoHold;

    public TileBase[] heroTiles;


    void Start()
    {
        grid = GetComponent<Grid>();
        tilemap = GetComponentInChildren<Tilemap>();
    }

    void Update()
    {
        bool isMouseLeftDown = Input.GetMouseButtonDown(0);
        bool isMouseLeftUp = Input.GetMouseButtonUp(0);
        //bool isMouseRight = Input.GetMouseButtonDown(1);
        if (isMouseLeftDown) // Detect left mouse click
        {
            ExplorerTileInfo info = GetTileInfoAtMouse();

            if (info.clickedTile != null)
            {
                Debug.Log($"Clicked on cell: {info.cellPosition} with tile: {info.clickedTile.name}");
                tileInfoHold = info;
            }
            else
            {
                Debug.Log($"Clicked on empty cell: {info.cellPosition}");
                tilemap.SetTile(info.cellPosition, heroTiles[0]);
            }

        }
        if (isMouseLeftUp)
        {
            if (tileInfoHold?.clickedTile != null)
            {
                ExplorerTileInfo info = GetTileInfoAtMouse();
                if (info.clickedTile != null)
                { 
                    Debug.Log($"Dragged from {tileInfoHold.clickedTile.name} to {info.clickedTile.name}");

                }
            }
            tileInfoHold = null;
        }
    }

    private ExplorerTileInfo GetTileInfoAtMouse()
    {
        Vector3 screenPosition = Input.mousePosition;
        return GetTileInfoAtScreenPosition(screenPosition);
    }

    private ExplorerTileInfo GetTileInfoAtScreenPosition(Vector3 screenPosition)
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        mouseWorldPosition.z = 0;

        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPosition);

        // Optionally, check if a tile exists
        TileBase clickedTile = tilemap.GetTile(cellPosition);

        ExplorerTileInfo info = new ExplorerTileInfo();
        info.cellPosition = cellPosition;
        info.clickedTile = clickedTile;
        return info;
    }
}
