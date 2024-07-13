using UnityEngine;
using Shapes2D;
using System.Collections.Generic;

public class PolygonGenerator : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private Shape shape;

    PathSegment[] pathList;
    List<Vector2> points;

    public PathSegment[] GetPaths()
    {
        return pathList;
    }

    public List<Vector2> GetPoints()
    {
        return points;
    }

    private void Awake()
    {
        GeneratePolygon();
    }

    private void GeneratePolygon()
    {
        shape.settings.fillColor = Color.clear;
        shape.settings.outlineColor = Color.white;

        // Creation of the points
        points = new();
        points.Add(new Vector2(-2f, 4f));
        points.Add(new Vector2(2f, 4f));
        points.Add(new Vector2(2f, -2f));
        points.Add(new Vector2(-2f, -2f));

        // Creation of the segments
        pathList = new PathSegment[4];
        pathList[0] = new(points[0], points[1]);
        pathList[1] = new(points[1], points[2]);
        pathList[2] = new(points[2], points[3]);
        pathList[3] = new(points[3], points[0]);

        // Apply new segments to the shape
        shape.settings.pathSegments = pathList;
        shape.settings.polyVertices = points.ToArray();

        // Add the points to the collider
        EdgeCollider2D collider = gameObject.AddComponent<EdgeCollider2D>();
        Vector2[] colliderpoints = new Vector2[points.Count + 1];
        for (int i = 0; i < points.Count; i++)
        {
            colliderpoints[i] = new Vector2(
                (points[i].x < 0f) ? -0.5f : 0.5f,
                (points[i].y < 0f) ? -0.5f : 0.5f
            );
        }
        colliderpoints[points.Count] = new Vector2(
            (points[0].x < 0f) ? -0.5f : 0.5f,
            (points[0].y < 0f) ? -0.5f : 0.5f
        ); // Close the loop
        collider.points = colliderpoints;

        UpdatePolygon();
    }

    private void UpdatePolygon()
    {
        // Implement any necessary updates to the polygon here
    }
}
