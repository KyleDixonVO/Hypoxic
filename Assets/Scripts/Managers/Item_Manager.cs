using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Manager : MonoBehaviour
{
    public static Item_Manager item_manager;

    private void Awake()
    {
        if (item_manager == null)
        {
            item_manager = this;
            DontDestroyOnLoad(this);
        }
        else if (item_manager != null && item_manager != this)
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
        
    }
}
