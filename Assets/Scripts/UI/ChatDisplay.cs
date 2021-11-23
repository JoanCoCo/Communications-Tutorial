using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MLAPI;

/// <summary>
/// Class that implements the chat display logic.
/// </summary>
public class ChatDisplay : MonoBehaviour
{
    /// <summary>
    /// UI box that will contain the messages.
    /// </summary>
    [SerializeField] private GameObject contentBox;

    /// <summary>
    /// UI box that contains the chat.
    /// </summary>
    [SerializeField] private GameObject chatBox;

    /// <summary>
    /// UI input field for wr¡tting in the chat.
    /// </summary>
    [SerializeField] private GameObject inputBox;

    /// <summary>
    /// Prefab used as container to show the desires message.
    /// </summary>
    [SerializeField] private GameObject messagePrefab;

    /// <summary>
    /// Chat's icon in the UI.
    /// </summary>
    [SerializeField] private GameObject chatIcon;

    /// <summary>
    /// Chat's state.
    /// </summary>
    [SerializeField] private bool isHidden = false;

    /// <summary>
    /// Key for hidding/showing the chat.
    /// </summary>
    private KeyCode hideKey = KeyCode.M;

    /// <summary>
    /// Reference to the InputAvailabilityManager.
    /// </summary>
    private InputAvailabilityManager inputAvailabilityManager;

    /// <summary>
    /// Reference to the NetworkManager.
    /// </summary>
    private NetworkManager networkManager;

    /// <summary>
    /// Method that is ran when the object is created.
    /// </summary>
    private void Start()
    {
        UpdateDisplay();
        //Locate the InputAvailabilityManager.
        inputAvailabilityManager = GameObject.FindWithTag("InputAvailabilityManager").GetComponent<InputAvailabilityManager>();
    }

    /// <summary>
    /// Method that is ran once in every update cycle.
    /// </summary>
    private void Update()
    {
        if((Input.GetKeyDown(hideKey) || Input.GetKeyDown(KeyCode.Escape) && !isHidden) && (inputAvailabilityManager == null || !inputAvailabilityManager.UserIsTyping))
        {
            ChangeState(); // Update the state (Hidden -> Shown -> Hidden).
        }
    }

    /// <summary>
    /// Update the UI elements based on the current state.
    /// </summary>
    private void UpdateDisplay()
    {
        chatBox.SetActive(!isHidden);
        inputBox.SetActive(!isHidden);
        chatIcon.SetActive(isHidden);
    }

    /// <summary>
    /// Method for adding a new message in the chat's UI.
    /// </summary>
    /// <param name="msg">String containing the message.</param>
    /// <param name="id">Network ID of the peer that send the message.</param>
    public void AddMessage(string msg, ulong id)
    {
        // Create the prefab containing the message.
        GameObject omsg = Instantiate(messagePrefab, contentBox.transform);
        TextMeshProUGUI tmsg = omsg.GetComponent<TextMeshProUGUI>();
        tmsg.text = msg;

        // Obtain our ID.
        ulong myPlayerNetId = 0;
        if(networkManager == null)
        {
            var o = GameObject.FindWithTag("NetworkManager");
            if (o != null) {
                networkManager = o.GetComponent<NetworkManager>();
                if(networkManager != null) myPlayerNetId = networkManager.LocalClientId;
            }
        } else
        {
            myPlayerNetId = networkManager.LocalClientId;
        }

        // If the ID is ours, change the message aligment to separate our own sent messages.
        if (myPlayerNetId == id) tmsg.alignment = TextAlignmentOptions.MidlineRight;
        omsg.transform.SetAsLastSibling();
    }

    /// <summary>
    /// Method to toggle the chat's state.
    /// </summary>
    public void ChangeState()
    {
        isHidden = !isHidden;
        UpdateDisplay();
    }
}
