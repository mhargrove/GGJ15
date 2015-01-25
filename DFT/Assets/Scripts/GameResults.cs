using UnityEngine;
using System.Collections;

<<<<<<< HEAD

public class GameResults : MonoBehaviour {

=======
public class GameResults : MonoBehaviour{
>>>>>>> cefa87cff158e8086cfe0466623cc27be19dc97f
	public int health;
	public int sleepy;
	public int social;
	public int hungry;
	public int romance;
	public int study;
	public int cash;


	public void setGameResults(int hea, int sle, int soc, int hun, int rom, int stu, int cash){
		this.health = hea;
		this.sleepy = sle;
		this.social = soc;
		this.hungry = hun;
		this.romance = rom;
		this.study = stu;
		this.cash = cash;
	}

}
