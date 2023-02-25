using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection2D : MonoBehaviour
{
    private Scene simulationScene;
    private PhysicsScene2D physicsScene2D;

    [SerializeField] Transform[] obstaclesParent;

    private void CreatePhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        physicsScene2D = simulationScene.GetPhysicsScene2D();

        foreach (Transform item in obstaclesParent)
        {
            var ghostObject = Instantiate(item.gameObject, item.position, Quaternion.identity);
            ghostObject.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObject, simulationScene);
        }
    }

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] int maxPhysicsFrameIteration = 30;

    public void SimulateTrajectory(BallTestNew ballPrefab, Vector2 pos, Vector2 velocity)
    {
        var ghostObject = Instantiate(ballPrefab, pos, Quaternion.identity);

        ghostObject.Push(velocity, false);

        lineRenderer.positionCount = maxPhysicsFrameIteration;

        for (int i = 0; i < maxPhysicsFrameIteration; i++)
        {
            physicsScene2D.Simulate(Time.fixedDeltaTime);
            lineRenderer.SetPosition(i, ghostObject.transform.position);
        }
    }
}
 