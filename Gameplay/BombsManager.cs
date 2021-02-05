using System.Reflection;
using UnityEngine;

public class BombsManager : MonoBehaviour
{
    bool done = false; //bool value that creates first empty space
    private void Update()
    {
        if(MapGenerator.Tiles.GetLength(0) != 0 && !done) //chekc if mapgenerator is not empty
        {
            //find empty tile
            var tile = FindEmptyTile();

            //creates a tile
            if (tile != null)
                ClickedLeft(tile);

            done = true;
        }
    }

    /// <summary>
    /// Method that called form input handler when left button is clicked and hitted a tile.
    /// </summary>
    /// <param name="tile">Clicked tile.</param>
    public void ClickedLeft(Tile tile)
    {
        //return if tile is equal to null
        if (tile == null) return;

        //next secure of clicking
        if (tile.State != 0) return;

        //tell server that u made this move and wait for int response
        if (tile.Value == -1) //there should be response (for -1 its 3, for 0-8 it should be 1)
            tile.SetState(3);
        else if(tile.Value == 0) //this open larger space when tiles are empty
        {
            tile.SetState(1);

            ClickTilesIfEmpty(tile);
        }
        else
            tile.SetState(1);
        tile.SetFieldValue(tile.Value); //form the serwer only if its 0-8
    }

    /// <summary>
    /// Method that called from input handler when right button is clicked and hitted a tile.
    /// </summary>
    /// <param name="tile">Clicked tile.</param>
    public void ClickedRight(Tile tile)
    {
        //return if tile is equal to null
        if (tile == null) return;

        //flag/unflag tile
        if (tile.State == 0)
            tile.SetState(2);
        else if (tile.State == 2)
            tile.SetState(0);
    }

    /// <summary>
    /// Method that open empty areas after finding them.
    /// </summary>
    /// <param name="tile">Clicked tile.</param>
    void ClickTilesIfEmpty(Tile tile)
    {
        //creates cord values to save clicked tile
        var cordX = 0;
        var cordY = 0;

        //iterate threw whole map and finds clicked one
        for (int x = 0; x < MapGenerator.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < MapGenerator.Tiles.GetLength(1); y++)
            {
                if(MapGenerator.Tiles[x,y] == tile)
                {
                    cordX = x;
                    cordY = y;

                    break;
                }
            }
        }

        //checks neighbours and open them if they are empty
        if(cordX > 0)
        {
            ClickedLeft(MapGenerator.Tiles[cordX - 1, cordY]);

            if(cordY > 0)
                ClickedLeft(MapGenerator.Tiles[cordX - 1, cordY - 1]);

            if(cordY < MapGenerator.Tiles.GetLength(1) - 1)
                ClickedLeft(MapGenerator.Tiles[cordX - 1, cordY + 1]);
        }

        ClickedLeft(MapGenerator.Tiles[cordX, cordY]);

        if (cordY > 0)
            ClickedLeft(MapGenerator.Tiles[cordX, cordY - 1]);

        if (cordY < MapGenerator.Tiles.GetLength(1) - 1)
            ClickedLeft(MapGenerator.Tiles[cordX, cordY + 1]);

        if(cordX < MapGenerator.Tiles.GetLength(0) - 1)
        {
            ClickedLeft(MapGenerator.Tiles[cordX + 1, cordY]);

            if (cordY > 0)
                ClickedLeft(MapGenerator.Tiles[cordX + 1, cordY - 1]);

            if (cordY < MapGenerator.Tiles.GetLength(1) - 1)
                ClickedLeft(MapGenerator.Tiles[cordX + 1, cordY + 1]);
        }
    }

    /// <summary>
    /// Finding first empty tile on the map.
    /// </summary>
    Tile FindEmptyTile()
    {
        //iterate threw whole map
        for (int x = 0; x < MapGenerator.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < MapGenerator.Tiles.GetLength(1); y++)
            {
                //if tile has no bombs nearby returns it
                if (MapGenerator.Tiles[x, y].Value == 0)
                    return MapGenerator.Tiles[x, y];
            }
        }

        return null;
    }

    /// <summary>
    /// Method that counts uncovered bombs on map.
    /// </summary>
    /// <returns>Returns count of bombs left.</returns>
    public int BombsValue()
    {
        var result = 0;

        //iterate threw whole map
        for (int x = 0; x < MapGenerator.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < MapGenerator.Tiles.GetLength(1); y++)
            {
                //changes value of output
                //bomb add 1 to output
                if (MapGenerator.Tiles[x, y].Value == -1)
                    result++;

                //flag substract from output
                if (MapGenerator.Tiles[x, y].State == 2)
                    result--;
            }
        }

        return result;
    }

    /// <summary>
    /// Check if all bombs are covered by flags, and whole map is uncovered.
    /// </summary>
    /// <returns>Returns win state.</returns>
    public bool CheckWin()
    {
        var result = true;

        //iterate threw whole map
        for (int x = 0; x < MapGenerator.Tiles.GetLength(0); x++)
        {
            for (int y = 0; y < MapGenerator.Tiles.GetLength(1); y++)
            {
                //checks if bombs are covered by map
                if (MapGenerator.Tiles[x, y].Value == -1 && result)
                {
                    result = MapGenerator.Tiles[x, y].State == 2 ? true : false;
                }

                //checks if all map is uncovered
                if (MapGenerator.Tiles[x, y].State == 0 && result)
                    result = false;
            }
        }

        return result;
    }
}
