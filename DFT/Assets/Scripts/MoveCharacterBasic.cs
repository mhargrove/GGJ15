using UnityEngine;
using System.Collections;

public class MoveCharacterBasic : MonoBehaviour 
{
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			transform.position = new Vector2(transform.position.x - .32f, transform.position.y);
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			transform.position = new Vector2(transform.position.x + .32f, transform.position.y);
		}
		else if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			transform.position = new Vector2(transform.position.x, transform.position.y + .32f);
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			transform.position = new Vector2(transform.position.x, transform.position.y - .32f);
		}
	}
}
