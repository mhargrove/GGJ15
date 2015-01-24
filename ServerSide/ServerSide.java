//Written for "Don't Fail Together"
//LSU Global Game Jam 2015

import java.net.*;
import java.io.*;
import java.util.*;

public class ServerSide{
	//Net vars
	static InetAddress hostAddr;
	static final int PORT = 8000;
	static ServerSocket server;
	static SocketListener socketWhisperer;
	static ArrayList<Socket> clients;

	//Gameplay vars
	static VoteHandler ballot;
	static int PlayerY;
	static int PlayerX;
	static int daysLeft;
	static Stats stats;
	static ArrayList<Item> items;

	//Other
	static final boolean DEBUG = true;

	public static void main(String[] args){
		System.out.println("----------Server started----------");
		init();
		boolean running = true;
		while(running){
			tick();
		}
	}

	public static void init(){
		try{
			//init net stuff
			hostAddr = InetAddress.getLocalHost();
			server = new ServerSocket(PORT, 100, hostAddr);
			if(server.isBound()){
				System.out.println("Server started with address "+hostAddr.toString() + " on port " + PORT);
			}
			socketWhisperer = new SocketListener(server);
			socketWhisperer.start();
			clients = new ArrayList<Socket>();

			//init gameplay stuff
			ballot = new VoteHandler();
			PlayerX = 0;
			PlayerY = 0;
			daysLeft = 7;
			stats = new Stats();
			items = new ArrayList<Item>();
		}
		catch(IOException ioe){
			ioe.printStackTrace();
		}
	}
	public static void tick(){
		//update new users
		if(socketWhisperer.newClients()){
			if(DEBUG){
				System.out.println("adding new client");
			}
			clients = socketWhisperer.getClients();
		}
		readUsers();
		//Check time left on voting
		//act on that
	}
	public static void updateGame(){}
	public static void updateUsers(){

	}
	public static void updateUser(Socket s, String msg){

	}
	public static void updateUser(Socket s){
		String data = getMessage();
		updateUser(s, data);
	}
	public static void readUsers(){
		BufferedReader inbox;
		for(Socket user : clients){
			try{
				inbox = new BufferedReader(new InputStreamReader(user.getInputStream()));
				if(inbox.ready()){
					String msg = "" + (char)inbox.read();
					
					//parse the message
					if(msg.equals("S")){
						updateUser(user);
					}
					else if(msg.equals("U")){
						ballot.voteUp(user);
					}
					else if(msg.equals("D")){
						ballot.voteDown(user);
					}
					else if(msg.equals("L")){
						ballot.voteLeft(user);
					}
					else if(msg.equals("R")){
						ballot.voteRight(user);
					}
					else{
						System.out.println("Client sent : " + msg + " -not recogonized command.");
					}
				}
			}	
			catch(IOException ioe){
				ioe.printStackTrace();
			}
		}
	}

	//Builds the message to be sent. Somewhat expensive
	public static String getMessage(){
		String temp = "";
		temp += Integer.toString(PlayerX) + "|";
		temp += Integer.toString(PlayerY) + "|";
		temp += Integer.toString(daysLeft) + "|";
		temp += Integer.toString(stats.health) + "|";
		temp += Integer.toString(stats.sleepy) + "|";
		temp += Integer.toString(stats.social) + "|";
		temp += Integer.toString(stats.food) + "|";
		temp += Integer.toString(stats.romance) + "|";
		temp += Integer.toString(stats.study) + "|";
		for(Item item : items){
			temp += item.name + "|";
			temp += Integer.toString(item.posX) + "|";
			temp += Integer.toString(item.posY) + "|";
		}
		return temp;
	}
}