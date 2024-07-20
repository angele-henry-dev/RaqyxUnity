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
    private Vector3 startPosition = new(x:-2f, y:3f, z:0f);

    [SerializeField]
    private bool isTerritoryInProgress = false;

    private const string axisHorizontal = "Horizontal";
    private const string axisVertical = "Vertical";
    // private const string tagWall = "Wall";

    void Start()
    {
        EventManager.StartListening(EventManager.Event.onReset, ResetPosition);
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
        isTerritoryInProgress = true;
    }

    private void MoveAuto()
    {
        Vector2 direction = transform.up;
        rb2d.velocity = direction * moveSpeed;
    }

    private void AdjustSpriteRotation()
    {
        float rotation = rb2d.velocity.x < 0f ? 90f : rb2d.velocity.x > 0f ? 270f : rb2d.velocity.y < 0f ? 180f : 0f;
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PolygonGenerator polygon = collision.GetComponent<PolygonGenerator>();
        if (isTerritoryInProgress && polygon)
        {
            BackOnPolygon(polygon);
        }
    }

    private void BackOnPolygon(PolygonGenerator polygon)
    {
        Vector3 currentPosition = transform.position;
        Debug.Log("Collide wall");
        isTerritoryInProgress = false;
    }
}
