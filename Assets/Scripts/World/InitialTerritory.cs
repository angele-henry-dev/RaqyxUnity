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
}
