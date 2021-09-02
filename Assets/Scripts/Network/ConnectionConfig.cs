using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionConfig
{
    private string m_ip;
    private int m_port;
    private bool m_isHost;

    public string ip
    {
        get
        {
            return m_ip;
        }

        set
        {
            m_ip = value;
            Debug.Log("Address: " + m_ip.ToString());
        }
    }

    public int port
    {
        get
        {
            return m_port;
        }

        set
        {
            m_port = value;
            Debug.Log("Port: " + m_port.ToString());
        }
    }

    public bool isHost
    {
        get
        {
            return m_isHost;
        }

        set
        {
            m_isHost = value;
            Debug.Log("Is host: " + m_isHost.ToString());
        }
    }

    public ConnectionConfig()
    {
        LoadConfiguration();
    }

    public void SaveConfiguration()
    {
        PlayerPrefs.SetString(UserPrefKey.SERVER_IP, m_ip);
        PlayerPrefs.SetInt(UserPrefKey.SERVER_PORT, m_port);
        PlayerPrefs.SetInt(UserPrefKey.IS_HOST, m_isHost ? 1 : 0);
    }

    public void LoadConfiguration()
    {
        m_ip = PlayerPrefs.GetString(UserPrefKey.SERVER_IP, "127.0.0.1");
        m_port = PlayerPrefs.GetInt(UserPrefKey.SERVER_PORT, 7777);
        m_isHost = PlayerPrefs.GetInt(UserPrefKey.IS_HOST, 1) == 1;
    }
}
