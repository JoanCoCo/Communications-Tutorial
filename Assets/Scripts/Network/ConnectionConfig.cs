using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that implements the connection configuration information container.
/// </summary>
public class ConnectionConfig
{
    /// <summary>
    /// Server IP address.
    /// </summary>
    private string m_ip;

    /// <summary>
    /// Server port.
    /// </summary>
    private int m_port;

    /// <summary>
    /// Is this device the host?
    /// </summary>
    private bool m_isHost;

    /// <summary>
    /// Accessor property for the server IP address.
    /// </summary>
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

    /// <summary>
    /// Accessor property for the server port.
    /// </summary>
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

    /// <summary>
    /// Accessor property for querying if this device is a host.
    /// </summary>
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

    /// <summary>
    /// Base constructor. It loads the configuration saved in the user preferences.
    /// </summary>
    public ConnectionConfig()
    {
        LoadConfiguration();
    }

    /// <summary>
    /// Method for saving in the user preferences the current configuration.
    /// </summary>
    public void SaveConfiguration()
    {
        PlayerPrefs.SetString(UserPrefKey.SERVER_IP, m_ip);
        PlayerPrefs.SetInt(UserPrefKey.SERVER_PORT, m_port);
        PlayerPrefs.SetInt(UserPrefKey.IS_HOST, m_isHost ? 1 : 0);
    }

    /// <summary>
    /// Method for updating the configuration from the stored parameters in the
    /// user preferences.
    /// </summary>
    public void LoadConfiguration()
    {
        m_ip = PlayerPrefs.GetString(UserPrefKey.SERVER_IP, "127.0.0.1");
        m_port = PlayerPrefs.GetInt(UserPrefKey.SERVER_PORT, 7777);
        m_isHost = PlayerPrefs.GetInt(UserPrefKey.IS_HOST, 1) == 1;
    }
}
