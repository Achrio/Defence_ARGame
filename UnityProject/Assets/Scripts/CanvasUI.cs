using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CanvasUI : MonoBehaviour {
    GameObject ARCam;

    void Start() {
        ARCam = GameObject.Find("ARCamera");    
    }

    void Update() {
        this.transform.rotation = ARCam.transform.rotation;
    }
}
