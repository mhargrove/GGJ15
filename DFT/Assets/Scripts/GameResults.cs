using UnityEngine;
using System.Collections;

public class GameResults : MonoBehaviour{

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
