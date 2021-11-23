using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to mark the input (keyword) as bussy. It is intended to be used for
/// avoiding that false keyboard inputs when the user writes in an input field.
/// </summary>
public class InputAvailabilityManager : MonoBehaviour
{
    /// <summary>
    /// Is the user typing in an input field?
    /// </summary>
    private bool userIsTyping = false;

    /// <summary>
    /// Accessor to query if the user is typing in an input field.
    /// </summary>
    public bool UserIsTyping
    {
        get
        {
            return userIsTyping;
        }
    }

    /// <summary>
    /// Method to indicate that the user has started typing. It's meant to be
    /// used be input fields or input field's managers.
    /// </summary>
    /// <param name="s">It must be non-empty.</param>
    public void UserStartedTyping(string s) { if(!s.Equals("")) userIsTyping = true; }

    /// <summary>
    /// Method to indicate that the user has stopped typing. It's meant to be
    /// used be input fields or input field's managers.
    /// </summary>
    /// <param name="s">Not used.</param>
    public void UserFinishedTyping(string s) { userIsTyping = false; }
}
