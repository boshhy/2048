using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileBoard : MonoBehaviour
{
    public Tile tilePrefab;
    public TileState[] tileStates;


    private TileGrid grid;
    private List<Tile> tiles;



    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>();
    }

    private void Start()
    {
        CreateTile();
        CreateTile();
        CreateTile();
        CreateTile();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            // up
            MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            // down
            MoveTiles(Vector2Int.down, 0, 1, grid.height - 2, -1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            // left
            MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            // right
            MoveTiles(Vector2Int.right, grid.width - 2, -1, 0, 1);
        }
    }

    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        for (int x = startX; x >= 0 && x < grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.occupied)
                {
                    MoveTile(cell.tile, direction);
                }
            }
        }

    }

    private void MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;

        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                // TODO: merge
                break;
            }

            newCell = adjacent;

            adjacent = grid.GetAdjacentCell(adjacent, direction);
        }
        if (newCell != null)
        {
            tile.MoveTo(newCell);
        }


    }

    private void CreateTile()
    {
        Tile newTile = Instantiate(tilePrefab, grid.transform);

        newTile.SetState(tileStates[0], 2);
        newTile.Spawn(grid.GetRandomEmptyCell());

        tiles.Add(newTile);

    }
}
