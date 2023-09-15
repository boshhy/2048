using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to keep track of cell information
public class TileCell : MonoBehaviour
{
    // Coordinates to the cells location on the grid
    public Vector2Int coordinates { get; set; }

    // Reference to the  tile attached to this cell
    public Tile tile { get; set; }

    // Returns true if tile attached, otherwise false
    public bool empty => tile == null;

    // Returns True if Cell is occupied, otherise false
    public bool occupied => tile != null;
}
