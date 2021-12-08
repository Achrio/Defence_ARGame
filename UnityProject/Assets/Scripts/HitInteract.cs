using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스킬 피격시 (스킬 collider와 충돌) 반응 스크립트
//플레이어와 적 공용

public class HitInteract : MonoBehaviour {
    SkillInfo hitSkill;         //피격된 스킬 정보
    GameObject hitObject;       //피격된 오브젝트
    PlayerStatus playerStatus; 
    EnemyStatus enemyStatus;
    GameManager game;
 
    void Start() {
        game = GameObject.Find("GameManager").GetComponent<GameManager>();
        hitObject = transform.parent.gameObject;
        if(hitObject.tag == "Player")
            playerStatus = hitObject.GetComponent<PlayerStatus>();
        if(hitObject.tag == "Enemy")
            enemyStatus = hitObject.GetComponent<EnemyStatus>();
    }

    void Update() {
        
    }

    void OnTriggerEnter(Collider col) {
        //플레이어가 적 스킬에 충돌할 때
        //(스킬 삭제는 스킬 스크립트 내에서)
        if(col.tag == "EnemySkill" && hitObject.tag == "Player") {

            //플레이어가 무적이 아닐 때 HP 감소
            if(playerStatus.isHit == 0) {
                playerStatus.isHit = 1;
                hitSkill = col.gameObject.transform.parent.GetComponent<SkillInfo>();
                playerStatus.curHP -= hitSkill.damage;
            }
        }

        //타워가 적 스킬에 충돌할 때
        //(스킬 삭제는 스킬 스크립트 내에서)
        if(col.tag == "EnemySkill" && hitObject.tag == "Tower") {

            //플레이어가 무적이 아닐 때 HP 감소
            if(playerStatus.isHit == 0) {
                playerStatus.isHit = 1;
                hitSkill = col.gameObject.transform.parent.GetComponent<SkillInfo>();
                playerStatus.curHP -= hitSkill.damage;
            }
        }

        //적이 플레이어 스킬에 충돌할 때
        //(스킬 삭제는 스킬 스크립트 내에서)
        if(col.tag == "PlayerSkill" && hitObject.tag == "Enemy") {
            Debug.Log("On hit");
            hitSkill = col.gameObject.transform.parent.GetComponent<SkillInfo>();
            enemyStatus.hp -= hitSkill.damage;
            
            //피격 이펙트 표시
            GameObject hitEffect = Instantiate(hitSkill.hitEffect);
            hitEffect.transform.parent = this.transform.parent.transform.Find("EnemyRot");
            hitEffect.transform.localPosition = new Vector3(0, 0.2f, -0.01f);
            hitEffect.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            Destroy(hitEffect, 0.2f);
        }

        //아이템이 플레이어에 충돌할 때
        if(col.tag == "Item" && hitObject.tag == "Player") {
            Debug.Log("Item Get");
            FieldItem itemInfo = col.gameObject.transform.parent.GetComponent<FieldItem>();
            playerStatus.item.Add(itemInfo.itemCode);
            Destroy(col.gameObject);
        }
    }
}
