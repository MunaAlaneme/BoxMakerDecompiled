using UnityEngine;

public class login_gui_main : MonoBehaviour
{
	public GameObject m_play;

	public GameObject m_edit;

	public GameObject m_go;

	private void OnEnable()
	{
		if (mario._instance.m_self.guide == 0)
		{
			m_play.SetActive(false);
			m_edit.SetActive(false);
			m_go.SetActive(true);
		}
		else
		{
			m_play.SetActive(true);
			m_edit.SetActive(true);
			m_go.SetActive(false);
		}
	}
}
