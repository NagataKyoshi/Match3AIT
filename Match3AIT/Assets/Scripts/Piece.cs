using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    private Grid grid;
    private GameObject otherPiece;

    private Vector2 tempPos;
    private Vector2 firstTouchPos;
    private Vector2 lastTouchPos;
    public float swipeAngle = 0;


    // Start is called before the first frame update
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
    }

    // Update is called once per frame
    void Update()
    {
        targetX = column;
        targetY = row;

        //if (Mathf.Abs(targetX - transform.position.x) > .1)
        //{
        //    //move towards to the target
        //    tempPos = new Vector2(targetX, transform.position.y);
        //    transform.position = Vector2.Lerp(transform.position, tempPos, .4f);
        //}
        //else
        //{
        //    //set the position
        //    tempPos = new Vector2(targetX, transform.position.y);
        //    transform.position = tempPos;
        //    grid.allPieces[column, row] = this.gameObject;
        //}

        //if (Mathf.Abs(targetY - transform.position.y) > .1)
        //{
        //    //move towards to the target
        //    tempPos = new Vector2(transform.position.x, targetY);
        //    transform.position = Vector2.Lerp(transform.position, tempPos, .4f);
        //}
        //else
        //{
        //    // set the position
        //    tempPos = new Vector2(transform.position.x, targetY);
        //    transform.position = tempPos;
        //    grid.allPieces[column, row] = gameObject;
        //}
    }

    private void OnMouseDown()
    {
        firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        lastTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(lastTouchPos.y - firstTouchPos.y, lastTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI;
        Debug.Log(swipeAngle);
        MovePieces();
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < grid.width)
        {
            //right swipe
            otherPiece = grid.allPieces[column + 1, row];
            otherPiece.GetComponent<Piece>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < grid.height)
        {
            //up swipe
            otherPiece = grid.allPieces[column, row + 1];
            otherPiece.GetComponent<Piece>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left swipe
            otherPiece = grid.allPieces[column - 1, row];
            otherPiece.GetComponent<Piece>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //down swipe
            otherPiece = grid.allPieces[column, row - 1];
            otherPiece.GetComponent<Piece>().row += 1;
            row -= 1;
        }
    }
}
