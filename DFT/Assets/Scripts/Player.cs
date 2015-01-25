using UnityEngine;
using System.Collections;
//using System.Threading.Tasks;

public class Player: MonoBehaviour 
{
	[SerializeField] private bool isDebug = false;
	[SerializeField] private ArrowSelection arrows;

	private Animator animator;

	void Start ()
	{
		animator = this.GetComponent<Animator>();
		animator.StopPlayback ();
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
	
	void Update() 
	{
		if(isDebug){
			animator.StopPlayback ();
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

			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				arrows.Select(0);
				//animator.SetInteger("Direction", 2);
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				arrows.Select(1);
				//animator.SetInteger("Direction", 0);
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				arrows.Select(2);
				//animator.SetInteger("Direction", 1);
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				arrows.Select(3);
				//animator.SetInteger("Direction", 3);
			}

			if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				arrows.Reset();
			}
		}
	}
}
