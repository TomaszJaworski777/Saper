using System.Drawing;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //tile map for visual part
    Tile[,] mapSet;

    public static Tile[,] Tiles { get; private set; }

    //serwer map of bombs (-1 are bombs)
    int[,] valueField;

    //prefab of a visual tile
    [SerializeField] GameObject tilePrefab;

    /// <summary>
    /// Generate map function called on clintes to show visuals.
    /// </summary>
    /// <param name="sizeX">X size.</param>
    /// <param name="sizeY">Y size.</param>
    /// <param name="scale">Scale of the tile.</param>
    public void Generate(int sizeX, int sizeY, float scale)
    {
        //initialzie visual tile map
        mapSet = new Tile[sizeX, sizeY];

        //loop for all of the tiles
        for (int x = -sizeX / 2; x < sizeX/2; x++)
        {
            for (int y = -sizeY / 2; y < sizeY/2; y++)
            {
                //creates tile
                var tile = Instantiate(tilePrefab, transform);

                //sets scale of the tile
                tile.transform.localScale = new Vector3(scale, scale, 1);

                //converting cords into array indexes
                var cordX = x + sizeX / 2;
                var cordY = y + sizeY / 2;

                //initalizing tiles
                mapSet[cordX, cordY] = tile.GetComponent<Tile>();
                mapSet[cordX, cordY].Initialize(transform.position.x + x * (scale * 5), transform.position.y + y * (scale * 5));

                //only serwer
                mapSet[cordX, cordY].SetFieldValue(valueField[cordX, cordY]);
            }
        }

        Tiles = mapSet;
    }

    /// <summary>
    /// Server function to generate bombs on the map
    /// </summary>
    /// <param name="sizeX">X size.</param>
    /// <param name="sizeY">Y size.</param>
    /// <param name="bombsCount">Number of bombs.</param>
    public void GenerateValueField(int sizeX, int sizeY, int bombsCount)
    {
        //number of bombs cant be higher than tiles count
        if (bombsCount > sizeX * sizeY)
            bombsCount = sizeX * sizeY;

        //initalize value map
        valueField = new int[sizeX, sizeY];

        //loop threw all bombs
        for (int i = 0; i < bombsCount; i++)
        {
            //get random cords
            var cordX = Random.Range(0, sizeX);
            var cordY = Random.Range(0, sizeY);

            //if there is a bomb just find another place
            if(valueField[cordX, cordY] == -1)
            {
                i--;
                continue;
            }

            //sets bomb on this random cords
            valueField[cordX, cordY] = -1;

            //setting neighbours of the tiles
            //X = cordX - 1
            if (cordX - 1 >= 0)
            {
                if (cordY - 1 >= 0)
                {
                    if(valueField[cordX - 1, cordY - 1] != -1)
                        valueField[cordX - 1, cordY - 1]++;
                }

                if (valueField[cordX - 1, cordY] != -1)
                    valueField[cordX - 1, cordY]++;

                if (cordY + 1 < sizeY)
                {
                    if (valueField[cordX - 1, cordY + 1] != -1)
                        valueField[cordX - 1, cordY + 1]++;
                }
            }

            //X = cordX
            if (cordY - 1 >= 0)
            {
                if (valueField[cordX, cordY - 1] != -1)
                    valueField[cordX, cordY - 1]++;
            }

            if (cordY + 1 < sizeY)
            {
                if (valueField[cordX, cordY + 1] != -1)
                    valueField[cordX, cordY + 1]++;
            }

            //X = cordX + 1
            if (cordX + 1 < sizeX)
            {
                if (cordY - 1 >= 0)
                {
                    if (valueField[cordX + 1, cordY - 1] != -1)
                        valueField[cordX + 1, cordY - 1]++;
                }

                if (valueField[cordX + 1, cordY] != -1)
                    valueField[cordX + 1, cordY]++;

                if (cordY + 1 < sizeY)
                {
                    if (valueField[cordX + 1, cordY + 1] != -1)
                        valueField[cordX + 1, cordY + 1]++;
                }
            }
        }
    }

    //debug
    private void Start()
    {
        GenerateValueField(8, 8, 10);
        Generate(8, 8, 0.25f);
    }
}
