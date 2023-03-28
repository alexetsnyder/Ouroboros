using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ViewerController : MonoBehaviour
{
    public int regions;
    public Vector2Int imageSize;

    private VoronoiDiagram voronoiDiagram;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        voronoiDiagram = new VoronoiDiagram(regions, imageSize);
        GenerateDiagram();
        DrawDiagramWithColors();
    }

    public void GenerateDiagram()
    {
        voronoiDiagram.GeneratePoints();
    }

    public void DrawPoints()
    {
        Color[] pixels = CreatePointPixels();

        spriteRenderer.sprite = Sprite.Create(CreateTexture(pixels), new Rect(0.0f, 0.0f, imageSize.x, imageSize.y), Vector2.one * 0.5f);
    }

    private Color[] CreatePointPixels()
    {
        Color[] pixels = new Color[imageSize.x * imageSize.y];

        foreach (Vector2Int point in voronoiDiagram.GetVPoints())
        {
            int index = point.x * imageSize.x + point.y;
            pixels[index] = Color.black;
        }

        return pixels;
    }

    public void DrawDiagramWithColors()
    {
        Color[] pixels = CreateDiagramPixels();

        spriteRenderer.sprite = Sprite.Create(CreateTexture(pixels), new Rect(0.0f, 0.0f, imageSize.x, imageSize.y), Vector2.one * 0.5f);
    }

    private Color[] CreateDiagramPixels()
    {
        Color[] pixels = new Color[imageSize.x * imageSize.y];

        for (int x = 0; x < imageSize.x; x++)
        {
            for (int y = 0; y < imageSize.y; y++)
            {
                int index = x * imageSize.x + y;
                pixels[index] = voronoiDiagram.GetColor(new Vector2Int(x, y));
            }
        }

        return pixels;
    }

    private Texture2D CreateTexture(Color[] pixels)
    {
        Texture2D texture = new Texture2D(imageSize.x, imageSize.y);

        texture.filterMode = FilterMode.Point;
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }

}
