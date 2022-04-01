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
    //Touch Inputs
    private Vector2 mFirstPosition;
    private Vector2 mLastPosition;
    private bool isDragging;

    void Start()
    {
        grid = FindObjectOfType<Grid>();
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;

    }

    void Update()
    {
        targetX = column;
        targetY = row;
        //GetInputMobile();
        //GetInputTest();

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
            transform.position = Vector2.Lerp(transform.position, tempPos, .6f);

            if (grid.allPieces[column, row] != gameObject) // When objects destroyed, new pieces coming, with this code we are taking them again as gameobject so they will not glitch anymore
            {
                grid.allPieces[column, row] = gameObject;
            }
        }
        else
        {
            //set the position
            tempPos = new Vector2(targetX, transform.position.y);
            transform.position = tempPos;
        }

        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //move towards to the target
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPos, .6f);

            if (grid.allPieces[column, row] != gameObject)
            {
                grid.allPieces[column, row] = gameObject;
            }
        }
        else
        {
            // set the position
            tempPos = new Vector2(transform.position.x, targetY);
            transform.position = tempPos;
        }
    }


    public IEnumerator CheckMoveMatch()
    {
        yield return new WaitForSeconds(.6f);
        if (otherPiece != null)
        {
            if (!isMatched && !otherPiece.GetComponent<PieceControl>().isMatched)
            {
                otherPiece.GetComponent<PieceControl>().row = row;
                otherPiece.GetComponent<PieceControl>().column = column;

                row = oldRow;
                column = oldColumn;
            }
            else
            {
                grid.DestroyPieces(); //if any matches it access to grid class and destroy the pieces
            }

            otherPiece = null;
        }



    }

    //void GetInputMobile()
    //{

    //    if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
    //    {
    //        mFirstPosition = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
    //        isDragging = true;
    //    }
    //    if (isDragging && (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled))
    //    {
    //        mLastPosition = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
    //        CalculateAngle();
    //        isDragging = false;
    //    }
    //}
    //-------------------------------- TEST INPUTS -------------------------------------

    //void GetInputTest()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {

    //        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

    //        if (hit.collider != null)
    //        {
    //            Debug.Log("Hit Collider");
    //            firstTouchPos = hit.transform.position;
    //        }

    //        // firstTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    }
    //    if (Input.GetMouseButtonUp(0))
    //    {

    //        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

    //        if (hit.collider != null)
    //        {
    //            Debug.Log("Hit Collider");
    //            lastTouchPos = hit.transform.position;
    //        }


    //        //lastTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        CalculateAngle();
    //    }
    //}

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
        swipeAngle = Mathf.Atan2(lastTouchPos.y - firstTouchPos.y, lastTouchPos.x - firstTouchPos.x) * 180 / Mathf.PI; //Mouse Inputs

        //swipeAngle = Mathf.Atan2(mLastPosition.y - mFirstPosition.y, mLastPosition.x - mFirstPosition.x) * 180 / Mathf.PI;

        MovePieces();
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < grid.width - 1) //-1 because arrays are zero indexed
        {
            //right swipe
            otherPiece = grid.allPieces[column + 1, row];
            oldRow = row;
            oldColumn = column;
            otherPiece.GetComponent<PieceControl>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < grid.height - 1) //-1 because arrays are zero indexed
        {
            //up swipe
            otherPiece = grid.allPieces[column, row + 1];
            oldRow = row;
            oldColumn = column;
            otherPiece.GetComponent<PieceControl>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left swipe
            otherPiece = grid.allPieces[column - 1, row];
            oldRow = row;
            oldColumn = column;
            otherPiece.GetComponent<PieceControl>().column += 1;
            column -= 1;
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            //down swipe
            otherPiece = grid.allPieces[column, row - 1];
            oldRow = row;
            oldColumn = column;
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
            if (lefPiece != null && rightPiece != null)
            {
                if (lefPiece.tag == gameObject.tag && rightPiece.tag == gameObject.tag && lefPiece != gameObject && rightPiece != gameObject)
                {
                    lefPiece.GetComponent<PieceControl>().isMatched = true;
                    rightPiece.GetComponent<PieceControl>().isMatched = true;
                    isMatched = true;
                }
            }

        }

        // Vertical Matches --------------------------------------------------
        if (row > 0 && row < grid.height - 1) // check up and down
        {
            GameObject upperPiece = grid.allPieces[column, row + 1];
            GameObject lowerPiece = grid.allPieces[column, row - 1];

            //Check left and right pieces with tag, if they are same it means matched is true
            if (upperPiece != null && lowerPiece != null)
            {
                if (upperPiece.tag == gameObject.tag && lowerPiece.tag == gameObject.tag && upperPiece != gameObject && lowerPiece != gameObject)
                {
                    upperPiece.GetComponent<PieceControl>().isMatched = true;
                    lowerPiece.GetComponent<PieceControl>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
}
