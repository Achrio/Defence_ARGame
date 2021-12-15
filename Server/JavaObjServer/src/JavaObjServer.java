//JavaObjServer.java ObjectStream ��� ä�� Server

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;

import javax.swing.JScrollPane;
import javax.swing.JTextArea;
import javax.swing.JLabel;
import javax.swing.JTextField;
import javax.swing.JButton;
import java.awt.event.ActionListener;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.Vector;
import java.awt.event.ActionEvent;
import javax.swing.SwingConstants;
import java.util.Random;

public class JavaObjServer extends JFrame {

	private static final long serialVersionUID = 1L;
	private JPanel contentPane;
	JTextArea textArea;
	private JTextField txtPortNumber;

	private ServerSocket socket; // ��������
	private Socket client_socket; // accept() ���� ������ client ����
	private Vector UserVec = new Vector(); // ����� ����ڸ� ������ ����
	private static final int BUF_LEN = 128; // Windows ó�� BUF_LEN �� ����

	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					JavaObjServer frame = new JavaObjServer();
					frame.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	public JavaObjServer() {
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 338, 440);
		contentPane = new JPanel();
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));
		setContentPane(contentPane);
		contentPane.setLayout(null);

		JScrollPane scrollPane = new JScrollPane();
		scrollPane.setBounds(12, 10, 300, 298);
		contentPane.add(scrollPane);

		textArea = new JTextArea();
		textArea.setEditable(false);
		scrollPane.setViewportView(textArea);

		JLabel lblNewLabel = new JLabel("Port Number");
		lblNewLabel.setBounds(13, 318, 87, 26);
		contentPane.add(lblNewLabel);

		txtPortNumber = new JTextField();
		txtPortNumber.setHorizontalAlignment(SwingConstants.CENTER);
		txtPortNumber.setText("30000");
		txtPortNumber.setBounds(112, 318, 199, 26);
		contentPane.add(txtPortNumber);
		txtPortNumber.setColumns(10);

		JButton btnServerStart = new JButton("Server Start");
		btnServerStart.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				try {
					socket = new ServerSocket(Integer.parseInt(txtPortNumber.getText()));
				} catch (NumberFormatException | IOException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}
				AppendText("Chat Server Running..");
				btnServerStart.setText("Chat Server Running..");
				btnServerStart.setEnabled(false); // ������ ���̻� �����Ű�� �� �ϰ� ���´�
				txtPortNumber.setEnabled(false); // ���̻� ��Ʈ��ȣ ������ �ϰ� ���´�
				AcceptServer accept_server = new AcceptServer();
				accept_server.start();
			}
		});
		btnServerStart.setBounds(12, 356, 300, 35);
		contentPane.add(btnServerStart);
	}

	// ���ο� ������ accept() �ϰ� user thread�� ���� �����Ѵ�.
	class AcceptServer extends Thread {
		@SuppressWarnings("unchecked")
		public void run() {
			while (true) { // ����� ������ ����ؼ� �ޱ� ���� while��
				try {
					AppendText("Waiting new clients ...");
					client_socket = socket.accept(); // accept�� �Ͼ�� �������� ���� �����
					AppendText("New Client from " + client_socket);
					// User �� �ϳ��� Thread ����
					UserService new_user = new UserService(client_socket);
					UserVec.add(new_user); // ���ο� ������ �迭�� �߰�
					new_user.start(); // ���� ��ü�� ������ ����
					AppendText("Current Client : " + UserVec.size());
				} catch (IOException e) {
					AppendText("accept() error");
					//System.exit(0);
				}
			}
		}
	}

	public void AppendText(String str) {
		textArea.append(str + "\n");
		textArea.setCaretPosition(textArea.getText().length());
	}

	public void AppendObject(ChatMsg msg) {
		textArea.append("code = " + msg.code);
		textArea.append(", id = " + msg.UserName);
		textArea.append(", data = " + msg.data + "\n");
		textArea.setCaretPosition(textArea.getText().length());
	}

	// User �� �����Ǵ� Thread
	// Read One ���� ��� -> Write All
	class UserService extends Thread {
		private ObjectInputStream ois;
		private ObjectOutputStream oos;

		private Socket client_socket;
		private Vector user_vc;
		private Vector<Integer> progress_vc; //user progress
		private int progress = 0;				 //this progress
		public String UserName = "";
		
		public void Logout() {
			UserVec.removeElement(this);
			this.client_socket = null;
			AppendText("User [" + UserName + "] Logout.");
		}

		public UserService(Socket client_socket) {
			// TODO Auto-generated constructor stub
			// �Ű������� �Ѿ�� �ڷ� ����
			this.client_socket = client_socket;
			this.user_vc = UserVec;
			try {
				oos = new ObjectOutputStream(client_socket.getOutputStream());
				oos.flush();
				ois = new ObjectInputStream(client_socket.getInputStream());
			} catch (Exception e) {
				AppendText("userService error");
			}
		}
		
		//write to client
		public void WriteChatMsg(ChatMsg obj) {
			try {
			    oos.writeObject(obj.UserName);
			    oos.writeObject(obj.code);
			    oos.writeObject(obj.data);
			} 
			catch (IOException e) {
				AppendText("oos.writeObject(ob) error");		
				try {
					ois.close();
					oos.close();
					client_socket.close();
					client_socket = null;
					ois = null;
					oos = null;				
				} catch (IOException e1) {
					// TODO Auto-generated catch block
					e1.printStackTrace();
				}
				Logout();
			
			}
		}
		public void WriteAllObject(ChatMsg obj) {
			for (int i = 0; i < user_vc.size(); i++) {
				UserService user = (UserService) user_vc.elementAt(i);
				user.WriteChatMsg(obj);
			}
		}
		public void WriteRandObject(ChatMsg obj) {
			Random random = new Random();
			int rand = random.nextInt(user_vc.size());
			UserService user = (UserService) user_vc.elementAt(rand);
			user.WriteChatMsg(obj);
		}
		
		public void WriteAllExceptThis(ChatMsg obj) {
			for(int i = 0; i< user_vc.size(); i++) {
				UserService user = (UserService) user_vc.elementAt(i);
				if(user != this) user.WriteChatMsg(obj);
			}
		}
		
		//read from client
		public ChatMsg ReadChatMsg() {
			Object obj = null;
			String msg = null;
			ChatMsg cm = new ChatMsg("", "", "");
			// Android�� ȣȯ���� ���� ������ Field�� ���ε��� �д´�.
			try {
				obj = ois.readObject();
				cm.UserName = (String) obj;
				obj = ois.readObject();
				cm.code = (String) obj;
				obj = ois.readObject();
				cm.data = (String) obj;
			} catch (ClassNotFoundException e) {
				// TODO Auto-generated catch block
				Logout();
				e.printStackTrace();
				return null;
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
				Logout();
				return null;
			}
			return cm;
		}
	
		public void run() {
			while (true) { // ����� ������ ����ؼ� �ޱ� ���� while��
				ChatMsg cm = null;
				if (client_socket == null) break;
				
				cm = ReadChatMsg();
				if (cm == null) break;
				if (cm.code.length() == 0) break;
	
				AppendObject(cm); //���� �޽��� ����â�� ���
				UserService user;

				switch(cm.code) {
				
				//Login
				case "0" :
					UserName = cm.UserName;
					AppendText("User [" + UserName + "] Login.");
					WriteAllObject(cm);
					
					for (int i = 0; i < user_vc.size(); i++) {
						user = (UserService) user_vc.elementAt(i);
						cm = new ChatMsg(this.UserName, "10", Integer.toString(user.progress));
						WriteAllObject(cm);
					}
					
					break;
				
				//Logout
				case "1" :
					Logout();
					WriteAllObject(cm);
					break;
				//Update user Progress		
				case "10" :				
					//Find user index by UserName
					for (int i = 0; i < user_vc.size(); i++) {
						user = (UserService) user_vc.elementAt(i);	
						
						//Update this progress
						if(cm.UserName == this.UserName) {
							progress = Integer.parseInt(cm.data);
						}
					}
					for (int i = 0; i < user_vc.size(); i++) {
						user = (UserService) user_vc.elementAt(i);
						AppendText(user.UserName + " " + Integer.toString(user.progress));
					}
					WriteAllObject(cm);
					break;
				
				//Summon Knight
				case "20" :
					WriteAllExceptThis(cm);
					break;
				case "21" :
					WriteAllExceptThis(cm);
					break;
				case "22" :
					WriteAllExceptThis(cm);
					break;
				case "23" :
					WriteAllExceptThis(cm);
					break;
				case "24" :
					WriteAllExceptThis(cm);
					AppendText(cm.UserName + " summon knight ");
					break;

				//Write Received Chat
				case "100" :
					WriteAllObject(cm);
					break;
				
				
				default :
					break;
				}
			} // while
		} // run
	}

}