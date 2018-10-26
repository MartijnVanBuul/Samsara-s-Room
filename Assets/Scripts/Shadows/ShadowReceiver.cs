using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShadowReceiver : MonoBehaviour {
    void Start()
    {
        GetComponent<SpriteRenderer>().receiveShadows = true;
    }
}
