using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {


	void Start ()
	{
		if (Application.loadedLevel == 0)
			StartCoroutine("Load");
		else
			StartCoroutine("WaitNLoad");
	}

	IEnumerator Load()
	{
		yield return new WaitForSeconds(8);
		Application.LoadLevel(1);
		}

	IEnumerator WaitNLoad()
	{
		yield return new WaitForSeconds(20);
		Application.LoadLevel (1);
		}
}
