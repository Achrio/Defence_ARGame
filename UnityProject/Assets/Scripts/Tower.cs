using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tower : MonoBehaviour {
    public GameManager game;
    public TextMeshProUGUI healthText;
    public GameObject warning;

    void Awake() {
        //game = GameObject.Find("GameManger").GetComponent<GameManager>();
    }

    void Update() {
        
    }

    private void OnTriggerEnter(Collider enemy) {
        if(enemy.tag == "Enemy") {
            game.health--;
            healthText.text = game.health.ToString();
            if(game.health == 0) {
                game.GameOver();
            }
            else {
                warning.SetActive(true);
                Invoke("warningDisable", 1f);
            }
        }
    }

    void warningDisable() {
        warning.SetActive(false);
    }
}