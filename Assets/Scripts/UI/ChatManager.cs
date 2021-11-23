using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MLAPI;
using MLAPI.Messaging;

/// <summary>
/// Class that implements the manager for the chat system. It is resposible of
/// sending an receiving the messages.
/// </summary>
[RequireComponent(typeof(ChatDisplay))]
public class ChatManager : NetworkBehaviour
{
    /// <summary>
    /// Reference to the input field used to write the messages.
    /// </summary>
    [SerializeField] private TMP_InputField inputText;

    /// <summary>
    /// Reference to the chat display.
    /// </summary>
    private ChatDisplay display;

    /// <summary>
    /// Local user's name.
    /// </summary>
    private string myPlayerName;

    /// <summary>
    /// Reference to itself. Used to guarantee the ChatManager will be a singleton
    /// per peer.
    /// </summary>
    private static ChatManager _instance = null;

    /// <summary>
    /// Method that is ran when the object is created.
    /// </summary>
    private void Start()
    {
        if(_instance != null && _instance != this) // If there is already a ChatManager, replace it by this one.
        {
            Destroy(_instance.gameObject);
        }
        _instance = this;
        display = GetComponent<ChatDisplay>(); 
        inputText.onEndEdit.AddListener(OnSendNewMessage);
        myPlayerName = PlayerPrefs.GetString(UserPrefKey.USER_NAME);
    }

    /// <summary>
    /// Callback to be run when the user wants to send a new message.
    /// </summary>
    private void OnSendNewMessage(string msg)
    {
        string pmsg = myPlayerName + " - " + msg; // Append the user's name.
        if(IsServer) // If I'm the server, send to all clients.
        {
            Debug.Log("I'm server, running rpc.");
            SendMessageClientRpc(pmsg, NetworkManager.LocalClientId);
        }
        else // If I'm a client, send to the server.
        {
            Debug.Log("I'm client, running command.");
            SendMessageServerRpc(pmsg, NetworkManager.LocalClientId);
        }
        inputText.text = ""; // Clear the input field.
    }

    /// <summary>
    /// RPC to make the server resend the message to every client. RequireOwnership
    /// must be false so any peer instance can send a message.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    private void SendMessageServerRpc(string msg, ulong id)
    {
        SendMessageClientRpc(msg, id);
    }

    /// <summary>
    /// RPC to make all the clients update their chat displays with a new message.
    /// </summary>
    [ClientRpc]
    private void SendMessageClientRpc(string msg, ulong id)
    {
        display.AddMessage(msg, id);
    }
}
