using UnityEngine;
using System.Collections;
//using System.Threading.Tasks;

public class Player: MonoBehaviour 
{
	[SerializeField] private bool isDebug = false;
	[SerializeField] private ArrowSelection arrows;

	private Animator animator;
	private int curr;

	void Start ()
	{
		animator = this.GetComponent<Animator>();
		animator.SetInteger("Direction", 4);
	}

	IEnumerator leftFade() {
		for (float f = 31f; f >= 0; f -= 1f) {
			this.gameObject.transform.position = new Vector2 (transform.position.x - .01f, transform.position.y);
			yield return new WaitForSeconds(.01f);
		}
	}

	IEnumerator rightFade() {
		for (float f = 31f; f >= 0; f -= 1f) {
			this.gameObject.transform.position = new Vector2 (transform.position.x + .01f, transform.position.y);
			yield return new WaitForSeconds(.01f);
		}
	}

	IEnumerator upFade() {
		for (float f = 31f; f >= 0; f -= 1f) {
			this.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y + .01f);
			yield return new WaitForSeconds(.01f);
		}
	}

	IEnumerator downFade() {
		for (float f = 31f; f >= 0; f -= 1f) {
			this.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y - .01f);
			yield return new WaitForSeconds(.01f);
		}
	}

	public void moveUp(){StartCoroutine("upFade");}
	public void moveLeft(){StartCoroutine("leftFade");}
	public void moveRight(){StartCoroutine("rightFade");}
	public void moveDown(){StartCoroutine("downFade");}

	public void teleport(Vector2 pos){
		this.gameObject.transform.position = pos;
	}

	public void MoveSprite (int dir)
	{
		if (dir == 0) 
		{
			arrows.Select(0);
			animator.SetInteger("Direction", 2);
			curr = 6;
		} 
		else if (dir == 1) 
		{
			arrows.Select(1);
			animator.SetInteger("Direction", 0);
			curr = 4;
		}
		else if (dir == 2) 
		{
			arrows.Select(2);
			animator.SetInteger("Direction", 1);
			curr = 5;
		}
		else if (dir == 3) 
		{
			arrows.Select(3);
			animator.SetInteger("Direction", 3);
			curr = 7;
		}
	}

	public void StopSprite ()
	{
		animator.SetInteger("Direction", curr);
	}

	void Update() 
	{
		if(isDebug){
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

			if (Input.GetKeyDown (KeyCode.LeftArrow)) 
			{
				MoveSprite (2);
				curr = 6;
			} 
			else if (Input.GetKeyDown (KeyCode.RightArrow)) 
			{
				MoveSprite (3);
				curr = 7;
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				MoveSprite (0);
				curr = 5;
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				MoveSprite (1);
				curr = 4;
			}

			if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				arrows.Reset ();
				StopSprite ();
			}
		}
	}
}
