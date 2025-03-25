using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;
using UnityEngine;
using protocol.game;

public class net_http : MonoBehaviour, IMessage
{
	private string m_ip = "http://mario.yymoon.com:8080/";

	private string m_ips = "https://mario.yymoon.com:8080/";

	private string m_en_ip = "http://mario.en.yymoon.com:8080/";

	private string m_en_ips = "https://mario.en.yymoon.com:8080/";

	private List<net_pck> m_pcks = new List<net_pck>();

	private float m_wait;

	private WWW m_www;

	public static net_http _instance;

	void IMessage.net_message(s_net_message message)
	{
	}

	void IMessage.message(s_message message)
	{
		if (message.m_type == "net_restart")
		{
			net_start();
		}
	}

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		cmessage_center._instance.add_handle(this);
	}

	private void OnDestroy()
	{
		cmessage_center._instance.remove_handle(this);
	}

	public static int bytesToInt(byte[] bytes)
	{
		int num = bytes[0];
		num |= bytes[1] << 8;
		num |= bytes[2] << 16;
		return num | (bytes[3] << 24);
	}

	private void onRecMsg(opclient_t opcode, int res, byte[] msg)
	{
		s_net_message s_net_message2 = new s_net_message();
		s_net_message2.m_byte = msg;
		s_net_message2.m_opcode = opcode;
		s_net_message2.m_res = res;
		cmessage_center._instance.add_net_message(s_net_message2);
	}

	private bool do_www(WWW _www, opclient_t opcode)
	{
		if (!string.IsNullOrEmpty(_www.error))
		{
			mario._instance.wait(false, string.Empty);
			m_wait = 0f;
			do_fail();
			Debug.Log("http error  :" + _www.error);
			return false;
		}
		mario._instance.wait(false, string.Empty);
		m_wait = 0f;
		msg_response msg_response = _instance.parse_packet<msg_response>(_www.bytes);
		if (msg_response.res == 0)
		{
			if (mario._instance.m_self != null)
			{
				mario._instance.m_self.m_pck_id++;
			}
			onRecMsg(opcode, msg_response.res, msg_response.msg);
		}
		else if (msg_response.res == -1)
		{
			if (mario._instance.m_self != null)
			{
				mario._instance.m_self.m_pck_id++;
			}
			onRecMsg(opcode, msg_response.res, msg_response.error);
		}
		else
		{
			mario._instance.show_tip(game_data._instance.get_t_error(msg_response.res));
		}
		return true;
	}

	public void send_msg<T>(opclient_t opcode, T obj, bool restart = true, string text = "", float wait = 10f)
	{
		net_pck net_pck2 = new net_pck();
		net_pck2.opcode = opcode;
		net_pck2.obj = obj;
		net_pck2.wait = wait;
		net_pck2.text = text;
		net_pck2.restart = restart;
		m_pcks.Add(net_pck2);
		if (m_pcks.Count <= 1)
		{
			net_start();
		}
	}

	public T parse_packet<T>(byte[] bytes)
	{
		MemoryStream source = new MemoryStream(bytes);
		object obj = new object();
		obj = Serializer.Deserialize<T>(source);
		return (T)obj;
	}

	private void net_start()
	{
		m_wait = m_pcks[0].wait;
		mario._instance.wait(true, m_pcks[0].text);
		Type type = m_pcks[0].obj.GetType();
		if (type.GetProperty("common") != null)
		{
			msg_common msg_common = new msg_common();
			msg_common.userid = mario._instance.m_self.userid;
			msg_common.sig = mario._instance.m_self.m_sig;
			msg_common.pck_id = mario._instance.m_self.m_pck_id;
			type.GetProperty("common").SetValue(m_pcks[0].obj, msg_common, null);
		}
		MemoryStream memoryStream = new MemoryStream();
		Serializer.Serialize(memoryStream, m_pcks[0].obj);
		byte[] msg = encrypt_des.encode(memoryStream.ToArray());
		StartCoroutine(http(m_pcks[0].opcode, msg));
	}

	private void do_fail()
	{
		if (m_pcks[0].restart)
		{
			s_message s_message2 = new s_message();
			s_message2.m_type = "net_restart";
			mario._instance.show_single_dialog_box(game_data._instance.get_language_string("net_http_wlsb"), s_message2);
			return;
		}
		onRecMsg(m_pcks[0].opcode, 1, null);
		m_pcks.RemoveAt(0);
		if (m_pcks.Count > 0)
		{
			net_start();
		}
	}

	private void Update()
	{
		if (m_wait > 0f)
		{
			m_wait -= Time.deltaTime;
			if (m_wait <= 0f)
			{
				StopAllCoroutines();
				mario._instance.wait(false, string.Empty);
				do_fail();
			}
		}
	}

	private IEnumerator http(opclient_t opcode, byte[] msg)
	{
		string ip = m_ip;
		if (game_data._instance.m_channel == "web_facebook")
		{
			ip = m_ips;
		}
		if (game_data._instance.m_lang == e_language.el_english)
		{
			ip = m_en_ip;
			if (game_data._instance.m_channel == "web_facebook")
			{
				ip = m_en_ips;
			}
		}
		net_http obj = this;
		string text = ip;
		int num = (int)opcode;
		obj.m_www = new WWW(text + num, msg);
		while (!m_www.isDone)
		{
			yield return new WaitForSeconds(0.1f);
		}
		if (do_www(m_www, opcode))
		{
			m_pcks.RemoveAt(0);
			if (m_pcks.Count > 0)
			{
				net_start();
			}
		}
	}
}
