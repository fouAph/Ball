using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] AudioClip[] collidedSFX;
    [SerializeField] AudioSource audioSource;
    public Vector3 pos { get { return transform.position; } }

    private Rigidbody2D rb;
    private bool collided;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (rb.velocity.magnitude > .02f)
            audioSource.PlayOneShot(collidedSFX[Random.Range(0, collidedSFX.Length)]);
    }

    public bool GetCollided()
    {
        return collided;
    }

}
