using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameState currentGameState;
    [SerializeField] int currentLevelIndex;
    [SerializeField] private BallSpawner ballSpawner;
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] GameSceneManager gameSceneManager;
    [Header("Important Variables")]
    public GameSettings gameSettings;
    [SerializeField] LevelInfoSO[] levelInfoSOList;

    public event EventHandler onCurrentGoalCountChange;
    public event EventHandler onCurrentAttemptCountChange;

    public event EventHandler onGameLose;
    public event EventHandler onGameWin;


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
        currentAttemptCount = gameSettings.maxAttempt;
        onCurrentAttemptCountChange += GameManager_OnCurrentAttemptCountChange;
        onCurrentGoalCountChange += GameManager_OnCurrentGoalCountChange;
        // SetupGame();
        // MenuManager.Instance.SpawnLevelButton();
    }

    public void Resetlevel()
    {
        currentAttemptCount = gameSettings.maxAttempt;
    }

    private void Update()
    {
        if (previousGoalCount != currentGoalCount)
        {
            if (currentGameState == GameState.GAMEOVER_Lose) return;

            previousGoalCount = currentGoalCount;
            onCurrentGoalCountChange?.Invoke(this, EventArgs.Empty);
        }

        if (currentGameState != GameState.NEXT_ATTEMPT) return;

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

    #region GameLoadFuntions
    private void LoadGame()
    {

    }
    #endregion

    #region GameLoop
    public void Retry_OnButtonClick()
    {
        gameSceneManager.RetryLevel();
        Resetlevel();

    }

    public void NextLevel_OnButtonClick()
    {
        gameSceneManager.LoadNextLevel();
        Resetlevel();
    }

    public void MainMenu_OnButtonClick()
    {
        SceneManager.UnloadSceneAsync(currentLevelIndex);
        SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
    }

    public void SetupGame()
    {
        ballSpawner = FindObjectOfType<BallSpawner>();
        print("ball spawn");
        cam = Camera.main;
        PrepareNewBall();

        var goals = FindObjectsOfType<GoalBucket>();
        goalCountToReach = goals.Length;
    }

    #endregion

    #region DragFunctions_Region

    private void OnDragStart()
    {
        currentBall.DeactiveRb();
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        gameSettings.trajectory.Show();
    }

    private void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        distance = Mathf.Clamp(distance, 0, gameSettings.maxDragDistance);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * gameSettings.pushForce;

        Debug.DrawLine(startPoint, endPoint);

        gameSettings.trajectory.UpdateDots(currentBall.pos, force);
    }

    private void OnDragEnd()
    {
        currentBall.ActiveRb();
        currentBall.Push(force);

        gameSettings.trajectory.Hide();
        currentAttemptCount--;
        currentBall = null;
        ResetDragVariableRelated();

        onCurrentAttemptCountChange?.Invoke(this, EventArgs.Empty);

        currentGameState = GameState.WAITING;

        if (CheckOutOfBall())
        {
            //initiate GameOver
            if (currentGameState == GameState.GAMEOVER_WIN) return;

            print("Initiate game over");
            StartCoroutine(InitiateGameLose());
        }
        else
        {
            //Spawn New Ball
            print("Spawn New Ball");
            StartCoroutine(SpawnNewBall());
        }
    }

    private IEnumerator SpawnNewBall()
    {
        float waitTime = 3f;
        yield return new WaitForSeconds(waitTime);
        PrepareNewBall();
    }

    private IEnumerator InitiateGameLose()
    {
        yield return new WaitForSeconds(gameSettings.waitTimeWhenOutofBall);
        // print("GameOver");
        if (currentGameState != GameState.GAMEOVER_WIN)
        {
            LevelFailed("Out of attempt");
            onGameLose?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool CheckOutOfBall()
    {
        return currentAttemptCount == 0;
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

    public void PrepareNewBall()
    {
        // if (ballSpawner)

        GameObject ballGo = ballSpawner.SpawnBall();
        ballGo.transform.parent = UIManager.Instance.transform;
        ballGo.transform.parent = null;
        currentBall = ballGo.GetComponent<Ball>();
        currentBall.DeactiveRb();

        currentGameState = GameState.NEXT_ATTEMPT;
    }

    private void GameManager_OnCurrentAttemptCountChange(object sender, EventArgs e)
    {
        // CheckAttemptLeft();
    }

    private void GameManager_OnCurrentGoalCountChange(object sender, EventArgs e)
    {
        if (currentGoalCount >= goalCountToReach)
        {
            Debug.Log($"You Win This Level");
            currentGameState = GameState.GAMEOVER_WIN;
            onGameWin?.Invoke(this, EventArgs.Empty);
            StopAllCoroutines();
        }
    }

    private void LevelFailed(string message = "")
    {
        Debug.Log($"Level Failed + {message}");
        currentGameState = GameState.GAMEOVER_Lose;
        onGameLose?.Invoke(this, EventArgs.Empty);
    }

    #region Getter Setter Functions
    #region Getter

    public int GetMaxAttempt()
    {
        return gameSettings.maxAttempt;
    }

    public int GetCurrentAttemptCount()
    {
        return currentAttemptCount;
    }

    public LevelInfoSO[] GetLevelInfoSOList()
    {
        return levelInfoSOList;
    }

    public GameSceneManager GetGameSceneManager()
    {
        return gameSceneManager;
    }

    public GameState GetCurrentGameState()
    {
        return currentGameState;
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }
    #endregion
    #region Setter
    public void SetcurrentLevelIndex(int index)
    {
        currentLevelIndex = index;
    }

    public void SetCurrentGoalCount()
    {
        currentGoalCount++;
    }
    #endregion
    #endregion

}

public enum GameState { NEXT_ATTEMPT, WAITING, GAMEOVER_WIN, GAMEOVER_Lose }

[System.Serializable]
public class GameSettings
{
    public float waitTimeWhenOutofBall = 5f;

    public int maxAttempt = 3;
    public float pushForce = 4f;
    public float maxDragDistance = 3;
    public Trajectory trajectory;

}

