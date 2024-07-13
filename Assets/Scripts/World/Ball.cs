using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private PolygonGenerator territory;
    [SerializeField]
    private ParticleSystem collisionParticle;

    [Header("Config")]
    [Range(0f, 1f)]
    [SerializeField]
    private float maxInitialAngle = 0.67f;
    [SerializeField]
    private float moveSpeed = 1f;

    /*private float minStartY;
    private float maxStartY;
    private float minStartX;
    private float maxStartX;*/

    //private const float ballSize = 0.2f;
    private const string tagPlayer = "Player";
    private const string tagWallInProgress = "WallInProgress";

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
        /*minStartY = southWall.transform.position.y + ballSize + 0.1f;
        maxStartY = northWall.transform.position.y - ballSize - 0.1f;
        minStartX = westWall.transform.position.x + ballSize + 0.1f;
        maxStartX = eastWall.transform.position.x - ballSize - 0.1f;*/
        ResetPosition();
    }

    private void ResetPosition()
    {
        //float posY = Random.Range(minStartY, maxStartY);
        //float posX = Random.Range(minStartX, maxStartX);
        //Vector2 position = new(posX, posY);
        Vector2 position = new(0f, 0f);
        transform.position = position;

        Vector2 dir = Random.value < 0.5f ? Vector2.left : Vector2.right;
        dir.y = Random.value < 0.5f ? -maxInitialAngle : maxInitialAngle;
        rb2d.velocity = dir * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EmitParticle(8);
        if (collision.gameObject.CompareTag(tagPlayer))
        {
            GameManager.instance.screenShake.StartShake(0.33f, 0.1f);
            Debug.Log("Game over!");
            GameManager.instance.IncreaseScore();
        }
    }

    private void EmitParticle(int amount)
    {
        collisionParticle.Emit(amount);
    }
}
