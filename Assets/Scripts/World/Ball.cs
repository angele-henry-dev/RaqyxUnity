using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private GameObject northWall;
    [SerializeField]
    private GameObject southWall;
    [SerializeField]
    private GameObject westWall;
    [SerializeField]
    private GameObject eastWall;
    [SerializeField]
    private ParticleSystem collisionParticle;

    [Header("Config")]
    [Range(0f, 1f)]
    [SerializeField]
    private float maxInitialAngle = 0.67f;
    [SerializeField]
    private float moveSpeed = 1f;

    private float minStartY;
    private float maxStartY;
    private float minStartX;
    private float maxStartX;

    private readonly float ballSize = 0.2f;
    // private List<Vector2> directions = new List<Vector2> { Vector2.left, Vector2.right };

    private void Start()
    {
        GameManager.instance.onReset += ResetPosition;
        GameManager.instance.gameUI.onStartGame += InitialPush;
    }

    private void OnDestroy()
    {
        GameManager.instance.onReset -= ResetPosition;
        GameManager.instance.gameUI.onStartGame -= InitialPush;
    }

    private void InitialPush()
    {
        minStartY = southWall.transform.position.y + ballSize + 0.1f;
        maxStartY = northWall.transform.position.y - ballSize - 0.1f;
        minStartX = westWall.transform.position.x + ballSize + 0.1f;
        maxStartX = eastWall.transform.position.x - ballSize - 0.1f;
        ResetPosition();
    }

    private void ResetPosition()
    {
        float posY = Random.Range(minStartY, maxStartY);
        float posX = Random.Range(minStartX, maxStartX);
        Vector2 position = new Vector2(posX, posY);
        transform.position = position;

        Vector2 dir = Random.value < 0.5f ? Vector2.left : Vector2.right;
        dir.y = Random.value < 0.5f ? -maxInitialAngle : maxInitialAngle;
        //dir.y = Random.Range(-maxInitialAngle, maxInitialAngle);
        rb2d.velocity = dir * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        Player player = collision.GetComponent<Player>();
        if (player)
        {
            Debug.Log("Game over!");
            GameManager.instance.IncreaseScore();
            EmitParticle(8);
        }
        */

        if (collision.gameObject.CompareTag("Player"))
        {
            EmitParticle(8);
            GameManager.instance.screenShake.StartShake(0.33f, 0.1f);
            Debug.Log("Game over!");
            GameManager.instance.IncreaseScore();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            EmitParticle(8);
        }
    }

    private void EmitParticle(int amount)
    {
        collisionParticle.Emit(amount);
    }
}
