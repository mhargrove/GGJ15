using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;

// -IN TERMS OF FILES-   -0 TO 7-   ------------------- 0 TO 100 -------------------
// Player X | Player Y | daysLeft | health | sleepy | social | food | romance | study | itemName | item X | item Y | itemName | ...
// -------ALWAYS SENT----------------------------------------------------------------- ___MAY BE 0 OR MANY ITEMS___________________

public class NetControl : MonoBehaviour {

	private string connectionIP = "167.96.78.88";
	private int connectionPort = 8000;//25001;
	private float lasttime      = 0;
	private TcpClient client;
	private GameObject[] items;
	public  GameObject player;

	private float playerX, playerY, daysLeft, health, sleepy, social, food, romance, study;
	private string[] itemNames;
	private float[] itemX, itemY;

	void OnGUI()
	{
		if (Network.peerType == NetworkPeerType.Disconnected)
			GUI.Label (new Rect (10, 10, 200, 20), "Disconnected");
		if (GUI.Button (new Rect (10, 30, 120, 20), "Client Connect"))
		{
			Network.Connect (connectionIP, connectionPort);
			client = new TcpClient(connectionIP, connectionPort);
			ReadData();
		}
		if (GUI.Button(new Rect(10, 80, 120, 20), "Initialize Server"))
			Network.InitializeServer(1000, connectionPort, false);
		else if (Network.peerType == NetworkPeerType.Client)
		{
			GUI.Label(new Rect(10, 10, 300, 20), "Connected as Client");
			if (GUI.Button(new Rect(10, 55, 120, 20), "Disconnect"))
				Network.Disconnect(200);
		}
	}

	void ReadData ()
	{
		string[] data = client.GetStream ().ToString ().Split ('|');//--------------------------------------------------------------------------
		playerX  = float.Parse(data [0]);
		playerY  = float.Parse(data [1]);
		daysLeft = float.Parse(data [2]);
		health   = float.Parse(data [3]);
		sleepy   = float.Parse(data [4]);
		social   = float.Parse(data [5]);
		food     = float.Parse(data [6]);
		romance  = float.Parse(data [7]);
		study    = float.Parse(data [8]);

		int count = (data.Length - 9) / 3;
		int j     = 0;
		itemNames = new string[count];
		itemX     = new float[count];
		itemY     = new float[count];

		for (int i = 9; i < data.Length; i+=3)
		{
			itemNames[j] = data[i];
			itemX[j]     = float.Parse(data[i + 1]);
			itemY[j++]   = float.Parse(data[i + 2]);
		}
		StatusUpdate ();
	}

	void Update ()
	{
		if (Time.time - lasttime > 5)
		{
			ReadData();
			lasttime = Time.time;
		}
	}

	void StatusUpdate()
	{
		player.transform.Translate (playerX - player.transform.position.x, playerY - player.transform.position.y, 0);

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
