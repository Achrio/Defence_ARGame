using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private PluginActivity plugin;
    public EnemySpawnManager enemySpawn;

    [Header("UI")]
    public Text progressText;
    public GameObject timerUI;
    public Text timerText;
    public GameObject costUI;
    public Text costText;
    public GameObject onPause;
    public GameObject GameOverUI;
    public Text lastText;
    public GameObject upgradeUI;
    public GameObject notifyUI;
    public Text remainText;
    public GameObject logoutButton;

    [Header("Values")]
    public bool onWave = false;
    public bool onUpgrade = true;

    public int progress = 1;
    public float timer;

    public int health = 10;
    public float cost;
    public float costGain = 2f;

    public int enemyCount = 0;

    private bool isPause = false;

    void Awake() {
        plugin = GameObject.Find("UnityActivity").GetComponent<PluginActivity>();
    }

    void Update() {
        if(onWave) {
            timeUpdate();
            costUpdate();
        }
    }

    public void WaveStart() {
        onUpgrade = false;

        if(plugin) plugin.SendProgress(progress.ToString());
        progressText.text = "WAVE " + progress.ToString();
        onWave = true;
        enemySpawn.WaveStart();

        timerUI.SetActive(true);
        costUI.SetActive(true);
        upgradeUI.SetActive(false);

        timer = 60f;
        cost = 0f;
    }
    void timeUpdate() {
        timer -= Time.deltaTime;
        timerText.text = timer.ToString("F0");

        if(timer < 0f) WaveEnd();
    }
    void costUpdate() {
        cost += Time.deltaTime * costGain;
        costText.text = cost.ToString("F0");
    }

    public void updateRemain() {
        remainText.text = enemyCount.ToString();
    }

    //timer end
    void WaveEnd() {
        timerUI.SetActive(false);
        notifyUI.SetActive(true);
        progress++;
        onWave = false;
        enemySpawn.WaveEnd();
    }

    //remain = 0
    public void SetUpgrade() {
        notifyUI.SetActive(false);
        costUI.SetActive(false);
        upgradeUI.SetActive(true);
        onUpgrade = true;
    }

    public void Pause() {
        Time.timeScale = 0;
        onPause.SetActive(true);
    }
    public void Resume() {
        Time.timeScale = 1;
        onPause.SetActive(false);
    }

    public void GameOver() {
        Time.timeScale = 0;
        GameOverUI.SetActive(true);
        logoutButton.SetActive(true);
        lastText.text = progress.ToString();
    }

    public void ReturnLogin() {
        Destroy(plugin.gameObject);
        SceneManager.LoadScene("LoginScene");
    }

}
