using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPUI : MonoBehaviour {
    public GameObject player;
    public Image hpBar;
    PlayerStatus status;

    int maxHP; //최대 체력
    int curHP; //현재 체력

    void Start() {
        status = player.GetComponent<PlayerStatus>();
    }

    void Update() {
        curHP = status.curHP;
        maxHP = status.maxHP;
        hpBar.fillAmount = (float)curHP / (float)maxHP;
    }
}
