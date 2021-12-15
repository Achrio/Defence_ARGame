using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour {
    public GameManager game;

    public GameObject[] turretObject = new GameObject[4];
    private Turret[] turret = new Turret[4];

    //UI
    public Button[] skillButton = new Button[4];
    public Text[] levelText = new Text[4];

    //Turret Status
    private int[] level = new int[4];

    void Awake() {
        for(int i = 0; i < 4; i++) {
            turret[i] = turretObject[i].GetComponent<Turret>();
            level[i] = 1;
        }
    }

    void Update() {
    }

    public void onUpgrade(int index) {
        if(game.onUpgrade) {
            level[index]++;
            turret[index].damage += 10f;
        
            levelText[index].text = "Lv." + level[index].ToString();

            game.WaveStart();
        }
    }
}
