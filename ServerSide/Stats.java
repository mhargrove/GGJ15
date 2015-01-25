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

	public Stats(){
		health  = MAXVALUE;
		sleepy  = 0;
		social  = MAXVALUE / 2;
		hungry  = MAXVALUE / 2;
		romance = MAXVALUE / 2;
		study   = 0;
		cash = 0;
	}
}