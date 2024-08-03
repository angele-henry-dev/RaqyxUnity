using UnityEngine;
using Shapes2D;

public class PolygonGenerator : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private Shape shape;
    public Vector2[] points;

    public void DesignPolygon(Color fillColor, Color outlineColor)
    {
        shape.settings.fillColor = fillColor;
        shape.settings.outlineColor = outlineColor;
    }

    public void UpdatePolygon(Vector2[] newPoints)
    {
        Vector2[] normalizedPoints = NormalizePoints(newPoints);

        // I'll probably don't need it but I use it for the debug
        points = newPoints;

        // Apply the new points to the form
        shape.settings.polyVertices = normalizedPoints;
        shape.settings.polyVertices = shape.settings.polyVertices; // Force update

        // Remove the old colliders
        foreach (var collider in GetComponents<EdgeCollider2D>())
        {
            Destroy(collider);
        }

        // Add a new EdgeCollider2D
        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        // Create an array of points for the collider, including the closing point
        Vector2[] colliderPoints = new Vector2[normalizedPoints.Length + 1];
        for (int i = 0; i < normalizedPoints.Length; i++)
        {
            colliderPoints[i] = normalizedPoints[i];
        }
        // Close the loop adding the first point at the end
        colliderPoints[normalizedPoints.Length] = normalizedPoints[0];

        // Apply the points on the collider
        edgeCollider.points = colliderPoints;
    }

    // Normalize points to the range -0.5 to 0.5
    public Vector2[] NormalizePoints(Vector2[] points)
    {
        Vector2[] normalizedPoints = new Vector2[points.Length];
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;

        // Find the min and max values for x and y
        foreach (Vector2 point in points)
        {
            if (point.x < minX) minX = point.x;
            if (point.x > maxX) maxX = point.x;
            if (point.y < minY) minY = point.y;
            if (point.y > maxY) maxY = point.y;
        }

        // Normalize points to the range -0.5 to 0.5
        for (int i = 0; i < points.Length; i++)
        {
            normalizedPoints[i] = new Vector2(
                (points[i].x - minX) / (maxX - minX) - 0.5f,
                (points[i].y - minY) / (maxY - minY) - 0.5f
            );
        }

        return normalizedPoints;
    }

}
