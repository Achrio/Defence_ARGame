using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {
    public int charCode = 0; //UI 선택을 통해 지정
    
    //플레이어 스텟 관련 변수
    public int curHP = 100;       //현재 체력
    public int maxHP = 100;       //최대 체력
    public float nohitTime = 0.2F; //피격시 무적 시간

    public int action; //행동 확인
    public int isHit;  //피격 확인
    float timeCount;   //피격시 nohitTime 재는 타이머용

    public List<int> item; //보유 중인 아이템

    void Awake() {
        initStatus(); //플레이어 코드에 따라 스텟 초기화
    }

    void Update() {
        //체력이 0 -> 플레이어 오브젝트 삭제
        if(curHP <= 0) Destroy(this);

        //피격시 무적 -> 설정한 무적시간 동안
        if(isHit == 1) {
            timeCount += Time.deltaTime;
            if(timeCount >= nohitTime) {
                timeCount = 0;
                isHit = 0;
            }
        }
    }

    void initStatus() { //캐릭터별로 다른 능력치
        switch (charCode) {
            case 0 :
                curHP = 100;
                maxHP = 100;
                nohitTime = 0.2F;
                break;
        }
    }
}
