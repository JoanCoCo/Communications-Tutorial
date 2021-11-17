using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using TMPro;

/// <summary>
/// Class that implements a user.
/// </summary>
public class User : NetworkBehaviour
{
    /// <summary>
    /// User's index in the display grid. It must be synced between all the
    /// network instances as all of them will occupy the same place in the grid.
    /// </summary>
    private NetworkVariable<int> m_index = new NetworkVariable<int>();

    /// <summary>
    /// User's name. It must be synced between all the network instances to allow
    /// all the connected clients know the name of this user.
    /// </summary>
    private NetworkVariable<string> m_name = new NetworkVariable<string>();

    /// <summary>
    /// TMP Text to display the user's name. Must be linked with the user prefab.
    /// </summary>
    [SerializeField] private TMP_Text m_nameField;

    /// <summary>
    /// Method that is ran by MLAPI once the connection is stablished and the
    /// object is created.
    /// </summary>
    public override void NetworkStart()
    {
        if(IsLocalPlayer) // If the instance is on the user's device
        {
            // Broadcast an event to comunicate the user's object creation.
            Messenger<GameObject>.Broadcast(Event.LOCAL_USER_CREATED, gameObject);
            // Update the user's name from the stored value in the user preferences.
            SetNameServerRpc(PlayerPrefs.GetString(UserPrefKey.USER_NAME));
        }

        m_nameField.text = m_name.Value;
        m_name.OnValueChanged += OnNameChanged; // Sign up to the changes in the user's name value.
    }

    /// <summary>
    /// RPC to make the server instance to update the user's name.
    /// </summary>
    /// <param name="name">User's new name.</param>
    [ServerRpc]
    private void SetNameServerRpc(string name)
    {
        m_name.Value = name;
    }

    /// <summary>
    /// Callback to register changes on the user's name. It will be triggered
    /// when the server instance modifies the Network Variable's value.
    /// </summary>
    /// <param name="oldName">Previous value.</param>
    /// /// <param name="newName">New value.</param>
    private void OnNameChanged(string oldName, string newName)
    {
        m_nameField.text = newName;
    }

    /// <summary>
    /// Method to update the user's index value. It will sync this update with all
    /// the network instances.
    /// </summary>
    /// <param name="name">User's new index.</param>
    public void AssignIndex(int index)
    {
        AssignIndexServerRpc(index);
    }

    /// <summary>
    /// Property to access directly the user's index integer value from inside
    /// the Network Variable.
    /// </summary>
    /// <returns>Integer representing the user's index in the display grid.</returns>
    public int index
    {
        get
        {
            return m_index.Value;
        }
    }

    /// <summary>
    /// RPC to make the server instance to update the user's index.
    /// </summary>
    /// <param name="index">User's new index.</param>
    [ServerRpc]
    private void AssignIndexServerRpc(int index)
    {
        m_index.Value = index;
        Messenger.Broadcast(Event.NEW_USER_REGISTERED);
    }

    /// <summary>
    /// Method to update the user's position in the 2D scene.
    /// </summary>
    /// <param name="position">User's new position.</param>
    public void PlaceAt(Vector2 position)
    {
        PlaceAtClientRpc(position);
    }

    /// <summary>
    /// RPC to update the user's position in the 2D scene in all the clients.
    /// </summary>
    /// <param name="position">User's new position.</param>
    [ClientRpc]
    private void PlaceAtClientRpc(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    /// <summary>
    /// Before destroying the object, sign down to the changes in the user's name
    /// value to avoid errors.
    /// </summary>
    private void OnDestroy()
    {
        m_name.OnValueChanged -= OnNameChanged;
    }
}
