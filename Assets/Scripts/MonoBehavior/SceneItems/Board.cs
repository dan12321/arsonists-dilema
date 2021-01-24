using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// The tile set being played on
/// </summary>
public class Board : MonoBehaviour
{
    public GameState gameState;
    public float stepTime;
    private int width;
    private int height;
    private Building[][] buildings;
    private List<Transform> children = new List<Transform>();
    private Vector2 origin = Vector2.zero;
    private bool canStep = true;
    private int numberOfTargets;

    /// <summary>
    /// Set up before the first frame update
    /// </summary>
    void Start()
    {
        this.InitializeBuildings();

        this.CountTargets();
    }

    /// <summary>
    /// Update to run each frame
    /// </summary>
    private void Update()
    {
        if (gameState.running)
        {
            if (gameState.firstIteration)
            {
                StartCoroutine(Timers.BoolTimer(stepTime, (boolState) => { canStep = boolState; }));
                StartFires();
                gameState.firstIteration = false;
            }
            if (canStep)
            {
                StartCoroutine(Timers.BoolTimer(stepTime, (boolState) => { canStep = boolState; }));
                IterateFires();
            }
        }
    }

    /// <summary>
    /// Sets the first fires chosen
    /// </summary>
    private void StartFires()
    {
        int x = 0;
        int y = 0;
        while (y < height)
        {
            if (buildings[x][y] != null && buildings[x][y].initialTarget)
            {
                buildings[x][y].SetOnFire();
            }

            x++;
            if (x >= width)
            {
                x = 0;
                y++;
            }
        }
    }

    /// <summary>
    /// Initializes the buildings based off of the tiles in the tilemap
    /// </summary>
    private void InitializeBuildings()
    {
        Vector2 max = Vector2.zero;
        this.buildings = new Building[width][];
        this.buildings.Select(buildings => new Building[height]);
        foreach (Transform child in transform)
        {
            this.children.Add(child);
            origin.x = Mathf.Min(origin.x, child.position.x);
            origin.y = Mathf.Min(origin.y, child.position.y);
            max.x = Mathf.Max(max.x, child.position.x);
            max.y = Mathf.Max(max.y, child.position.y);
        }

        width = (int)(max.x - origin.x) + 1;
        height = (int)(max.y - origin.y) + 1;

        this.buildings = new Building[width][];
        for (int i = 0; i < this.buildings.Length; i++)
        {
            this.buildings[i] = new Building[height];
        }

        foreach (Transform child in children)
        {
            int xIndex = (int)(child.position.x - origin.x);
            int yIndex = (int)(child.position.y - origin.y);
            this.buildings[xIndex][yIndex] = child.GetComponent<Building>();
            this.buildings[xIndex][yIndex].SetGameState(this.gameState);
        }
    }

    /// <summary>
    /// Counts the number of target buildings
    /// </summary>
    private void CountTargets()
    {
        foreach (Building[] buildings in this.buildings)
        {
            foreach (Building building in buildings)
            {
                if (building && building.target)
                {
                    this.numberOfTargets++;
                }
            }
        }
    }

    /// <summary>
    /// Sets fires to building based of Conway's game of life
    /// </summary>
    private void IterateFires()
    {
        List<Building> buildingsToSetOnFire = new List<Building>();
        List<Building> buildingToPutOut = new List<Building>();
        int x = 0;
        int y = 0;
        while (y < height)
        {
            if (buildings[x][y] != null && buildings[x][y].burning && !buildings[x][y].burned)
            {
                buildings[x][y].Burn();
                if (buildings[x][y].target)
                {
                    numberOfTargets--;
                }
            }
            if (isToBePutOut(x, y))
            {
                buildingToPutOut.Add(this.buildings[x][y]);
            }
            else if (isToBeSetOnFire(x, y))
            {
                buildingsToSetOnFire.Add(this.buildings[x][y]);
            }

            x++;
            if (x >= width)
            {
                x = 0;
                y++;
            }
        }

        foreach (Building building in buildingsToSetOnFire)
        {
            building.SetOnFire();
        }
        foreach (Building building in buildingToPutOut)
        {
            building.StopFire();
        }

        if (numberOfTargets <= 0)
        {
            this.gameState.Win();
        }
    }

    /// <summary>
    /// Checks if a building meets the criteria to be set on fire
    /// </summary>
    /// <param name="x">The x position of the building</param>
    /// <param name="y">The y position of the building</param>
    /// <returns>Whether it passed the check</returns>
    private bool isToBeSetOnFire(int x, int y)
    {
        if (this.buildings[x][y]?.burning ?? true)
        {
            return false;
        }

        return CountAlive(x, y) == 3;
    }

    /// <summary>
    /// Checks if a building meets the criteria to be put out
    /// </summary>
    /// <param name="x">The x position of the building</param>
    /// <param name="y">The y position of the building</param>
    /// <returns>Whether it passed the check</returns>
    private bool isToBePutOut(int x, int y)
    {
        if (!(this.buildings[x][y]?.burning ?? false))
        {
            return false;
        }

        int countAlive = CountAlive(x, y);

        return countAlive < 2 || countAlive > 3;
    }

    /// <summary>
    /// Count the number of surrounding building that are on fire
    /// </summary>
    /// <param name="x">The x position of the building</param>
    /// <param name="y">The y position of the building</param>
    /// <returns>Whether it passed the check</returns>
    private int CountAlive(int x, int y)
    {
        int alive = 0;
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = y - 1; j <= y + 1; j++)
            {
                if (i >= 0 && i < width && j >= 0 && j < height && !(x == i && y == j))
                {
                    if (this.buildings[i][j]?.burning ?? false)
                    {
                        alive++;
                    }
                }
            }
        }
        return alive;
    }
}
