using UnityEditor.SceneManagement;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Vector2Int gridSize;
    public float tileSize;

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-gridSize.x / 2, -gridSize.y / 2);
            return new RectInt(position, gridSize);
        }
    }

    private void Start()
    {
        //CreateGrid();
    }

    private void CreateGrid()
    {
        RectInt bounds = Bounds;
        GameObject reference = (GameObject)Instantiate(Resources.Load("WarTile"));

        for (int row = bounds.xMin; row < bounds.xMax; row++)
        {
            for (int col = bounds.yMin; col < bounds.yMax; col++)
            {
                GameObject tile = Instantiate(reference, transform);
                tile.transform.position = new Vector2(row * tileSize, col * tileSize);
            }
        }

        Destroy(reference);
    }
}
