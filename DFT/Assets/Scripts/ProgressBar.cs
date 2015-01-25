using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour
{
	[SerializeField] private Image m_BarForeground;
	[SerializeField] private Color m_Normal;
	[SerializeField] private Color m_Dying;
	
	public float fill;
	
	void Start ()
	{
		m_BarForeground.color = m_Normal;
	}
	
	void Update ()
	{
		int val = 100;
		if (Input.GetKeyDown (KeyCode.H))
		{
			if(val > 0)
				val -= 1;
			UpdateHealth(val);
		}
		fill = m_BarForeground.fillAmount;
	}
	
	public void UpdateHealth(int percent)
	{
		m_BarForeground.fillAmount = percent * 0.01f;
		
		if (percent < 20)
						m_BarForeground.color = m_Dying;
				else
						m_BarForeground.color = m_Normal;		
	}
}
