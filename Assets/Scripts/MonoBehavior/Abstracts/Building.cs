using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract to describe a building
/// </summary>
public abstract class Building : MonoBehaviour
{
    public GameState gameState;
    public bool initialTarget = false;
    public bool target;
    public bool burned;
    public bool burning;

    /// <summary>
    /// Sets the building on fire
    /// </summary>
    public abstract void SetOnFire();

    /// <summary>
    /// Puts the fire out
    /// </summary>
    public abstract void StopFire();

    /// <summary>
    /// Chars the building
    /// </summary>
    public abstract void Burn();

    /// <summary>
    /// Tells the building what game state it belongs to
    /// </summary>
    /// <param name="gameState">The game state to set</param>
    public virtual void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }
}
