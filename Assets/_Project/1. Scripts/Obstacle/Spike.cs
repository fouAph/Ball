using UnityEngine;

public class Spike : MonoBehaviour
{ 
    private void OnCollisionEnter2D(Collision2D other) {
         if (other.collider.CompareTag("Ball"))
        {
            Ball ball = other.collider.GetComponent<Ball>();
            ball.PopBall();
        }
    }
}
