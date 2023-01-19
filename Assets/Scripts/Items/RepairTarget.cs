using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairTarget : MonoBehaviour
{
    public GameObject brokenObject;
    public GameObject repairedObject;

    // Start is called before the first frame update
    void Start()
    {
        BrokenObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RepairedObject()
    {
        repairedObject.SetActive(true);
        brokenObject.SetActive(false);
    }

    public void BrokenObject()
    {
        repairedObject.SetActive(false);
        brokenObject.SetActive(true);
    }
}
