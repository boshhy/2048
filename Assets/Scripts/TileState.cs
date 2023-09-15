using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used as a scriptable object for the state of a tile
[CreateAssetMenu(menuName = "Tile State")]
public class TileState : ScriptableObject
{
    // Stores background color and text color
    public Color backgroundColor;
    public Color textColor;
}
