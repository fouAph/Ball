using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] int goalToFill;
    private int currentGoalCount;

    [SerializeField] float pushForce = 4f;
    [SerializeField] float maxDragDistance;
    [SerializeField] Ball ball;
    [SerializeField] Trajectory trajectory;

    private Camera cam;
    private bool isDragging = false;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 direction;
    private Vector2 force;
    private float distance;


    private void Start()
    {
        cam = Camera.main;
        ball.DeactiveRb();

        var goals = FindObjectsOfType<GoalBucket>();
        goalToFill = goals.Length;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isDragging = true;
            OnDragStart();
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isDragging = false;
            OnDragEnd();
        }

        if (isDragging)
            OnDrag();
    }

    #region DragFunctions_Region

    private void OnDragStart()
    {
        ball.DeactiveRb();
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

        trajectory.UpdateDots(ball.pos, force);
    }

    private void OnDragEnd()
    {
        ball.ActiveRb();
        ball.Push(force);

        trajectory.Hide();
    }

    #endregion

    public int GetCurrentGoalCount()
    {
        return currentGoalCount;
    }
}

public class GoalBucket : MonoBehaviour
{
    private Collider col;
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
        isFilled = true;
        int currentGoal = GameManager.Instance.GetCurrentGoalCount();
        currentGoal++;
    }
}
