using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Ball : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField]
    private Rigidbody2D rb2d;
    [SerializeField]
    private ParticleSystem collisionParticle;

    [Header("Config")]
    [Range(0f, 1f)]
    [SerializeField]
    private float maxInitialAngle = 0.67f;
    [SerializeField]
    private float moveSpeed = 1f;

    private const string tagWallInProgress = "WallInProgress";

    void Start()
    {
        EventManager.StartListening(EventManager.Event.onReset, ResetPosition);
        EventManager.StartListening(EventManager.Event.onStartGame, ResetPosition);
    }

    void OnDestroy()
    {
        EventManager.StopListening(EventManager.Event.onReset, ResetPosition);
        EventManager.StopListening(EventManager.Event.onStartGame, ResetPosition);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        EmitParticle(8);

        if (collision.gameObject.CompareTag(tagWallInProgress))
        {
            Debug.Log("Touched the territory in progress!");
            GameManager.instance.screenShake.StartShake(0.33f, 0.1f);
            GameManager.instance.IncreaseScore();
        }
    }

    private void ResetPosition(Dictionary<string, object> message=null)
    {
        Vector2 position = new(0f, 0f);
        transform.position = position;

        Vector2 dir = Random.value < 0.5f ? Vector2.left : Vector2.right;
        dir.y = Random.value < 0.5f ? -maxInitialAngle : maxInitialAngle;
        rb2d.velocity = dir * moveSpeed;
    }

    private void EmitParticle(int amount)
    {
        collisionParticle.Emit(amount);
    }
}
