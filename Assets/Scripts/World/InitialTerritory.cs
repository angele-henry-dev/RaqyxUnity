using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class InitialTerritory : PolygonGenerator
{
    [Header("Config")]
    public bool isTransparent = false;
    //private float decay;

    void Awake()
    {
        EventManager.StartListening(EventManager.Event.onStartGame, GenerateInitialTerritory);
    }

    private void GenerateInitialTerritory(Dictionary<string, object> message = null)
    {
        if (message.TryGetValue("polygonPoints", out object value))
        {
            Vector2[] newPoints = (Vector2[])value;
            /*if (decay > 0f)
            {
                for (int i = 0; i < newPoints.Length; i++)
                {
                    newPoints[i].y = newPoints[i].y > 0 ? (newPoints[i].y + (decay)) : (newPoints[i].y - (decay));
                    newPoints[i].x = newPoints[i].x > 0 ? (newPoints[i].x + (decay)) : (newPoints[i].x - (decay));
                }
            }*/

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
}
