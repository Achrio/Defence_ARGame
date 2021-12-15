using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {
    public GameManager game;
    public Transform WorldCenter;
    public GameObject enemy;

    [Header("Spawn Location")]
    public List<Transform> spawnPoint;

    [Header("Enemy Prefab")]
    public GameObject[] enemyPrefab = new GameObject[3];

    //Enemies Spawn List
    private List<GameObject> enemyList;

    [Header("Enemies Bonus")]
    public int bonusHP = 0;
    public float bonusSpeed = 0f;
    public float delay = 2f;
    float timer = 0f;

    void Awake() {
        //game = GameObject.Find("GameManger").GetComponent<GameManager>();
    }

    void Update() {
        if(game.onWave) {
            timer -= Time.deltaTime;
            if(timer <= 0f) {
                SpawnEnemy();
                timer = delay;
            } 
        }
    }

    public void WaveStart() {
        //bonus stats set
        bonusHP = game.progress * 10;
        bonusSpeed = game.progress * 0.01f;
        delay = 1f - (game.progress * 0.025f);

        //limits to bonus stats
        if(delay < 1f) delay = 1f;
        if(bonusSpeed > 0.5f) bonusSpeed = 0.5f;
    }

    void SpawnEnemy() {
        int code = Random.Range(0, 3);  //enemy code 0 ~ 2
        int point = Random.Range(0, 8); //spawn point 0 ~ 7

        //set position
        enemy = Instantiate(enemyPrefab[code]) as GameObject;
        enemy.transform.SetParent(WorldCenter, false);
        enemy.transform.position = spawnPoint[point].position;

        //bonus stats
        Enemy enemyStatus = enemy.GetComponent<Enemy>();
        enemyStatus.maxHP += bonusHP;
        enemyStatus.curHP += bonusHP;
        enemyStatus.speed += bonusSpeed;

        game.enemyCount++;
    }

    public void WaveEnd() {
        timer = delay;
    }
}
