using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Manager : MonoBehaviour
{
    public static Interactable_Manager item_manager;
    public Interactable[] interactables;
    public string[] interactableNames;

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
        interactables = new Interactable[interactableNames.Length];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
