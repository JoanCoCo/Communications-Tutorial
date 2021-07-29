using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MediaCommunicationManager : MonoBehaviour
{
    [SerializeField] private MediaInputController camController;
    [SerializeField] private MediaInputController micController;

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
