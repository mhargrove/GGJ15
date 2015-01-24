import java.net.*;
import java.util.*;
import java.util.concurrent.CopyOnWriteArrayList;
import java.io.*;

public class SocketListener extends Thread{
	public CopyOnWriteArrayList<Socket> clients;
	private ServerSocket acceptor;

	public SocketListener(ServerSocket s){
		this.acceptor = s;
		clients = new CopyOnWriteArrayList<Socket>();
	}

	public void run(){
		boolean threadRunning = true;
		while(threadRunning){
			try{
				Socket temp = acceptor.accept(); //runs until someone connects
				clients.add(temp); 
			}
			catch(IOException ioe){
				ioe.printStackTrace();
			}
		}
	}
}