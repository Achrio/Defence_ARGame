using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPUI : MonoBehaviour {
    public GameObject enemy;
    public Image hpBar;
    EnemyStatus status;

    int maxHP;
    int curHP;

    void Start() {
        status = enemy.GetComponent<EnemyStatus>();
        maxHP = status.hp;
    }

    void Update() {
        curHP = status.hp;
        hpBar.fillAmount = (float)curHP / (float)maxHP;
    }
}
