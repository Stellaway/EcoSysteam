using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObjectManager : MonoBehaviour
{
    public static SmartObjectManager Instance { get; private set; } = null;

    public List<SmartObject> RegisteredObjects = new List<SmartObject>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log($"Trying to create a secong singleton {gameObject.name}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void RegisterSmartObject(SmartObject toRegister)
    {
        RegisteredObjects.Add(toRegister);
    }

    public void DeregisterSmartObject(SmartObject toDeregister)
    {
        RegisteredObjects.Remove(toDeregister);
    }


    public List<SmartObject> getSmartObjectsInRange(float range, Vector3 callerPostition)
    {
        var result = new List<SmartObject>();
        foreach (SmartObject obj in RegisteredObjects)
        {
            if((obj.transform.position - callerPostition).magnitude < range)
            {
                result.Add(obj);
            }
        }

        return result;
    }
}
