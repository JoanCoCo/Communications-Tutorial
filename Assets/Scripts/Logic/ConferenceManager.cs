using System.Collections;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;

/// <summary>
/// Class that implements the local system responsible of managing the conference.
/// </summary>
public class ConferenceManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the local Network Manager.
    /// </summary>
    [SerializeField]
    private NetworkManager m_netManager;

    /// <summary>
    /// Reference to the local Media Communication Manager.
    /// </summary>
    [SerializeField]
    private MediaCommunicationManager m_mediaCommunicationManager;

    /// <summary>
    /// Reference to the local connection configuration.
    /// </summary>
    private ConnectionConfig m_config;

    /// <summary>
    /// Reference to the local network transport.
    /// </summary>
    private UNetTransport m_transport;

    /// <summary>
    /// Reference to the local user object.
    /// </summary>
    private GameObject m_localUser;

    /// <summary>
    /// Reference to the grid view.
    /// </summary>
    private ArrangedView m_arrangedView;

    /// <summary>
    /// Width of the grid view.
    /// </summary>
    [SerializeField]
    private int m_width;

    /// <summary>
    /// Height of the grid view.
    /// </summary>
    [SerializeField]
    private int m_height;

    /// <summary>
    /// Reference to the scene camera.
    /// </summary>
    [SerializeField]
    private Camera m_camera;

    /// <summary>
    /// Reference to the indexes manager.
    /// </summary>
    [SerializeField]
    private IndexManager m_indexManager;

    /// <summary>
    /// Index of the local user.
    /// </summary>
    private int m_localUserIndex;

    /// <summary>
    /// Method that is ran when the object is created.
    /// </summary>
    private void Start()
    {
        // Load the connection configuration.
        m_config = new ConnectionConfig();
        m_config.LoadConfiguration();
        // Sign up to the event associated with the creation of the local user object.
        Messenger<GameObject>.AddListener(Event.LOCAL_USER_CREATED, OnLocalUserCreated);

        // If the user wants to be a host, set up the grid view and sign up to
        // the event associated with the registration of a new user in the conference.
        if (m_config.isHost)
        {
            m_arrangedView = new ArrangedView(m_width, m_height, m_camera);
            Messenger.AddListener(Event.NEW_USER_REGISTERED, OnNewUserRegistered);
        }

        // Obtain the Network Transport.
        if (m_netManager != null)
        {
            m_transport = m_netManager.GetComponent<UNetTransport>();
        }

        if (m_transport != null) // If the transport was correctly obtained.
        {
            if (m_config.isHost) // If we are the host,
            {
                m_transport.ServerListenPort = m_config.port; // Set up the listening port.
                m_netManager.StartHost(); // Ask MLAPI's Network Manager to start a host.
            }
            else // If we are a client,
            {
                m_transport.ConnectPort = m_config.port; // Set up the host port.
                m_transport.ConnectAddress = m_config.ip; // Set up the host address.
                m_netManager.StartClient(); // Ask MLAPI's Network Manager to start a client connected to the host.
            }
            ObtainIndex(); // Obtain the index in the grid for our local user.
        }
    }

    /// <summary>
    /// Callback to be run when the local user's object is created to store the
    /// reference to it.
    /// </summary>
    /// <param name="o">User's GameObject.</param>
    private void OnLocalUserCreated(GameObject o)
    {
        m_localUser = o;
    }

    /// <summary>
    /// Coroutine to set up properly the local user. It will wait until the local
    /// user is created and then it will link the user object to the communication
    /// manager. It will also assing the first available index to the user.
    /// </summary>
    private Coroutine SetUpUser() => StartCoroutine(WaitForUser());

    /// <summary>
    /// IEnumerator that implements the courutine to set up the user.
    /// </summary>
    private IEnumerator WaitForUser()
    {
        while(m_localUser == null)
        {
            yield return new WaitForSecondsRealtime(0.05f);
        }

        Debug.Log("Local user found.");

        if(m_mediaCommunicationManager != null)
        {
            m_mediaCommunicationManager.SetLocalUser(m_localUser);
        }

        m_localUser.GetComponent<User>().AssignIndex(m_localUserIndex);
    }

    /// <summary>
    /// Coroutine to obtain the local user's index. It will wait until the index
    /// manager is activated and then request an index. Once the index is obtained,
    /// the local user is set up.
    /// </summary>
    private Coroutine ObtainIndex() => StartCoroutine(WaitForIndex());

    /// <summary>
    /// IEnumerator that implements the courutine to obtain an index.
    /// </summary>
    private IEnumerator WaitForIndex()
    {
        while(!m_indexManager.IsOn)
        {
            yield return new WaitForSecondsRealtime(0.05f);
        }
        m_localUserIndex = m_indexManager.GetNextIndex();
        Debug.Log("Obtained index: " + m_localUserIndex);
        SetUpUser();
    }

    /// <summary>
    /// Callback to be run when a new remote user joins the conference.
    /// </summary>
    private void OnNewUserRegistered()
    {
        GameObject[] users = GameObject.FindGameObjectsWithTag("User"); // Obtain all user objects.

        foreach (var userObj in users) // Update all their positions in the grid based on their index.
        {
            User user = userObj.GetComponent<User>();
            user.PlaceAt(m_arrangedView.GetPositionInView(user.index));
        }
    }

    /// <summary>
    /// Before destroying the object, sign down to the events we sign up for to
    /// avoid errors.
    /// </summary>
    private void OnDestroy()
    {
        if(m_config.isHost) Messenger.RemoveListener(Event.NEW_USER_REGISTERED, OnNewUserRegistered);
        Messenger<GameObject>.RemoveListener(Event.LOCAL_USER_CREATED, OnLocalUserCreated);
    }
}
