public class SocketListener extends Thread{
	private ArrayList<Socket> clients;
	private boolean newClients;
	private ServerSocket acceptor;

	public SocketListener(ServerSocket s){
		this.acceptor = s;
		clients = new Arraylist<Socket>();
		newClients = false;
	}

	public void run(){
		boolean threadRunning = true;
		while(threadRunning){
			clients.add(acceptor.accept()); //runs until someone connects
			newClients = true;
			System.out.println("New connection.");
		}
	}

	public boolean areNewClients(){
		return newClients;
	}

	public Arraylist<Socket> getClients(){
		newClients = false;
		return clients;
	}
}