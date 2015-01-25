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
	static final long VOTEWAITTIME = 1500; // in millis
	static final long ITEMWAITTIME = 4000;
	static boolean collisionMap[][];
	static VoteHandler ballot;
	static int PlayerY;
	static int PlayerX;
	static int mapWidth;
	static int mapHeight;
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
		long voteLoopTime = System.currentTimeMillis();
		long itemLoopTime = voteLoopTime;
		while(running){
			//read the data from users first
			readUsers();
			//print the current state
			printStats();

			//initiate time-based decisions
			if(System.currentTimeMillis() - voteLoopTime > VOTEWAITTIME){
				updateGame(); //handles votes and movement
				updateUsers(); //Sends out the results
				voteLoopTime = System.currentTimeMillis();
			}
			//spawn a new item
			if(System.currentTimeMillis() - itemLoopTime > ITEMWAITTIME){
				try{
					int x, y;
					double variance = 10.0f;
					Random rand = new Random();
					//generate locations close to the player
					x = Math.min((int)(PlayerX + rand.nextGaussian() * variance), mapWidth);
					y = Math.min((int)(PlayerY + rand.nextGaussian() * variance), mapHeight);
					//make sure they aren't in a wall or something
					boolean needNewLocation = collisionMap[x][-y];
					if(needNewLocation){
						int newX = x, newY = -y, loops = 0;
						while(needNewLocation){
							newX+=(-1 + (Math.random() * 2));
							newY+=(-1 + (Math.random() * 2));
							needNewLocation = collisionMap[newX][newY];
							if(loops++ == 10){
								//skip this one
								break;
							}
						}
						if(!needNewLocation){
							x = newX; 
							y = -newY;
						}
					}
					if(!needNewLocation){
						Item i = new Item(ItemTypes.getNonStaticRandom(), x , y);
						items.add(i);
						System.out.println("\nnew item: " + i.type.netName + " at location " + i.posX + " ," + i.posY);
					} 
				}
				catch(ArrayIndexOutOfBoundsException e){
					//oh well
				}
				itemLoopTime = System.currentTimeMillis();
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
			PlayerX = 23;
			PlayerY = -30;
			daysLeft = 7;
			stats = new Stats();
			items = new ArrayList<Item>();
			//static items
			items.add(new Item(ItemTypes.BED, 21, 31));
			timeSinceLastVerify = System.currentTimeMillis();

			//load the collision map
			System.out.println("Loading collison map");
			String fileLocation = "../world editor/world.objects";
			String[] textFile = openFile(fileLocation);
			String[] vector = textFile[0].split(" ");
			mapWidth =  Integer.parseInt(vector[0]);
			mapHeight = Integer.parseInt(vector[1]);
			if(DEBUG) System.out.println("Map width " + mapWidth + " height " + mapHeight);
			//build the collision map
			collisionMap = new boolean[mapHeight][mapWidth];
			for(int i=0; i<mapHeight; i++){
				for(int j=0; j<mapWidth; j++){
					collisionMap[i][j] = (textFile[i+1].charAt(j * 2) == '1');
				}
			}
			printCollisionMap(collisionMap);

			//spawn items

		}
		catch(IOException ioe){
			ioe.printStackTrace();
		}
	}

	public static void printCollisionMap(boolean[][] map){
		for(int i=0; i<map.length; i++){
			for(int j=0; j<map[i].length; j++){
				if(map[i][j])
					System.out.print("0");
				else
					System.out.print(" ");
			}
			System.out.println("");
		}
	}

	public static String[] openFile(String path) throws IOException{
		FileReader fr = new FileReader(path);
		BufferedReader br = new BufferedReader(fr);
		Scanner fRead = new Scanner(br);
		String ret = "";
		while(fRead.hasNext()){
			ret += fRead.nextLine() + "\n";
		}
		return ret.split("\n");
	}

	public static void printStats(){
		System.out.print("\r");
		System.out.print("Users: " + socketWhisperer.clients.size() + " votes: " + ballot.votesSumbmitted());
	}

	//update the game logic. Collisions, item uses, etc
	public static void updateGame(){
		Action voteResult = ballot.getResults();
		switch(voteResult){
			case UP: PlayerY++; break;
			case DOWN: PlayerY--; break;
			case LEFT: PlayerX--; break;
			case RIGHT: PlayerX++; break;
			case NONE: break;
		}
		try{
			if(collisionMap[-PlayerY][PlayerX]){
			//revert the changes and do nothing else
			switch(voteResult){
				case UP: PlayerY--; break;
				case DOWN: PlayerY++; break;
				case LEFT: PlayerX++; break;
				case RIGHT: PlayerX--; break;
				case NONE: break;
			}
		}
		}
		catch(ArrayIndexOutOfBoundsException e){
			System.out.println("Ouch");
			//revert the changes anyway
			switch(voteResult){
				case UP: PlayerY--; break;
				case DOWN: PlayerY++; break;
				case LEFT: PlayerX++; break;
				case RIGHT: PlayerX--; break;
				case NONE: break;
			}
		}

		//check to see if we've enountered an item
		Item usedItem = null;
		for(Item i : items){
			stats = i.apply(stats);
			usedItem = i;
			break;
		}
		items.remove(usedItem);

		//check stats for effects or triggers
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
		temp += Integer.toString(stats.hungry) + "|";
		temp += Integer.toString(stats.romance) + "|";
		temp += Integer.toString(stats.study) + "|";
		for(Item item : items){
			temp += item.type.netName + "|";
			temp += Integer.toString(item.posX) + "|";
			temp += Integer.toString(item.posY) + "|";
		}
		return temp;
	}
}