using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button to start the fires
/// </summary>
public class StartButton : MonoBehaviour
{
    Button button;

    /// <summary>
    /// Set up before the first frame update
    /// </summary>
    void Start()
    {
        this.button = GetComponent<Button>();
        this.button.onClick.AddListener(() =>
        {
            this.button.interactable = false;
        });
    }
}
