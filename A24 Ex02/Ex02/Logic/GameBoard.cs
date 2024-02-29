using static Ex02.PlayerType;

namespace Ex02
{
    public class GameBoard
    {
        private readonly int r_Rows;
        private readonly int r_Cols;
        private ePlayerType[,] m_Board = null;

        public GameBoard(int r_Rows, int r_Cols)
        {
            this.r_Rows = r_Rows;
            this.r_Cols = r_Cols;
            m_Board = new ePlayerType[r_Rows, r_Cols];
        }

        public int Columns
        {
            get
            {
                return r_Cols;
            }
        }

        public int Rows
        {
            get
            {
                return r_Rows;
            }
        }

        public void PrepareTheBoardForANewGame()
        {
            for (int i = 0; i < r_Rows; i++)
            {
                for (int j = 0; j < r_Cols; j++)
                {
                    m_Board[i, j] = ePlayerType.None;
                }
            }
        }

        public void InsertTheChipIntoTheBoard(int i_Column, ePlayerType i_PlayerChip)
        {
            for (int i = r_Rows - 1; i >= 0; i--)
            {
                if (m_Board[i, i_Column] == ePlayerType.None)
                {
                    m_Board[i, i_Column] = i_PlayerChip;
                    break;
                }
            }
        }

        public void PutOutChipFromTheBoard(int i_Col)
        {
            for (int i = 0; i < r_Rows; i++)
            {
                if (m_Board[i, i_Col] != ePlayerType.None)
                {
                    m_Board[i, i_Col] = ePlayerType.None;
                    break;
                }
            }
        }

        public bool HaveSpaceInCol(int i_Col)
        {
            return m_Board[0, i_Col] == ePlayerType.None;
        }

        public bool CheckFourInARow(ePlayerType i_CurrentPlayer)
        {
            return checkHorizontal(i_CurrentPlayer) ||
                checkVertical(i_CurrentPlayer) ||
                checkDiagonal(i_CurrentPlayer);
        }

        public bool CheckIfBoardFull()
        {
            bool thereIsSpace = false;

            for (int i = 0; i < r_Cols; i++)
            {
                if (m_Board[0, i] == ePlayerType.None)
                {
                    thereIsSpace = true;
                    break;
                }
            }

            return !thereIsSpace;
        }

        private bool checkHorizontal(ePlayerType i_CurrentPlayer)
        {
            bool isConnectFour = false;

            for (int i = 0; i < r_Rows; i++)
            {
                for (int j = 0; j < r_Cols - 3; j++)
                {
                    if (m_Board[i, j] == i_CurrentPlayer &&
                        m_Board[i, j + 1] == i_CurrentPlayer &&
                        m_Board[i, j + 2] == i_CurrentPlayer &&
                        m_Board[i, j + 3] == i_CurrentPlayer)
                    {
                        isConnectFour = true;
                        break;
                    }
                }

                if (isConnectFour)
                {
                    break;
                }
            }

            return isConnectFour;
        }

        private bool checkVertical(ePlayerType i_CurrentPlayer)
        {
            bool isConnectFour = false;

            for (int i = 0; i < r_Rows - 3; i++)
            {
                for (int j = 0; j < r_Cols; j++)
                {
                    if (m_Board[i, j] == i_CurrentPlayer &&
                        m_Board[i + 1, j] == i_CurrentPlayer &&
                        m_Board[i + 2, j] == i_CurrentPlayer &&
                        m_Board[i + 3, j] == i_CurrentPlayer)
                    {
                        isConnectFour = true;
                        break;
                    }
                }

                if (isConnectFour)
                {
                    break;
                }
            }

            return isConnectFour;
        }

        private bool checkDiagonal(ePlayerType i_CurrentPlayer)
        {
            bool isConnectFour = false;

            for (int i = 0; i < r_Rows - 3; i++)
            {
                for (int j = 0; j < r_Cols - 3; j++)
                {
                    if (m_Board[i, j] == i_CurrentPlayer &&
                        m_Board[i + 1, j + 1] == i_CurrentPlayer &&
                        m_Board[i + 2, j + 2] == i_CurrentPlayer &&
                        m_Board[i + 3, j + 3] == i_CurrentPlayer)
                    {
                        isConnectFour = true;
                        break;
                    }
                    else if (m_Board[i, j + 3] == i_CurrentPlayer &&
                             m_Board[i + 1, j + 2] == i_CurrentPlayer &&
                             m_Board[i + 2, j + 1] == i_CurrentPlayer &&
                             m_Board[i + 3, j] == i_CurrentPlayer)
                    {
                        isConnectFour = true;
                        break;
                    }
                }

                if (isConnectFour)
                {
                    break;
                }
            }

            return isConnectFour;
        }

        public ePlayerType GetValueInCell(int i_Row, int i_Col)
        {
            return m_Board[i_Row, i_Col];
        }
    }
}