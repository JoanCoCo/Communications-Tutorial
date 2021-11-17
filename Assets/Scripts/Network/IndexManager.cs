using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

/// <summary>
/// Class that implements the system responsible of managing the user indexes.
/// </summary>
public class IndexManager : NetworkBehaviour
{
    /// <summary>
    /// Currently available index. It must be a network variable so the next available
    /// index is visible from all the peers.
    /// </summary>
    private NetworkVariable<int> m_index = new NetworkVariable<int>();

    /// <summary>
    /// Has the system been correctly started?
    /// </summary>
    private bool m_isOn = false;

    /// <summary>
    /// Property to queary if the manager has been started.
    /// </summary>
    public bool IsOn
    {
        get
        {
            return m_isOn;
        }
    }

    /// <summary>
    /// Method that is ran by MLAPI once the connection is stablished and the
    /// object is created.
    /// </summary>
    public override void NetworkStart()
    {
        if (IsServer)
        {
            m_index.Value = 0;
        }

        m_isOn = true;
    }

    /// <summary>
    /// RPC make the server update the available index to the next one. RequireOwnership
    /// must be false so any IndexManager instance can request the update of the
    /// available index.
    /// </summary>
    [ServerRpc(RequireOwnership = false)]
    public void UpdateIndexServerRpc()
    {
        m_index.Value += 1;
        Debug.Log("Available index value updated to: " + m_index.Value);
    }

    /// <summary>
    /// Method to obtain the available index.
    /// </summary>
    public int GetNextIndex()
    {
        int tmp = m_index.Value; // Obtain the current value to be returned.
        UpdateIndexServerRpc(); // Ask the server to update the available index.
        return tmp;
    }
}
