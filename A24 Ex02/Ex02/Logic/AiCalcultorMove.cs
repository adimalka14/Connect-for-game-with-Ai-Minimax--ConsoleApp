using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Ex02.PlayerType;

namespace Ex02
{
    public class AiCalcultorMove
    {
        private const ePlayerType k_AiPlayer = ePlayerType.PlayerTwo;
        private const int k_WiningScore = 100000;
        private const int k_NumberOfMovesForward = 7;
        private const int k_ThereIsNoPossibleMove = -1;
        private readonly Dictionary<int, int> r_Memory = new Dictionary<int, int>();

        public int FindBestComputerMove(GameBoard i_Board)
        {
            int boardKey = getBoardKey(i_Board);
            int bestCol;
            bool IsTheColumnInMemory = r_Memory.TryGetValue(boardKey, out bestCol);

            if (!IsTheColumnInMemory)
            {
                (bestCol, _) = miniMax(i_Board);
                r_Memory.Add(boardKey, bestCol);
            }

            return bestCol;
        }

        private (int bestCol, int bestScore) miniMax(GameBoard i_Board, int i_Depth = k_NumberOfMovesForward,
            int i_Alpha = int.MinValue, int i_Beta = int.MaxValue, bool i_MaximizingPlayer = true)
        {
            List<int> availableMoves = getAvailableMoves(i_Board);
            int bestScore = i_MaximizingPlayer ? int.MinValue : int.MaxValue;
            int bestColMove;

            if (i_Board.CheckFourInARow(k_AiPlayer))
            {
                bestScore = k_WiningScore * (i_Depth + 1);//win in min moves
                bestColMove = k_ThereIsNoPossibleMove;
            }
            else if (i_Board.CheckFourInARow(getOpponentPlayer(k_AiPlayer)))
            {
                bestScore = -(k_WiningScore * (i_Depth + 1));//lose in max moves
                bestColMove = k_ThereIsNoPossibleMove;
            }
            else if (availableMoves.Count == 0)
            {
                bestScore = 0;
                bestColMove = k_ThereIsNoPossibleMove;
            }
            else if (i_Depth == 0)
            {
                bestScore = scorePosition(i_Board, k_AiPlayer);
                bestColMove = k_ThereIsNoPossibleMove;
            }
            else
            {
                bestColMove = availableMoves.First();
                ePlayerType currentPiece = i_MaximizingPlayer ? k_AiPlayer : ePlayerType.PlayerOne;
                foreach (int position in availableMoves)
                {
                    i_Board.InsertTheChipIntoTheBoard(position, currentPiece);
                    (int returndCol, int rturndScore) = miniMax(i_Board, i_Depth - 1, i_Alpha, i_Beta, !i_MaximizingPlayer);
                    i_Board.PutOutChipFromTheBoard(position);
                    if (i_MaximizingPlayer)
                    {
                        if (rturndScore > bestScore)
                        {
                            bestScore = rturndScore;
                            bestColMove = position;
                        }

                        i_Alpha = Math.Max(i_Alpha, rturndScore);
                    }
                    else//minimizing
                    {
                        if (rturndScore < bestScore)
                        {
                            bestScore = rturndScore;
                            bestColMove = position;
                        }

                        i_Beta = Math.Min(i_Beta, rturndScore);
                    }

                    if (i_Beta <= i_Alpha)
                    {
                        break;
                    }
                }
            }

            return (bestColMove, bestScore);
        }

        private int getBoardKey(GameBoard i_Board)
        {
            StringBuilder boardBuilder = new StringBuilder();
            string boardString;

            for (int i = 0; i < i_Board.Rows; i++)
            {
                for (int j = 0; j < i_Board.Columns; j++)
                {
                    boardBuilder.Append((char)(i_Board.GetValueInCell(i, j)));
                }
            }

            boardString = boardBuilder.ToString();

            return boardString.GetHashCode();
        }

        private List<int> getAvailableMoves(GameBoard i_Board)
        {
            List<int> availableMoves = new List<int>();

            for (int col = 0; col < i_Board.Columns; col++)
            {
                if (i_Board.HaveSpaceInCol(col))
                {
                    availableMoves.Add(col);
                }
            }

            return availableMoves;
        }

        private int scorePosition(GameBoard i_Board, ePlayerType i_CurrPlayer)
        {
            int scoreOfBoardPosition = 0;

            scoreOfBoardPosition += scoreCenterColumn(i_Board, i_CurrPlayer);
            scoreOfBoardPosition += scoreHorizontal(i_Board, i_CurrPlayer);
            scoreOfBoardPosition += scoreVertical(i_Board, i_CurrPlayer);
            scoreOfBoardPosition += scoreDiagonals(i_Board, i_CurrPlayer);

            return scoreOfBoardPosition;
        }

        private int scoreDiagonals(GameBoard i_Board, ePlayerType i_CurrPlayer)
        {
            int scoreDiagonalsOfBoardPosition = 0;
            List<ePlayerType> windowOfFour = new List<ePlayerType>();

            for (int i = 0; i < i_Board.Rows - 3; i++)//Score positive sloped diagonal
            {
                for (int j = 0; j < i_Board.Columns - 3; j++)
                {
                    for (int k = 0; k < 4; k++)
                        windowOfFour.Add(i_Board.GetValueInCell(i + k, j + k));
                    scoreDiagonalsOfBoardPosition += evaluateWindow(windowOfFour.ToArray(), i_CurrPlayer);
                    windowOfFour.Clear();
                }
            }

            for (int i = 0; i < i_Board.Rows - 3; i++) //Score negative sloped diagonal
            {
                for (int j = 0; j < i_Board.Columns - 3; j++)
                {
                    for (int k = 0; k < 4; k++)
                        windowOfFour.Add(i_Board.GetValueInCell(i + 3 - k, j + k));
                    scoreDiagonalsOfBoardPosition += evaluateWindow(windowOfFour.ToArray(), i_CurrPlayer);
                    windowOfFour.Clear();
                }
            }

            return scoreDiagonalsOfBoardPosition;
        }

        private int scoreVertical(GameBoard i_Board, ePlayerType i_CurrPlayer)
        {
            int scoreVerticalOfBoardPosition = 0;
            List<ePlayerType> windowOfFour = new List<ePlayerType>();

            for (int i = 0; i < i_Board.Columns; i++)
            {
                for (int j = 0; j < i_Board.Rows - 3; j++)
                {
                    for (int k = j; k < j + 4; k++)
                        windowOfFour.Add(i_Board.GetValueInCell(k, i));
                    scoreVerticalOfBoardPosition += evaluateWindow(windowOfFour.ToArray(), i_CurrPlayer);
                    windowOfFour.Clear();
                }
            }

            return scoreVerticalOfBoardPosition;
        }

        private int scoreHorizontal(GameBoard i_Board, ePlayerType i_CurrPlayer)
        {
            int scoreHorizontalOfBoardPosition = 0;
            List<ePlayerType> windowOfFour = new List<ePlayerType>();

            for (int i = 0; i < i_Board.Rows; i++)
            {
                for (int j = 0; j < i_Board.Columns - 3; j++)
                {
                    for (int k = j; k < j + 4; k++)
                        windowOfFour.Add(i_Board.GetValueInCell(i, k));
                    scoreHorizontalOfBoardPosition += evaluateWindow(windowOfFour.ToArray(), i_CurrPlayer);
                    windowOfFour.Clear();
                }
            }

            return scoreHorizontalOfBoardPosition;
        }

        private int scoreCenterColumn(GameBoard i_Board, ePlayerType i_CurrPlayer)
        {
            int centerArrayCounter = 0;
            int centerCol = i_Board.Columns / 2;
            bool numOfColIsEven = i_Board.Columns % 2 == 0;

            for (int i = 0; i < i_Board.Rows; i++)
            {
                if (i_Board.GetValueInCell(i, centerCol) == i_CurrPlayer)
                {
                    centerArrayCounter++;
                }

                if (numOfColIsEven)//There are 2 center cols
                {
                    if (i_Board.GetValueInCell(i, centerCol - 1) == i_CurrPlayer)
                    {
                        centerArrayCounter++;
                    }
                }
            }

            return centerArrayCounter * 10;
        }

        private int evaluateWindow(ePlayerType[] i_WindowOfFour, ePlayerType i_PlayerType)
        {
            int score = 0;
            int playerCoinCounter = 0;
            int emptyCellCounter = 0;
            int opponentCoinCounter = 0;
            ePlayerType opponent = getOpponentPlayer(i_PlayerType);

            foreach (var cell in i_WindowOfFour)
            {
                if (cell == i_PlayerType)
                {
                    playerCoinCounter++;
                }
                else if (cell == opponent)
                {
                    opponentCoinCounter++;
                }
                else
                {
                    emptyCellCounter++;
                }
            }

            if (playerCoinCounter == 3 && emptyCellCounter == 1)
            {
                score = 150;
            }
            else if (playerCoinCounter == 2 && emptyCellCounter == 2)
            {
                score = 10;
            }
            if (opponentCoinCounter == 3 && emptyCellCounter == 1)
            {
                score = -150;
            }

            return score;
        }

        private ePlayerType getOpponentPlayer(ePlayerType i_Player)
        {
            return i_Player == ePlayerType.PlayerOne ? ePlayerType.PlayerTwo : ePlayerType.PlayerOne;
        }
    }
}