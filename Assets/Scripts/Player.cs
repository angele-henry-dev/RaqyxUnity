using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float moveSpeed = 1f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        GameManager.instance.onReset += ResetPosition;
    }

    private void ResetPosition()
    {
        transform.position = startPosition;
    }

    private void Update()
    {
        float[] movements = ProcessInput();
        Move(movements);
    }

    private float[] ProcessInput()
    {
        float[] movements = { Input.GetAxis("Horizontal"), Input.GetAxis("Vertical") };
        return movements;
    }

    private void Move(float[] movements)
    {
        Vector2 velo = rb2d.velocity;
        velo.x = moveSpeed * movements[0];
        velo.y = moveSpeed * movements[1];
        rb2d.velocity = velo;
    }
}
