using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Vector2Int boardSize;
    public TMP_Text scoreText;
    public TMP_Text gameProgressText;

    private Tilemap TileMap { get; set; }

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, boardSize); 
        }
    }

    private void Awake()
    {
        TileMap = GetComponentInChildren<Tilemap>();
    }

    private void Start()
    {

    }

    public bool IsValidPosition(Vector2Int position)
    {
        RectInt bounds = Bounds;

        if (bounds.Contains(position) &&
            !TileMap.HasTile((Vector3Int)position))
        {
            return true;
        }

        return false;
    }

}
