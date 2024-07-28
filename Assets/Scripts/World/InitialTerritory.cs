using System.Collections.Generic;
using UnityEngine;

public class InitialTerritory : PolygonGenerator
{
    [Header("Config")]
    [SerializeField]
    private float decay;

    void Awake()
    {
        EventManager.StartListening(EventManager.Event.onStartGame, GenerateInitialTerritory);
    }

    private void GenerateInitialTerritory(Dictionary<string, object> message = null)
    {
        if (message.TryGetValue("polygonPoints", out object value))
        {
            Vector2[] newPoints = (Vector2[])value;
            // Decay will probably be removed
            if (decay > 0f)
            {
                for (int i = 0; i < newPoints.Length; i++)
                {
                    newPoints[i].y = newPoints[i].y > 0 ? (newPoints[i].y + (decay)) : (newPoints[i].y - (decay));
                    newPoints[i].x = newPoints[i].x > 0 ? (newPoints[i].x + (decay)) : (newPoints[i].x - (decay));
                }
            }

            DesignPolygon(Color.clear, Color.white);
            UpdatePolygon(newPoints);
        }
    }
}
