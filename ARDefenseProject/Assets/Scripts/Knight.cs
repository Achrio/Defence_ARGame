using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Knight : MonoBehaviour {
    private float time;
    public Transform target;            //attack target
    public TextMeshProUGUI username;

    [Header("Status Setup")]
    public GameObject attack;           //normal attack
    public float delay = 1f;          //normal attack delay
    public float range = 2f;            //target find range
    public float damage = 10f;          //attack damage

    void Start() {
        Destroy(gameObject, 60f);
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void Update() {
        //rotate to target
        transform.LookAt(target);

        //normal attack every delay
        if(time <= 0f) {
            time = delay;
            Attack();
        }
        time -= Time.deltaTime;
    }

    //타겟 인식
    void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDist = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);

            //nearest enemy to tower first
            if(distanceToEnemy < shortestDist) {
                shortestDist = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        //nearest enemy is in range -> tower
        if(shortestDist <= range) {
            target = nearestEnemy.transform;
        }
        else {
            target = null;
        }
    }

    //normal attack
    void Attack() {
        GameObject hitbox = Instantiate(attack, 
            transform.position, transform.rotation);
        HitBox hit = hitbox.GetComponent<HitBox>();

        if(hit != null) {
            hit.damage = damage;
        }
    }
}
