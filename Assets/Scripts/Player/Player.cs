using System.Collections.Generic;
using UnityEngine;

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
    private float moveSpeed = 1.5f;
    [SerializeField]
    private Vector2 startPosition;

    [SerializeField]
    public bool isTerritoryInProgress = false;

    [SerializeField]
    private Direction direction = Direction.RIGHT;

    private Vector2[] territoryPoints;
    private int currentPointIndex = 0;
    private float playerDecay;
    private const string axisHorizontal = "Horizontal";
    private const string axisVertical = "Vertical";

    private float pointThreshold = 0.1f; // Distance ? laquelle le personnage doit ?tre consid?r? comme ayant atteint un point

    enum Direction
    {
        UP,
        DOWN,
        RIGHT,
        LEFT
    }

    void Start()
    {
        Vector2 size = spriteRenderer.transform.localScale;
        playerDecay = 0.1f;
        startPosition = new(x: -2.5f, y: (3f + playerDecay));

        territoryPoints = GetPolygonPoints();
        EventManager.StartListening(EventManager.Event.onReset, ResetPosition);

        ResetPosition();
    }

    void Update()
    {
        float[] movements = ProcessInput();
        if (movements != null)
        {
            MoveManually(movements);
        }
    }

    void FixedUpdate()
    {
        MoveAuto();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PolygonGenerator polygon = collision.GetComponent<PolygonGenerator>();
        if (polygon)
        {
            if (isTerritoryInProgress)
            {
                BackOnPolygon();
            }
        }
    }

    void OnDestroy()
    {
        EventManager.StopListening(EventManager.Event.onReset, ResetPosition);
    }

    // Movements

    private void MoveAuto()
    {
        /*Vector2 direction = transform.up;
        rb2d.velocity = direction * moveSpeed;*/

        if (territoryPoints.Length < 2)
            return;

        Vector2 newDirection;

        if (TriggerPolygonPoint())
        {
            currentPointIndex = (currentPointIndex + 1) % territoryPoints.Length;
            Vector2 targetPoint = territoryPoints[currentPointIndex];
            newDirection = (targetPoint - rb2d.position).normalized;
        } else
        {
            newDirection = (territoryPoints[currentPointIndex] - rb2d.position).normalized;
        }

        rb2d.velocity = newDirection * moveSpeed;

        AdjustSpriteRotation();
    }

    private void MoveManually(float[] movements)
    {
        ChangeDirection(movements);
        isTerritoryInProgress = true;
    }

    private void ChangeDirection(float[] movements)
    {
        Vector2 velo = rb2d.velocity;
        velo.x = moveSpeed * movements[0];
        velo.y = moveSpeed * movements[1];
        rb2d.velocity = velo;
        AdjustSpriteRotation();
    }

    private float[] GetNextMove()
    {
        float[] movements = direction switch
        {
            Direction.RIGHT => new[] { 0f, -1f },
            Direction.LEFT => new[] { 0f, 1f },
            Direction.DOWN => new[] { -1f, 0f },
            _ => new[] { 1f, 0f },
        };
        return movements;
    }

    // UI

    private void AdjustSpriteRotation()
    {
        /*float rotation = 0f;
        direction = Direction.UP;

        if (rb2d.velocity.x < 0f)
        {
            rotation = 90f;
            direction = Direction.LEFT;
        } else if (rb2d.velocity.x > 0f)
        {
            rotation = 270f;
            direction = Direction.RIGHT;
        }
        else if (rb2d.velocity.y < 0f)
        {
            rotation = 180f;
            direction = Direction.DOWN;
        }
        transform.eulerAngles = new Vector3(0, 0, rotation);*/

        float angle = Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) * Mathf.Rad2Deg;
        rb2d.rotation = angle - 90f;
    }

    private void ResetPosition(Dictionary<string, object> message = null)
    {
        transform.position = startPosition;

        /*Vector2 direction = startPosition - rb2d.position;
        if (direction.magnitude < 0.1f)
        {
            rb2d.velocity = Vector2.zero;
            transform.position = startPosition;
        }
        else
        {
            rb2d.velocity = direction.normalized * moveSpeed;
        }*/

        /*float t = moveSpeed * Time.deltaTime / Vector2.Distance(transform.position, startPosition);
        Vector2 newPosition = Vector2.Lerp(transform.position, startPosition, t);
        Vector2 newDirection = (newPosition - rb2d.position).normalized;
        rb2d.velocity = newDirection * moveSpeed;*/

        /*Vector2 newDirection = startPosition - rb2d.position;
        rb2d.velocity = newDirection.normalized * moveSpeed;*/
    }

    private Vector2[] GetPolygonPoints()
    {
        List<Vector2> pointList = polygonGenerator.GetPoints();
        Vector2 [] points = pointList.ToArray();
        for (int i = 0; i < points.Length; i++)
        {
            points[i].y -= 1;

            points[i].y = points[i].y > 0 ? (points[i].y + playerDecay) : (points[i].y - playerDecay);
            points[i].x = points[i].x > 0 ? (points[i].x + playerDecay) : (points[i].x - playerDecay);
            Debug.Log($"Point {i}: {points[i].x}/{points[i].y}");
        }
        return points;
    }

    // Trigger

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

    private void BackOnPolygon()
    {
        isTerritoryInProgress = false;

        transform.position = direction switch
        {
            Direction.RIGHT => new(x: (transform.position.x + playerDecay), y: transform.position.y, z: transform.position.z),
            Direction.LEFT => new(x: (transform.position.x - playerDecay), y: transform.position.y, z: transform.position.z),
            Direction.DOWN => new(x: transform.position.x, y: (transform.position.y - playerDecay), z: transform.position.z),
            _ => new(x: transform.position.x, y: (transform.position.y + playerDecay), z: transform.position.z),
        };

        ChangeDirection(GetNextMove());
    }

    private bool TriggerPolygonPoint()
    {
        Vector2 startingFrom = rb2d.position;
        Vector2 targetPoint = territoryPoints[currentPointIndex];
        float distanceToPoint = Vector2.Distance(startingFrom, targetPoint);

        if (distanceToPoint < pointThreshold)
        {
            return true;
        }
        return false;
    }
}
