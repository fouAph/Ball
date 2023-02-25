using System;
using UnityEngine;
public class BallTestNew : MonoBehaviour
{
    [SerializeField] AudioClip[] collidedSFX;
    [SerializeField] AudioSource audioSource;
    public Vector3 pos { get { return transform.position; } }
    private PhysicsMaterial2D material2D;
    private Collider2D col2d;
    private Rigidbody2D rb;
    private bool collided;

    bool isGhost;

    [SerializeField] float bounciness;
    Camera cam;
    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 direction;
    Vector2 force;
    float distance;
    [SerializeField] bool isDragging = false;

    [SerializeField] Trajectory trajectory;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        material2D = col2d.sharedMaterial;
        cam = Camera.main;
    }

    private void Update()
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

    #region DragFunctions_Region

    private void OnDragStart()
    {
        DeactiveRb();
        startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        trajectory.Show();
    }

    private void OnDrag()
    {
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        distance = Vector2.Distance(startPoint, endPoint);
        direction = (startPoint - endPoint).normalized;
        force = direction * distance * 4;

        trajectory.UpdateDots(transform.position, force);
    }

    private void OnDragEnd()
    {
        ActiveRb();
        Push(force, false);

        trajectory.Hide();

        ResetDragVariableRelated();
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


    public void Push(Vector2 force, bool g)
    {
        isGhost = g;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ActiveRb()
    {
        rb.isKinematic = false;
    }

    public void DeactiveRb()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
        rb.isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collided)
        {
            audioSource.PlayOneShot(collidedSFX[UnityEngine.Random.Range(0, collidedSFX.Length)]);
            collided = true;
 
        }
    }
}