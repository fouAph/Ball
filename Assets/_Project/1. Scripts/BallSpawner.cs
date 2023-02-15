using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] Transform ballPrefab;
    [SerializeField] Transform ballSpawnPosition;

    private void Awake()
    {
        ballSpawnPosition = GameObject.FindWithTag("BallSpawnPosition").transform;
    }

    public GameObject SpawnBall()
    {
        return Instantiate(ballPrefab, ballSpawnPosition.position, Quaternion.identity).gameObject;
    }
}