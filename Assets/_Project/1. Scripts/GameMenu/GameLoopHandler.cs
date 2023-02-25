using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameLoopHandler : MonoBehaviour
{
    [SerializeField] Transform ballSpawnPosition;
    [SerializeField] Transform ballPrefab;

    private void Start()
    {
        RetrieveGoalObject();

        SpawnBall();
        GameManager.Instance.onDragEnd += SpawnBall_OnDragEnd;
    }

    private List<GoalBucket> goals = new List<GoalBucket>();
    public GameObject SpawnBall()
    {
        Transform ballGo = Instantiate(ballPrefab, ballSpawnPosition.position, Quaternion.identity, transform);
        ballGo.parent = null;

        Ball currentBall = ballGo.GetComponent<Ball>();
        GameManager.Instance.CurrentBall = currentBall;
        currentBall.DeactiveRb();

        GameManager.Instance.SetGameState(GameState.NEXT_ATTEMPT);
        return ballGo.gameObject;

    }

    public void SpawnBall_OnDragEnd(object sender, EventArgs e)
    {
        if (GameManager.Instance.GetCurrentAttemptCount() == 0) return;
        StartCoroutine(SpawnBall_Coroutine());
    }

    private IEnumerator SpawnBall_Coroutine()
    {
        float waitTime = 3f;
        yield return new WaitForSeconds(waitTime);
        SpawnBall();
    }

    private void RetrieveGoalObject()
    {
        goals = FindObjectsOfType<GoalBucket>().ToList();
    }
}