using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSystemGeneric : MonoBehaviour
{
    public static PoolSystemGeneric Singleton;

    private void Awake()
    {
        if (Singleton != null)
        {
            Debug.LogWarning("moreThanOneInstance");
            DestroyImmediate(Singleton);
        }
        Singleton = this;
    }

    public List<PoolItem> poolItems;
    public List<PoolHelper> T_Item;
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

            // AddObjectToPooledObject(item.prefab, item.size, item.parent, item.tag);
        }
    }

    public T AddObjectToPooledObject<T>(T prefab, int size, Transform transformParent = null, string newTag = null) where T : Component
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
                GameObject obj = Instantiate(prefab.gameObject);
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


    public T SpawnFromPool<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null, bool useLocalTransform = false) where T : Component
    {
        if (!poolDict.ContainsKey(prefab.name))
        {
            Debug.LogWarning("Pool with tag " + prefab.name + " doesn't exist.");
            return prefab;
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
        return prefab;
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

    private T[] CreateArray<T>(T first, T second)
    {

        return new T[] { first, second };
    }
}



