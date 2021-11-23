using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class that inplements the input controller for media communication.
/// </summary>
[RequireComponent(typeof(Button))]
public class MediaInputController : MonoBehaviour
{
    /// <summary>
    /// Reference to the stream manager.
    /// </summary>
    [SerializeField]
    private StreamManager mediaInputManager;

    /// <summary>
    /// State of the communication stream. On or Off.
    /// </summary>
    [SerializeField]
    private State state = State.Off;

    /// <summary>
    /// Should the controller manage the button text?
    /// </summary>
    [SerializeField]
    private bool manageButtonText = true;

    /// <summary>
    /// Button that toggles the stream.
    /// </summary>
    private Button button;

    /// <summary>
    /// Text on the button.
    /// </summary>
    private TextMeshProUGUI buttonText;

    /// <summary>
    /// Enumeration to represent the values that the state can take.
    /// </summary>
    private enum State { On, Off };

    /// <summary>
    /// Reference to the input availability manager.
    /// </summary>
    [SerializeField]
    private InputAvailabilityManager inputAvailabilityManager;

    /// <summary>
    /// Method that is ran when the object is created.
    /// </summary>
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeState);
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        UpdateState();
    }

    /// <summary>
    /// Method to update the stream's state and button's text based of the
    /// current state value.
    /// </summary>
    private void UpdateState()
    {
        if(state == State.On)
        {
            if(mediaInputManager != null) mediaInputManager.StartRecording(); // Turn on the stream.
            if (manageButtonText && button != null && buttonText != null)
            {
                buttonText.text = "On";
            }
        } else
        {
            if(mediaInputManager != null) mediaInputManager.StopRecording(); // Turn off the stream.
            if (manageButtonText && button != null && buttonText != null)
            {
                buttonText.text = "Off";
            }
        }
    }

    /// <summary>
    /// Mehtod to toggle the state.
    /// </summary>
    private void ChangeState()
    {
        state = (state == State.On) ? State.Off : State.On;
        UpdateState();
    }

    /// <summary>
    /// Method to update the reference to the stream manager.
    /// </summary>
    /// <param name="mediaInputManager">Stream manager to control.</param>
    public void SetMedia(StreamManager mediaInputManager)
    {
        if (mediaInputManager != null)
        {
            this.mediaInputManager = mediaInputManager;
            state = mediaInputManager.IsOn ? State.On : State.Off;
            UpdateState();
        }
    }

    /// <summary>
    /// Method to query if the stream manager hasn't been set.
    /// </summary>
    /// <returns>Bool representing if the stream manager is set.</returns>
    public bool IsNotSet() { return mediaInputManager == null; }
}
