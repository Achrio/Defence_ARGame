/*
0. 이 PluginActivity.java 파일을 Android Library 파일(.aar)로 만들어서 Unity에 plugin으로 넣었습니다.
1. 유니티 오브젝트에서 C# 스크립트를 통해 이 PluginActivity를 실행하고
2. C# 스크립트에서 이 PluginActivity에 있는 함수들을 실행하게 할 수 있으며
2-1. UnitySendMessage를 통해 Unity 액티비티에 메시지를 전송하여
C# 스크립트에 작성된 함수를 실행하도록 할 수 있습니다.
단, 인수를 하나 밖에, string 타입으로 밖에 전달 못하므로 Username을 따로 전송하는 함수를 매번 호출했습니다.
*/

package com.example.uactivity;

import android.os.Bundle;
import android.widget.Toast;

import com.unity3d.player.UnityPlayer;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.SocketAddress;

public class PluginActivity extends UnityPlayerActivity {
    Socket socket = null;
    public ObjectInputStream ois;
    public ObjectOutputStream oos;
    public boolean LoginStatus = false;

    String username;
    String ip_addr = "192.168.219.105";
    String port_no = "30000";

    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Toast.makeText(UnityPlayer.currentActivity, "NetworkActivity Started", Toast.LENGTH_SHORT).show();
    }

    public void Login(String name, String ip, String port) {
        try {
            ip_addr = ip; port_no = port;
            SocketAddress addr = new InetSocketAddress(ip_addr, Integer.parseInt(port_no));
            socket = new Socket();
            socket.connect(addr, 1000);
            Toast.makeText(UnityPlayer.currentActivity, "Server Connected", Toast.LENGTH_SHORT).show();
            if(!socket.isConnected()) {
                Toast.makeText(UnityPlayer.currentActivity, "Login Failed", Toast.LENGTH_SHORT).show();
                return;
            }
            oos = new ObjectOutputStream(socket.getOutputStream());
            oos.flush();
            ois = new ObjectInputStream(socket.getInputStream());
            username = name;
            Send("0", "Login");
            LoginStatus = true;
            DoReceive();
        } catch (IOException e) {
            Toast.makeText(UnityPlayer.currentActivity, "Socket Connect Failed", Toast.LENGTH_SHORT).show();
            e.printStackTrace();
        }
    }
    synchronized public void Logout() {
        if(socket.isClosed()) return;
        Toast.makeText(UnityPlayer.currentActivity, "Logout", Toast.LENGTH_SHORT).show();

        try {
            oos.writeObject(username);
            oos.writeObject("1");
            oos.writeObject("Logout");
            socket.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
        LoginStatus = false;
    }

    public void DoReceive() {
        new Thread() {
            public void run() {
                while (true && LoginStatus) {
                    Read();
                    if (socket.isClosed()) return;
                }
            }
        }.start();
    }

    //인수로 받은 code, data 전송 / username은 저장된 변수 전송
    public void Send(String code, String data) {
        try {
            if(socket == null || socket.isClosed() || !socket.isConnected()) return;
            oos.writeObject(username);
            oos.writeObject(code);
            oos.writeObject(data);
            oos.reset();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
    public void Read() {
        String Code = null, Username = null, Data = null;
        try {
            if (socket.isClosed())
                return;
            Username = (String) ois.readObject();
            Code = (String) ois.readObject();
            Data = (String) ois.readObject();

            //다른 플레이어 로그인 처리
            if(Code.equals("0")) {
                UnityPlayer.UnitySendMessage("UnityActivity", "SaveSender", Username);
                UnityPlayer.UnitySendMessage("UnityActivity", "UserLog", Data);
                UnityPlayer.UnitySendMessage("UnityActivity", "NewUser", Username);
            }

            //다른 플레이어 로그아웃 처리
            if(Code.equals("1")) {
                UnityPlayer.UnitySendMessage("UnityActivity", "SaveSender", Username);
                UnityPlayer.UnitySendMessage("UnityActivity", "UserLog", Data);
                UnityPlayer.UnitySendMessage("UnityActivity", "DeleteUser", Username);
            }

            //다른 플레이어 진행상황 처리
            if(Code.equals("10")) {
                UnityPlayer.UnitySendMessage("UnityActivity", "SaveSender", Username);
                UnityPlayer.UnitySendMessage("UnityActivity", "ResultRivalProgress", Data);
            }

            //다른 플레이어 점수 처리
            if(Code.equals("11")) {
                UnityPlayer.UnitySendMessage("UnityActivity", "SaveSender", Username);
                UnityPlayer.UnitySendMessage("UnityActivity", "ResultRivalScore", Data);
            }

            //채팅 처리
            if(Code.equals("100")) {
                UnityPlayer.UnitySendMessage("UnityActivity", "SaveSender", Username);
                UnityPlayer.UnitySendMessage("UnityActivity", "ResultChat", Data);
            }

        } catch (ClassNotFoundException | IOException e) {
            e.printStackTrace();
            Logout();
        }
    }

}
