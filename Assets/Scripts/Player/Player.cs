using System.Collections.Generic;
using UnityEngine;
using static PolygonGenerator;

public class Player : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private PolygonGenerator polygonGenerator;

    [Header("Config")]
    [SerializeField]
    private float moveSpeed = 1f;

    private Vector3 startPosition;
    private const string axisHorizontal = "Horizontal";
    private const string axisVertical = "Vertical";

    private Vector2[] territoryPoints;
    private Path[] territoryPaths;
    private int currentPointIndex = 0;
    private float t = 0f;

    private void Start()
    {
        // Get the points from the current territory
        territoryPoints = polygonGenerator.GetPoints().ToArray();
        for (int i=0; i<territoryPoints.Length; i++)
        {
            territoryPoints[i].y -= 1;
        }
        territoryPaths = polygonGenerator.GetPaths();
        /*for (int i = 0; i < territoryPaths.Length; i++)
        {
            Debug.Log("territoryPaths " + i);
            Debug.Log($"P0: {territoryPaths[i].p0.x}:{territoryPaths[i].p0.y}");
            Debug.Log($"P1: {territoryPaths[i].p1.x}:{territoryPaths[i].p1.y}");
        }*/

        // Get the initial position
        startPosition = transform.position;

        EventManager.StartListening(EventManager.Event.onReset, ResetPosition);
    }

    private void FixedUpdate()
    {
        float[] movements = ProcessInput();
        if (movements == null)
        {
            MoveAlongTerritoryPoints();
        } else
        {
            MoveManually(movements);
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventManager.Event.onReset, ResetPosition);
    }

    private void ResetPosition(Dictionary<string, object> message)
    {
        transform.position = startPosition;
    }

    private float[] ProcessInput()
    {
        float[] movements = { Input.GetAxis(axisHorizontal), Input.GetAxis(axisVertical) };

        if (movements[0] == 0 && movements[1] == 0)
        {
            return null;
        }
        else if (Mathf.Abs(movements[0]) > Mathf.Abs(movements[1]))
        {
            movements[1] = 0;
        }
        else
        {
            movements[0] = 0;
        }
        return movements;
    }

    private void MoveManually(float[] movements)
    {
        Vector2 velo = rb2d.velocity;
        velo.x = moveSpeed * movements[0];
        velo.y = moveSpeed * movements[1];
        rb2d.velocity = velo;
        AdjustSpriteRotation();
    }

    private void MoveAlongTerritoryPaths()
    {
        if (territoryPaths.Length < 4)
            return;
    }

    private void MoveAlongTerritoryPoints()
    {
        if (territoryPoints.Length < 2)
            return;

        // Points between which we are moving
        Vector2 startPoint = territoryPoints[currentPointIndex];
        Vector2 endPoint = territoryPoints[(currentPointIndex + 1) % territoryPoints.Length];

        // Interpolate between points
        t += moveSpeed * Time.deltaTime / Vector2.Distance(startPoint, endPoint);

        // Calculate the new position
        Vector2 newPosition = Vector2.Lerp(startPoint, endPoint, t);
        Vector2 direction = (newPosition - rb2d.position).normalized;

        // Apply velocity to the Rigidbody2D
        rb2d.velocity = direction * moveSpeed;

        AdjustSpriteRotation();

        // Move to the next segment
        if (t >= 1f)
        {
            t = 0f;
            currentPointIndex = (currentPointIndex + 1) % territoryPoints.Length;
        }
    }

    private void AdjustSpriteRotation()
    {
        float rotation = rb2d.velocity.x < 0f ? 90f : rb2d.velocity.x > 0f ? 270f : rb2d.velocity.y < 0f ? 180f : 0f;
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
