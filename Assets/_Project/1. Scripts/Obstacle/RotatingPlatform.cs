using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{

    public float speed = 10f;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.forward * speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ball"))
        {
            Vector2 force = transform.up * speed * 10f;
            collision.collider.attachedRigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
