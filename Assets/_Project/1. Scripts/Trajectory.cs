using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] int dotsNumber;
    [SerializeField] Transform dotsParent;
    [SerializeField] Transform dotPrefab;
    [SerializeField] float dotSpacing;

    [SerializeField, Range(0.01f, 0.3f)] float dotMinScale = 0.178f;
    [SerializeField, Range(0.3f, 1f)] float dotMaxScale = 0.383f;


    private Transform[] dotsList;
    private Vector2 pos;
    private float timeStamp;

    private void Start()
    {
        Hide();
        PrepareDots();
    }

    private void PrepareDots()
    {
        dotsList = new Transform[dotsNumber];
        dotPrefab.localScale = Vector3.one * dotMaxScale;
        float scale = dotMaxScale;
        float scaleFactor = scale / dotsNumber;

        for (int i = 0; i < dotsList.Length; i++)
        {
            dotsList[i] = Instantiate(dotPrefab, null).transform;
            dotsList[i].parent = dotsParent;

            dotsList[i].localScale = Vector3.one * scale;
            if (scale > dotMinScale)
                scale -= scaleFactor;
        }

    }

    public void UpdateDots(Vector3 ballPos, Vector2 forceApplied)
    {
        timeStamp = dotSpacing;
        for (int i = 0; i < dotsNumber; i++)
        {
            pos.x = (ballPos.x + forceApplied.x * timeStamp);
            pos.y = (ballPos.y + forceApplied.y * timeStamp) - (Physics2D.gravity.magnitude * timeStamp * timeStamp) / 2f;

            dotsList[i].position = pos;
            timeStamp += dotSpacing;
        }

    }

    public void Show()
    {
        dotsParent.gameObject.SetActive(true);
    }

    public void Hide()
    {
        dotsParent.gameObject.SetActive(false);
    }
}