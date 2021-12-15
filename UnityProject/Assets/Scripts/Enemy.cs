using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {
    [Header("Enemy Setup")]
    public float maxHP;    //monster maxHP
    public float speed;    //speed

    [Header("Floating Values")]
    public float curHP;     //current HP
    public float curSpeed;  //current Speed

    private GameObject tower;
    private GameObject game;
    private GameManager gameManage;

    public Image hpBar;

    void Awake() {
        tower = GameObject.FindWithTag("Tower");
        game = GameObject.FindWithTag("GameManager");
        gameManage = game.GetComponent<GameManager>();
        curHP = maxHP;
        curSpeed = speed;

        //Make Elite in 20% chance when spawn
        int elite = Random.Range(0, 101);
        if(elite >= 80) {
            maxHP *= 0.3f;
            curHP *= 0.3f;
            curSpeed /= 0.3f;
            gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }
    }

    void Update() {
        //Move to Tower
        this.transform.position = Vector3.MoveTowards(
            this.transform.position, tower.transform.position , Time.deltaTime * speed);

        this.transform.LookAt(tower.transform);

        hpBar.fillAmount = (float)curHP / (float)maxHP;

        //Destroy when HP = 0
        if(curHP <= 0) { 
            Destroy(this.gameObject);
            gameManage.enemyCount--;
            gameManage.updateRemain();
            if(gameManage.enemyCount == 0) gameManage.SetUpgrade();
        }
    }
}
