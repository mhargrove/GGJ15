using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	[SerializeField] private Image m_BarForeground;
	[SerializeField] private Color m_Normal;
	[SerializeField] private Color m_Dying;


	// Use this for initialization
	void Start () {
		m_BarForeground.color = m_Normal;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateHealth(float percent)
	{
		m_BarForeground.fillAmount = percent;

		if (percent < 0.2)
				m_BarForeground.color = m_Dying;

	}
}
