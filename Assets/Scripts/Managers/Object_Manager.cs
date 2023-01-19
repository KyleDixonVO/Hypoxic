using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Manager : MonoBehaviour
{
    public static Object_Manager object_Manager;
    [SerializeField] List<Object> objects = new List<Object>();
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
        if (FirstPersonController_Sam.fpsSam == null || objects.Count == 0) return;
    }

    public bool WithinPickupRange()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].GetType() != typeof(HeavyObject)) continue;
            if ((HeavyObject)objects[i].WithinPickupRange()) return true;
            return false;
        }
    }
}
