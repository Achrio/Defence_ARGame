using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {
    private PluginActivity plugin;
    public GameManager game;
    private Knight knight;

    public Vector3 nearest = new Vector3(1f, 1f, 1f);

    public GameObject[] itemEffect = new GameObject[2];

    //UI
    public Button[] itemButton = new Button[2];
    public Text[] levelText = new Text[2];
    public Text[] costText = new Text[2];

    //Item Status
    private int[] level = new int[2];
    private float[] cost = new float[2];

    //Item CoolDown
    private float[] cooldown = new float[2];
    private bool[] onCooldown = new bool[2];

    void Awake() {
        plugin = GameObject.Find("UnityActivity").GetComponent<PluginActivity>();

        for(int i = 0; i < 2; i++) {
            level[i] = 1;
            onCooldown[i] = false;
        }
        cost[0] = 20f;
        cost[1] = 40f;
    }

    void Update() {
        for(int i = 0; i < 2; i++) {
            if(onCooldown[i]) {
                cooldown[i] -= Time.deltaTime;
                costText[i].text = cooldown[i].ToString("F1");

                if(cooldown[i] <= 0f) {
                    costText[i].text = cost[i].ToString("F0");
                    onCooldown[i] = false;
                    itemButton[i].interactable = true;
                }
            }
        }

        
    }

    public void onUpgrade(int index) {
        if(game.onUpgrade) {
            if(index == 0 && cost[index] >= 10f) {
                cost[index] -= 5f;
            }
            if(index == 1 && cost[index] >= 25f) {
                cost[index] -= 5f;
            }
            level[index]++;

            costText[index].text = cost[index].ToString();
            levelText[index].text = "Lv." + level[index].ToString();

            game.WaveStart();
            Debug.Log("Upgrade");
        }
    }

    public void onItem(int index) {
        if(!game.onUpgrade) {
            if(game.cost >= cost[index]) {
                game.cost -= cost[index];
                GameObject item = Instantiate(itemEffect[index]);
                knight = item.GetComponent<Knight>();
                item.transform.position = nearest;
                knight.damage = level[index] * 30;
                
                if(plugin) {
                knight.username.text = plugin.myName;

                plugin.SendKnight(index, item.transform.position.x, 
                    item.transform.position.y, item.transform.position.z, level[index]);
                }
            }
            onCooldown[index] = true;
            cooldown[index] = 3.0f;
            itemButton[index].interactable = false;
            
        }
    }
}
