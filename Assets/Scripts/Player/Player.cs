using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Player : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    Rigidbody2D rb2d;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]

    [Header("Config")]
    private float speed = 1.5f;
    [SerializeField]
    private Vector2 startPosition;

    TrailRenderer trail;

    public bool isReversed = false;

    public Vector2 Direction { get; private set; }
    public Vector2 NextDirection { get; private set; }

    private const string axisHorizontal = "Horizontal";
    private const string axisVertical = "Vertical";
    private readonly string tagEnnemy = Enums.GetTagsValue(Enums.Tags.Ennemy);
    private readonly string tagWallOutside = Enums.GetTagsValue(Enums.Tags.WallOutside);

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }

    void Start()
    {
        // Initialize the starting position, the direction and the territory
        Vector2 size = spriteRenderer.transform.localScale;
        startPosition = new(x: -1.5f, y: 3f + (size.x / 2f));
        SetDirection(Vector2.right);
        SetTerritoryInProgress(false);

        EventManager.StartListening(EventManager.Event.onReset, ResetPosition);
        EventManager.StartListening(EventManager.Event.onStartGame, ResetPosition);
    }

    void OnDestroy()
    {
        EventManager.StopListening(EventManager.Event.onReset, ResetPosition);
    }

    void FixedUpdate()
    {
        MoveAutomatically();
    }

    void Update()
    {
        HandlePlayerInput();
        AdjustSpriteRotation();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag(tagEnnemy) && GameManager.instance.isTerritoryInProgress)
            HandleCollisionEnnemy();
        else if (collision.gameObject.CompareTag(tagWallOutside))
            HandleCollisionOutsideWall();
    }

    private bool Valid(Vector2 newDirection)
    {
        // IF already in direction NO
        if (newDirection == Direction)
            return false;

        // IF trying to reverse NO
        if ((newDirection == Vector2.right && Direction == Vector2.left) ||
        (newDirection == Vector2.left && Direction == Vector2.right) ||
        (newDirection == Vector2.up && Direction == Vector2.down) ||
        (newDirection == Vector2.down && Direction == Vector2.up))
            return false;

        if (!GameManager.instance.isTerritoryInProgress)
        {
            // IF WALL in new direction NO
            if ((Direction == Vector2.left && newDirection == Vector2.down) ||
            (Direction == Vector2.right && newDirection == Vector2.up) ||
            (Direction == Vector2.down && newDirection == Vector2.right) ||
            (Direction == Vector2.up && newDirection == Vector2.left))
                return false;
        }

        return true;
    }

    private void MoveAutomatically()
    {
        Vector2 position = rb2d.position;
        Vector2 translation = speed * Time.fixedDeltaTime * Direction;
        rb2d.MovePosition(position + translation);
    }

    private void HandleCollisionEnnemy()
    {
        GameManager.instance.screenShake.StartShake(0.33f, 0.1f);
        GameManager.instance.IncreaseScore();
    }

    private void HandleCollisionOutsideWall()
    {
        const float pushOutFactor = 0.01f;

        // Determine the next direction based on the current direction and whether the movement is reversed
        NextDirection = Direction switch
        {
            Vector2 v when v == Vector2.right => new Vector2(0, isReversed ? 1 : -1),
            Vector2 v when v == Vector2.left => new Vector2(0, isReversed ? -1 : 1),
            Vector2 v when v == Vector2.down => new Vector2(isReversed ? 1 : -1, 0),
            Vector2 v when v == Vector2.up => new Vector2(isReversed ? -1 : 1, 0),
            _ => Vector2.zero,
        };

        // Move the player slightly to avoid staying against the wall
        Vector2 pushOut = rb2d.position * -pushOutFactor;
        rb2d.position += pushOut;

        SetTerritoryInProgress(false);
    }

    private void HandlePlayerInput()
    {
        // If the next direction is defined then use it
        if (NextDirection != Vector2.zero)
        {
            SetDirection(NextDirection);
        }

        // Get the player input
        float horizontalMovement = Input.GetAxis(axisHorizontal);
        float verticalMovement = Input.GetAxis(axisVertical);

        // If not inputs from the player
        if (horizontalMovement == 0 && verticalMovement == 0)
            return;

        // Determine the new direction depending on the player input
        Vector2 newDirection = Vector2.zero;

        if (verticalMovement > 0)
        {
            newDirection = Vector2.up;
        }
        else if (verticalMovement < 0)
        {
            newDirection = Vector2.down;
        }
        else if (horizontalMovement < 0)
        {
            newDirection = Vector2.left;
        }
        else if (horizontalMovement > 0)
        {
            newDirection = Vector2.right;
        }

        // If the new direction is valid then let's draw a territory!
        if (Valid(newDirection))
        {
            SetDirection(newDirection);
            SetTerritoryInProgress(true);
        }
    }

    private void SetDirection(Vector2 direction)
    {
        Direction = direction;
        NextDirection = Vector2.zero;
    }

    private void AdjustSpriteRotation()
    {
        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        rb2d.rotation = angle - 90f;
    }

    private void ResetPosition(Dictionary<string, object> message = null)
    {
        NextDirection = Vector2.zero;
        transform.position = startPosition;
        SetTerritoryInProgress(false);

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

    private void SetTerritoryInProgress(bool inProgress)
    {
        GameManager.instance.isTerritoryInProgress = inProgress;
        trail.enabled = inProgress;
        if (!inProgress)
            trail.Clear();
    }
}
