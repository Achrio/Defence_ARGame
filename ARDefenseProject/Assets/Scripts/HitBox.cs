using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    ParticleSystem particle;
    private Enemy enemyStatus;
    public GameObject hitEffect; //투사체 피격 이펙트
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
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
