using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    char[] board = new char[9];
    bool playerTurn = true;
    bool gameOver = false;
    Dictionary<string, int> scores = new Dictionary<string, int>(){{ "X", -10 },{ "O", 10 },{ "tie", 0 } };
    
    // Start is called before the first frame update
    void Start()
    {
        InitBoard();
        ShowBoard();
    }


    void InitBoard()
    {
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = '_';
        }
    }

    void ShowBoard()
    {
        string boardStr = "";
        for (int i = 0; i < board.Length; i += 3)
        {
            boardStr += board[i] + " " +
                board[i + 1] + " " + board[i + 2] + "\n";
        }
        Debug.Log(boardStr);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (playerTurn)
        {
            for (int i = 0; i < board.Length; i++)
            {
                HandleKeyPress(i + 1);
            }
        }
        else //computer turn
        {
            FillFreeSpot();
        }
    }

    void HandleKeyPress(int key)
    {
        if (Input.GetKeyDown(key.ToString()) &&
            board[key - 1] == '_')
        {
            board[key - 1] = 'X';
            ShowBoard();
            if (IsWinning() == "X")
            {
                Debug.Log("Player Won!!!!");
                gameOver = true;
            }
            else
            if (isFullBoard())
            {
                Debug.Log("Game Over, no one Won...");
                gameOver = true;
            }
            playerTurn = false;
        }
    }

    void FillFreeSpot()
    {
        int spot = 0;
        int bestScore = -9999999;
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i].Equals('_'))
            {
                board[i] = 'O';
                int score = minimax(board, 0, -99999,99999, false);
                Debug.Log("score: " + score);
                board[i] = '_';
                if (score > bestScore)
                {
                    bestScore = score;
                    spot = i;
                }
            }
        }

        Debug.Log("spot: " + spot);
        if (board[spot] == '_')
        {
            board[spot] = 'O';
            ShowBoard();
            if (IsWinning() == "O")
            {
                Debug.Log("Computer Won!!!!");
                gameOver = true;
            }
            playerTurn = true;
        }
    }

    int minimax(char[] board, int depth,int alpha,int beta, bool isMaximizing)
    {
        var result = IsWinning();
        if (result != null)
        {
            return scores[result] - depth;
        }

        if (isMaximizing)
        {
            int bestScore = -999999;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i].Equals('_'))
                {
                    board[i] = 'O';
                    int score = minimax(board, depth + 1, alpha, beta, false);
                    board[i] = '_';
                    bestScore = Math.Max(score, bestScore);
                    alpha = Math.Max(alpha, score);
                    if (beta <= alpha)
                        break;
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = 999999;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i].Equals('_'))
                {
                    board[i] = 'X';
                    int score = minimax(board, depth + 1, alpha, beta, true);
                    board[i] = '_';
                    bestScore = Math.Min(score, bestScore);
                    
                    beta = Math.Min(beta, score);
                    if (beta <= alpha)
                        break;
                }
            }
            
            return bestScore;
        }
    }

    string IsWinning()
    {
        string winner = null;
        //row
        for (int i = 0; i < board.Length; i += 3)
        {
            if (board[i] == board[i + 1] && board[i + 1] == board[i + 2] && board[i] != '_')
            {
                winner = board[i].ToString();
            }
        }
        //column
        for (int i = 0; i < 3; i++)
        {
            if (board[i] == board[i + 3] && board[i + 3] == board[i + 6] && board[i] != '_')
            {
                winner = board[i].ToString();
            }
        }

        if (board[0] == board[4] && board[4] == board[8] && board[0] != '_')
        {
            winner = board[0].ToString();
        }

        if (board[2] == board[4] && board[4] == board[6] && board[2] != '_')
        {
            winner = board[2].ToString();
        }

        int openSpots = 0;
        for(int i = 0; i < board.Length; i++)
        {
            if(board[i] == '_')
            {
                openSpots++;
            }
        }

        if(winner == null && openSpots == 0)
        {
            return "tie";
        } else {
            return winner;
        }
    }

    bool isFullBoard()
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == '_')
            {
                return false;
            }
        }
        return true;
    }
}