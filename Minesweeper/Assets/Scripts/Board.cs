using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public int rows;
    public int cols;
    public int mineCount;

    private Tilemap tileMap;

    private MineTile[,] mineTiles;

    private Dictionary<string, Tile> resources;

    public void Awake()
    {
        tileMap = GetComponentInChildren<Tilemap>();
        
        resources = new Dictionary<string, Tile>();
        LoadResources();
    }

    public void Start()
    {
        NewGame();
    }

    private void LoadResources()
    {
        string namePrefix = "Tile";

        Load(namePrefix + "Empty");
        Load(namePrefix + "Unknown");
        Load(namePrefix + "Mine");
        Load(namePrefix + "Exploded");
        Load(namePrefix + "Flag");
        Load(namePrefix + "1");
        Load(namePrefix + "2");
        Load(namePrefix + "3");
        Load(namePrefix + "4");
        Load(namePrefix + "5");
        Load(namePrefix + "6");
        Load(namePrefix + "7");
        Load(namePrefix + "8");
    }

    private void Load(string name)
    {
        string filePathPrefix = "TileSets/";

        resources.Add(name, (Tile)Resources.Load(filePathPrefix + name));
    }

    public void NewGame()
    {
        mineTiles = new MineTile[rows, cols];

        GenerateTiles();
        GenerateMines();
        GenerateNumbers();

        Draw();
    }

    public void GenerateTiles()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Vector2Int tilePos = new Vector2Int(row - rows / 2, col - cols / 2);
                MineTile tile = new MineTile(MineType.EMPTY, tilePos);
                mineTiles[row, col] = tile;    
            }
        }
    }

    public void GenerateMinesOld()
    {
        for (int i = 0; i < mineCount; i++)
        {
            int row = Random.Range(0, rows);
            int col = Random.Range(0, cols);

            var tile = mineTiles[row, col];

            while (tile.type == MineType.MINE)
            {
                row++;

                if (row >= rows)
                {
                    row = 0;
                    col++;

                    if (col >= cols)
                    {
                        col = 0;
                    }
                }

                tile = mineTiles[row, col];
            }

            mineTiles[row, col].type = MineType.MINE;
        }
    }

    public void GenerateMines()
    {
        List<Vector2Int> emptyTiles = new List<Vector2Int>();

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                emptyTiles.Add(new Vector2Int(row, col));
            }
        }

        for (int i = 0; i < mineCount; i++)
        {
            var tile = emptyTiles[Random.Range(0, emptyTiles.Count)];
            emptyTiles.Remove(tile);

            mineTiles[tile.x, tile.y].type = MineType.MINE;
        }
    }

    public void GenerateNumbers()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                var tile = mineTiles[row, col];
                if (tile.type != MineType.MINE)
                {
                    tile.number = CountMines(new Vector2Int(row, col));

                    if (tile.number > 0)
                    {
                        tile.type = MineType.NUMBER;
                    }

                    mineTiles[row, col] = tile;
                }
            }
        }
    }

    private int CountMines(Vector2Int tilePos)
    {
        int mines = 0;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                var newPos = tilePos + new Vector2Int(i, j);
                if (IsValid(newPos) && mineTiles[newPos.x, newPos.y].type == MineType.MINE)
                {
                    mines++;
                }
            }
        }

        return mines;
    }

    private bool IsValid(Vector2Int position)
    {
        return (position.x >= 0 && position.x < rows && position.y >= 0 && position.y < cols);
    }

    public void Draw()
    {
        for (int row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            { 
                var tile = mineTiles[row, col];
                tileMap.SetTile((Vector3Int)tile.position, GetTile(tile));
            }
        }
    }

    private Tile GetTile(MineTile tile)
    {
        if (tile.isRevealed)
        {
            return GetRevealedTile(tile);
        }
        else if (tile.isFlagged)
        {
            return resources["TileFlag"];
        }
        else
        {
            return resources["TileUnknown"];
        }      
    }

    private Tile GetRevealedTile(MineTile tile)
    {
        switch (tile.type)
        {

            case MineType.EMPTY:
                return resources["TileEmpty"];
            case MineType.NUMBER:
                return resources["Tile" + tile.number.ToString()];
            case MineType.MINE:
                return (tile.hasExploded) ? resources["TileExploded"] : resources["TileMine"];
            default:
                return null;
        }
    }
}
