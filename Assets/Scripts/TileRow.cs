using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to keep track of cells in current row
public class TileRow : MonoBehaviour
{
    // Save the cells in this row
    public TileCell[] cells { get; private set; }

    private void Awake()
    {
        // Get all the cells in this row
        cells = GetComponentsInChildren<TileCell>();
    }
}
