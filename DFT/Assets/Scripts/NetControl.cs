using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.Net.Sockets;

// -IN TERMS OF FILES-   -0 TO 7-   ------------------- 0 TO 100 -------------------
// Player X | Player Y | daysLeft | health | sleepy | social | food | romance | study | itemName | item X | item Y | itemName | ...
// -------ALWAYS SENT----------------------------------------------------------------- ___MAY BE 0 OR MANY ITEMS___________________

public class NetControl : MonoBehaviour {

	//players and status bars
	[SerializeField] private Player player;
	[SerializeField] private ProgressBar healthBar;
	[SerializeField] private ProgressBar sleepBar;
	[SerializeField] private ProgressBar socialBar;
	[SerializeField] private ProgressBar hungerBar;
	[SerializeField] private ProgressBar romanceBar;
	[SerializeField] private ProgressBar studyBar;
	[SerializeField] private Text money;
	[SerializeField] private Text Timer;
	[SerializeField] private ArrowSelection arrows;

	//prefabs
	[SerializeField] private GameObject bed;
	[SerializeField] private GameObject flower;
	[SerializeField] private GameObject medkit;
	[SerializeField] private GameObject nyquil;
	[SerializeField] private GameObject adderall;
	[SerializeField] private GameObject foodItem;
	[SerializeField] private GameObject coin;
	[SerializeField] private GameObject book;

	//other
	[SerializeField] private string connectionIP = "0.0.0.0";
	[SerializeField] private int connectionPort = 8000;
	[SerializeField] private Animator animator;
	[SerializeField] private GameObject results;

	private readonly float tileSize = 0.32f;
	private TcpClient client;
	private NetworkStream toServer;
	private GameObject[] items;
	private byte[] netRecvBuffer;
	
	private Vector2 tilePos = new Vector2(0, 0);
	private int playerX, playerY, health, sleepy, social, food, romance, study;
	private string timeLeft = "", cash = "";
	private string[] itemNames;
	private float[] itemX, itemY;
	
	void Start()
	{
		//figure out if the canvas from last scene persists
		GameObject lastScene = GameObject.Find ("Canvas");
		if (lastScene != null) {
			Debug.Log("Got IP field.");
			lastScene.SetActive(false);
			connectionIP = lastScene.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<Text>().text;
		}
		animator = this.GetComponent<Animator>();
		try{
			//net varibles
			client = new TcpClient ();
			try{
				client.Connect(connectionIP, connectionPort);}
			catch(SocketException e)
			{
				Application.LoadLevel(0);
			}
			toServer = client.GetStream();
			sendMessage("S");
			netRecvBuffer = new byte[1];
			
			//Game variables
			items = new GameObject[0];
			healthBar  = (ProgressBar)GameObject.Find("HealthBar").GetComponent("ProgressBar");
			sleepBar   = (ProgressBar)GameObject.Find("SleepBar").GetComponent("ProgressBar");
			socialBar  = (ProgressBar)GameObject.Find("SocialBar").GetComponent("ProgressBar");
			hungerBar  = (ProgressBar)GameObject.Find("HungerBar").GetComponent("ProgressBar");
			romanceBar = (ProgressBar)GameObject.Find("RomanceBar").GetComponent("ProgressBar");
			studyBar   = (ProgressBar)GameObject.Find("StudyBar").GetComponent("ProgressBar");
		}
		catch(SocketException se){
			Debug.LogError(se.Message);
			Debug.Log ("No socket on address " + connectionIP);
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
				sendMessage("V");
				handleMessage(msgs[i]);
			}
		}
	}
	
	void sendMessage(string msg){
		byte[] toSend = System.Text.Encoding.UTF8.GetBytes(msg);
		toServer.Write(toSend, 0, toSend.Length);
	}
	
	private void handleMessage(string msg){
		if (msg.Trim ().Equals ("LOSEGAME")) {
			GameObject res = Instantiate(results) as GameObject;
			res.GetComponent<GameResults>().setGameResults(health, sleepy, social, food, romance, study, int.Parse(cash));
			Object.DontDestroyOnLoad(res);
			Application.LoadLevel (3);
		}
		else if (msg.Trim ().Equals ("WINGAME")) {
			GameObject res = Instantiate(results) as GameObject;
			res.GetComponent<GameResults>().setGameResults(health, sleepy, social, food, romance, study, int.Parse(cash));
			Object.DontDestroyOnLoad(res);
			Application.LoadLevel (2);
		}
		else if (msg.Length != 2) {
			arrows.Reset();
			//Handle the message
			string[] data = msg.Split ('|');
			playerX = int.Parse (data[0]);
			playerY = int.Parse (data [1]);
			timeLeft = data[2];
			health = int.Parse (data [3]);
			sleepy = int.Parse (data [4]);
			social = int.Parse (data [5]);
			food = int.Parse (data [6]);
			romance = int.Parse (data [7]);
			study = int.Parse (data [8]);
			cash = data[9];

			//From here, it's the optional items
			int count = (data.Length - 10) / 3;
			itemNames = new string[count];
			itemX = new float[count];
			itemY = new float[count];
			if(count > 0){
				for(int i = 10, j = 0; i < data.Length - 3; i+=3, j++) {
					itemNames [j] = data [i];
					itemX [j] = int.Parse (data [i + 1]);
					itemY [j] = int.Parse (data [i + 2]);
				}
			}
			else{
				Debug.Log("Not items");
			}
			StatusUpdate ();
		} else {
			sendMessage("V");
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
		Timer.text = "Days left: " + timeLeft;
		if(client.Available > 0)
			ReadData();
		if(Input.GetButtonDown("VoteUp")){sendMessage("U"); arrows.Select(0); player.MoveSprite(0);}
		else if(Input.GetButtonDown("VoteDown")){sendMessage("D"); arrows.Select(1); player.MoveSprite(1);}
		else if(Input.GetButtonDown("VoteLeft")){sendMessage("L"); arrows.Select(2); player.MoveSprite(2);}
		else if(Input.GetButtonDown("VoteRight")){sendMessage("R"); arrows.Select(3); player.MoveSprite(3);}
	}
	
	void StatusUpdate()
	{
		bool behind = false;
		//figure out if the player is lagging
		if (Mathf.Abs((int)playerY - tilePos.y) > 1 || Mathf.Abs((int)playerX - tilePos.x) > 1) {
			behind = true;
		}
		if ((int)playerY != tilePos.y && (int)playerX != tilePos.x) {
			behind = true;
		}
		//if we aren't move normally
		if (!behind) {
			if ((int)playerY > tilePos.y) {
				player.moveUp();
				tilePos.y++;
			}
			else if ((int)playerY < tilePos.y) {
				player.moveDown();
				tilePos.y--;
			}
			else if ((int)playerX > tilePos.x) {
				player.moveRight();
				tilePos.x++;
			}
			else if ((int)playerX < tilePos.x) {
				player.moveLeft();
				tilePos.x--;
			}
			else{
				//TODO whatever happens on nothing
			}
		}
		//lagging, just teleport.
		else{
			Debug.Log("Catching up.");
			//play some catch up
			player.teleport(new Vector2(playerX * tileSize, playerY * tileSize));
			tilePos.x = playerX;
			tilePos.y = playerY;
		}
		
		Debug.Log ("player: " + tilePos + " net: " + playerX + " ," + playerY);
		

		if (health != healthBar.fill)
			healthBar.UpdateHealth (health);
		if (sleepy != sleepBar.fill)
			sleepBar.UpdateHealth (sleepy);
		if (social != socialBar.fill)
			socialBar.UpdateHealth (social);
		if (food != hungerBar.fill)
			hungerBar.UpdateHealth (food);
		if (romance != romanceBar.fill)
			romanceBar.UpdateHealth (romance);
		if (study != studyBar.fill)
			studyBar.UpdateHealth (study);
		if (!cash.Equals(money.text))
			money.text = cash;


		//Destroy all items previous to these ones
		for (int i = 0; i < items.Length; i++) Destroy (items [i]);

		items = new GameObject[itemNames.Length];
		for (int i = 0; i < itemNames.Length; i++)
		{
			Vector3 pos = new Vector3(itemX[i] * tileSize, itemY[i] * tileSize, 0);
			switch (itemNames[i])
			{
			case "BED":
				items[i] = Instantiate(bed, pos, Quaternion.identity) as GameObject;
				break;
			case "FLOWER":
				items[i] = Instantiate(flower, pos, Quaternion.identity) as GameObject;
				break;
			case "MEDKIT":
				items [i] = Instantiate(medkit, pos, Quaternion.identity) as GameObject;
				break;
			case "NYQUIL":
				items[i] = Instantiate(nyquil, pos, Quaternion.identity) as GameObject;
				break;
			case "ADDERAL":
				items[i] = Instantiate(adderall, pos, Quaternion.identity) as GameObject;
				break;
			case "FOOD":
				items[i] = Instantiate(foodItem, pos, Quaternion.identity) as GameObject;
				break;
			case "COIN":
				items[i] = Instantiate(coin, pos, Quaternion.identity) as GameObject;
				break;
			case "BOOKS":
				items[i] = Instantiate(book, pos, Quaternion.identity) as GameObject;
				break;
			default: //BOOK
				Debug.LogError("Invalid item: " + itemNames[i]);
				break;
			}
		}
	}
}
