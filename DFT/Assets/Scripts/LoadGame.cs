using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour {

	public GameObject ipAddressTextfield;

	void Start ()
	{
//		if (Application.loadedLevel == 0 || Application.loadedLevel == 4) 
//		{StartCoroutine("Load");
//				}
//		else
		if (Application.loadedLevel == 2 || Application.loadedLevel == 3)
			StartCoroutine("LoadResults");
	}

//	IEnumerator Load()
//	{
//		yield return new WaitForSeconds(8);
//		Application.LoadLevel(1);
//		}

	IEnumerator LoadResults()
	{
		Object.DontDestroyOnLoad(GameObject.Find("results"));
		yield return new WaitForSeconds(8);
		Application.LoadLevel (4);
		}

	public void validate()
	{
		string teststring = ipAddressTextfield.transform.GetChild (0).transform.GetChild(2).GetComponent<Text>().text;
		Debug.Log ("TEST" + teststring);
		Application.LoadLevel(1);
	}
}
