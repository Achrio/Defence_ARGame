using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//터렛 기본 공격
public class Bullet : MonoBehaviour {
    [Header("Floating GameObjects")]
    public Transform target;
    private Enemy enemyStatus;

    [Header("Bullet Setup")]
    public float speed = 1.0f;   //투사체 속도
    public float damage;         //투사체 데미지
    public GameObject hitEffect; //투사체 피격 이펙트

    ParticleSystem particle;

    //타겟 설정 (터렛에서 받아옴)
    public void Seek(Transform _target) {
        target = _target;
    }

    void Update() {
        if(target == null) { Destroy(gameObject); return; }

        //자동으로 적에게 이동
        this.transform.position = Vector3.MoveTowards(
            this.transform.position, target.transform.position , Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider enemy) {
        if(enemy.tag == "Enemy") {
            //적 피격
            enemyStatus = enemy.gameObject.GetComponent<Enemy>();
            enemyStatus.curHP -= damage;

            //적 피격 이펙트
            GameObject effect = Instantiate(hitEffect, transform.position, transform.rotation);
            particle = effect.GetComponent<ParticleSystem>();
            Destroy(effect, 1f);
            Destroy(this.gameObject);
            particle.Clear();
        }
    }
}
