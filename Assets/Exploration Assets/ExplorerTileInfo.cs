using UnityEngine;
using UnityEngine.Tilemaps;

class ExplorerTileInfo
{
    public TileBase clickedTile;
    public Vector3Int cellPosition;

    public bool IsHome => CompareTileName("home");
    public bool IsForest => CompareTileName("arrowUp");
    public bool IsHero => CompareTileName("abuela") || CompareTileName("lance") || CompareTileName("stela");

    private bool CompareTileName(string name)
    {
        Tile tile = clickedTile as Tile;
        if (tile == null) return false;
        return tile.name.StartsWith( name);
    }
}
