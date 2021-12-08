using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//레벨 생성
//1. 적 프리팹, 배경 프리팹 불러오기
//2. GameManager에 필요한 값 전달하기
//역할 끝나면 오브젝트 바로 삭제하기

public class LevelManager : MonoBehaviour {
    public float multiply = 0.05f; //스테이지 진행에 따른 체력 배수
    
    int world; //진행하고 있는 월드 -> 배경 프리팹, 나오는 몬스터 타입에 영향
    int progress; //진행한 레벨 수 <- GameManager

    int enemyCount; //적 갯수
    int[] enemyType; //적 유형

    public GameObject[] enemyPrefab = new GameObject[26]; //적 프리팹

    GameManager game;
    GameObject[] enemy; //생성 결정된 적 프리팹
    EnemyStatus[] enemyStatus; //생성 결정된 적 상태

    void Start() {

        //필요한 값 GameManager에서 넘겨받기
        GameObject gameManager = GameObject.Find("GameManager");
        game = gameManager.GetComponent<GameManager>();
        progress = game.progress;
        
        //적 유형 결정, 프리팹 생성
        if(progress % 10 == 0) { //10번째 레벨마다 보스 등장
            MakeBoss();
            game.enemyCount = 1; //GameManager에 생성된 보스 수 (1) 전달
        }
        else { //그 외에는 일반 적들
            MakeEnemy();
            game.enemyCount = enemyCount; //GameManager에 생성된 적 수 전달
        }
    }

    float LevelMultiplier() { //스테이지 진행에 따른 적 스텟 배율 계산
        return 1f + (float)(progress) * multiply;
    }

    void MakeEnemy() { //적 유형 결정, 프리팹 생성       
        enemyCount = Random.Range(4, 7); //적의 수 4~6마리
        enemy = new GameObject[enemyCount]; //생성된 적 프리팹 담아둘 배열 동적할당
        enemyType = new int[enemyCount]; //적 코드 담아둘 배열 동적할당
        enemyStatus = new EnemyStatus[enemyCount];

        switch(enemyCount) {
            case 4 : //일반 2~3 + 엘리트 1~2
            enemyType[0] = Random.Range(0, 2); //0~1 중 하나 생성
            enemyType[1] = Random.Range(0, 2); //0~1 중 하나 생성
            enemyType[2] = Random.Range(0, 4); //0~3 중 하나 생성
            enemyType[3] = Random.Range(2, 5); //2~4 중 하나 생성
            break;

            case 5 : //일반 3 + 엘리트 2
            enemyType[0] = Random.Range(0, 2); //0~1 중 하나 생성
            enemyType[1] = Random.Range(0, 2); //0~1 중 하나 생성
            enemyType[2] = Random.Range(0, 2); //0~1 중 하나 생성
            enemyType[3] = Random.Range(2, 4); //2~3 중 하나 생성
            enemyType[4] = Random.Range(2, 5); //2~4 중 하나 생성
            break;

            case 6 : //일반 4 + 엘리트 2
            enemyType[0] = Random.Range(0, 2); //0~1 중 하나 생성
            enemyType[1] = Random.Range(0, 2); //0~1 중 하나 생성
            enemyType[2] = Random.Range(0, 2); //0~1 중 하나 생성
            enemyType[3] = Random.Range(0, 2); //0~1 중 하나 생성
            enemyType[4] = Random.Range(2, 5); //2~4 중 하나 생성
            enemyType[5] = Random.Range(2, 5); //2~4 중 하나 생성
            break;
        }

        //월드, 코드에 맞는 적 프리팹 생성
        for(int i = 0; i < enemyCount; i++) {
            if(world > 4) 
                enemy[i] = Instantiate(enemyPrefab[enemyType[i] + (Random.Range(0, 3) * 10)]);
            else
                enemy[i] = Instantiate(enemyPrefab[enemyType[i] + ((world - 1) * 10)]);
            enemy[i].transform.SetParent(GameObject.Find("SizeModifier").transform, false);

            enemy[i].transform.Translate(-1.0f + ((2.0f / enemyCount) * i), 0, -0.5f);
            enemyStatus[i] = enemy[i].GetComponent<EnemyStatus>();

            //적 스텟에 스테이지 배율 반영
            enemyStatus[i].hp = (int)(enemyStatus[i].hp * LevelMultiplier());
        }
    }

    void MakeBoss() { //보스 생성

    }
}
