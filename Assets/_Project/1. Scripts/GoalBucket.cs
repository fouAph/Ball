using UnityEngine;

public class GoalBucket : MonoBehaviour
{
    private bool isFilled = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball"))
        {
            OnFilled();
        }
    }

    private void OnFilled()
    {
        if (isFilled) return;
        isFilled = true;
        GameManager.Instance.SetCurrentGoalCount();
    }
}
