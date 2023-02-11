using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState gameState;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] float pushForce = 4f;
    [SerializeField] float maxDragDistance;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Trajectory trajectory;
    [SerializeField] Transform ballSpawnPosition;

    public event EventHandler onCurrentGoalCountChange;
    public event EventHandler onCurrentAttemptCountChange;

    private int maxAttempt = 3;
    private int currentAttemptCount;
    private int previousAttemptCount;

    private int goalCountToReach;
    private int currentGoalCount = 0;
    private int previousGoalCount;

    private Ball currentBall;

    [NonSerialized] Camera cam;
    [NonSerialized] Vector2 startPoint;
    [NonSerialized] Vector2 endPoint;
    [NonSerialized] Vector2 direction;
    [NonSerialized] Vector2 force;
    [NonSerialized] float distance;
    [NonSerialized] bool isDragging = false;


    private void Start()
    {
        onCurrentAttemptCountChange += GameManager_OnCurrentAttemptCountChange;
        onCurrentGoalCountChange += GameManager_OnCurrentGoalCountChange;
        SetupGame();
    }

    private void SetupGame()
    {
        cam = Camera.main;
        SetupNewBall();

        var goals = FindObjectsOfType<GoalBucket>();
        goalCountToReach = goals.Length;
    }

    private void Update()
    {
        if (gameState == GameState.GAMEOVER || gameState == GameState.WAITING) return;

        if (previousGoalCount != currentGoalCount)
        {
            previousGoalCount = currentGoalCount;
            print("Calling OnCurrentGoalCountChange");
            onCurrentGoalCountChange?.Invoke(this, EventArgs.Empty);
        }

        if (previousAttemptCount != currentAttemptCount)
        {
            previousAttemptCount = currentAttemptCount;
            print("Calling OnCurrentAttemptCountChange");
            onCurrentAttemptCountChange?.Invoke(this, EventArgs.Empty);
        }


        if (currentBall)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                isDragging = true;
                OnDragStart();
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && isDragging == true)
            {
                isDragging = false;
                OnDragEnd();
            }

            if (isDragging)
                OnDrag();
        }
    }

    #region DragFunctions_Region

    private void OnDragStart()
    {
        currentBall.DeactiveRb();
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        trajectory.Show();
    }

    private void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        distance = Mathf.Clamp(distance, 0, maxDragDistance);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * pushForce;

        Debug.DrawLine(startPoint, endPoint);

        trajectory.UpdateDots(currentBall.pos, force);
    }

    private void OnDragEnd()
    {
        currentBall.ActiveRb();
        currentBall.Push(force);

        trajectory.Hide();
        currentAttemptCount++;
        currentBall = null;
        ResetDragVariableRelated();

        gameState = GameState.WAITING;
        // if (gameState != GameState.GAMEOVER || currentBall.GetCollided())
        //     Invoke("SetupNewBall", 1f);
    }

    private void ResetDragVariableRelated()
    {
        startPoint = Vector3.zero;
        endPoint = Vector3.zero;
        direction = Vector3.zero;
        force = Vector3.zero;
        distance = 0;
        isDragging = false;
    }
    #endregion

    public void SetupNewBall()
    {
        GameObject ballGo = Instantiate(ballPrefab, ballSpawnPosition.position, Quaternion.identity);
        currentBall = ballGo.GetComponent<Ball>();
        currentBall.DeactiveRb();

        gameState = GameState.NEXT_ATTEMPT;
    }

    public void SetCurrentGoalCount()
    {
        currentGoalCount++;
    }

    private void GameManager_OnCurrentAttemptCountChange(object sender, EventArgs e)
    {
        CheckAttemptLeft();
    }

    private void CheckAttemptLeft()
    {
        if (currentAttemptCount >= maxAttempt)
        {
            LevelFailed("Out of attempt");
        }
    }

    private void GameManager_OnCurrentGoalCountChange(object sender, EventArgs e)
    {
        if (currentGoalCount >= goalCountToReach)
        {
            Debug.Log($"You Win This Level");
            gameState = GameState.GAMEOVER;
        }
    }

    private void LevelFailed(string message = "")
    {
        Debug.Log($"Level Failed + {message}");
        gameState = GameState.GAMEOVER;
    }
}

public enum GameState { NEXT_ATTEMPT, WAITING, GAMEOVER }
