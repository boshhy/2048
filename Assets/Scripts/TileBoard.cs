using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

// Used to keep track of the game board
public class TileBoard : MonoBehaviour
{
    // Reference to the game manager
    public GameManager gameManager;

    // Reference to the tile prefab
    public Tile tilePrefab;

    // Used to store references to different state
    public TileState[] tileStates;

    // Reference to grid
    private TileGrid grid;

    // Used to keep track of tiles in the current game
    private List<Tile> tiles;

    // Used to disable input while changes are being made
    private bool waiting;

    // Get the tile grid, and create an empty list of tiles
    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>();
    }

    // Used to get input from user
    private void Update()
    {
        // Accept input if not waiting
        if (!waiting)
        {
            // up
            if (Input.GetKeyDown(KeyCode.W))
            {
                // up
                MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            }
            // down
            else if (Input.GetKeyDown(KeyCode.S))
            {
                // down
                MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
            // left
            else if (Input.GetKeyDown(KeyCode.A))
            {
                // left
                MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            }
            // right
            else if (Input.GetKeyDown(KeyCode.D))
            {
                // right
                MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
        }
    }

    // Used to move tiles in a certain direction
    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        // Set a bool to see if any changes will be made
        bool changed = false;

        // Check to see if any tiles can move
        for (int x = startX; x >= 0 && x < grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incrementY)
            {
                // Get the cell in this x and y location
                TileCell cell = grid.GetCell(x, y);

                // If the cell is has a tile, try to move tile and see if anything was changed
                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        // If anything changed then start coroutine
        if (changed)
        {
            StartCoroutine(WaitForChanges());
        }
    }

    // Used to move tile if possible and returns true if moved, otherwise false
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        // Make a null variable for a cell
        TileCell newCell = null;

        // Get the adjacent cell next to the the tile in the direction requested
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        // Keeep updating adjacent cell as long adjacent is not null (out of bounds)
        while (adjacent != null)
        {
            // check to see if the cell is occupied
            if (adjacent.occupied)
            {
                // If occupied check to see if cells can merge
                if (CanMerge(tile, adjacent.tile))
                {
                    // If can merge then merge 
                    // this cell's tile to the adjacent cell's tile
                    Merge(tile, adjacent.tile);
                    return true;
                }
                // If occupied but can't merge then exit loop
                break;
            }

            // Update the new cell to that this tile should move to
            newCell = adjacent;

            // Check the next adjacent cell of current adjacent cell
            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }

        // If the new cell has been updated, then move the tile to the new cell
        // Return true because tile moved
        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        // Return false because tile was not moved
        return false;
    }

    // Used to check to see if two tiles can merge, 
    // true if can merge otherwise return false
    private bool CanMerge(Tile a, Tile b)
    {
        // Do both tiles have the same number and Tile B is not locked
        return a.number == b.number && !b.isLocked;
    }

    // Used to merge two tiles
    private void Merge(Tile a, Tile b)
    {
        // Remove Tile a from the list of tiles on the board
        tiles.Remove(a);

        // Merge Tile A into tile B
        a.Merge(b.cell);

        // Update the index of current state of tile B
        int index = Math.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);

        // Update the number by two
        int number = b.number * 2;

        // set the new state of tile B and increase the score
        b.SetState(tileStates[index], number);
        gameManager.IncreaseScore(number);
    }

    // Used to create a new tile
    public void CreateTile()
    {
        // Create a new tile on the middle of the grid
        Tile newTile = Instantiate(tilePrefab, grid.transform);

        // Set the state of the new tile
        newTile.SetState(tileStates[0], 2);

        // Spawn the tile in the on the grid on a random cell
        newTile.Spawn(grid.GetRandomEmptyCell());

        // Add the tiles to the list of active tiles
        tiles.Add(newTile);
    }

    // Returns the index of the current tile
    private int IndexOf(TileState state)
    {
        // Check to see what state the tile is currenlty in
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i])
            {
                return i;
            }
        }

        // If state is out of bound for some reason, return error
        return -1;
    }

    // Used to clear the board
    public void ClearBoard()
    {
        // Sets each cell to null
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }

        // Destroys each tile in active tiles
        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        // Clears active tile list
        tiles.Clear();
    }

    // Used to pause movement while tiles are moving
    private IEnumerator WaitForChanges()
    {
        // Set waiting to true and wait some time before continuing
        waiting = true;
        yield return new WaitForSeconds(0.1f);

        // After everything has moved activate input
        waiting = false;

        // Unlock all tiles in active tiles
        foreach (var tile in tiles)
        {
            tile.isLocked = false;
        }

        // If empty cells exist create a new tile
        if (tiles.Count != grid.size)
        {
            CreateTile();
        }

        // Check for game over
        if (CheckForGameOver())
        {
            gameManager.GameOver();
        }
    }

    // Used to check for game over
    private bool CheckForGameOver()
    {
        // If empty cells exist return false
        if (tiles.Count != grid.size)
        {
            return false;
        }

        // check each tile and see if it can merge in all directions
        foreach (var tile in tiles)
        {
            // Get cell in up, down, left, and right directions
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            // If any direction causes a merge return false
            if (up != null && CanMerge(tile, up.tile))
            {
                return false;
            }
            if (down != null && CanMerge(tile, down.tile))
            {
                return false;
            }

            if (left != null && CanMerge(tile, left.tile))
            {
                return false;
            }

            if (right != null && CanMerge(tile, right.tile))
            {
                return false;
            }
        }

        // If no empty cells exist or any tiles can merge
        // then return true for game over
        return true;
    }
}
