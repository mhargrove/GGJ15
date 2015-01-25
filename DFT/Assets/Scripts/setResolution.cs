using UnityEngine;
using System.Collections;

public class setResolution : MonoBehaviour {
	[ExecuteInEditMode]

	public int width;
	public int height;

	// Use this for initialization
	void Start () {
		Screen.SetResolution (width, height, false);
	}
}
