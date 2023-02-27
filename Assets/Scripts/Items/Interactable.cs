using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string typeName;
    public virtual void Awake()
    {
        gameObject.layer = 10;
    }
    
    public abstract void OnInteract(); 
    public abstract void OnFocus(); // can run the tool tip here and or apply a highlight effect to the item - Edmund
    public abstract void OnLoseFocus(); // can disable tool tip or effect on item - Edmund

}

