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
    private float moveSpeed = 1f;

    private Vector3 startPosition;
    private const string axisHorizontal = "Horizontal";
    private const string axisVertical = "Vertical";

    private Vector2[] territoryPoints;
    private int currentPointIndex = 0;
    private float t = 0f;

    private void Start()
    {
        // Get the points from the current territory
        territoryPoints = polygonGenerator.GetPoints().ToArray();

        // I have an issue with the position of the points
        // For some reason y is always 1 too high
        // So I need to decrease it :/
        for (int i=0; i<territoryPoints.Length; i++)
        {
            territoryPoints[i].y -= 1;
            //Debug.Log($"{territoryPoints[i].x}:{territoryPoints[i].y}");
        }
        // Get the initial position
        startPosition = transform.position;

        GameManager.instance.onReset += ResetPosition;
    }

    private void FixedUpdate()
    {
        float[] movements = ProcessInput();
        MoveManually(movements);
        MoveAlongTerritory(territoryPoints);
    }

    private void OnDestroy()
    {
        GameManager.instance.onReset -= ResetPosition;
    }

    private void ResetPosition()
    {
        transform.position = startPosition;
    }

    private float[] ProcessInput()
    {
        float[] movements = { Input.GetAxis(axisHorizontal), Input.GetAxis(axisVertical) };

        if (Mathf.Abs(movements[0]) > Mathf.Abs(movements[1]))
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

    private void MoveAlongTerritory(Vector2[] points)
    {
        if (points.Length < 2)
            return;

        // Points between which we are moving
        Vector2 startPoint = points[currentPointIndex];
        Vector2 endPoint = points[(currentPointIndex + 1) % points.Length];

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
            currentPointIndex = (currentPointIndex + 1) % points.Length;
        }
    }

    private void AdjustSpriteRotation()
    {
        float rotation = rb2d.velocity.x < 0f ? 90f : rb2d.velocity.x > 0f ? 270f : rb2d.velocity.y < 0f ? 180f : 0f;
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
