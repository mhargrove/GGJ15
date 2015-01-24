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
	static long timeSinceLastVerify;

	//Gameplay vars
	static final long VOTEWAITTIME = 5000; // in millis
	static VoteHandler ballot;
	static int PlayerY;
	static int PlayerX;
	static int daysLeft;
	static Stats stats;
	static ArrayList<Item> items;

	//Other
	static final boolean DEBUG = false;

	public static void main(String[] args){
		System.out.println("----------Server started----------");
		init();
		boolean running = true;
		int ticks;
		long loopStartTime = System.currentTimeMillis();
		while(running){
			//read the data from users first
			readUsers();
			//print the current state
			printStats();

			//initiate time-based decisions
			if(System.currentTimeMillis() - loopStartTime > VOTEWAITTIME){
				updateGame(); //handles votes and movement
				updateUsers(); //Sends out the results
				loopStartTime = System.currentTimeMillis();
			}
			else{
				if(DEBUG){
					System.out.println("Not yet. Time: " + (System.currentTimeMillis() - loopStartTime));
				}
			}
			//wait a bit to break up the loop
			try {
				Thread.sleep(100);
			} 
			catch(InterruptedException ex) {
    			Thread.currentThread().interrupt();
			}
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

			//init gameplay stuff
			ballot = new VoteHandler();
			PlayerX = 0;
			PlayerY = 0;
			daysLeft = 7;
			stats = new Stats();
			items = new ArrayList<Item>();
			timeSinceLastVerify = System.currentTimeMillis();
		}
		catch(IOException ioe){
			ioe.printStackTrace();
		}
	}

	public static void printStats(){
		System.out.print("\r");
		System.out.print("Users: " + socketWhisperer.clients.size() + " votes: " + ballot.votesSumbmitted());
	}

	public static void updateGame(){
		switch(ballot.getResults()){
			case UP: PlayerY++; break;
			case DOWN: PlayerY--; break;
			case LEFT: PlayerX--; break;
			case RIGHT: PlayerX++; break;
			case NONE: break;
		}
	}

	public static void updateUsers(){
		String data = getMessage();
		for(Socket s : socketWhisperer.clients){
			updateUser(s, data);
		}
	}

	public static void updateUser(Socket s, String msg){
		try{
			PrintWriter toUser = new PrintWriter(s.getOutputStream(), true);
			toUser.println(msg);
		}
		catch(IOException ioe){
			ioe.printStackTrace();
		}
	}

	public static void updateUser(Socket s){
		String data = getMessage();
		updateUser(s, data);
	}

	public static void readUsers(){
		BufferedReader inbox;
		for(Socket user : socketWhisperer.clients){
			try{
				if(DEBUG){
					System.out.println("Reading user " + user.toString());
				}
				inbox = new BufferedReader(new InputStreamReader(user.getInputStream()));
				if(inbox.ready()){
					char msg = (char)inbox.read();					
					//parse the message
					if(msg == 'S'){
						updateUser(user);
					}
					else if(msg == 'U'){
						ballot.voteUp(user);
					}
					else if(msg == 'D'){
						ballot.voteDown(user);
					}
					else if(msg == 'L'){
						ballot.voteLeft(user);
					}
					else if(msg == 'R'){
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