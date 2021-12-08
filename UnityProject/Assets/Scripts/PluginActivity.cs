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

    public int targetIndex;

    void Start() {
        _pluginActivity = new AndroidJavaObject("com.example.uactivity.PluginActivity");
    }

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    void Update() {

    }

    //솔로 플레이
    public void SoloPlay() {
        Destroy(this.gameObject);
        SceneManager.LoadScene("GameScene");
    }

    //로그인
    public void CallLogin() {
        if(_pluginActivity != null) {
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

    //점수 전송
    public void SendScore(string score) {
        if(_pluginActivity != null) {
            Send("11", score);
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

    //받은 라이벌 상황 ScrollView에 표시
    public void ResultRivalProgress(string progress) {
        if(_pluginActivity != null) {
            int index = FindUser(UserName);
            rivalList[index].text = UserName + " Wave " + progress;
        }
    }
    public void ResultRivalScore(string score) {
        if(_pluginActivity != null) {
            int index = FindUser(UserName);

            rivalList[index].text += " " + score + "\n";
        }
    }

    //토글 그룹 토클으로 선택한 상대 가져오기
    public void getTarget(string text) {
        string curTarget = text;
        targetIndex = FindUser(curTarget);
        Debug.Log(curTarget);
    }
}
