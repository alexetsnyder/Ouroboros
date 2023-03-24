using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType
{
    SNAKE,
    APPLE,
}

[System.Serializable]
public struct SnakeData
{
    public TileType type;
    public Tile tile;
}
