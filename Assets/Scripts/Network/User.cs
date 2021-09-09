using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;

public class User : NetworkBehaviour
{
    private NetworkVariable<int> m_index = new NetworkVariable<int>();

    public override void NetworkStart()
    {
        if(IsLocalPlayer)
        {
            Messenger<GameObject>.Broadcast(Event.LOCAL_USER_CREATED, gameObject);
        }
    }

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
        Messenger.Broadcast(Event.NEW_USER_REGISTERED);
    }

    public void PlaceAt(Vector2 position)
    {
        PlaceAtClientRpc(position);
    }

    [ClientRpc]
    private void PlaceAtClientRpc(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
}
