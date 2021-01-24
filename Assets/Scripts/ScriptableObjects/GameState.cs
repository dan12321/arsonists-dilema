using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the state of a level
/// </summary>
[CreateAssetMenu()]
public class GameState : ScriptableObject
{
    public EventHandler winEvent;
    private int _maxTargets;
    public bool running;
    public bool firstIteration;
    public delegate void EmptyHandler();
    public event EmptyHandler targetChangedHandler;
    public event EmptyHandler startedHandler;

    public int baseTargets;

    public int maxTargets
    {
        get
        {
            return this._maxTargets;
        }
        set
        {
            this._maxTargets = value;
            this.targetChangedHandler?.Invoke();
        }
    }

    /// <summary>
    /// Starts the fires
    /// </summary>
    public void Start()
    {
        this.firstIteration = true;
        this.running = true;
        this.startedHandler?.Invoke();
    }

    /// <summary>
    /// Fires event that the level has been won
    /// </summary>
    public void Win()
    {
        this.winEvent?.Invoke(this, null);
        this.running = false;
    }

    /// <summary>
    /// Set up when the scene loads
    /// </summary>
    private void OnEnable()
    {
        this.maxTargets = this.baseTargets;
        this.running = false;
        this.firstIteration = true;
    }

    /// <summary>
    /// Resets the level
    /// </summary>
    public void Reset()
    {
        this.maxTargets = this.baseTargets;
        this.running = false;
        this.firstIteration = true;
    }
}
