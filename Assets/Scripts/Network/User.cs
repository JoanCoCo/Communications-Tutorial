using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class User : NetworkBehaviour
{
    private NetworkVariable<int> m_index = new NetworkVariable<int>();

    public void AssignIndex(int index)
    {
        AssignIndexServerRpc(index);
    }

    public int index
    {
        get
        {
            return m_index.Value;
        }
    }

    [ServerRpc]
    private void AssignIndexServerRpc(int index)
    {
        m_index.Value = index;
        ComunicateRegisteringClientRpc();
    }

    [ClientRpc]
    private void ComunicateRegisteringClientRpc()
    {
        Messenger.Broadcast(Event.NEW_USER_REGISTERED);
    }

    public void PlaceAt(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
}
