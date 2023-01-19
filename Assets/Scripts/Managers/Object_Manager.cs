using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Manager : MonoBehaviour
{
    public static Object_Manager object_Manager;
    [SerializeField] List<HeavyObject> heavyObjects = new List<HeavyObject>();
    [SerializeField] List<RepairTarget> repairTargets = new List<RepairTarget>();
    private void Awake()
    {
        if (object_Manager == null)
        {
            object_Manager = this;
            DontDestroyOnLoad(this);
        }
        else if (object_Manager != null && object_Manager != this)
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindHeavyObjects();
        if (FirstPersonController_Sam.fpsSam == null || heavyObjects.Count == 0) return;
        WithinPickupRange();
    }

    public bool WithinPickupRange()
    {
        for (int i = 0; i < heavyObjects.Count; i++)
        {
            if (heavyObjects[i].WithinPickupRange()) return true;   
        }
        return false;
    }

    public void FindHeavyObjects()
    {
        for (int i = 0; i < GameObject.FindObjectsOfType<HeavyObject>().Length; i++)
        {
            if (!heavyObjects.Contains(GameObject.FindObjectsOfType<HeavyObject>()[i]))
            {
                heavyObjects.Add(GameObject.FindObjectsOfType<HeavyObject>()[i]);
            }
        }
    }
}
