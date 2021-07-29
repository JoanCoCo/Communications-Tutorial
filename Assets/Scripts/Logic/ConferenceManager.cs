using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Transports.UNET;

public class ConferenceManager : MonoBehaviour
{
    [SerializeField]
    private NetworkManager netManager;

    [SerializeField]
    private MediaCommunicationManager mediaCommunicationManager;

    private ConnectionConfig config;
    private UNetTransport transport;
    private GameObject localUser;

    private void Start()
    {
        config = new ConnectionConfig();

        if (netManager != null)
        {
            transport = netManager.GetComponent<UNetTransport>();
        }

        if (transport != null)
        {
            if (config.isHost)
            {
                transport.ServerListenPort = config.port;
                netManager.StartHost();
            }
            else
            {
                transport.ConnectPort = config.port;
                transport.ConnectAddress = config.ip;
                netManager.StartClient();
            }

            SetUpUser();
        }
    }

    private Coroutine SetUpUser() => StartCoroutine(WaitForUser());

    private IEnumerator WaitForUser()
    {
        while(!netManager.ConnectedClients.ContainsKey(netManager.LocalClientId) ||
            netManager.ConnectedClients[netManager.LocalClientId].PlayerObject == null ||
            netManager.ConnectedClients[netManager.LocalClientId].PlayerObject.gameObject == null)
        {
            yield return new WaitForSecondsRealtime(0.05f);
        }

        localUser = netManager.ConnectedClients[netManager.LocalClientId].PlayerObject.gameObject;

        if(mediaCommunicationManager != null)
        {
            mediaCommunicationManager.SetLocalUser(localUser);
        }
    }
}
