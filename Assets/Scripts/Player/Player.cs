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
    public bool isReversed = false;

    public Vector2 Direction { get; private set; }
    public Vector2 NextDirection { get; private set; }

    //private float playerDecay;
    private const string axisHorizontal = "Horizontal";
    private const string axisVertical = "Vertical";
    private const string tagWallOutside = "WallOutside";
    private const string tagEnnemy = "Ennemy";

    void Start()
    {
        // Initialize the starting position, the direction and the territory
        Vector2 size = spriteRenderer.transform.localScale;
        //playerDecay = size.x / 2f;
        startPosition = new(x: -1.5f, y: 3f + (size.x / 2f));
        SetDirection(Vector2.right);
        isTerritoryInProgress = false;

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

        if (isTerritoryInProgress && collision.gameObject.CompareTag(tagEnnemy))
            HandleCollisionEnnemy();
        else if (collision.gameObject.CompareTag(tagWallOutside))
            HandleCollisionOutsideWall();
    }

    private bool Valid(Vector2 dir)
    {
        // TODO
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

        NextDirection = Direction switch
        {
            Vector2 v when v == Vector2.right => new Vector2(0, isReversed ? 1 : -1),
            Vector2 v when v == Vector2.left => new Vector2(0, isReversed ? -1 : 1),
            Vector2 v when v == Vector2.down => new Vector2(isReversed ? 1 : -1, 0),
            Vector2 v when v == Vector2.up => new Vector2(isReversed ? -1 : 1, 0),
            _ => Vector2.zero,
        };

        Vector2 pushOut = rb2d.position * -pushOutFactor;
        rb2d.position += pushOut;

        isTerritoryInProgress = false;
    }

    private void HandlePlayerInput()
    {
        if (NextDirection != Vector2.zero)
        {
            SetDirection(NextDirection);
        }

        float horizontalMovement = Input.GetAxis(axisHorizontal);
        float verticalMovement = Input.GetAxis(axisVertical);

        if (horizontalMovement == 0 && verticalMovement == 0)
            return;

        if (verticalMovement > 0)
            SetDirection(Vector2.up);
        else if (verticalMovement < 0)
            SetDirection(Vector2.down);
        else if (horizontalMovement < 0)
            SetDirection(Vector2.left);
        else if (horizontalMovement > 0)
            SetDirection(Vector2.right);

        isTerritoryInProgress = true;
    }

    private void SetDirection(Vector2 direction, bool forced = false)
    {
        if (forced || Valid(direction))
        {
            Direction = direction;
            NextDirection = Vector2.zero;
        }
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
}
