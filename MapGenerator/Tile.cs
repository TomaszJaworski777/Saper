using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [Header("Setup")]

    //icon to set up tile
    [SerializeField] Sprite tile;
    [SerializeField] Sprite clickedTile;
    [SerializeField] Sprite bombIcon;
    [SerializeField] Sprite flagIcon;

    //renderer of this objects
    [SerializeField] SpriteRenderer visual;
    [SerializeField] SpriteRenderer icon;
    [SerializeField] Text neighbours;

    //color array
    [SerializeField] Color[] numberColors = new Color[8];

    //public getter of state value
    public int State { get => state; }

    public int Value { get; private set; }

    //private state value container
    int state = 0;

    /// <summary>
    /// Initialize this tile
    /// </summary>
    public void Initialize(float x, float y)
    {
        //sets position of this tile
        transform.position = new Vector2(x, y);

        SetState(0);
    }

    /// <summary>
    /// Sets state value of tile.
    /// </summary>
    /// <param name="stateSet">0-3 int state value.</param>
    public void SetState(int stateSet)
    {
        //u cant return from 1 and 3 state
        if (state == 1 || state == 3) return;

        //sets state of this tile
        state = stateSet;

        //sets visuals of this tile
        switch(state)
        {
            case 0:
                //tile without icon
                visual.sprite = tile;
                visual.color = Color.white;
                icon.sprite = null;
                break;
            case 1:
                //clicked without icon
                visual.sprite = clickedTile;
                visual.color = Color.white;
                icon.sprite = null;
                break;
            case 2:
                //tile with flag icon
                visual.sprite = tile;
                visual.color = Color.white;
                icon.sprite = flagIcon;
                break;
            case 3:
                //clicked with bomb icon
                visual.sprite = clickedTile;
                visual.color = Color.black;
                icon.sprite = bombIcon;
                break;
        }
    }

    /// <summary>
    /// Sets visual number filtered by tile status.
    /// </summary>
    /// <param name="value">Field value from server array.</param>
    public void SetFieldValue(int value)
    {
        Value = value;

        //sets to null for bombs and empty fields
        if (value == -1 || value == 0 || state != 1) 
        {
            neighbours.text = "";
            return;
        }

        //sets color of text
        neighbours.color = numberColors[value - 1];

        //sets text from proporty
        neighbours.text = value.ToString();
    }
}
