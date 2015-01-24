using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour 
{
	[SerializeField] private Image m_BarForeground;
	[SerializeField] private Color m_Normal;
	[SerializeField] private Color m_Dying;

	void Start () 
	{
		m_BarForeground.color = m_Normal;
	}

	void Update () 
	{
		float val = -0.01f;
		if (Input.GetKeyDown (KeyCode.H)) 
		{
			UpdateHealth(val);
		}
	}

	public void UpdateHealth(float percent)
	{
		m_BarForeground.fillAmount = percent;

		if (percent < 0.2)
			m_BarForeground.color = m_Dying;
	}
}
