using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using TMPro;

public class User : NetworkBehaviour
{
    private NetworkVariable<int> m_index = new NetworkVariable<int>();
    private NetworkVariable<string> m_name = new NetworkVariable<string>();

    [SerializeField] private TMP_Text m_nameField;

    public override void NetworkStart()
    {
        if(IsLocalPlayer)
        {
            Messenger<GameObject>.Broadcast(Event.LOCAL_USER_CREATED, gameObject);
            SetNameServerRpc(PlayerPrefs.GetString(UserPrefKey.USER_NAME));
        }

        m_nameField.text = m_name.Value;
        m_name.OnValueChanged += OnNameChanged;
    }

    [ServerRpc]
    private void SetNameServerRpc(string name)
    {
        m_name.Value = name;
    }

    private void OnNameChanged(string oldName, string newName)
    {
        m_nameField.text = newName;
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

    private void OnDestroy()
    {
        m_name.OnValueChanged -= OnNameChanged;
    }
}
