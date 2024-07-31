using Clipper2Lib;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InitialTerritory : PolygonGenerator
{
    [Header("Config")]
    public bool isTransparent = false;

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
    /*Debug.Log("**************************************");
    Debug.Log("newShapePoints");
    foreach (Vector2 point in newShapePoints)
    {
        Debug.Log($"{point.x} :: {point.y}");
    }*/

    // Method to cut out the new shape from the existing polygon
    public void CutOutShape(Vector2[] newShapePoints)
    {
        // Convert to Clipper paths
        Path64 polygonPath = ToClipperPath(points);
        Path64 shapePath = ToClipperPath(newShapePoints);

        // Perform the clipping operation
        Clipper clipper = new Clipper();
        clipper.AddSubject(polygonPath);
        clipper.AddClip(shapePath);

        Paths64 solution = new Paths64();
        clipper.Execute(ClipType.Difference, FillRule.NonZero, solution);

        // Convert the solution back to Vector2
        if (solution.Count > 0)
        {
            List<Vector2> newPolygonPoints = ToVector2List(solution[0]);
            points = newPolygonPoints;
            UpdatePolygon(newPolygonPoints.ToArray());
        }
    }

    // Helper method to convert Vector2 to Clipper Path64
    private Path64 ToClipperPath(List<Vector2> vectorPath)
    {
        Path64 path = new Path64(vectorPath.Count);
        foreach (Vector2 vec in vectorPath)
        {
            path.Add(new Point64((long)(vec.x * 1000), (long)(vec.y * 1000)));
        }
        return path;
    }

    // Helper method to convert Clipper Path64 to Vector2
    private List<Vector2> ToVector2List(Path64 intPath)
    {
        List<Vector2> vectorPath = new List<Vector2>(intPath.Count);
        foreach (Point64 ip in intPath)
        {
            vectorPath.Add(new Vector2(ip.X / 1000.0f, ip.Y / 1000.0f));
        }
        return vectorPath;
    }
}
