using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2D;
    private bool collided;
    public Vector3 pos { get { return transform.position; } }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActiveRb()
    {
        rb.isKinematic = false;
    }

    public void DeactiveRb()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        rb.isKinematic = true;
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (collided) return;
    //     if (other.gameObject.CompareTag("Ball")) return;
    //     collided = true;
    //     Invoke("SetupNewBall", 1f);
    //     this.enabled = false;
    // }

    // private void SetupNewBall()
    // {
    //     if (GameManager.Instance.ballSpawnPosition)
    //         GameManager.Instance.SetupNewBall();
    //     print("setup new ball");
    // }

    public bool GetCollided()
    {
        return collided;
    }

}
