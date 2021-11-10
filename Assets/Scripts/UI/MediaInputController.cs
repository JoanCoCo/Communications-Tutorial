using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class MediaInputController : MonoBehaviour
{
    [SerializeField]
    private StreamManager mediaInputManager;

    [SerializeField]
    private State state = State.Off;

    [SerializeField]
    private bool manageButtonText = true;

    private Button button;
    private TextMeshProUGUI buttonText;
    private enum State { On, Off };

    [SerializeField]
    private InputAvailabilityManager inputAvailabilityManager;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeState);
        buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        UpdateState();
    }

    private void UpdateState()
    {
        if(state == State.On)
        {
            if(mediaInputManager != null) mediaInputManager.StartRecording();
            if (manageButtonText && button != null && buttonText != null)
            {
                buttonText.text = "On";
            }
        } else
        {
            if(mediaInputManager != null) mediaInputManager.StopRecording();
            if (manageButtonText && button != null && buttonText != null)
            {
                buttonText.text = "Off";
            }
        }
    }

    private void ChangeState()
    {
        state = (state == State.On) ? State.Off : State.On;
        UpdateState();
    }

    public void SetMedia(StreamManager mediaInputManager)
    {
        if (mediaInputManager != null)
        {
            this.mediaInputManager = mediaInputManager;
            state = mediaInputManager.IsOn ? State.On : State.Off;
            UpdateState();
        }
    }

    public bool IsNotSet() { return mediaInputManager == null; }
}
