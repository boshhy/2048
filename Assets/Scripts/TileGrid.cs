using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Used to  keep track of the rows and cells on the board
public class TileGrid : MonoBehaviour
{
    // Used to store the rows of the board
    public TileRow[] rows { get; private set; }

    // Used to store the cells on the board
    public TileCell[] cells { get; private set; }

    // Returns the Length of the cells and rows 
    public int size => cells.Length;
    public int height => rows.Length;

    // Returns the width of the baord
    public int width => size / height;

    // Get the rows and cells 
    void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start()
    {
        // For every row in the grid
        for (int y = 0; y < rows.Length; y++)
        {
            // For every cell in current row
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                // Set the coordinates of cell's y and x location in the array
                rows[y].cells[x].coordinates = new Vector2Int(x, y);
            }
        }
    }

    // Returns an empty cell
    public TileCell GetRandomEmptyCell()
    {
        // Create a list for empty cells index location
        List<int> emptyCells = new List<int>();

        // Go through the every cell in cells array
        for (int i = 0; i < cells.Length; i++)
        {
            // If this cell is empty add it to the emptycell list
            if (cells[i].empty)
            {
                emptyCells.Add(i);
            }
        }

        // If not empty cells were found, return null
        if (emptyCells.Count == 0)
        {
            return null;
        }

        // Pick a random index number from the emptycell list
        int index = emptyCells[Random.Range(0, emptyCells.Count)];

        // Return an empty cell
        return cells[index];
    }

    // Returns a cell at x and y location
    public TileCell GetCell(int x, int y)
    {
        // Return cell if the location is within bounds of the board
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return rows[y].cells[x];
        }
        // If not in bounds return null
        else
        {
            return null;
        }
    }

    // Returns a cell adjecent to passed in cell according to the direction
    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        // Get coordinates of the passed in cell
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        // Return the cell
        return GetCell(coordinates.x, coordinates.y);
    }
}
