using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour{
    void Start() {
        
    }

    void Update() {
        this.gameObject.transform.Rotate(new Vector3(0, 0.3f, 0));

    }
}
