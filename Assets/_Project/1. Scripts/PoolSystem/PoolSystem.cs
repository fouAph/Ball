using System.Collections.Generic;
using UnityEngine;

public class PoolSystem : MonoBehaviour
{
    public static PoolSystem Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("moreThanOneInstance");
            DestroyImmediate(Instance);
        }
        Instance = this;
    }

    public List<PoolItem> poolItems;
    public Dictionary<string, Queue<GameObject>> poolDict;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolItem item in poolItems)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            AddObjectToPooledObject(item.prefab, item.size, item.parent, item.tag);
        }
    }

    public GameObject AddObjectToPooledObject(GameObject prefab, int size, Transform transformParent = null, string newTag = null)
    {
        if (newTag == null)
        {
            newTag = prefab.name;
        }
        if (!poolDict.ContainsKey(newTag))
        {
            Queue<GameObject> objPool = new Queue<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject obj = Instantiate(prefab);
                if (transformParent)
                {
                    obj.transform.SetParent(transformParent);
                    // obj.transform.localPosition = Vector3.zero;
                }

                else
                    obj.transform.SetParent(transform);

                obj.SetActive(false);
                objPool.Enqueue(obj);

            }

            poolDict.Add(newTag, objPool);
            // print($"{prefab.name} has been added to the Poolsystem Collection");
            return prefab;
        }

        else return null;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent = null, bool useLocalTransform = false)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDict[tag].Dequeue();
        if (parent)
        {
            objectToSpawn.transform.SetParent(parent);
        }
        else
        {
            objectToSpawn.transform.SetParent(transform.parent);
        }

        objectToSpawn.SetActive(true);
        if (!useLocalTransform)
        {
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }
        else
        {
            objectToSpawn.transform.localPosition = position;
            objectToSpawn.transform.localRotation = rotation;
        }

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        poolDict[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public GameObject SpawnFromPool(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool useLocalTransform = false)
    {
        if (!poolDict.ContainsKey(prefab.name))
        {
            Debug.LogWarning("Pool with tag " + prefab.name + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDict[prefab.name].Dequeue();

        if (parent)
            objectToSpawn.transform.SetParent(parent);

        else
            objectToSpawn.transform.SetParent(transform.parent);

        if (!useLocalTransform)
        {
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
        }
        else
        {
            objectToSpawn.transform.localPosition = position;
            objectToSpawn.transform.localRotation = rotation;
        }

        objectToSpawn.SetActive(true);
        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        poolDict[prefab.name].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}

[System.Serializable]
public class PoolItem
{
    public string tag;
    public GameObject prefab;
    public int size;
    public Transform parent;
}
