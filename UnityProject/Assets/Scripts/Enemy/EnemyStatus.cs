using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour {
    //적 스텟 관련 변수
    public int hp;                //체력
    public int enemyType = 1;     //일반, 엘리트, 보스
    public float localSpeed = 1f; //적 개인 속도
    public float worldSpeed;      //게임 전체 속도
    private float actualSpeed;

    private Transform target;
    private int wavepointIndex = 0;

    GameManager game;             //GameManager enemyCount, speed 접근


    public int action;    //행동 확인

    void Start() {
        GameObject gameManager = GameObject.Find("GameManager");
        game = gameManager.GetComponent<GameManager>();
    }

    void Update() {
        worldSpeed = game.speed;
        actualSpeed = localSpeed * worldSpeed;

        if(hp <= 0) {
            game.enemyCount--;        //적 사망시 GameManager enemyCount 감소
            Destroy(this.gameObject); //체력이 0이 되면 적 오브젝트 삭제
        }

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * actualSpeed * Time.deltaTime,Space.World);
    
        if(Vector3.Distance(transform.position,target.position)<=0.4f) {
            GetNextWaypoint();
        }
    }

    private void GetNextWaypoint() {
        if(wavepointIndex >= Waypoints.points.Length -1) {
            Destroy(gameObject);
            return;
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }
}
