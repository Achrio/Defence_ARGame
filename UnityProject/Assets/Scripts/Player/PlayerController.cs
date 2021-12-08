using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {
    public Transform Player;        // 플레이어
    public Transform Direction;     // 바라보는 방향
    public Transform Stick;         // 조이스틱

    public Animator PlayerSprite;   //플레이어 스프라이트 애니메이션
    public GameObject Skill01;       //스킬1 히트박스

    public float speed = 1f;
 
    // 비공개
    private Vector3 StickFirstPos;  // 조이스틱의 처음 위치
    private Vector3 JoyVec;         // 조이스틱의 벡터(방향)
    private float Radius;           // 조이스틱 배경의 반지름
    private bool MoveFlag;          // 플레이어 움직임 스위치
    private bool AttackFlag;        // 플레이어 공격 스위치
    private float rad;

 
    void Start()
    {
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        StickFirstPos = Stick.transform.position;
 
        // 캔버스 크기에대한 반지름 조절
        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= Can;
 
        MoveFlag = false;
    }
 
    void Update()
    {
        if (MoveFlag && !AttackFlag) {
            Player.transform.Translate(Vector3.forward * Time.deltaTime * speed);
            PlayerSprite.SetBool("isWalk", true);
        }

        if(!MoveFlag) {
            PlayerSprite.SetBool("isWalk", false);
        }
    }
 
    // 드래그
    public void Drag(BaseEventData _Data)
    {
        MoveFlag = true;
        PointerEventData Data = _Data as PointerEventData;
        Vector3 Pos = Data.position;
        
        // 조이스틱을 이동시킬 방향을 구함.(오른쪽,왼쪽,위,아래)
        JoyVec = (Pos - StickFirstPos).normalized;
 
        // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
        float Dis = Vector3.Distance(Pos, StickFirstPos);
        
        // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는 곳으로 이동.
        if (Dis < Radius)
            Stick.position = StickFirstPos + JoyVec * Dis;
        // 거리가 반지름보다 커지면 조이스틱을 반지름의 크기만큼만 이동.
        else
            Stick.position = StickFirstPos + JoyVec * Radius;
        
        rad = Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg;
        Player.eulerAngles = new Vector3(0, rad, 0);
        if(rad > 0 && rad < 180) Direction.eulerAngles = new Vector3(0, 180, 0);
        if(rad < 0 && rad > -180) Direction.eulerAngles = new Vector3(0, 0, 0);
    }
 
    // 드래그 끝.
    public void DragEnd()
    {
        Stick.position = StickFirstPos; // 스틱을 원래의 위치로.
        JoyVec = Vector3.zero;          // 방향을 0으로.
        MoveFlag = false;
    }


    // 공격 종료
    public void AttackEnd() {
        AttackFlag = false;
        PlayerSprite.SetInteger("isAttack", 0);
    }

    // 일반 공격
    public void Attack01() {
        if(!AttackFlag) {
            GameObject skill = Instantiate(Skill01);
            float time = skill.GetComponent<SkillInfo>().effectFrame;
            skill.transform.SetParent(GameObject.Find("PlayerDirection").transform, false);
            PlayerSprite.SetInteger("isAttack", 1);
            AttackFlag = true;
            Invoke("AttackEnd", time);
        }
    }
}
