using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public int progress; //진행한 레벨 수
    public int enemyCount; //적 count -> if 0, nextStageFlag = 1
    public int nextLevelFlag; //다음 레벨 진입 플래그

    public float speed; //적 속도
    public float delay; //공격 속도
   
    public GameObject LevelManager; //LevelManager 프리팹
    public Text Wave;               //진행한 웨이브 수 표시

    private GameObject Plugin;
    private PluginActivity PluginScript;

    public bool isLogin;
    
    void Start() { //게임 시작 초기값
        progress = 0;
        enemyCount = 0;
        nextLevelFlag = 1;

        speed = 1f;
        delay = 1f;

        Plugin = GameObject.Find("UnityActivity");
        PluginScript = Plugin.GetComponent<PluginActivity>();
    }

    void Update() {
        //적이 0명이 되면 다음 웨이브로
        if(enemyCount == 0 && nextLevelFlag == 0) {
            nextLevelFlag = 1;
        }

        if(nextLevelFlag == 1) {
            //playerPos.transform.localPosition = new Vector3(0, 0, 0);
            nextLevelFlag = 0;
            progress++; //진행한 레벨 증가
            Wave.text = "WAVE " + progress.ToString();
            PluginScript.SendProgress(progress.ToString());
            GameObject level = Instantiate(LevelManager); //레벨 생성
            Debug.Log("Made Level");
        }
    }
}