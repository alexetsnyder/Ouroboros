using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Vector2Int boardSize;
    public SnakeData[] snakeData;

    private RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, boardSize); 
        }
    }

    private Tilemap TileMap { get; set; }

    private void Awake()
    {
        TileMap = GetComponentInChildren<Tilemap>();
    }

    private void Start()
    {
        RectInt bounds = Bounds;

        for (int i = bounds.xMin; i < bounds.xMax; i++)
        {
            for (int j = bounds.yMin; j < bounds.yMax; j++)
            {
                Vector3Int position = new Vector3Int(i, j, 0);
                TileMap.SetTile(position, snakeData[0].tile);
            }
        }
    }

}
