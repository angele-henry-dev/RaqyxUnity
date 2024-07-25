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
    private float speed = 1.5f;
    [SerializeField]
    private Vector2 startPosition;

    public bool isTerritoryInProgress = false;

    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }

    private float playerDecay;
    private const string axisHorizontal = "Horizontal";
    private const string axisVertical = "Vertical";
    private const string tagWall = "Wall";
    private const string tagWallOutside = "WallOutside";

    void Start()
    {
        Vector2 size = spriteRenderer.transform.localScale;
        playerDecay = size.x - 0.1f;
        startPosition = new(x: -1.5f, y: (3f + playerDecay));

        EventManager.StartListening(EventManager.Event.onReset, ResetPosition);
        EventManager.StartListening(EventManager.Event.onStartGame, ResetPosition);
    }

    void OnDestroy()
    {
        EventManager.StopListening(EventManager.Event.onReset, ResetPosition);
    }

    void FixedUpdate()
    {
        Vector2 position = rb2d.position;
        Vector2 translation = speed * Time.fixedDeltaTime * direction;
        rb2d.MovePosition(position + translation);
    }

    void Update()
    {
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }

        if (Input.GetAxis(axisVertical) > 0)
        {
            SetDirection(Vector2.up);
        }
        else if (Input.GetAxis(axisVertical) < 0)
        {
            SetDirection(Vector2.down);
        }
        else if (Input.GetAxis(axisHorizontal) < 0)
        {
            SetDirection(Vector2.left);
        }
        else if (Input.GetAxis(axisHorizontal) > 0)
        {
            SetDirection(Vector2.right);
        }

        AdjustSpriteRotation();
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        //if (forced || Valid(direction))
        //{
            this.direction = direction;
            nextDirection = Vector2.zero;
        //}
    }

    /*void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision!");
        if (collision.gameObject.CompareTag(tagWall) && isTerritoryInProgress)
        {
            Debug.Log("Collision wall!");
            ChangeDirection(GetNextMove());
        } else if (collision.gameObject.CompareTag(tagWallOutside) && !isTerritoryInProgress)
        {
            Debug.Log("Collision outside wall!");
            ChangeDirection(GetNextMove());
            Debug.Log($"Go {direction}");
        }
    }*/

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.CompareTag(tagWallOutside))
        {
            Debug.Log("Collision with outside wall");
            if (direction == Vector2.right)
            {
                nextDirection = new(x: 0, y: -1);

            }
            else if (direction == Vector2.left)
            {
                nextDirection = new(x: 0, y: 1);
            }

            else if (direction == Vector2.down)
            {
                nextDirection = new(x: -1, y: 0);
            }

            else if (direction == Vector2.up)
            {
                nextDirection = new(x: 1, y: 0);
            }
            else
            {
                nextDirection = Vector2.zero;
            }
        }
    }

    /*public bool Occupied(Vector2 direction)
    {
        // If no collider is hit then there is no obstacle in that direction
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f);
        return hit.collider != null;
    }*/

    bool Valid(Vector2 dir)
    {
        // TODO Adapt it to ignore inside walls
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        Debug.Log($"{hit.collider.tag}");
        return hit.collider.CompareTag(tagWallOutside);
    }

    /*private float[] ProcessInput()
    {
        float[] movements = { Input.GetAxis(axisHorizontal), Input.GetAxis(axisVertical) };

        if (movements[0] == 0 && movements[1] == 0)
        {
            return null;
        }
        else if (Mathf.Abs(movements[0]) > Mathf.Abs(movements[1]))
        {
            movements[0] = movements[0] < 0 ? -1 : 1;
            movements[1] = 0;
        }
        else
        {
            movements[0] = 0;
            movements[1] = movements[1] < 0 ? -1 : 1;
        }
        return movements;
    }

    private void ChangeDirection(float[] movements)
    {
        Vector2 velo = rb2d.velocity;
        velo.x = speed * movements[0];
        velo.y = speed * movements[1];
        rb2d.velocity = velo;
        AdjustSpriteRotation();
    }*/

    private void AdjustSpriteRotation()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb2d.rotation = angle - 90f;
    }

    private void ResetPosition(Dictionary<string, object> message = null)
    {
        nextDirection = Vector2.zero;
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

    /*void Update()
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
    }*/

    /*void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision!");
        if (collision.gameObject.CompareTag(tagWall) && isTerritoryInProgress)
        {
            Debug.Log("Collision wall!");
            ChangeDirection(GetNextMove());
        } else if (collision.gameObject.CompareTag(tagWallOutside) && !isTerritoryInProgress)
        {
            Debug.Log("Collision outside wall!");
            ChangeDirection(GetNextMove());
            Debug.Log($"Go {direction}");
        }
    }*/

    // Movements

    /*private void MoveAuto()
    {
        Vector2 newDirection = transform.up;
        rb2d.velocity = newDirection * moveSpeed;

        /*if (!isTerritoryInProgress)
        {
            Vector2? nextPoint = TriggerPolygonPoint();
            if (nextPoint != null)
            {
                transform.position = nextPoint.Value;
                ChangeDirection(GetNextMove());
            }
        }
    }

    private void MoveManually(float[] movements)
    {
        ChangeDirection(movements);
        isTerritoryInProgress = true;
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
    }*/

    // UI

    // Trigger

    /*private void BackOnPolygon()
    {
        isTerritoryInProgress = false;
        // ContactPoint2D contact = collision.GetContact(0);

        switch (direction) {
            case Direction.RIGHT:
                transform.position = new Vector2((transform.position.x + playerDecay), (transform.position.y));
                break;
            case Direction.LEFT:
                transform.position = new Vector2((transform.position.x - playerDecay), (transform.position.y));
                break;
            case Direction.DOWN:
                transform.position = new Vector2((transform.position.x), (transform.position.y - playerDecay));
                break;
            case Direction.UP:
                transform.position = new Vector2((transform.position.x), (transform.position.y + playerDecay));
                break;
        }

        /*transform.position = direction switch
        {
            Direction.RIGHT => new(x: (transform.position.x), y: transform.position.y, z: transform.position.z),
            Direction.LEFT => new(x: (transform.position.x), y: transform.position.y, z: transform.position.z),
            Direction.DOWN => new(x: transform.position.x, y: (transform.position.y), z: transform.position.z),
            _ => new(x: transform.position.x, y: (transform.position.y), z: transform.position.z),
        };

        ChangeDirection(GetNextMove());
    }*/

    /*private Vector2? TriggerPolygonPoint()
    {
        Vector2 startingFrom = transform.position;
        Vector2? targetPoint = GetNextPoint();

        if (targetPoint != null)
        {
            float distanceToPoint = Vector2.Distance(startingFrom, (Vector2) targetPoint);

            if (distanceToPoint < pointThreshold)
            {
                return targetPoint;
            }
        }
        return null;
    }

    private Vector2? GetNextPoint()
    {
        Vector2 currentPosition = transform.position;

        for (int i = 0; i < territoryPoints.Length; i++)
        {
            switch (direction)
            {
                case Direction.UP:
                    if (IsEqualTo(territoryPoints[i].x, currentPosition.x) && territoryPoints[i].y > currentPosition.y)
                    {
                        return territoryPoints[i];
                    }
                    break;
                case Direction.DOWN:
                    if (IsEqualTo(territoryPoints[i].x, currentPosition.x) && territoryPoints[i].y < currentPosition.y)
                    {
                        return territoryPoints[i];
                    }
                    break;
                case Direction.LEFT:
                    if (IsEqualTo(territoryPoints[i].y, currentPosition.y) && territoryPoints[i].x < currentPosition.x)
                    {
                        return territoryPoints[i];
                    }
                    break;
                case Direction.RIGHT:
                    if (IsEqualTo(territoryPoints[i].y, currentPosition.y) && (territoryPoints[i].x > currentPosition.x))
                    {
                        return territoryPoints[i];
                    }
                    break;
            }
        }
        return null;
    }

    private bool IsEqualTo(float a, float b)
    {
        return System.Math.Abs(a - b) < float.Epsilon;
    }*/
}
