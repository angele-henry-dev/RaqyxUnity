using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TrailRenderer))]
public class TrailColision : MonoBehaviour
{
    TrailRenderer trail;
    EdgeCollider2D trailCollider;

    static List<EdgeCollider2D> unusedColliders = new();

    void Start()
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
            validCollider = new GameObject("TrailCollider", typeof(EdgeCollider2D)).GetComponent<EdgeCollider2D>();
        }
        return validCollider;
        //return GetComponent<EdgeCollider2D>();
    }

    private void SetColliderPointsFromTrail(TrailRenderer trail, EdgeCollider2D collider)
    {
        List<Vector2> points = new();
        // Avoid having default points at (-.5,0),(.5,0)
        if (trail.positionCount == 0)
        {
            points.Add(transform.position);
            points.Add(transform.position);
        }
        else for (int position = 0; position < trail.positionCount; position++)
            {
                points.Add(trail.GetPosition(position));
            }
        collider.SetPoints(points);
    }
}
