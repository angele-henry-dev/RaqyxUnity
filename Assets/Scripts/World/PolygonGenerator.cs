using UnityEngine;
using Shapes2D;
using System.Collections.Generic;

public class PolygonGenerator : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private Shape shape;
    [SerializeField]
    private float decay;

    Vector2[] points;

    private void Awake()
    {
        // GeneratePolygon();
        EventManager.StartListening(EventManager.Event.onStartGame, GeneratePolygon);
    }

    /*private void GenerateOutsidePolygon()
    {
        Vector2 size = spriteDecay.transform.localScale;
        float decay = size.x - 0.1f;

        List<Vector2> pointList = GetPoints();
        Vector2 [] points = pointList.ToArray();
        for (int i = 0; i < points.Length; i++)
        {
            points[i].y -= 1;

            points[i].y = points[i].y > 0 ? (points[i].y + (decay)) : (points[i].y - (decay));
            points[i].x = points[i].x > 0 ? (points[i].x + (decay)) : (points[i].x - (decay));
        }
    }*/

    private void GeneratePolygon(Dictionary<string, object> message = null)
    {
        if (message.TryGetValue("polygonPoints", out object value))
        {
            points = (Vector2[]) value;
            if (decay > 0f)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    // points[i].y -= 1;

                    points[i].y = points[i].y > 0 ? (points[i].y + (decay)) : (points[i].y - (decay));
                    points[i].x = points[i].x > 0 ? (points[i].x + (decay)) : (points[i].x - (decay));
                }
            }
        }

        shape.settings.fillColor = Color.clear;
        // shape.settings.outlineColor = Color.white;

        // Apply new points to the shape
        shape.settings.polyVertices = points;

        // Add the points to the collider
        EdgeCollider2D collider = gameObject.AddComponent<EdgeCollider2D>();
        Vector2[] colliderpoints = new Vector2[points.Length + 1];
        for (int i = 0; i < points.Length; i++)
        {
            colliderpoints[i] = new Vector2(
                (points[i].x < 0f) ? -0.5f : 0.5f,
                (points[i].y < 0f) ? -0.5f : 0.5f
            );
        }
        colliderpoints[points.Length] = new Vector2(
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
