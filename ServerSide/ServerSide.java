//Written for "Don't fail together"
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
	static int PlayerY;
	static int PlayerX;
	static int daysLeft;
	static Stats stats;
	static ArrayList<Item> items;

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
			clients = new ArrayList<Socket>();

			//init gameplay stuff
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
	public static void tick(){}
	public static void updateGame(){}
	public static void updateUsers(){}
	public static void readUsers(){}
	public static String getMessage(){

	}
}