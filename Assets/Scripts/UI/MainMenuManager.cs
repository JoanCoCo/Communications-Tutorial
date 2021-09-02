using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private ConnectionConfig m_connectionConfig;

    [SerializeField] private TMP_InputField addressInput;
    [SerializeField] private TMP_InputField portInput;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private Toggle hostToggle;

    private void Start()
    {
        m_connectionConfig = new ConnectionConfig();
        m_connectionConfig.ip = addressInput.text;
        m_connectionConfig.port = int.Parse(portInput.text);
        m_connectionConfig.isHost = hostToggle.isOn;
        Save();
    }

    private void Save()
    {
        m_connectionConfig.SaveConfiguration();
        PlayerPrefs.SetString(UserPrefKey.USER_NAME, nameInput.text);
    }

    public void UpdateAddress(string address)
    {
        m_connectionConfig.ip = addressInput.text;
    }

    public void UpdatePort(string port)
    {
        if (portInput.text != "") m_connectionConfig.port = int.Parse(portInput.text);
        else Debug.Log("Empty port");
    }

    public void UpdateIsHost(bool isHost)
    {
        m_connectionConfig.isHost = hostToggle.isOn;
    }

    public void OpenConferenceRoom()
    {
        Save();
        SceneManager.LoadScene("ChatScene");
    }
}
