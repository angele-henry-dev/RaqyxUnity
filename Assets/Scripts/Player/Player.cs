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

    public Vector2 Direction { get; private set; }
    public Vector2 NextDirection { get; private set; }

    //private float playerDecay;
    private const string axisHorizontal = "Horizontal";
    private const string axisVertical = "Vertical";
    private const string tagWallOutside = "WallOutside";
    private const string tagEnnemy = "Ennemy";

    void Start()
    {
        Vector2 size = spriteRenderer.transform.localScale;
        //playerDecay = size.x / 2f;
        startPosition = new(x: -1.5f, y: 3f + (size.x / 2f));
        isTerritoryInProgress = false;
        SetDirection(Vector2.right);

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
        Vector2 translation = speed * Time.fixedDeltaTime * Direction;
        rb2d.MovePosition(position + translation);
    }

    void Update()
    {
        if (NextDirection != Vector2.zero)
        {
            SetDirection(NextDirection);
        }

        if (Input.GetAxis(axisVertical) > 0)
        {
            SetDirection(Vector2.up);
            isTerritoryInProgress = true;
        }
        else if (Input.GetAxis(axisVertical) < 0)
        {
            SetDirection(Vector2.down);
            isTerritoryInProgress = true;
        }
        else if (Input.GetAxis(axisHorizontal) < 0)
        {
            SetDirection(Vector2.left);
            isTerritoryInProgress = true;
        }
        else if (Input.GetAxis(axisHorizontal) > 0)
        {
            SetDirection(Vector2.right);
            isTerritoryInProgress = true;
        }

        AdjustSpriteRotation();
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if (forced || Valid(direction))
        {
            Direction = direction;
            NextDirection = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (isTerritoryInProgress && collision.gameObject.CompareTag(tagEnnemy))
        {
            // UI effect
            GameManager.instance.screenShake.StartShake(0.33f, 0.1f);

            // Game effect
            Debug.Log("Game over!");
            GameManager.instance.IncreaseScore();
        }

        else if (collision.gameObject.CompareTag(tagWallOutside))
        {
            if (Direction == Vector2.right)
            {
                NextDirection = new(x: 0, y: -1);

            }
            else if (Direction == Vector2.left)
            {
                NextDirection = new(x: 0, y: 1);
            }

            else if (Direction == Vector2.down)
            {
                NextDirection = new(x: -1, y: 0);
            }

            else if (Direction == Vector2.up)
            {
                NextDirection = new(x: 1, y: 0);
            }
            else
            {
                NextDirection = Vector2.zero;
            }

            // Move a bit the player to avoid staying against the outside wall
            Vector2 pushOut = rb2d.position * -0.01f;
            rb2d.position += pushOut;
            isTerritoryInProgress = false;
        }
    }

    bool Valid(Vector2 dir)
    {
        // TODO
        return true;
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
