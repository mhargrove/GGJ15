using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultScreen : MonoBehaviour {

	public GameResults results;
	public GameObject resultsContainer;
	public Text text;

	// Use this for initialization
	void Start () {
		text.text += "Testing\n";
		if (resultsContainer == null) {
			resultsContainer = GameObject.Find("results");
		}
	}
}