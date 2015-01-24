public class Stats{
	public final int MAXVALUE = 100; // max value for each stat
	public int health;
	public int sleepy;
	public int social;
	public int food;
	public int romance;
	public int study;

	public Stats(){
		health  = MAXVALUE;
		sleepy  = 0;
		social  = MAXVALUE / 2;
		food    = MAXVALUE / 2;
		romance = MAXVALUE / 2;
		study   = 0;
	}
}