using UnityEngine;

public class login_anim : MonoBehaviour
{
	public GameObject m_t2;

	public GameObject m_t3;

	public GameObject m_t4;

	public GameObject m_t5;

	public GameObject m_t6;

	public GameObject m_t7;

	public GameObject m_dj;

	private void OnEnable()
	{
		m_t2.SetActive(false);
		m_t3.SetActive(false);
		m_t4.SetActive(false);
		m_t5.SetActive(false);
		m_t6.SetActive(false);
		m_t7.SetActive(false);
	}

	private void t2()
	{
		m_t2.SetActive(true);
	}

	private void t3()
	{
		m_t3.SetActive(true);
	}

	private void t4()
	{
		m_t4.SetActive(true);
	}

	private void t5()
	{
		m_t5.SetActive(true);
	}

	private void t6()
	{
		m_t6.SetActive(true);
	}

	private void t7()
	{
		m_t7.SetActive(true);
	}

	private void end()
	{
		m_dj.SetActive(true);
	}
}
