using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller_With_Minimax
{
    private static  int winScore = 100000000;
    private readonly float MAX = 1000000;
    private readonly float MIN = -1000000;
    string patter12 = "1,1,";
    string patter13 = "1,1,1,";
    string patter14 = "1,1,1,1,";
    string patter15 = "1,1,1,1,1,";
    string patter22 = "2,2,";
    string patter23 = "2,2,2,";
    string patter24 = "2,2,2,2,";
    string patter25 = "2,2,2,2,2,";
    int totalCalculation = 0;
    public AI_Controller_With_Minimax()
    {

    }

    public int[] findBestTurn(float[,,] board)
    {
       double bestValue = MIN;
        int[] bestTurnIndex = {-1,-1};

        //List<int[]> list = findBenifitedMove(board);
        /* foreach (int[] move in list)
         {

             if (board[move[0], move[1], 0] == 0.0f)
                 {

                     board[move[0], move[1], 0] = 2.0f;
                     double newbestValue = minimax(board, MIN, MAX, false, 0);
                     board[move[0], move[1], 0] = 0.0f;
                     if (newbestValue > bestValue)
                     {
                         bestValue = newbestValue;
                         bestTurnIndex[0] = move[0];
                         bestTurnIndex[1] = move[1];

                     }
                 }


         }*/

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();
        
        
        totalCalculation = 0;
        for (int i=0;i<9;i++)
        {
            for (int j=0;j<9;j++)
            {
                if (board[i,j, 0] == 0.0f)
                {

                    board[i, j, 0] = 2.0f;
                    double newbestValue = minimax(board, MIN, MAX, false, 0);
                    board[i, j, 0] = 0.0f;
                    if (newbestValue > bestValue)
                    {
                        bestValue = newbestValue;
                        bestTurnIndex[0] = i;
                        bestTurnIndex[1] = j;

                    }
                }

            }
        }
        sw.Stop();
        long end = System.DateTime.Now.Millisecond;
        Debug.Log("Total Calculation: "+ totalCalculation+" Time: "+(sw.Elapsed.TotalMilliseconds));
        
        return bestTurnIndex;
    }

    public float minimax(float[,,] board, float alpha, float beta,bool isMax,int depth)
    {



        if (depth==2)
        {

            //return (float)evaluateBoardForWhite(board, false);
            return evaluation_function2(board);
        }

        if (isMax)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i,j,0]==0.0f)
                    {
                       
                        board[i, j, 0] = 2.0f;
                        alpha = Mathf.Max(alpha,minimax(board, alpha, beta, !isMax, depth + 1));
                        board[i, j, 0] = 0.0f;
                        if (alpha >= beta) return alpha;
                    }
                    
                }
            }
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (board[i, j, 0] == 0.0f)
                    {
                        
                        board[i, j, 0] = 1.0f;
                        beta = Mathf.Min(beta,minimax(board, alpha, beta, !isMax, depth + 1));
                        board[i, j, 0] = 0.0f;
                        if (alpha >= beta) return beta;
                    }
                }
            }
        }

        return 0;
        //return evaluation_function2(board);
    }


    public float minimax2(float[,,] board, float alpha, float beta, bool isMax, int depth)
    {




        if (depth == 2)
        {

            return evaluation_function(board);
        }

        List<int[]> list = findBenifitedMove(board);
        if (list.Count==0)
        {
            return evaluation_function(board);
        }

        if (isMax)
        {
            foreach (int[] move in list)
            {
                board[move[0], move[1], 0] = 2.0f;
                alpha = Mathf.Max(alpha, minimax(board, alpha, beta, !isMax, depth + 1));
                board[move[0], move[1], 0] = 0.0f;
                if (alpha >= beta) return alpha;

            }
        }
        else
        {

            foreach (int[] move in list)
            {
                board[move[0], move[1], 0] = 1.0f;
                beta = Mathf.Min(beta, minimax(board, alpha, beta, !isMax, depth + 1));
                board[move[0], move[1], 0] = 0.0f;
                if (alpha >= beta) return beta;
            }
           
        }

        return evaluation_function(board);
    }

    

    public float evaluation_function(float[,,] board)
    {

        List<string> list = new List<string>();
        string row = "", column = "", diagonal1 = "", diagonal2 = "";
        for (int i = 0; i < 9; i++)
        {
            diagonal1 += board[i, i, 0] + ",";
            diagonal2 += board[8 - i, i, 0] + ",";
            row = "";
            column = "";
            for (int j = 0; j < 9; j++)
            {
                row += board[i, j, 0] + ",";
                column += board[j, i, 0] + ",";
            }

            list.Add(row);
            list.Add(column);
          
        }

        int length = board.GetLength(0);

        for (int k = 0; k < length * 2; k++)
        {

            for (int j = 0; j <= k; j++)
            {
                int i = k - j;

                if (i < length && j < length)
                {

                    diagonal1 += board[i, j, 0] + ",";
                    diagonal2 += board[length - i - 1, j, 0] + ",";

                }
            }
            list.Add(diagonal1);
            list.Add(diagonal2);

        }




        float player1TotalEarn = 0.0f;
        float player2TotalEarn = 0.0f;

        for (int i=0;i<list.Count;i++)
        {
            string str = list[i];


            //player1
            if (str.Contains(patter15)) return -20000;
            if (str.Contains(patter25)) return 20000;
            while (str.IndexOf(patter14) !=-1)
            {
                str = str.Replace(patter14,"");
                player1TotalEarn += 40;
            }
            while (str.IndexOf(patter13) != -1)
            {
                str = str.Replace(patter13, "");
                player1TotalEarn += 30;
            }
            while (str.IndexOf(patter12) != -1)
            {
                str = str.Replace(patter12, "");
                player1TotalEarn += 20;
            }

            //player2   
            while (str.IndexOf(patter24) != -1)
            {
                str = str.Replace(patter24,"");
                player2TotalEarn += 40;
            }
            while (str.IndexOf(patter23) != -1)
            {
                str = str.Replace(patter23, "");
                player2TotalEarn += 30;
            }
            while (str.IndexOf(patter22) != -1)
            {
                str = str.Replace(patter22, "");
                
                player2TotalEarn += 20;
                Debug.Log("Return value:" + (player2TotalEarn - player1TotalEarn));
            }

        }


        
       // printBoard(board);
        return player2TotalEarn - player1TotalEarn;
    }


    public float evaluation_function2(float[,,] board)
    {

       
        string boardString = "";
        string row = "", column = "";
        for (int i = 0; i < 9; i++)
        {
          
            row = "";
            column = "";
            for (int j = 0; j < 9; j++)
            {
                row += board[i, j, 0] + ",";
                column += board[j, i, 0] + ",";
            }
            boardString += row + "\n";
            boardString += column + "\n";          

        }

        int length = board.GetLength(0);

        for (int k = 0; k < length * 2; k++)
        {
            string  diagonal1 = "", diagonal2 = "";
            for (int j = 0; j <= k; j++)
            {
                int i = k - j;

                if (i < length && j < length)
                {

                    diagonal1 += board[i,j,0] + ",";
                    diagonal2 += board[length - i - 1,j,0] + ",";

                }
            }
            boardString+=diagonal1+"\n";
            boardString += diagonal2+"\n";

        }

      






        float player1TotalEarn = 0.0f;
        float player2TotalEarn = 0.0f;

        


            //player1
            if (boardString.Contains(patter15)) return -20000;
            if (boardString.Contains(patter25)) return 20000;
            while (boardString.IndexOf(patter14) != -1)
            {
                boardString = boardString.Replace(patter14, "*,");
                player1TotalEarn +=100;
            }
            while (boardString.IndexOf(patter13) != -1)
            {
                boardString = boardString.Replace(patter13, "*,");
                player1TotalEarn += 50;
            }
            while (boardString.IndexOf(patter12) != -1)
            {
                boardString = boardString.Replace(patter12, "*,");
                player1TotalEarn += 20;
            }

            //player2   
            while (boardString.IndexOf(patter24) != -1)
            {
                boardString = boardString.Replace(patter24, "*,");
                player2TotalEarn += 100;
            }
            while (boardString.IndexOf(patter23) != -1)
            {
                boardString = boardString.Replace(patter23, "*,");
                player2TotalEarn += 50;
            }
            while (boardString.IndexOf(patter22) != -1)
            {
                boardString = boardString.Replace(patter22, "*,");

                player2TotalEarn += 20;
                
            }

      

        totalCalculation++;
        Debug.Log("Return value:" + (player2TotalEarn - player1TotalEarn));
        // printBoard(board);
        return player2TotalEarn - player1TotalEarn;
    }

    private void printBoard(float[,,] board)
    {
        string s = "";
        for (int i=0;i<9;i++)
        {
            for (int j=0;j<9;j++)
            {
                s += board[i, j, 0];
               
            }
            s += "\n";

        }

        Debug.Log(s);
    }


    public List<int[]> findBenifitedMove(float[,,] board)
    {


        List<int[]> list = new List<int[]>();
        int row1, row2, col1,col2, diagonal1, diagonal2;
        for (int i = 0; i < 9; i++)
        {
            row1 = 0;
            row2 = 0;
            col1 = 0;
            col2 = 0;
            for (int j = 0; j < 8; j++)
            {

                if (board[i, j, 0]==1)
                {
                    row1++;
                }
                else
                {
                    if (row1>0 && board[j, i, 0] == 0)
                    {
                        int[] point = { i, j };
                        list.Add(point);
                    }
                    row1 = 0;
                }
                if (board[i, j, 0] == 2)
                {
                    row2++;
                }
                else
                {
                    if (row2 > 0 && board[j, i, 0] == 0)
                    {
                        int[] point = {i,j};
                        list.Add(point);
                       
                    }
                    row2 = 0;

                }

                if (board[j, i, 0]==1)
                {
                    col1++;
                }
                else
                {
                    if (col1 > 0 && board[j, i, 0] == 0)
                    {
                        int[] point = { j, i };
                        list.Add(point);

                    }
                    col1 = 0;
                }

                if (board[j, i, 0] == 2)
                {
                    col2++;
                }
                else
                {
                    if (col2 > 0&& board[j, i, 0]==0)
                    {
                        int[] point = { j, i };
                        list.Add(point);

                    }
                    col2 = 0;
                }



            }

        }
        Debug.Log("Size:"+list.Count);
        return list;

    }


}
