public class Stats{
	public static final int MAXVALUE = 100; // max value for each stat
	public int health;
	public int sleepy;
	public int social;
	public int hungry;
	public int romance;
	public int study;
	public int cash;

	//considered each update.
	public int timeDelta = 0;
	public int moveFastFor = 0;

	//odder data, but nessesary for endgame
	public boolean hasGirlFriend = true;

	public Stats(){
		health  = MAXVALUE;
		sleepy  = MAXVALUE;
		social  = MAXVALUE;
		hungry  = MAXVALUE;
		romance = MAXVALUE;
		study   = MAXVALUE;
		cash = 0;
	}
}
