using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    Text textObject;
    public GameObject canvas;
    public GameObject pawn1;
    public GameObject pawn2;
    private string player1WinPattern = "1,1,1,1,1";
    private string player2WinPattern = "2,2,2,2,2";
    private bool player1_Turn = true, player2_Turn = false;
    private static int mode = 0;
    private float radius = .2f;
    float cornerPointX=-2.23f, cornerPointY=2.25f,differenceX=.552f, differenceY = .592f;
    float[,,] board = new float[9,9,3];
    List<int[,]> list = new List<int[,]>();
    

    void Start()
    {
        canvas.SetActive(false);
        boardInitialize();

    }

    
    void Update()
    {

      
        if (mode==1)
        {
            //single player
            singlePlayerController();
           

        }
        else if (mode == 2)
        {
            //two player
            twoPlayerController();
           
        }

        
      

    }

    public void singlePlayerController()
    {
        Debug.Log("single player");
        placedPawnListenForSinglePlayer();
    }

    public void twoPlayerController()
    {
        Debug.Log("two player");
        placedPawnListenForTwoPlayer();
        winCheck();

    }
    private void placedPawnListenForSinglePlayer()
    {
        if (Input.GetMouseButtonDown(0)&& !player2_Turn)
        {

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.touchCount > 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    mousePosition = Camera.main.ScreenToWorldPoint(touch.position);
                }
            }
            
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (mousePosition.x < (board[i, j, 1] + radius) && mousePosition.x > (board[i, j, 1] - radius) && mousePosition.y < (board[i, j, 2] + radius) && mousePosition.y > (board[i, j, 2] - radius))
                    {
                        
                        Instantiate(pawn1, new Vector3(board[i, j, 1], board[i, j, 2], -9), pawn1.transform.rotation);
                        int[] movePoint = { i, j };
                       
                        board[i, j, 0] = 2.0f;
                        player2_Turn = true;
                    }

                }

            }

           
            float[,,] board2 = new float[9, 9, 3];
            Array.Copy(board, board2, board.Length);
            AI_Controller_With_Minimax minimax = new AI_Controller_With_Minimax();
            int[] bestTurnIndex = minimax.calculateNextMove(3, board2);
            Debug.Log("i,j:"+ bestTurnIndex[0] + ","+bestTurnIndex[1]);
            Instantiate(pawn2, new Vector3(board[bestTurnIndex[0], bestTurnIndex[1], 1], board[bestTurnIndex[0], bestTurnIndex[1], 2], -9), pawn2.transform.rotation);
            board[bestTurnIndex[0], bestTurnIndex[1], 0] = 1.0f;
            int[] movePoint2 = { bestTurnIndex[0], bestTurnIndex[1] };
            
            player2_Turn = false;

        }
       

    }

    private void placedPawnListenForTwoPlayer()
    {
       
        if (Input.GetMouseButtonDown(0)|| Input.touchCount>1)
        {

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Touch touch = Input.GetTouch(0);
            if (touch.phase==TouchPhase.Began) {
                mousePosition = Camera.main.ScreenToWorldPoint(touch.position);
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (mousePosition.x < (board[i, j, 1] + radius) && mousePosition.x > (board[i, j, 1] - radius) && mousePosition.y < (board[i, j, 2] + radius) && mousePosition.y > (board[i, j, 2] - radius))
                    {
                        if (player1_Turn)
                        {
                            Instantiate(pawn1, new Vector3(board[i, j, 1], board[i, j, 2], -9), pawn1.transform.rotation);
                            board[i, j, 0] = 1.0f;
                            player1_Turn = false;
                        }
                        else
                        {
                            Instantiate(pawn2, new Vector3(board[i, j, 1], board[i, j, 2], -9), pawn1.transform.rotation);
                            board[i, j, 0] = 2.0f;
                            player1_Turn =true;
                        }
                    }

                }

            }


        }
    }

    private void boardInitialize()
    {

        for (int i = 0; i < 9; i++)
        {
            float tempX = cornerPointX;
            for (int j = 0; j < 9; j++)
            {

                board[i, j, 0] = 0.0f;
                board[i, j, 1] = tempX;
                board[i, j, 2] = cornerPointY;
                tempX += differenceX;

               

            }
            cornerPointY -= differenceY;

        }
    }

    private void winCheck()
    {

        List<string> list = new List<string>();
        string row="", column="", diagonal1="", diagonal2="";
        for (int i=0;i<9;i++)
        {
            diagonal1 += board[i, i, 0] + ",";
            diagonal2 += board[8-i,i, 0] + ",";
            row = "";
            column = "";
            for (int j=0; j < 9;j++)
            {
                row += board[i,j,0] + ",";
                column += board[j, i, 0] + ",";
               
               
            }

            //Debug.Log("row:"+ i+" pat:"+row);
            //Debug.Log("column:" + i + " pat:" + column);
            if (row.Contains(player1WinPattern)|| column.Contains(player1WinPattern))
            {
               
                canvas.transform.Find("Winner").GetComponent<Text>().text = "Black Player Have Won The Match";
                canvas.SetActive(true);
            }
            else if (row.Contains(player2WinPattern) || column.Contains(player2WinPattern))
            {
               
                canvas.transform.Find("Winner").GetComponent<Text>().text = "White Player Have Won The Match";
                canvas.SetActive(true);
            }
        }

        if (diagonal1.Contains(player1WinPattern) || diagonal2.Contains(player1WinPattern))
        {
            canvas.transform.Find("Winner").GetComponent<Text>().text = "Black Player Have Won The Match";
            canvas.SetActive(true);
        }
        else if (diagonal1.Contains(player2WinPattern) || diagonal2.Contains(player2WinPattern))
        {
            canvas.transform.Find("Winner").GetComponent<Text>().text = "White Player Have Won The Match";
            canvas.SetActive(true);
        }

    }

    public void SinglePlayer()
    {
        mode = 1;
        SceneManager.LoadScene("MainGame");
    }
    public void TwoPlayer()
    {
        mode = 2;
        SceneManager.LoadScene("MainGame");
    }

    public void newGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    


}
