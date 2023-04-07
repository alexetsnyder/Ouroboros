using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public ViewerController viewerController;

    public void ResetDiagram()
    {
        viewerController.ResetAll();
    }

    public void ShowCircumscribedCircle()
    {
        viewerController.VisualizeCircumscribedCircleOfTriangle();
    }

    public void ShowVoronoiEdge()
    {
        viewerController.VisualizeVoronoiEdge();
    }

    public void IncrementDelaunayTriangulation()
    {
        viewerController.IncrementDelaunayTriangulation();
    }

    public void ShowDelaunayTriangulation()
    {
        viewerController.GenerateDelaunayTriangulation();
    }

    public void ShowVoronoi()
    {
        viewerController.GenerateVoronoiDiagram();
    }

    public void RelaxVoronoi()
    {
        viewerController.RelaxVoronoiDiagram();
    }
}
