using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameState currentGameState;
    [SerializeField] int currentLevelIndex;
    private int currentLevelSOIndex { get { return currentLevelIndex - 2; } }
    private int nextLevelIndex { get { return currentLevelIndex + 1; } }
    private int nextLevelSOIndex { get { return currentLevelIndex - 1; } }
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

    public event EventHandler onDragEnd;


    private int currentAttemptCount;
    private int previousAttemptCount;

    private int goalCountToReach;
    private int currentGoalCount = 0;
    private int previousGoalCount;

    public Ball CurrentBall;

    private Camera cam;
    [NonSerialized] Vector2 startPoint;
    [NonSerialized] Vector2 endPoint;
    [NonSerialized] Vector2 direction;
    [NonSerialized] Vector2 force;
    [SerializeField] float distance;
    [NonSerialized] bool isDragging = false;

    private List<GoalBucket> goalBuckets;
    private int currentLevelStarScore = 0;

    private void Start()
    {
        goalBuckets = new List<GoalBucket>();
        currentAttemptCount = gameSettings.maxAttempt;
        onCurrentAttemptCountChange += GameManager_OnCurrentAttemptCountChange;
        onCurrentGoalCountChange += GameManager_OnCurrentGoalCountChange;
        onGameWin += OnGameWin_Event;
        cam = Camera.main;
        LoadGameMenu();
    }

    public void Resetlevel()
    {
        currentAttemptCount = gameSettings.maxAttempt;
        goalBuckets.Clear();
        goalBuckets = FindObjectsOfType<GoalBucket>().ToList();
        goalCountToReach = goalBuckets.Count;
        currentGoalCount = 0;
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

        if (CurrentBall)
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
    private void LoadGameMenu()
    {
        gameSceneManager.LoadGameMenu();
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
        // SceneManager.UnloadSceneAsync(currentLevelIndex);
        // SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
        SceneManager.LoadScene((int)SceneIndexes.MANAGER);

    }

    public void SetupGame()
    {
        ballSpawner = FindObjectOfType<BallSpawner>();
        print("ball spawn");
        cam = Camera.main;

        var goals = FindObjectsOfType<GoalBucket>();
        goalCountToReach = goals.Length;
        PrepareNewBall();
    }

    #endregion

    #region DragFunctions_Region

    private void OnDragStart()
    {
        CurrentBall.DeactiveRb();
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

        gameSettings.trajectory.UpdateDots(CurrentBall.pos, force);
    }

    private void OnDragEnd()
    {
        if (distance / gameSettings.maxDragDistance < gameSettings.minimumDragPercent)
        {
            gameSettings.trajectory.Hide();
            return;
        }

        CurrentBall.ActiveRb();
        CurrentBall.Push(force);

        gameSettings.trajectory.Hide();
        currentAttemptCount--;
        CurrentBall = null;
        ResetDragVariableRelated();

        onCurrentAttemptCountChange?.Invoke(this, EventArgs.Empty);
        onDragEnd?.Invoke(this, EventArgs.Empty);
        currentGameState = GameState.WAITING;

        if (CheckOutOfBall())
        {
            //initiate GameOver
            if (currentGameState == GameState.GAMEOVER_WIN) return;
            StartCoroutine(InitiateGameLose());
        }
        else
        {
            //Spawn New Ball 
            StartCoroutine(SpawnNewBall());
        }
        CalculateGameResult();

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
            currentLevelStarScore = 0;
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
        GameObject ballGo = ballSpawner.SpawnBall();
        ballGo.transform.parent = UIManager.Instance.transform;
        ballGo.transform.parent = null;
        CurrentBall = ballGo.GetComponent<Ball>();
        CurrentBall.DeactiveRb();

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
            currentGameState = GameState.GAMEOVER_WIN;
            onGameWin?.Invoke(this, EventArgs.Empty);
            StopAllCoroutines();
        }
    }

    private void OnGameWin_Event(object sender, EventArgs e)
    {
        levelInfoSOList[currentLevelSOIndex].SetStarScore(currentLevelStarScore);
        levelInfoSOList[nextLevelSOIndex].SetIsUnlocked(true);
    }

    private void LevelFailed(string message = "")
    {
        currentGameState = GameState.GAMEOVER_Lose;
        onGameLose?.Invoke(this, EventArgs.Empty);
        // CalculateGameResult();
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

    public void SetGameState(GameState _gameState)
    {
        currentGameState = _gameState;
    }
    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    public int GetStarsResult()
    {
        return currentLevelStarScore;
    }
    #endregion
    #region Setter
    public void SetcurrentLevelIndex(int index)
    {
        currentLevelIndex = index;
    }

    public int GetNextLevelIndex()
    {
        return nextLevelIndex;
    }

    public int GetCurrentLevelSOIndex()
    {
        return currentLevelSOIndex;
    }

    public void SetCurrentGoalCount()
    {
        currentGoalCount++;
    }
    #endregion
    #endregion

    private void CalculateGameResult()
    {
        float percentResult = 0;
        percentResult = (((float)currentAttemptCount + 1) / (float)GetMaxAttempt());

        if (percentResult <= .35f)
        {
            currentLevelStarScore = 1;
        }

        else if (percentResult <= .67f)
        {
            currentLevelStarScore = 2;
        }

        else if (percentResult > .67)
        {
            currentLevelStarScore = 3;
        }




    }

    #region SaveLoadGameData
    public void LoadGameData()
    {

    }

    public void SaveGameData()
    {

    }

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
    [Range(0f, 1f)]
    public float minimumDragPercent = .2f;
    public Trajectory trajectory;

}
