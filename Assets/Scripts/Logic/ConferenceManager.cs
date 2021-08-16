using System.Collections;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;

public class ConferenceManager : MonoBehaviour
{
    [SerializeField]
    private NetworkManager m_netManager;

    [SerializeField]
    private MediaCommunicationManager m_mediaCommunicationManager;

    private ConnectionConfig m_config;
    private UNetTransport m_transport;
    private GameObject m_localUser;

    private ArrangedView m_arrangedView;

    [SerializeField]
    private int m_width;

    [SerializeField]
    private int m_height;

    [SerializeField]
    private Camera m_camera;

    [SerializeField]
    private IndexManager m_indexManager;

    private int m_localUserIndex;

    private void Start()
    {
        m_config = new ConnectionConfig();

        if(m_config.isHost)
        {
            m_arrangedView = new ArrangedView(m_width, m_height, m_camera);
        }

        if (m_netManager != null)
        {
            m_transport = m_netManager.GetComponent<UNetTransport>();
        }

        if (m_transport != null)
        {
            if (m_config.isHost)
            {
                m_transport.ServerListenPort = m_config.port;
                m_netManager.StartHost();
            }
            else
            {
                m_transport.ConnectPort = m_config.port;
                m_transport.ConnectAddress = m_config.ip;
                m_netManager.StartClient();
            }
            ObtainIndex();
            SetUpUser();
        }

        Messenger.AddListener(Event.NEW_USER_REGISTERED, OnNewUserRegistered);
    }

    private Coroutine SetUpUser() => StartCoroutine(WaitForUser());

    private IEnumerator WaitForUser()
    {
        while(!m_netManager.ConnectedClients.ContainsKey(m_netManager.LocalClientId) ||
            m_netManager.ConnectedClients[m_netManager.LocalClientId].PlayerObject == null ||
            m_netManager.ConnectedClients[m_netManager.LocalClientId].PlayerObject.gameObject == null)
        {
            yield return new WaitForSecondsRealtime(0.05f);
        }

        m_localUser = m_netManager.ConnectedClients[m_netManager.LocalClientId].PlayerObject.gameObject;

        if(m_mediaCommunicationManager != null)
        {
            m_mediaCommunicationManager.SetLocalUser(m_localUser);
        }

        m_localUser.GetComponent<User>().AssignIndex(m_localUserIndex);
    }

    private Coroutine ObtainIndex() => StartCoroutine(WaitForIndex());

    private IEnumerator WaitForIndex()
    {
        while(!m_indexManager.isActiveAndEnabled)
        {
            yield return new WaitForSecondsRealtime(0.05f);
        }
        m_localUserIndex = m_indexManager.GetNextIndex();
    }

    private void OnNewUserRegistered()
    {
        GameObject[] users = GameObject.FindGameObjectsWithTag("User");

        foreach (var userObj in users)
        {
            User user = userObj.GetComponent<User>();
            user.PlaceAt(m_arrangedView.GetPositionInView(user.index));
        }
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(Event.NEW_USER_REGISTERED, OnNewUserRegistered);
    }
}
