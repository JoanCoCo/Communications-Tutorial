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

    private Button button;
    private enum State { On, Off };

    [SerializeField]
    private InputAvailabilityManager inputAvailabilityManager;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeState);
        UpdateState();
    }

    private void UpdateState()
    {
        if(state == State.On)
        {
            if(mediaInputManager != null) mediaInputManager.StartRecording();
        } else
        {
            if(mediaInputManager != null) mediaInputManager.StopRecording();
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
