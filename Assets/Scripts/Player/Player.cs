using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [Header("Config")]
    [SerializeField]
    private float moveSpeed = 1f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        GameManager.instance.onReset += ResetPosition;
    }

    private void Update()
    {
        float[] movements = ProcessInput();
        Move(movements);
        //Debug.DrawLine(new Vector3(movements[0], movements[1]), transform.position);
        //Debug.Break();
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
        float[] movements = { Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") };

        if (Mathf.Abs(movements[0]) > Mathf.Abs(movements[1]))
        {
            movements[1] = 0;
        }
        else
        {
            movements[0] = 0;
        }

        /*float[] movements = { 0f, 0f };
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movements[1] = Input.GetAxis("Vertical");
        } else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movements[0] = Input.GetAxis("Horizontal");
        }*/
        return movements;
    }

    private void Move(float[] movements)
    {
        Vector2 velo = rb2d.velocity;
        velo.x = moveSpeed * movements[0];
        velo.y = moveSpeed * movements[1];
        rb2d.velocity = velo;
        AdjustSpriteRotation();
    }

    private void AdjustSpriteRotation()
    {
        //float z = rb2d.velocity.x < 0f ? 270f : 90f;
        //z = rb2d.velocity.y < 0f ? 0f : 180f;
        //Vector3 movements = new Vector3(0, 0, z);
        //transform.Rotate(movements);

        //spriteRenderer.flipX = rb2d.velocity.x < 0f;
        //spriteRenderer.flipY = rb2d.velocity.y < 0f;

        //transform.LookAt(null, Vector3.left);

        //rb2d.AddTorque(10 * Time.deltaTime);

        float rotation = rb2d.velocity.x < 0f ? 90f : rb2d.velocity.x > 0f ? 270f : rb2d.velocity.y < 0f ? 180f : 0f;
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
