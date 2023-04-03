using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Apple : MonoBehaviour
{
    public Tile appleTile;

    private Vector2Int appleSpawnPos;
    private Board board;
    private Tilemap tileMap;

    private void Awake()
    {
        board = GetComponent<Board>();
        tileMap = GetComponentInChildren<Tilemap>();
    }

    public bool IsApple(Vector2Int position)
    {
        return (position == appleSpawnPos);
    }

    public void SetAppleSpawn()
    {
        GetAppleSpawn();
        SpawnApple();
    }

    private void GetAppleSpawn()
    {
        RectInt bounds = board.Bounds;
        List<Vector2Int> emptyTileList = new List<Vector2Int>();

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector2Int tilePos = new Vector2Int(x, y);
                if (board.IsValidPosition(tilePos))
                {
                    emptyTileList.Add(tilePos);
                }
            }
        }

        if (emptyTileList.Count > 0)
        {
            appleSpawnPos = RandomChoice(emptyTileList);
        }
        else
        {
            board.GameWin();
        }
    }

    private void SpawnApple()
    {
        tileMap.SetTile((Vector3Int)appleSpawnPos, appleTile);
    }

    private Vector2Int RandomChoice(List<Vector2Int> vectorList)
    {
        return vectorList[Random.Range(0, vectorList.Count)];
    }
}
