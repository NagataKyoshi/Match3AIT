using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public int width;
    public int height;
    public GameObject tilePrefab;
    private BackgroundTile[,] allTiles;
    public GameObject[] pieces;
    public GameObject[,] allPieces;

    // Start is called before the first frame update
    void Start()
    {
        allTiles = new BackgroundTile[width, height];
        allPieces = new GameObject[width, height];
        SetUp();
    }

    private void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPos = new Vector2(i,j);

                GameObject backgroundTile = Instantiate(tilePrefab, tempPos, Quaternion.identity);
                backgroundTile.transform.parent = transform;
                backgroundTile.name = "(" + i + "," + j + ")";

                //We are picking random objects from our array and we instantiate them with prefabs to tile
                int piecesToUse = Random.Range(0, pieces.Length);
                GameObject piece = Instantiate(pieces[piecesToUse], tempPos, Quaternion.identity);
                piece.transform.parent = transform;
                piece.name = "(" + i + "," + j + ")";

                allPieces[i,j] = piece;
            }
        }
    }

    //if i use this function i had problem swiping system because array down to negative so giving error
    //Vector2 GetWorldPosition(int x, int y)
    //{
    //    return new Vector2(transform.position.x - width / 2.0f + x, transform.position.y + height / 2.0f - y);
    //}
}
