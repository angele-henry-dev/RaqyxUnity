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

        Vector2[] test = {
            new Vector2(-2f, 4f),
            new Vector2(2f, 4f),
            new Vector2(2f, -2f),
            new Vector2(-2f, -2f),
            new Vector2(-2f, 4f)
        };
        Vector2[] newTest = {
            new Vector2(1f, 4f),
            new Vector2(1f, 1f),
            new Vector2(2f, 1f),
            new Vector2(2f, 4f),
            new Vector2(1f, 4f)
        };

        // Convert to Clipper paths
        Path64 polygonPath = ToClipperPath(test);
        Path64 shapePath = ToClipperPath(newTest);
        Debug.Log("**************************************");
        Debug.Log("polygonPath");
        foreach (Point64 point in polygonPath)
        {
            Debug.Log($"{point.X} :: {point.Y}");
        }
        Debug.Log("**************************************");
        Debug.Log("shapePath");
        foreach (Point64 point in shapePath)
        {
            Debug.Log($"{point.X} :: {point.Y}");
        }

        // Prepare Paths64 collections
        Paths64 subject = new() { polygonPath };
        Paths64 clip = new() { shapePath };

        // Perform the clipping operation using static methods
        Paths64 solution = Clipper.Difference(subject, clip, FillRule.NonZero);

        // Convert the solution back to Vector2
        if (solution.Count > 0)
        {
            Vector2[] newPolygonPoints = ToVector2List(solution[0]);
            Debug.Log("**************************************");
            Debug.Log("newPolygonPoints");
            foreach (Point64 point in solution[0])
            {
                Debug.Log($"{point.X} :: {point.Y}");
            }
            UpdatePolygon(newPolygonPoints);
        }

        // TEST
        /*Paths64 subj = new Paths64();
        Paths64 clip = new Paths64();
        subj.Add(Clipper.MakePath(new int[] { 100, 50, 10, 79, 65, 2, 65, 98, 10, 21 }));
        clip.Add(Clipper.MakePath(new int[] { 98, 63, 4, 68, 77, 8, 52, 100, 19, 12 }));
        Paths64 solution = Clipper.Difference(subj, clip, FillRule.NonZero);
        if (solution.Count > 0)
        {
            Vector2[] newPolygonPoints = ToVector2List(solution[0]);
            UpdatePolygon(newPolygonPoints);
        }*/
    }

    // Helper method to convert Vector2 to Clipper Path64
    private Path64 ToClipperPath(Vector2[] vectorPath)
    {
        Path64 path = new(vectorPath.Length);
        foreach (Vector2 vec in vectorPath)
        {
            path.Add(new Point64((long)(vec.x * 1000), (long)(vec.y * 1000)));
        }
        return path;
    }

    // Helper method to convert Clipper Path64 to Vector2
    private Vector2[] ToVector2List(Path64 intPath)
    {
        Vector2[] vectorPath = new Vector2[intPath.Count];
        for (int i = 0; i < intPath.Count; i++)
        {
            vectorPath[i] = (new Vector2(intPath[i].X / 1000.0f, intPath[i].Y / 1000.0f));
        }
        return vectorPath;
    }
}
