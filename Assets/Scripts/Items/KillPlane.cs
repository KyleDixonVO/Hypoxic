using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KillPlane : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Abosolutely obliterate the player on contact
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Entered KillPlane");
            PlayerStats.playerStats.TakeDamage(10000);
        }
    }
}
