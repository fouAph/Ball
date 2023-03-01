using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [SerializeField] Vector3[] paths;
    [SerializeField] bool debugPath;

    private int currentPath = 0;
    private void Start()
    {
        transform.position = paths[0];
    }

    private void FixedUpdate()
    {
        Moving();
    }

    private void Moving()
    {

        if (Vector2.Distance(transform.position, paths[currentPath]) < .5f)
        {
            currentPath++;
            if (currentPath == paths.Length)
            {
                currentPath = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, paths[currentPath], moveSpeed * Time.deltaTime);

    }


    private void OnDrawGizmos()
    {
        if (debugPath)
            for (int i = 0; i < paths.Length; i++)
            {
                Gizmos.DrawWireSphere(paths[i], .5f);
            }
    }
}
