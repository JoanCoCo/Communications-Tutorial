using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that implements the manager for the main menu.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    /// <summary>
    /// Connection configuration object.
    /// </summary>
    private ConnectionConfig m_connectionConfig;

    /// <summary>
    /// Input field for the server's IP address.
    /// </summary>
    [SerializeField] private TMP_InputField addressInput;

    /// <summary>
    /// Input field for the server's port.
    /// </summary>
    [SerializeField] private TMP_InputField portInput;

    /// <summary>
    /// Input field for the user's name.
    /// </summary>
    [SerializeField] private TMP_InputField nameInput;

    /// <summary>
    /// Toggle for choosing to be the host or a client.
    /// </summary>
    [SerializeField] private Toggle hostToggle;

    /// <summary>
    /// Method that is ran when the object is created.
    /// </summary>
    private void Start()
    {
        m_connectionConfig = new ConnectionConfig();
        m_connectionConfig.ip = addressInput.text;
        m_connectionConfig.port = int.Parse(portInput.text);
        m_connectionConfig.isHost = hostToggle.isOn;
        Save();
    }

    /// <summary>
    /// Method for saving the connection configuration.
    /// </summary>
    private void Save()
    {
        m_connectionConfig.SaveConfiguration();
        PlayerPrefs.SetString(UserPrefKey.USER_NAME, nameInput.text);
    }

    /// <summary>
    /// Method for updating the IP address.
    /// </summary>
    /// <param name="address">The server's IP address.</param>
    public void UpdateAddress(string address)
    {
        m_connectionConfig.ip = addressInput.text;
    }

    /// <summary>
    /// Method for updating the port.
    /// </summary>
    /// <param name="port">The server's port.</param>
    public void UpdatePort(string port)
    {
        if (portInput.text != "") m_connectionConfig.port = int.Parse(portInput.text);
        else Debug.Log("Empty port");
    }

    /// <summary>
    /// Method for updating if the user will be the host.
    /// </summary>
    /// <param name="isHost">Boolean value saying the user must be the host.</param>
    public void UpdateIsHost(bool isHost)
    {
        m_connectionConfig.isHost = hostToggle.isOn;
    }

    /// <summary>
    /// Method for opening the conference room scene.
    /// </summary>
    public void OpenConferenceRoom()
    {
        Save();
        SceneManager.LoadScene("ChatScene");
    }
}
