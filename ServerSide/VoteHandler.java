import java.util.*;
import java.net.*;

public class VoteHandler{
	//number of votes for each movement
	private HashMap<Socket, Action> ballotBox;

	public VoteHandler(){
		ballotBox = new HashMap<Socket, Action>();
	}

	public void voteUp(Socket s){ballotBox.put(s, Action.UP);}
	public void voteDown(Socket s){ballotBox.put(s, Action.DOWN);}
	public void voteLeft(Socket s){ballotBox.put(s, Action.LEFT);}
	public void voteRight(Socket s){ballotBox.put(s, Action.RIGHT);}
	
	//Gets the stored results and resets the data
	public Action getResults(){
		//count the votes
		int u=0, d=0, l=0, r=0;
		for(Action vote : ballotBox.values()){
			switch(vote){
				case UP: u++; break;
				case DOWN: d++; break;
				case LEFT: l++; break;
				case RIGHT: r++; break;
			}
		}

		//decide the winner
		if(u > d && u > l && u > r){ballotBox = new HashMap<Socket, Action>(); return Action.UP;}
		if(d > u && d > l && d > r){ballotBox = new HashMap<Socket, Action>(); return Action.DOWN;}
		if(l > d && l > u && l > r){ballotBox = new HashMap<Socket, Action>(); return Action.LEFT;}
		if(r > d && r > l && r > u){ballotBox = new HashMap<Socket, Action>(); return Action.RIGHT;}

		//tie
		ballotBox = new HashMap<Socket, Action>();
		return Action.NONE;
	}

	public int votesSumbmitted(){return ballotBox.size();}
}