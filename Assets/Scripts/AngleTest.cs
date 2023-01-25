using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTest : MonoBehaviour
{
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(target.transform.position, this.transform.position);
        Debug.DrawRay(this.transform.position, target.transform.forward *  100, Color.yellow);
        Vector3 targetDir = target.transform.position - transform.position;
        Debug.Log(Vector3.Angle(targetDir, this.transform.forward));

    }
}
