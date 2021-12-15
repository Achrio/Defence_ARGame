using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    private float time;
    private GameObject tower;

    [Header("Floating GameObjects")]
    public Transform target;         //attack target
    private Vector3 targetPosition;  //target position (not to rotate in x or z)
    public ItemManager item;         //to transport target axis

    [Header("Status Setup")]
    public GameObject attack;           //normal attack
    public float delay = 0.5f;          //normal attack delay
    public float range = 2f;            //target find range
    public float damage = 10f;          //attack damage

    //타겟 인식
    void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDist = Mathf.Infinity;
        float shortestDistToTower = Mathf.Infinity;
        GameObject nearestEnemy = null;
        GameObject nearestEnemyToTower = null;

        foreach(GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(this.transform.position, enemy.transform.position);
            float distanceToTower = Vector3.Distance(tower.transform.position, enemy.transform.position);

            //nearest enemy to tower first
            if(distanceToEnemy < shortestDist) {
                shortestDist = distanceToEnemy;
                nearestEnemy = enemy;
            }
            if(distanceToTower < shortestDistToTower) {
                shortestDistToTower = distanceToTower;
                nearestEnemyToTower = enemy;
                item.nearest = enemy.transform.position;
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

    void Start() {
        tower = GameObject.FindWithTag("Tower");
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void Update() {
        if(target == null) return;

        //rotate to target
        targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetPosition);

        //normal attack every delay
        if(time <= 0f) {
            time = delay;
            Attack();
        }
        time -= Time.deltaTime;
    }

    //normal attack
    void Attack() {
        GameObject bulletObj = Instantiate(attack, 
            new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), transform.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();

        if(bullet != null) {
            bullet.Seek(target);
            bullet.damage = damage;
        }
    }
}