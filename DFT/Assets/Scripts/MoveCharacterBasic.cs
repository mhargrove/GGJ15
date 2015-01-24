﻿using UnityEngine;
using System.Collections;
//using System.Threading.Tasks;

public class MoveCharacterBasic : MonoBehaviour 
{
	IEnumerator leftFade() 
	{
		for (float f = 32f; f >= 0; f -= 1f) 
		{
			transform.position = new Vector2 (transform.position.x - .01f, transform.position.y);
			yield return new WaitForSeconds(.01f);
		}
	}

	IEnumerator rightFade() 
	{
		for (float f = 32f; f >= 0; f -= 1f) 
		{
			transform.position = new Vector2 (transform.position.x + .01f, transform.position.y);
			yield return new WaitForSeconds(.01f);
		}
	}

	IEnumerator upFade() 
	{
		for (float f = 32f; f >= 0; f -= 1f) 
		{
			transform.position = new Vector2(transform.position.x, transform.position.y + .01f);
			yield return new WaitForSeconds(.01f);
		}
	}

	IEnumerator downFade() 
	{
		for (float f = 32f; f >= 0; f -= 1f) 
		{
			transform.position = new Vector2(transform.position.x, transform.position.y - .01f);
			yield return new WaitForSeconds(.01f);
		}
	}
	
	void Update() 
	{

		if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			StartCoroutine("leftFade");
		} 
		else if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			StartCoroutine("rightFade");
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			StartCoroutine("upFade");
		}
		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			StartCoroutine("downFade");
		}
	}
}