import java.net.*;
import java.util.*;
import java.io.*;

public class SocketListener extends Thread{
	private ArrayList<Socket> clients;
	private boolean newClients;
	private ServerSocket acceptor;
	private boolean clientListLocked = false;

	public SocketListener(ServerSocket s){
		this.acceptor = s;
		clients = new ArrayList<Socket>();
		newClients = false;
	}

	public void run(){
		boolean threadRunning = true;
		while(threadRunning){
			try{
				clients.add(acceptor.accept()); //runs until someone connects
				clientListLocked = false;
				newClients = true;
				System.out.println("New connection.");
			}
			catch(IOException ioe){
				ioe.printStackTrace();
			}
		}
	}

	public boolean clientListLocked(){
		return clientListLocked;
	}

	public boolean newClients(){
		return newClients;
	}

	public ArrayList<Socket> getClients(){
		newClients = false;
		return clients;
	}
}