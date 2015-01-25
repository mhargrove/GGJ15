using UnityEngine;
using System.Collections;

<<<<<<< HEAD
public class GameResults : MonoBehaviour{
=======
public class GameResults : MonoBehaviour {
>>>>>>> e6a5d33b18d55adc4fffc746011bc42c2b8b0e30

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
