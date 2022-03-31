using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceControl : MonoBehaviour
{
    [Header("Piece Variables")]
    public int column;
    public int row;
    public int oldColumn;
    public int oldRow;
    public int targetX;
    public int targetY;
    public bool isMatched = false;

    private Grid grid;
    private GameObject otherPiece;

    private Vector2 tempPos;
    private Vector2 firstTouchPos;
    private Vector2 lastTouchPos;
    public float swipeAngle = 0;


    void Start()
    {
        grid = FindObjectOfType<Grid>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
        oldRow = row;
        oldColumn = column;
    }

    void Update()
    {
        targetX = column;
        targetY = row;

        FindMatches();

        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(0f, 0f, 0f, .2f);
        }

        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //move towards to the target
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPos, .1f);
        }
        else
        {
            //set the position
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
            grid.allPieces[column, row] = gameObject;
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //move towards to the target
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, .1f);
        }
        else
        {
            // set the position
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = tempPos;
            grid.allPieces[column, row] = gameObject;
        }
    }


    public IEnumerator CheckMoveMatch()
    {
        yield return new WaitForSeconds(.5f);
        if (otherPiece != null)
        {
            if (!isMatched && !otherPiece.GetComponent<PieceControl>().isMatched)
            {
                otherPiece.GetComponent<PieceControl>().row = row;
                otherPiece.GetComponent<PieceControl>().column = column;

                row = oldRow;
                column = oldColumn;
            }

            otherPiece = null;
        }


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
        //Debug.Log(swipeAngle);
        MovePieces();
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < grid.width - 1) //-1 because arrays are zero indexed
        {
            //right swipe
            otherPiece = grid.allPieces[column + 1, row];
            otherPiece.GetComponent<PieceControl>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < grid.height - 1) //-1 because arrays are zero indexed
        {
            //up swipe
            otherPiece = grid.allPieces[column, row + 1];
            otherPiece.GetComponent<PieceControl>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left swipe
            otherPiece = grid.allPieces[column - 1, row];
            otherPiece.GetComponent<PieceControl>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //down swipe
            otherPiece = grid.allPieces[column, row - 1];
            otherPiece.GetComponent<PieceControl>().row += 1;
            row -= 1;
        }

        StartCoroutine(CheckMoveMatch());
    }

    void FindMatches()
    {
        // Horizontal Matches -------------------------------------------------
        if (column > 0 && column < grid.width - 1) // check left and right
        {
            GameObject lefPiece = grid.allPieces[column - 1, row];
            GameObject rightPiece = grid.allPieces[column + 1, row];

            //Check left and right pieces with tag, if they are same it means matched is true

            if (lefPiece.tag == gameObject.tag && rightPiece.tag == gameObject.tag)
            {
                lefPiece.GetComponent<PieceControl>().isMatched = true;
                rightPiece.GetComponent<PieceControl>().isMatched = true;
                isMatched = true;
            }
        }

        // Vertical Matches --------------------------------------------------
        if (row > 0 && row < grid.height - 1) // check up and down
        {
            GameObject upperPiece = grid.allPieces[column, row + 1];
            GameObject lowerPiece = grid.allPieces[column, row - 1];

            //Check left and right pieces with tag, if they are same it means matched is true

            if (upperPiece.tag == gameObject.tag && lowerPiece.tag == gameObject.tag)
            {
                upperPiece.GetComponent<PieceControl>().isMatched = true;
                lowerPiece.GetComponent<PieceControl>().isMatched = true;
                isMatched = true;
            }
        }
    }
}
