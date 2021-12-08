using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteControl : MonoBehaviour {
    public GameObject pos;

    void Start() {
        
    }

    void Update() {
        this.transform.position = new Vector3(pos.transform.position.x, 0, pos.transform.position.z);

    }
}
