using UnityEngine;

public enum MineType
{
    NONE,
    EMPTY,
    NUMBER,
    MINE,
}

public struct MineTile 
{
    public MineType type;
    public Vector2Int position;
    public bool isRevealed;
    public bool isFlagged;
    public bool hasExploded;
    public int number;

    public MineTile(MineType type, Vector2Int position)
    {
        this.type = type;
        this.position = position;
        isRevealed = false;
        isFlagged = false;
        hasExploded = false;
        number = 0;
    }
}
