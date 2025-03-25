using UnityEngine;

public class wait_gui : MonoBehaviour
{
	public GameObject m_normal;

	public GameObject m_sp;

	public GameObject m_text;

	public GameObject m_ltext;

	private int m_index;

	private void OnEnable()
	{
		InvokeRepeating("time", 0.5f, 0.5f);
	}

	private void OnDisable()
	{
		CancelInvoke("time");
	}

	public void reset(bool flag, string text)
	{
		if (!flag)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		if (text == string.Empty)
		{
			m_normal.SetActive(true);
			m_sp.SetActive(false);
		}
		else
		{
			m_sp.SetActive(true);
			m_normal.SetActive(false);
			m_text.GetComponent<UILabel>().text = text;
		}
	}

	private void time()
	{
		m_index = (m_index + 1) % 3;
		string text = "Loading";
		for (int i = 0; i < m_index; i++)
		{
			text += ".";
		}
		m_ltext.GetComponent<UILabel>().text = text;
	}
}
