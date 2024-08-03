using Clipper2Lib;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InitialTerritory : PolygonGenerator
{
    [Header("Config")]
    public bool isTransparent = false;

    private const float scale = 1000.0f; // Scale factor for converting to Clipper's integer coordinates

    void Awake()
    {
        EventManager.StartListening(EventManager.Event.onStartGame, GenerateInitialTerritory);
    }

    private void GenerateInitialTerritory(Dictionary<string, object> message = null)
    {
        if (message.TryGetValue("polygonPoints", out object value))
        {
            Vector2[] newPoints = (Vector2[])value;

            if (isTransparent)
            {
                Color transparent = new();
                transparent.a = 0;
                DesignPolygon(Color.clear, transparent);
            } else
            {
                DesignPolygon(Color.clear, Color.white);
            }

            UpdatePolygon(newPoints);
        }
    }

    // Method to cut out the new shape from the existing polygon
    public void CutOutShape(Vector2[] newShapePoints)
    {
        Debug.Log("*************** Dynamic newShapePoints");
        foreach (Vector2 point in newShapePoints)
        {
            Debug.Log($"{point.x} :: {point.y}");
        }

        // Hardcoded points for comparison
        Vector2[] hardcodedShapePoints = {
            new Vector2(1f, 4f),
            new Vector2(1f, 1f),
            new Vector2(2f, 1f),
            new Vector2(2f, 4f),
            new Vector2(1f, 4f)
        };

        Debug.Log("*************** Hardcoded newShapePoints");
        foreach (Vector2 point in hardcodedShapePoints)
        {
            Debug.Log($"{point.x} :: {point.y}");
        }

        // Convert to Clipper paths using original points
        Path64 polygonPath = ToClipperPath(points);
        Path64 dynamicShapePath = ToClipperPath(newShapePoints);
        Path64 hardcodedShapePath = ToClipperPath(hardcodedShapePoints);

        Debug.Log("*************** polygonPath");
        foreach (Point64 point in polygonPath)
        {
            Debug.Log($"{point.X} :: {point.Y}");
        }
        Debug.Log("*************** dynamicShapePath");
        foreach (Point64 point in dynamicShapePath)
        {
            Debug.Log($"{point.X} :: {point.Y}");
        }
        Debug.Log("*************** hardcodedShapePath");
        foreach (Point64 point in hardcodedShapePath)
        {
            Debug.Log($"{point.X} :: {point.Y}");
        }

        // Prepare Paths64 collections
        Paths64 subject = new() { polygonPath };
        Paths64 dynamicClip = new() { dynamicShapePath };
        Paths64 hardcodedClip = new() { hardcodedShapePath };

        // Perform the clipping operation using static methods
        Paths64 dynamicSolution = Clipper.Difference(subject, dynamicClip, FillRule.NonZero);
        Paths64 hardcodedSolution = Clipper.Difference(subject, hardcodedClip, FillRule.NonZero);

        // Convert the dynamic solution back to Vector2
        if (dynamicSolution.Count > 0)
        {
            Debug.Log("*************** dynamicSolution[0]");
            foreach (Point64 point in dynamicSolution[0])
            {
                Debug.Log($"{point.X} :: {point.Y}");
            }
            Vector2[] newPolygonPoints = ToVector2List(dynamicSolution[0]);
            UpdatePolygon(newPolygonPoints);
        }

        // Convert the hardcoded solution back to Vector2
        if (hardcodedSolution.Count > 0)
        {
            Debug.Log("*************** hardcodedSolution[0]");
            foreach (Point64 point in hardcodedSolution[0])
            {
                Debug.Log($"{point.X} :: {point.Y}");
            }
            Vector2[] newPolygonPoints = ToVector2List(hardcodedSolution[0]);
            UpdatePolygon(newPolygonPoints);
        }
    }

    // Helper method to convert Vector2 to Clipper Path64
    private Path64 ToClipperPath(Vector2[] vectorPath)
    {
        Path64 path = new(vectorPath.Length);
        foreach (Vector2 vec in vectorPath)
        {
            path.Add(new Point64((long)(vec.x * scale), (long)(vec.y * scale)));
        }
        return path;
    }

    // Helper method to convert Clipper Path64 to Vector2
    private Vector2[] ToVector2List(Path64 intPath)
    {
        Vector2[] vectorPath = new Vector2[intPath.Count];
        for (int i = 0; i < intPath.Count; i++)
        {
            vectorPath[i] = new Vector2(intPath[i].X / scale, intPath[i].Y / scale);
        }
        return vectorPath;
    }
}
