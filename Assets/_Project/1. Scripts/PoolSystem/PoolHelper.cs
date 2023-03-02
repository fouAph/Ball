using System.Collections;
using UnityEngine;

public class PoolHelper : MonoBehaviour, IPooledObject
{
    public bool useDeactive = true;
    public float deactiveAfter = .5f;

    public virtual void OnObjectSpawn()
    {
        if (useDeactive)
            StartCoroutine(HideObject());
    }

    IEnumerator HideObject()
    {
        yield return new WaitForSeconds(deactiveAfter);
        gameObject.SetActive(false);
    }
}
