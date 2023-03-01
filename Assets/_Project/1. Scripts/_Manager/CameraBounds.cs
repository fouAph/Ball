using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    private Camera cam;
    private Bounds bounds;
    GameObject go;
    private Vector3[] corners = new Vector3[4];
    private Vector3 top, left, right, bottom;

    [SerializeField] Transform targetObj;
    [SerializeField] Transform anchorTop, anchorBottom, anchorLeft, anchorRight;
    [SerializeField] public float offsetFromEdge = 1f;


    private Vector3 anchorRightOffset;
    private Vector3 anchorTopOffset;
    private Vector3 anchorLeftOffset;
    private Vector3 anchorBottomOffset;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        go = new GameObject("GO ");

        CalculateCameraBounds(cam, out top, out left, out right, out bottom);

        anchorTop.position = top;
        anchorBottom.position = bottom;
        anchorLeft.position = left;
        anchorRight.position = right;
    }


    void Start()
    {
        anchorRightOffset = targetObj.position - anchorRight.position;
        anchorTopOffset = targetObj.position - anchorTop.position;
        anchorLeftOffset = targetObj.position - anchorLeft.position;
        anchorBottomOffset = targetObj.position - anchorBottom.position;
    }

    void Update()
    {
        CalculateCameraBounds(cam, out top, out left, out right, out bottom);

        // anchorTop.position = top;
        // // anchorBottom.position = bottom;
        // anchorLeft.position = left;
        anchorRight.position = right;

        Vector3 targetPos = anchorRight.position + anchorRightOffset
            + anchorTop.position + anchorTopOffset
            + anchorLeft.position + anchorLeftOffset
            + anchorBottom.position + anchorBottomOffset;

        targetPos /= 4f; // Divide by the number of anchors to calculate the average position

        targetObj.position = targetPos;
    }

    private Bounds CalculateCameraBounds(Camera camera, out Vector3 top, out Vector3 left, out Vector3 right, out Vector3 bottom)
    {


        corners[0] = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        corners[1] = camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane));
        corners[2] = camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane));
        corners[3] = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));



        bounds = new Bounds(corners[0], Vector3.zero);
        for (int i = 1; i < corners.Length; i++)
        {
            bounds.Encapsulate(corners[i]);
        }

        top = new Vector3((bounds.max.x + bounds.min.x) / 2f, bounds.max.y, 0f);
        left = new Vector3(bounds.min.x, (bounds.max.y + bounds.min.y) / 2f, 0f);
        right = new Vector3(bounds.max.x, (bounds.max.y + bounds.min.y) / 2f, 0f);
        bottom = new Vector3((bounds.max.x + bounds.min.x) / 2f, bounds.min.y, 0f);

        return bounds;
    }
}
