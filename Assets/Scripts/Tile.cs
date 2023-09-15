using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


// Used to keep track of the state of the tile and
// what cell it belongs to
public class Tile : MonoBehaviour
{
    // Used to set and get the state of this tile
    public TileState state { get; private set; }

    // Used to assign this tile to a cell
    public TileCell cell { get; private set; }

    // Used to keep track this tile's current number
    public int number { get; private set; }

    // Used to look and unlock tile from merging
    public bool isLocked { get; set; }

    // Used to change background color of tile
    private Image background;

    // Used to display number value
    private TextMeshProUGUI text;

    // Runs when object is Awaked
    void Awake()
    {
        // Gets the background and text properties of the tile
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Used to set the state
    public void SetState(TileState state, int number)
    {
        // sets the tilestate and number from scriptable object
        this.state = state;
        this.number = number;

        // sets background and text color
        background.color = state.backgroundColor;
        text.color = state.textColor;

        // Change text to display the number value of this tile
        text.text = number.ToString();
    }

    // Used to spawn this tile to a cell
    public void Spawn(TileCell cell)
    {
        // If the cell is not empty, set it to null
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        // Assign the cell to belong to this tiles cell
        // And set the tile to the cell that was just assigned to be this tile
        // cell <-> tile (cell assigned to tile, tile assigned to cell)
        this.cell = cell;
        this.cell.tile = this;

        // Set to position of this tile to be the same position as the cell's
        transform.position = cell.transform.position;
    }

    // Used to move this tile to a cell
    public void MoveTo(TileCell cell)
    {
        // If the cell is not empty, set it to null
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        // Assign the cell to belong to this tiles cell
        // And set the tile to the cell that was just assigned to be this tile
        // cell <-> tile (cell assigned to tile, tile assigned to cell)
        this.cell = cell;
        this.cell.tile = this;

        // Set to position of this tile to be the
        // same position as the cell's by animation the tile
        // to move to that location. merging = false
        StartCoroutine(Animate(cell.transform.position, false));
    }

    // Used to animate position of tile to new cell
    private IEnumerator Animate(Vector3 to, bool merging)
    {
        // Time elapsed and the duration of how long to animate
        float elapsed = 0f;
        float duration = 0.1f;

        // Get the position from the tile
        Vector3 from = transform.position;

        // Keep shifting while the time has not surpassed the duration
        while (elapsed < duration)
        {
            // Move tile position of the tile interpolationg by elapsed / duration
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set the final position of the tile to equal extactly the 'to' position
        transform.position = to;

        // If this tile is merging then delete it
        if (merging)
        {
            Destroy(gameObject);
        }

    }

    // Used to merge this tile's cell to the incoming TileCell
    public void Merge(TileCell cell)
    {
        // If this tile's cell is not empty then 'detach' this tile from the cell
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        // Set this tile's cell to empty and lock the cell the we are merging to
        this.cell = null;
        cell.tile.isLocked = true;

        // Start the merging animation by passing in the 
        // position this tile is going to, merging = true
        StartCoroutine(Animate(cell.transform.position, true));
    }
}
