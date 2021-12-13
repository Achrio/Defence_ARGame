using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PluginActivity : MonoBehaviour {
    AndroidJavaObject _pluginActivity;
    string UserName;

    //로그인 관련
    public InputField username;
    public string myName;
    public InputField ip_addr;
    public InputField port;
    public bool isLogin = false;

    //채팅 관련
    public InputField chatInput;
    public Scrollbar chatScroll;
    public Text chatText;
    public Button chatButton;
    public GameObject chatCanvas;

    //상대 관련
    public GameObject rivalViewcontents;
    public Text rivalText;
    public List<Text> rivalList;
    public List<string> rivalName;

    public GameObject rivalCanvas;
    public int userNum;

    public GameObject[] knight = new GameObject[2];

    //리더보드
    public GameObject boardUI;
    public Text boardText;


    void Start() {
        _pluginActivity = new AndroidJavaObject("com.example.uactivity.PluginActivity");
    }

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Update() {

    }

    //상대 기사 좌표
    private int knightcode;
    private float knightx;
    private float knighty;
    private float knightz;
    private int knightlevel;

    //로그인
    public void CallLogin() {
        if(_pluginActivity != null) {
            myName = username.text;
            _pluginActivity.Call("Login", username.text, ip_addr.text, port.text);

            chatCanvas.SetActive(true);
            rivalCanvas.SetActive(true);
            isLogin = true;
            SceneManager.LoadScene("GameScene");
        }
    }

    public void SaveSender(string username) {
        UserName = username;
    }
    public void Send(string code, string data) {
        if(_pluginActivity != null) {
            _pluginActivity.Call("Send", code, data);
        }
    }
    public int FindUser(string username) {
        if(_pluginActivity != null) {
            for(int i = 0; i < userNum; i++) {
                if(rivalName[i].Equals(username)) {
                    return i;
                }
            }
        }
        return -1;
    }

    //로그인, 로그아웃 채팅창에 표시
    public void UserLog(string data) {
        if(_pluginActivity != null) {
            chatText.text += UserName + " " + data + "\n";
            chatScroll.value = 0;
        }
    }
    //새로운 유저용 정보 표시창 생성
    public void NewUser(string username) {
        if(_pluginActivity != null) {
            rivalName.Add(username);

            Text newUser = Instantiate<Text>(rivalText);
            newUser.transform.SetParent(rivalViewcontents.transform, false);
            rivalList.Add(newUser);

            userNum++;

            //이렇게 하면 둘은 같은 인덱스를 가진다 =>
            //닉네임으로 rivalName 인덱스 n을 찾아서
            //rivalText 인덱스 n에 업데이트한다
        }
    }
    //유저 로그아웃시 정보 삭제
    public void DeleteUser(string username) {
        if(_pluginActivity != null) {
            int index = FindUser(username);
            rivalName.RemoveAt(index);
            Destroy(rivalList[index]);
            rivalList.RemoveAt(index);
            userNum--;
        }
    }

    //진행상황 전송
    public void SendProgress(string progress) {
        if(_pluginActivity != null) {
            Send("10", progress);
        }
    }

    //채팅 전송
    public void SendChat() {
        if(chatInput.text != "") {
            Send("100", chatInput.text);
            chatInput.text = "";
        }
    }

    //받은 채팅 메시지 ScrollView에 표시
    public void ResultChat(string chat) {
        if(_pluginActivity != null) {
            chatText.text += UserName + " : " + chat + "\n";
            chatScroll.value = 0;
        }
    }

    //받은 다른 플레이어 상황 ScrollView에 표시
    public void ResultRivalProgress(string progress) {
        if(_pluginActivity != null) {
            int index = FindUser(UserName);
            rivalList[index].text = UserName + " Wave " + progress;
        }
    }

    //기사 관련
    public void SendKnight(int type, float x, float y, float z, int level) {
        Send("20", type.ToString());
        Send("21", x.ToString());
        Send("22", y.ToString());
        Send("23", z.ToString());
        Send("24", level.ToString());
    }

    public void KnightType(string type) {
        knightcode = int.Parse(type);
    }
    public void KnightX(string x) {
        knightx = float.Parse(x);
    }
    public void KnightY(string y) {
        knighty = float.Parse(y);
    }
    public void KnightZ(string z) {
        knightz = float.Parse(z);
    }
    public void KnightLevel(string level) {
        knightlevel = int.Parse(level);
    }

    public void SummonKnight(string data) {
        GameObject knights = Instantiate(knight[knightcode]);
        knights.transform.position = new Vector3(knightx, knighty, knightz);
        Knight status = knights.GetComponent<Knight>();
        status.username.text = UserName;
        status.damage = knightlevel * 30;
    }
}
