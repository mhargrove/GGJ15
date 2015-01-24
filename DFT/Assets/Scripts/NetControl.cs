using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;

// -IN TERMS OF FILES-   -0 TO 7-   ------------------- 0 TO 100 -------------------
// Player X | Player Y | daysLeft | health | sleepy | social | food | romance | study | itemName | item X | item Y | itemName | ...
// -------ALWAYS SENT----------------------------------------------------------------- ___MAY BE 0 OR MANY ITEMS___________________

public class NetControl : MonoBehaviour {

	[SerializeField] private Player player;
	[SerializeField]private string connectionIP = "167.96.64.74";
	[SerializeField]private int connectionPort = 8000;
	

	private readonly float tileSize = 0.32f;
	private float lasttime      = 0;
	private TcpClient client;
	private NetworkStream toServer;
	private GameObject[] items;
	private byte[] netRecvBuffer;

	private Vector2 tilePos = new Vector2(0, 0);
	private int playerX, playerY, daysLeft, health, sleepy, social, food, romance, study;
	private string[] itemNames;
	private float[] itemX, itemY;

	void Start()
	{
		try{
			//net varibles
			client = new TcpClient ();
			client.Connect(connectionIP, connectionPort);
			toServer = client.GetStream();
			sendMessage("S");
			netRecvBuffer = new byte[1];

			//Game variables
			items = new GameObject[0];
        }
		catch(SocketException se){
			Debug.Log ("No socket...");
		}
		if (client.Connected) {
			Debug.Log ("Connected");
		} else {
			Debug.Log("Not connected.");		
		}
	}

	void ReadData ()
	{
		string msg = getRemoteMessage();
		string[] msgs = msg.Split ('\n');
		for (int i=0; i<msgs.Length; i++) {
			if(!msgs[i].Equals("")) {
				Debug.Log (Time.time + " Got Message: " + msgs[i] + " with length " + msgs[i].Length + " and index " + i);
				handleMessage(msgs[i]);
			}
		}
	}

	void sendMessage(string msg){
		byte[] toSend = System.Text.Encoding.UTF8.GetBytes(msg);
		toServer.Write(toSend, 0, toSend.Length);
    }

	private void handleMessage(string msg){
		if (msg.Length != 2) {
			string[] data = msg.Split ('|');//--------------------------------------------------------------------------
			playerX = int.Parse (data[0]);
			playerY = int.Parse (data [1]);
			daysLeft = int.Parse (data [2]);
			health = int.Parse (data [3]);
			sleepy = int.Parse (data [4]);
			social = int.Parse (data [5]);
			food = int.Parse (data [6]);
			romance = int.Parse (data [7]);
			study = int.Parse (data [8]);
			int count = (data.Length - 9) / 3;
			itemNames = new string[count];
			itemX = new float[count];
			itemY = new float[count];
			if(count > 0){
				for(int i = 9, j = 0; i < data.Length; i+=3) {
					itemNames [j] = data [i];
					itemX [j] = float.Parse (data [i + 1]);
					itemY [j++] = float.Parse (data [i + 2]);
                }
			}
			StatusUpdate ();			
		} else {
			sendMessage("V");
			Debug.Log("Sent keep alive response");
        }
    }
    
    private string getRemoteMessage(){
		int bytesToRead = client.Available;
		netRecvBuffer = new byte[bytesToRead];
		toServer.Read (netRecvBuffer, 0, bytesToRead);
		return System.Text.Encoding.Default.GetString (netRecvBuffer);
	}

	void Update ()
	{
		if(client.Available > 0)
			ReadData();
		if(Input.GetButtonDown("VoteUp")){sendMessage("U");}
		else if(Input.GetButtonDown("VoteDown")){sendMessage("D");}
		else if(Input.GetButtonDown("VoteLeft")){sendMessage("L");}
		else if(Input.GetButtonDown("VoteRight")){sendMessage("R");}
	}

	void StatusUpdate()
	{
		bool moved = false;
		if ((int)playerY > tilePos.y) {
			player.moveUp();
			tilePos.y++;
			moved = true;
		}
		else if ((int)playerY < tilePos.y) {
			player.moveDown();
			tilePos.y--;
			moved = true;
		}

		if ((int)playerX > tilePos.x) {
			player.moveRight();
			tilePos.x++;
			moved = true;
		}
		else if ((int)playerX < tilePos.x) {
			player.moveLeft();
			tilePos.x--;
			moved = true;
		}

		if (!moved) {
			player.gameObject.transform.position = new Vector2((float) playerX * tileSize, (float) playerY * tileSize);
		}

		Debug.Log ("player: " + tilePos + " net: " + playerX + " ," + playerY);

		for (int i = 0; i < items.Length; i++)
			Destroy (items [i]);
		items = new GameObject[itemNames.Length];
		for (int i = 0; i < items.Length; i++)
		{
			switch (itemNames[i])
			{
				case "Adderall":
//					items[i] = GameObject.Instantiate("Adderall", Vector3(itemX[i], itemY[i], 0));
					break;
				case "Nyquill":
					//items[i] = GameObject.Instantiate("Nyquill", Vector3(itemX[i], itemY[i], 0));
					break;
				case "Book":
					//items [i] = GameObject.Instantiate("Book", Vector3(itemX[i], itemY[i], 0));
					break;
				default:
					//etc
					break;
			}
		}
	}
}
