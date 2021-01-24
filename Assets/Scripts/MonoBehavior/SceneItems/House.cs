using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A house on the board
/// </summary>
public class House : Building
{
    public Sprite house;
    public Sprite houseBurned;
    private SpriteRenderer houseRenderer;
    public SpriteRenderer fireRenderer;
    public SpriteRenderer targetRenderer;

    /// <summary>
    /// Set up before the first frame update
    /// </summary>
    void Start()
    {
        this.houseRenderer = GetComponent<SpriteRenderer>();
        this.fireRenderer.enabled = burning;
        this.targetRenderer.enabled = initialTarget;
        this.houseRenderer.sprite = burned ? this.houseBurned : this.house;
    }

    /// <summary>
    /// Called when the house is clicked on
    /// </summary>
    void OnMouseDown()
    {
        if (!burning && !this.gameState.running)
        {
            if (initialTarget)
            {
                this.gameState.maxTargets++;
                initialTarget = false;
                this.targetRenderer.enabled = false;
            }
            else if (this.gameState.maxTargets > 0)
            {
                initialTarget = true;
                this.gameState.maxTargets--;
                this.targetRenderer.enabled = true;
            }
        }
    }

    /// <summary>
    /// Sets the house on fire
    /// </summary>
    public override void SetOnFire()
    {
        this.burning = true;
        this.fireRenderer.enabled = true;
    }

    /// <summary>
    /// Puts the fire out
    /// </summary>
    public override void StopFire()
    {
        this.burning = false;
        this.fireRenderer.enabled = false;
    }

    /// <summary>
    /// Chars the house
    /// </summary>
    public override void Burn()
    {
        this.burned = true;
        this.houseRenderer.sprite = this.houseBurned;
    }

    /// <summary>
    /// Tells the building what game state it belongs to
    /// </summary>
    /// <param name="gameState">The game state to set</param>
    public override void SetGameState(GameState gameState)
    {
        base.SetGameState(gameState);
        this.gameState.startedHandler += this.ClearTarget;
    }

    /// <summary>
    /// Removes the target from the building
    /// </summary>
    public void ClearTarget()
    {
        this.targetRenderer.enabled = false;
    }

    private void OnDestroy()
    {
        if (this.gameState) this.gameState.startedHandler -= this.ClearTarget;
    }
}
