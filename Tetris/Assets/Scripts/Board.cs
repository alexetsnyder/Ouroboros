using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize;

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(this.activePiece, this.spawnPosition))
        {
            Set(this.activePiece);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        this.tilemap.ClearAllTiles();
    }

    public void Set(Piece piece)
    {
        foreach (Vector3Int cellPos in piece.cells)
        {
            this.tilemap.SetTile(cellPos + piece.position, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        foreach (Vector3Int cellPos in piece.cells)
        {
            this.tilemap.SetTile(cellPos + piece.position, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        foreach (Vector3Int cellPos in piece.cells)
        {
            Vector3Int tilePosition = cellPos + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bound = this.Bounds;
        int row = Bounds.yMin;

        while (row < Bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for (int i = bounds.xMin; i < bounds.xMax; i++)
        {
            Vector3Int position = new Vector3Int(i, row, 0);

            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        for (int i = bounds.xMin; i < bounds.xMax; i++)
        {
            Vector3Int position = new Vector3Int(i, row, 0);
            this.tilemap.SetTile(position, null);
        }

        while (row < bounds.yMax)
        {
            for (int i = bounds.xMin; i < bounds.xMax; i++)
            {
                TileBase above = this.tilemap.GetTile(new Vector3Int(i, row + 1, 0));
                Vector3Int position = new Vector3Int(i, row, 0);

                this.tilemap.SetTile(position, above);
            }
            row++;
        }
    }
}
