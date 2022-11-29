using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType {Player, CollectibleCube}
public class PoolingManager : MonoBehaviour
{
    public List<PoolingObject> ObjectsWillBePooled = new List<PoolingObject>();

    public Transform PoolParent;


    public List<PooledObject> PooledObjects = new List<PooledObject>();
    void Awake()
    {
        SpawnPoolingObjects();
    }
    
    public GameObject ObtainFromPool(ObjectType type)
    {

        GameObject returnObject = null;

        for(int i=0; i<PooledObjects.Count; i++)
        {

            PooledObject obj = PooledObjects[i];

            if(obj.Type == type && !obj.Self.activeSelf)
            {
                returnObject = obj.Self;
                break;
            }
            
        }

        if(returnObject == null)
        {
            

            for(int i=0; i<ObjectsWillBePooled.Count; i++)
            {

                PoolingObject obj = ObjectsWillBePooled[i];

                if(obj.Type == type)
                {
                    returnObject = Instantiate(obj.Prefab, PoolParent);

                    if(returnObject != null)
                    {
                        AddToPool(returnObject, obj.Type);
                    }
                }

            }
        }

        return returnObject;

    }

    void AddToPool(GameObject poolObj, ObjectType type)
    {
        poolObj.SetActive(false);
        PooledObject data = new PooledObject(poolObj, type);

        PooledObjects.Add(data);
    }

    void SpawnPoolingObjects()
    {
        for(int i=0; i<ObjectsWillBePooled.Count; i++)
        {
            for(int k=0; k<ObjectsWillBePooled[i].StartQuantity; k++)
            {

                PoolingObject obj = ObjectsWillBePooled[i];

                GameObject temp = Instantiate(obj.Prefab, PoolParent);

                AddToPool(temp, obj.Type);
            }
        }
    }

    public void SendBack(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void CleanScene()
    {
        for(int i=0; i<PooledObjects.Count; i++)
        {
            PooledObjects[i].Self.SetActive(false);
        }
    }
    
}


[System.Serializable]
public class PoolingObject
{
    public GameObject Prefab;
    public ObjectType Type;
    public int StartQuantity;
}

public class PooledObject
{
    public GameObject Self;
    public ObjectType Type;

    public PooledObject(GameObject self, ObjectType type)
    {
        Self = self;
        Type = type;
    }
}
