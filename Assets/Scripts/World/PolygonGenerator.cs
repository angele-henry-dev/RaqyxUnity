using UnityEngine;
using Shapes2D;

public class PolygonGenerator : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private Shape shape;
    public bool isTransparent = false;
    public Vector2[] points;

    public void DesignPolygon(Color fillColor, Color outlineColor)
    {
        if (isTransparent)
        {
            Color transparent = new();
            transparent.a = 0;
            shape.settings.fillColor = transparent;
            shape.settings.outlineColor = transparent;
        }
        else
        {
            shape.settings.fillColor = fillColor;
            shape.settings.outlineColor = outlineColor;
        }
    }

    public void UpdatePolygon(Vector2[] newPoints)
    {
        // Remove it after everything is working
        points = newPoints;

        // Apply new points to the shape
        shape.settings.polyVertices = newPoints;

        // Add the points to the collider
        EdgeCollider2D collider = gameObject.AddComponent<EdgeCollider2D>();
        Vector2[] colliderpoints = new Vector2[newPoints.Length + 1];
        for (int i = 0; i < newPoints.Length; i++)
        {
            colliderpoints[i] = new Vector2(
                (newPoints[i].x < 0f) ? -0.5f : 0.5f,
                (newPoints[i].y < 0f) ? -0.5f : 0.5f
            );
        }
        colliderpoints[newPoints.Length] = new Vector2(
            (newPoints[0].x < 0f) ? -0.5f : 0.5f,
            (newPoints[0].y < 0f) ? -0.5f : 0.5f
        ); // Close the loop
        collider.points = colliderpoints;
    }
}
