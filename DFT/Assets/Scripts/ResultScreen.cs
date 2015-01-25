using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultScreen : MonoBehaviour {

	private GameResults res;
	public Text text;

	/*
	public int health;
	public int sleepy;
	public int social;
	public int hungry;
	public int romance;
	public int study;
	public int cash;
	 */
	void Start () {
		res = GameObject.Find ("Results(Clone)").GetComponent<GameResults> ();

		//add witty messages for each stat
		char grade;
		if(res.study > 80) grade = 'A';
		else if(res.study > 60) grade = 'B';
		else if(res.study > 50) grade = 'C';
		else if(res.study > 40) grade = 'D';
		else grade = 'F';

		addText("Grades", "Your studies earned you a");

		switch (grade) {
			case 'A': text.text += "n A. You aced finals week! Neeeerrrrdddd."; break;
			case 'B': text.text += " B. Those get degrees too, right?"; break;
			case 'C': text.text += " C. It's probably what you expected."; break;
			case 'D': text.text += " D. It's just like the little league 'You tried' award.";break;
			case 'F': text.text += "n F. At least you're pretty, right?"; break;
		}

		if (res.sleepy > 50) addText ("Sleep level","You stayed well rested.");
		else addText ("Sleep level","You could have fallen asleep standing up.");

		if(res.health != 0) addText("Health","You at least survived.");
		else addText("Health","You became another tragic victim of the system");

		if(res.social > 50) addText("Social", "You kept your friends!");
		else addText("Social", "Hope you like cats");

		if(res.hungry > 50) addText("Hunger", "You stayed well-fed");
		else addText("Hunger","Hey, you at least lost some weight!");

		if(res.romance > 50) addText("Romance", "You managed to keep your relationship alive");
		else addText("Romance", "Looks like you're living the single life.");

		addText("Cash", "You wound up with $" + res.cash);
	}

	void addText(string header, string msg){
		text.text += "\n" + header + ":\t";
		text.text += msg;
	}
}