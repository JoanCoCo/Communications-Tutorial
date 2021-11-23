using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class that implements the webcam and microphone communication manager.
/// </summary>
public class MediaCommunicationManager : MonoBehaviour
{
    /// <summary>
    /// Webcam input controller.
    /// </summary>
    [SerializeField] private MediaInputController camController;

    /// <summary>
    /// Microphone input controller
    /// </summary>
    [SerializeField] private MediaInputController micController;

    /// <summary>
    /// Method obtains the user's objet webcam manager and microphone manager.
    /// </summary>
    /// <param name="localUser">Local user's object.</param>
    public void SetLocalUser(GameObject localUser)
    {
        var cam = localUser.GetComponent<CamManager>();
        var mic = localUser.GetComponent<MicManager>();

        if (cam != null)
        {
            camController.SetMedia(cam);
        }

        if (mic != null)
        {
            micController.SetMedia(mic);
        }
    }
}
