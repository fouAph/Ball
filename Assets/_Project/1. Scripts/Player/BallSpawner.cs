using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] Transform ballPrefab;
    public GameObject SpawnBall()
    {
        return Instantiate(ballPrefab, transform.position, Quaternion.identity).gameObject;
    }
}