using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaction : MonoBehaviour {

    public virtual void PerformInteraction()
    {
        Debug.Log("Interacting with " + name);
    }
}
