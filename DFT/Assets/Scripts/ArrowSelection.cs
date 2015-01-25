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

	public void Select (int dir)
	{
		if (dir == 0) 
		{
			m_UpArrow.color = m_Selected;
			m_DownArrow.color = m_Default;
			m_LeftArrow.color = m_Default;
			m_RightArrow.color = m_Default;
		}
		else if (dir == 1) 
		{
			m_UpArrow.color = m_Default;
			m_DownArrow.color = m_Selected;
			m_LeftArrow.color = m_Default;
			m_RightArrow.color = m_Default;
		}
		else if (dir == 2) 
		{
			m_UpArrow.color = m_Default;
			m_DownArrow.color = m_Default;
			m_LeftArrow.color = m_Selected;
			m_RightArrow.color = m_Default;
		}
		else if (dir == 3) 
		{
			m_UpArrow.color = m_Default;
			m_DownArrow.color = m_Default;
			m_LeftArrow.color = m_Default;
			m_RightArrow.color = m_Selected;
		}
	}
}
