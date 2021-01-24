using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Text to display the number of matches
/// </summary>
public class PropertyText : MonoBehaviour
{
    public GameState gameState;
    private Text text;

    /// <summary>
    /// Set up before the first frame update
    /// </summary>
    void Start()
    {
        text = GetComponent<Text>();
        this.UpdateText();
        this.gameState.targetChangedHandler += this.UpdateText;
    }

    /// <summary>
    /// Updates the message displayed
    /// </summary>
    private void UpdateText()
    {
        this.text.text = this.gameState.maxTargets.ToString() + " matches";
    }

    private void OnDestroy()
    {
        this.gameState.targetChangedHandler -= this.UpdateText;
    }
}
