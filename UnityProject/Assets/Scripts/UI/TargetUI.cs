using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetUI : MonoBehaviour {
    public GameObject rivalText;
    RectTransform textTran;
    RectTransform thisTran;

    void Start() {
        textTran = rivalText.GetComponent<RectTransform>();
        thisTran = this.GetComponent<RectTransform>();
    }

    void Update() {
        thisTran.sizeDelta = new Vector2(textTran.sizeDelta.x, textTran.sizeDelta.y - 30);
    }
}
