using UnityEngine;

[ExecuteInEditMode]
public class FitAllGameObejectToCamera : MonoBehaviour
{
    private Camera cam;
    private void Start()
    {
        cam = Camera.main;

        // FitAllObjectToCamera();
        print(Screen.width);
        print(Screen.height);

    }

    private void FitAllObjectToCamera()
    {
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);
        Vector3 bottomLeft = cam.ViewportToWorldPoint(Vector3.zero);
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(cam.rect.width, cam.rect.height));
        Vector3 screenSize = topRight - bottomLeft;
        float screenRatio = screenSize.x / screenSize.y;
        float desiredRatio = transform.localScale.x / transform.localScale.y;

        if (screenRatio > desiredRatio)
        {
            float height = screenSize.y;
            transform.localScale = new Vector3(height * desiredRatio, height);
        }

        else
        {
            float width = screenSize.x;
            transform.localScale = new Vector3(width, width / desiredRatio);
        }
    }


    private void OnDrawGizmos()
    {
        Camera camera = GetComponent<Camera>();

        if (camera != null)
        {
            Gizmos.matrix = camera.transform.localToWorldMatrix;
            Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.farClipPlane, camera.nearClipPlane, camera.aspect);
        }
    }

}
