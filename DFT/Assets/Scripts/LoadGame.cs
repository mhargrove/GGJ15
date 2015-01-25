using UnityEngine;
using System.Collections;

public class LoadGame : MonoBehaviour {


	void Start ()
	{
		if (Application.loadedLevel == 0 || Application.loadedLevel == 4)
			StartCoroutine("Load");
		else
			StartCoroutine("LoadResults");
	}

	IEnumerator Load()
	{
		yield return new WaitForSeconds(8);
		Application.LoadLevel(1);
		}

	IEnumerator LoadResults()
	{
		Object.DontDestroyOnLoad(GameObject.Find("results"));
		yield return new WaitForSeconds(8);
		Application.LoadLevel (4);
		}
}
