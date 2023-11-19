using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefabs;

    [SerializeField] private int poolSize;

    [SerializeField] private bool expandable;

    private List<GameObject> freeList;
    private List<GameObject> usedList;

    private void Awake()
    {
        freeList = new List<GameObject>();
        usedList = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GenerateNewObject();
        }
    }

    // get the object from pool
    public GameObject GetObject()
    {
        int totalFree = freeList.Count;
        if (totalFree == null && !expandable) { return null; }
        else if (totalFree == 0) GenerateNewObject();

        GameObject poolObject = freeList[totalFree - 1];
        freeList.RemoveAt(totalFree - 1);
        usedList.Add(poolObject);

        return poolObject;
    }

    // Return an object to the pool
    public void ReturnObject(GameObject obj)
    {
        Debug.Assert(usedList.Contains(obj));
        obj.SetActive(false);
        usedList.Remove(obj);
        freeList.Add(obj);
    }

    //Instantiate the object
    private void GenerateNewObject()
    {
        GameObject poolObject = Instantiate(objectPrefabs);
        poolObject.transform.parent = transform;
        poolObject.SetActive(false);
        freeList.Add(poolObject);
    }
}