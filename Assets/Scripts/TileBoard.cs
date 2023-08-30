using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }

    private void CreateTile()
    {
        Tile newTile = Instantiate(tilePrefab, grid.transform);

        newTile.SetState(tileStates[0], 2);
        newTile.Spawn(grid.GetRandomEmptyCell());

        tiles.Add(newTile);

    }
}
