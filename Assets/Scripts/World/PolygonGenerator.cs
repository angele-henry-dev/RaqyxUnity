using UnityEngine;
using Shapes2D;
using System.Collections.Generic;

public class PolygonGenerator : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private Shape shape;

    private void Start()
    {
        GeneratePolygon();
    }

    private void GeneratePolygon()
    {
        shape.settings.fillColor = Color.clear;
        shape.settings.outlineColor = Color.white;

        // Creation of the points
        List<Vector2> points = new();
        points.Add(new Vector2(-2f, 4f));
        points.Add(new Vector2(2f, 4f));
        points.Add(new Vector2(2f, -2f));
        points.Add(new Vector2(-2f, -2f));

        // Creation of the segments
        PathSegment[] pathList = new PathSegment[4];
        pathList[0] = new(points[0], points[1]);
        pathList[1] = new(points[1], points[2]);
        pathList[2] = new(points[2], points[3]);
        pathList[3] = new(points[3], points[0]);

        // Apply new segments to the shape
        shape.settings.pathSegments = pathList;
        shape.settings.polyVertices = points.ToArray();

        // Add the points to the collider
        EdgeCollider2D collider = gameObject.AddComponent<EdgeCollider2D>();
        Vector2[] colliderpoints = new Vector2[5];
        colliderpoints[0] = new Vector2(-0.5f, 0.5f);
        colliderpoints[1] = new Vector2(0.5f, 0.5f);
        colliderpoints[2] = new Vector2(0.5f, -0.5f);
        colliderpoints[3] = new Vector2(-0.5f, -0.5f);
        colliderpoints[4] = new Vector2(-0.5f, 0.5f);
        collider.points = colliderpoints;

        UpdatePolygon();
    }

    private void UpdatePolygon()
    {
        //
    }
}
