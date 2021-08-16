using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class IndexManager : NetworkBehaviour
{
    private NetworkVariable<int> m_index = new NetworkVariable<int>();

    public override void NetworkStart()
    {
        if (IsServer)
        {
            m_index.Value = 0;
        }
    }

    [ServerRpc]
    public void UpdateIndexServerRpc()
    {
        m_index.Value++;
    }

    public int GetNextIndex()
    {
        int tmp = m_index.Value;
        UpdateIndexServerRpc();
        return tmp;
    }
}
