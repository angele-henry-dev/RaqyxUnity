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
        // I'll probably don't need it but I use it for the debug
        points = newPoints;

        // Apply the new points to the form
        shape.settings.polyVertices = newPoints;

        // Remove the old colliders
        foreach (var collider in GetComponents<EdgeCollider2D>())
        {
            Destroy(collider);
        }

        // Add a new EdgeCollider2D
        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        // Create an array of points for the collider, including the closing point
        Vector2[] colliderPoints = new Vector2[newPoints.Length + 1];
        for (int i = 0; i < newPoints.Length; i++)
        {
            colliderPoints[i] = newPoints[i];
        }
        // Close the loop adding the first point at the end
        colliderPoints[newPoints.Length] = newPoints[0];

        // Apply the points on the collider
        edgeCollider.points = colliderPoints;
    }

}
