using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string typeName;
    public bool singleton;

    [Header("save position")]
    public Vector3 savePos;
    public Vector3 initalPos;

    public void SetSaveGamePos()
    {
        savePos = this.gameObject.transform.position;
    }

    public void ReloadToSave()
    {
        this.gameObject.transform.position = savePos;
    }

    public virtual void Awake()
    {
        gameObject.layer = 10;
        //initalPos = transform.position;
    }

    public virtual void ResetForNewRun()
    {
        this.gameObject.SetActive(true);
        this.gameObject.transform.position = initalPos;
        gameObject.layer = 10;
    }
    
    public abstract void OnInteract(); 
    public abstract void OnFocus(); // can run the tool tip here and or apply a highlight effect to the item - Edmund
    public abstract void OnLoseFocus(); // can disable tool tip or effect on item - Edmund

}

