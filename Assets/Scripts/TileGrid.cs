using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] rows { get; private set; }
    public TileCell[] cells { get; private set; }

    public int size => cells.Length;
    public int height => rows.Length;

    public int width => size / height;



    void Awake()
    {
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start()
    {
        // rows
        for (int y = 0; y < rows.Length; y++)
        {
            // cells size
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y);

            }
        }
    }

    public TileCell GetRandomEmptyCell()
    {
        List<int> emptyCells = new List<int>();

        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i].empty)
            {
                emptyCells.Add(i);
            }
        }

        if (emptyCells.Count == 0)
        {
            return null;
        }

        int index = emptyCells[Random.Range(0, emptyCells.Count)];
        return cells[index];


        // int index = Random.Range(0, cells.Length);
        // int startingIndex = index;

        // while (cells[index].occupied)
        // {
        //     index++;

        //     if (index >= cells.Length)
        //     {
        //         index = 0;
        //     }

        //     if (index == startingIndex)
        //     {
        //         return null;
        //     }

        // }

        //return cells[index];
    }

    public TileCell GetCell(int x, int y)
    {

        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return rows[y].cells[x];
        }
        else
        {
            return null;
        }
    }

    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates.x, coordinates.y);
    }


}
