using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : MonoBehaviour
{
    public int damage;

    GameObject player;
    GameObject playerDirection;
    PlayerStatus playerStats;

    public GameObject hitboxPrefab; //피격 범위
    Collider hitbox;
    public GameObject hitEffect; //히트 이미지 프리팹

    float time;

    //스킬 수치
    public float multiplier; //enemy power * skill multiplier = skill damage 
    public float hitFrameStart;
    public float hitFrameEnd; //스킬 시작한 시점부터의 hit 판정 시간
    public float effectFrame; //스킬 effect 수명 = skill prefab 수명
    
    void Start() {
        player = GameObject.Find("Player00");
        playerDirection = GameObject.Find("PlayerDirection");
        playerStats = player.GetComponent<PlayerStatus>();
        hitbox = hitboxPrefab.GetComponent<Collider>();
        hitbox.enabled = false;
        Destroy(this.gameObject, effectFrame);
    }

    void Update() {
        time += Time.deltaTime;
        if(time > hitFrameStart) {
            if(hitbox != null) hitbox.enabled = true;
            Destroy(hitbox, hitFrameEnd - hitFrameStart);
        }
    }
}
