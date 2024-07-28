using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TrailRenderer))]
public class TrailColision : MonoBehaviour
{
    TrailRenderer trail;
    EdgeCollider2D trailCollider;

    private const string tagEnnemy = "Ennemy";

    static List<EdgeCollider2D> unusedColliders = new();

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        trailCollider = GetValidCollider();
    }

    void Update()
    {
        SetColliderPointsFromTrail(trail, trailCollider);
    }

    void OnDestroy()
    {
        if (trailCollider != null)
        {
            trailCollider.enabled = false;
            unusedColliders.Add(trailCollider);
        }
    }

    private EdgeCollider2D GetValidCollider()
    {
        EdgeCollider2D validCollider;
        if (unusedColliders.Count > 0)
        {
            validCollider = unusedColliders[0];
            validCollider.enabled = true;
            unusedColliders.RemoveAt(0);
        }
        else
        {
            GameObject gameObject = new("TrailCollider", typeof(EdgeCollider2D));
            gameObject.tag = "WallInProgress";
            gameObject.layer = 8;
            validCollider = gameObject.GetComponent<EdgeCollider2D>();
        }
        return validCollider;
    }

    private void SetColliderPointsFromTrail(TrailRenderer trail, EdgeCollider2D collider)
    {
        List<Vector2> points = new();
        // Avoid having default points at (0,0),(0,0)
        if (trail.positionCount == 0)
        {
            points.Add(transform.position);
            points.Add(transform.position);
            /*for (int i=0; i<points.Count; i++)
            {
                Debug.Log($"{points[i].x}:{points[i].y}");
            }*/
        }
        else for (int position = 0; position < trail.positionCount; position++)
            {
                points.Add(trail.GetPosition(position));
            }
        collider.SetPoints(points);
    }
}
