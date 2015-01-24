using UnityEngine;
using System.Collections;

public class MoveCharacterBasic : MonoBehaviour {

	public delegate void MoveGuyEvent();
	/*public static event MoveGuyEvent OnTurnLeft; 
	public static event MoveGuyEvent OnTurnRight; 
	public static event MoveGuyEvent OnMoveLeft; 
	public static event MoveGuyEvent OnMoveRight; 
	public static event MoveGuyEvent OnStopMovining; */

	public event MoveGuyEvent OnTurnLeft; 
	public event MoveGuyEvent OnTurnRight; 
	public event MoveGuyEvent OnMoveLeft; 
	public event MoveGuyEvent OnMoveRight; 
	public event MoveGuyEvent OnStopMovining;

	
	private KeyCode m_CurrentKey = KeyCode.None;
	private bool m_Pressed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			m_Pressed = true;
			if(m_CurrentKey != KeyCode.LeftArrow)
			{
				m_CurrentKey = KeyCode.LeftArrow;
				if(OnTurnLeft != null)
					OnTurnLeft();
			}
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			m_Pressed = true;
			if(m_CurrentKey != KeyCode.RightArrow)
			{
				m_CurrentKey = KeyCode.RightArrow;
				if(OnTurnRight != null)
					OnTurnRight();
			}
		}
		else if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
		{
			m_Pressed = false;
			if(OnStopMovining != null)
				OnStopMovining();
		}
		else if(m_Pressed && m_CurrentKey == KeyCode.LeftArrow)
		{
			if(OnMoveLeft != null)
				OnMoveLeft();
		}
		else if(m_Pressed && m_CurrentKey == KeyCode.RightArrow)
		{
			if(OnMoveRight != null)
				OnMoveRight();
		}

	
	}
}
