using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArrowSelection : MonoBehaviour 
{
	[SerializeField] private Image m_UpArrow;
	[SerializeField] private Image m_DownArrow;
	[SerializeField] private Image m_LeftArrow;
	[SerializeField] private Image m_RightArrow;
	[SerializeField] private Color m_Default;
	[SerializeField] private Color m_Selected;
	
	void Start () 
	{
		m_UpArrow.color = m_Default;
		m_DownArrow.color = m_Default;
		m_LeftArrow.color = m_Default;
		m_RightArrow.color = m_Default;
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			m_UpArrow.color = m_Selected;
		}
		else if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			m_UpArrow.color = m_Default;
		}

		else if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			m_DownArrow.color = m_Selected;
		}
		else if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			m_DownArrow.color = m_Default;
		}

		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			m_LeftArrow.color = m_Selected;
		}
		else if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			m_LeftArrow.color = m_Default;
		}

		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			m_RightArrow.color = m_Selected;
		}
		else if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			m_RightArrow.color = m_Default;
		}
	}
}
