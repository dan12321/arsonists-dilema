using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A congratulations message
/// </summary>
public class Congratulations : MonoBehaviour
{
    public GameState gameState;
    private Canvas canvas;

    /// <summary>
    /// Set up before the first frame update
    /// </summary>
    void Start()
    {
        this.canvas = GetComponent<Canvas>();
        this.canvas.enabled = false;
        this.gameState.winEvent += DisplayText;
    }

    /// <summary>
    /// Displays the message when the player wins
    /// </summary>
    /// <param name="source">The emmiter of the win event</param>
    /// <param name="e">The arguments of the event</param>
    private void DisplayText(object source, EventArgs e)
    {
        this.canvas.enabled = true;
    }

    private void OnDestroy()
    {
        this.gameState.winEvent -= DisplayText;
    }
}
