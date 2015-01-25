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
	static final long TOTALGAMETIME = 100 * 60 * 14; //1000 * 60  * 14;
	static final int MAXITEMS = 40;
	static long gameStartTime;
	static long voteLoopTime;
	static long itemLoopTime;
	static boolean collisionMap[][];
	static VoteHandler ballot;
	static int PlayerY;
	static int PlayerX;
	static int mapWidth;
	static int mapHeight;
	static Stats stats;
	static ArrayList<Item> items;
	static ArrayList<Vector2> itemSpawnLocations;

	//Other
	static final boolean DEBUG = false;

	public static void main(String[] args){
		System.out.println("----------Server started----------");
		init();

		boolean running = true;
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
			if(System.currentTimeMillis() - itemLoopTime > ITEMWAITTIME && items.size() < MAXITEMS){
				Random rand = new Random();
				Vector2 nextCoord = itemSpawnLocations.get(rand.nextInt(itemSpawnLocations.size()));
				items.add(new Item(ItemTypes.getNonStaticRandom(), nextCoord.x, nextCoord.y));
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
			if(server == null){
				server = new ServerSocket(PORT, 100, hostAddr);
				if(server.isBound()){
					System.out.println("Server started with address "+hostAddr.toString() + " on port " + PORT);
				}
			}

			if(socketWhisperer == null){
				socketWhisperer = new SocketListener(server);
				socketWhisperer.start();
			}

			//init gameplay stuff
			gameStartTime = System.currentTimeMillis();
			voteLoopTime = gameStartTime;
			itemLoopTime = voteLoopTime;
			ballot = new VoteHandler();
			PlayerX = 31;
			PlayerY = -36;
			stats = new Stats();
			resetItems();
			timeSinceLastVerify = System.currentTimeMillis();

			//build the collision map (and the item spawn locations. Same job)
			if(itemSpawnLocations == null)
				itemSpawnLocations = new ArrayList<Vector2>();
			if(collisionMap == null){
				//load the collision map text file
				System.out.println("Loading collison map");
				String fileLocation = "../world editor/world.objects";
				String[] textFile = openFile(fileLocation);
				String[] vector = textFile[0].split(" ");
				mapWidth =  Integer.parseInt(vector[0]);
				mapHeight = Integer.parseInt(vector[1]);
				System.out.println("\nMap width " + mapWidth + " height " + mapHeight);

				collisionMap = new boolean[mapHeight][mapWidth];
				for(int i=0; i<mapHeight; i++){
					for(int j=0; j<mapWidth; j++){
						collisionMap[i][j] = (textFile[i+1].charAt(j * 2) == '1');
						if(!collisionMap[i][j]){
							itemSpawnLocations.add(new Vector2(j, -i));
						}
					}
				}
				printMap(collisionMap);
			}


		}
		catch(IOException ioe){
			ioe.printStackTrace();
		}
	}

	public static void resetItems(){
		items = new ArrayList<Item>();
		items.add(new Item(ItemTypes.BED, 27, -37));
		items.add(new Item(ItemTypes.BED, 34, -37));
		items.add(new Item(ItemTypes.BED, 51, -8));
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

	public static void printMap(boolean[][] map){
		System.out.println("");
		for(int i=0; i<map.length; i++){
			for(int j=0; j<map[i].length; j++){
				if(i==-PlayerY && j==PlayerX)
					System.out.print("X");
				else{
					if(map[i][j]){
						System.out.print("0");
					}
					else
						System.out.print(" ");
				}
			}
			System.out.println("");
		}
	}

	public static long getTimeElapsed(){
		return System.currentTimeMillis() + stats.timeDelta - gameStartTime;
	}

	public static void printStats(){
		System.out.print("\r");
		System.out.print("Users: " + socketWhisperer.clients.size() + " votes: " + ballot.votesSumbmitted() +
			" items count: "+items.size() + " time left: " + formatTimeRemaining());
	}

	public static void endGame(boolean winner){
		//TODO send endgame signal
		updateUsers();
		for(Socket user : socketWhisperer.clients){
			if(winner) updateUser(user, "WINGAME");
			else updateUser(user, "LOSEGAME");
		}
		System.out.println("\nGame over. Reseting in 18 seconds.");
		try{
			Thread.sleep(18000);
		}
		catch(InterruptedException ie){
			//whateva
		}
		init();
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
			System.out.println(" bonk.");
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
			System.out.println(" Ouch");
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
			if(i.posX == PlayerX && i.posY == PlayerY){
				stats = i.apply(stats);
				usedItem = i;
				break;
			}
		}
		items.remove(usedItem);

		//gradually deteorate stats
		stats.romance--;
		stats.sleepy--;
		stats.hungry--;

		//Check for trigger conditions
		if(stats.health <= 0){
			endGame(false); //dead. Terrible ending.
		}
		if(stats.sleepy <= 0){
			//pass out and wind up in hospital
			stats.sleepy = 100;
			stats.cash = 0;
			PlayerX = 50;
			PlayerY = -8;
		}
		if(stats.hungry <= 0){
			stats.health-=2;
		}
		if(stats.romance <=0){
			stats.hasGirlFriend = false;
		}

		//end game
		if(getTimeElapsed() > TOTALGAMETIME){
			if(stats.study > 60){
				endGame(true);
			}
			else{
				endGame(false);
			}
		}
	}

	public static void updateUsers(){
		String data = getMessage();
		for(Socket s : socketWhisperer.clients){
			updateUser(s, data);
		}
		try{
			Thread.sleep(750);
		}
		catch(InterruptedException ie){
			//whatevah
		}
		for(Socket s : socketWhisperer.clients){
			if(!connectionOpen(s)){
				try{
					s.close();
				}
				catch(IOException ioe){
					//Do nothing
				}
				finally{
					socketWhisperer.clients.remove(s);
				}
			}
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
					else if(msg == 'V'){
						//do nothing, but we shouldn't get this
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

	public static boolean connectionOpen(Socket s){
		try{
			BufferedReader in = new BufferedReader(new InputStreamReader(s.getInputStream()));
			if(in.ready()){
				char msg = (char)in.read();
				//just read it and deposit
				return true;
			}
			else{
				return false;
			}
		}
		catch(IOException ioe){

		}
		return false;
	}

	//Builds the message to be sent. Somewhat expensive
	public static String getMessage(){
		String temp = "";
		temp += Integer.toString(PlayerX) + "|";
		temp += Integer.toString(PlayerY) + "|";
		temp += formatTimeRemaining() + "|";
		temp += Integer.toString(stats.health) + "|";
		temp += Integer.toString(stats.sleepy) + "|";
		temp += Integer.toString(stats.social) + "|";
		temp += Integer.toString(stats.hungry) + "|";
		temp += Integer.toString(stats.romance) + "|";
		temp += Integer.toString(stats.study) + "|";
		temp += Integer.toString(stats.cash) + "|";
		for(Item item : items){
			temp += item.type.netName + "|";
			temp += Integer.toString(item.posX) + "|";
			temp += Integer.toString(item.posY) + "|";
		}
		return temp;
	}

	public static String formatTimeRemaining(){
		long r = TOTALGAMETIME - getTimeElapsed();

		r = r / 1000;
		r = r / 60;
		return "" + r;
	}
}
