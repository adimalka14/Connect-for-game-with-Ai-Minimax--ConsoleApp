using static Ex02.GameFormat;
using static Ex02.PlayerType;
using static Ex02.GameSituation;
using System;

namespace Ex02
{
    public class GameLogic
    {
        private int m_PlayerXWins = 0;
        private int m_PlayerOWins = 0;
        private ePlayerType m_Winner = ePlayerType.None;
        private eGameSituation m_CurrentGameSituation = eGameSituation.None;
        private ePlayerType m_CurrentPlayer = ePlayerType.None;
        private readonly eGameFormat r_GameFormat = eGameFormat.None;
        private readonly GameBoard r_GameBoard = null;
        private readonly AiCalcultorMove r_AiLogic = null;

        public GameLogic(int i_Rows, int i_Cols, eGameFormat i_GameFormat)
        {
            r_GameFormat = i_GameFormat;
            r_GameBoard = new GameBoard(i_Rows, i_Cols);
            if (i_GameFormat == eGameFormat.Player1VSComputer)
            {
                r_AiLogic = new AiCalcultorMove();
            }
        }

        public ePlayerType CurrentPlayer
        {
            get
            {
                return m_CurrentPlayer;
            }
        }

        public eGameSituation CurrentGameSituation
        {
            get
            {
                return m_CurrentGameSituation;
            }
        }

        public ePlayerType Winner
        {
            get
            {
                return m_Winner;
            }
        }

        public int PlayerXWins
        {
            get
            {
                return m_PlayerXWins;
            }
        }

        public int PlayerOWins
        {
            get
            {
                return m_PlayerOWins;
            }
        }

        public bool MakeMove(int i_Column)
        {
            bool haveSpaceInCol = r_GameBoard.HaveSpaceInCol(i_Column - 1);

            if (haveSpaceInCol)
            {
                r_GameBoard.InsertTheChipIntoTheBoard(i_Column - 1, m_CurrentPlayer);
                updateGameSituation();
                if (r_GameFormat == eGameFormat.Player1VSComputer &&
                    m_CurrentGameSituation == eGameSituation.RoundContinues)
                {
                    switchCurrentPlayer();
                    int col = r_AiLogic.FindBestComputerMove(r_GameBoard);
                    r_GameBoard.InsertTheChipIntoTheBoard(col, m_CurrentPlayer);
                    updateGameSituation();
                }

                switchCurrentPlayer();
            }

            return haveSpaceInCol;
        }

        private void updateGameSituation()
        {
            bool wasWin = r_GameBoard.CheckFourInARow(m_CurrentPlayer);

            if (wasWin)
            {
                m_CurrentGameSituation = eGameSituation.WasVictory;
                addWinForCurrentPlayer();
            }
            else//!WasWin
            {
                bool wasDraw = r_GameBoard.CheckIfBoardFull();
                if (wasDraw)
                {
                    m_CurrentGameSituation = eGameSituation.WasDraw;
                }
            }
        }

        public void PrepareNewRound()
        {
            r_GameBoard.PrepareTheBoardForANewGame();
            m_CurrentPlayer = ePlayerType.PlayerOne;
            m_CurrentGameSituation = eGameSituation.RoundContinues;
            m_Winner = ePlayerType.None;
        }

        public void RetierdFromRound()
        {
            m_CurrentGameSituation = eGameSituation.WasRetirement;
            switchCurrentPlayer();
            addWinForCurrentPlayer();
        }

        private void switchCurrentPlayer()
        {
            m_CurrentPlayer = m_CurrentPlayer == ePlayerType.PlayerOne ?
                ePlayerType.PlayerTwo : ePlayerType.PlayerOne;
        }

        private void addWinForCurrentPlayer()
        {
            m_Winner = m_CurrentPlayer;
            if (m_CurrentPlayer == ePlayerType.PlayerOne)
            {
                m_PlayerXWins++;

            }
            else if (m_CurrentPlayer == ePlayerType.PlayerTwo)
            {
                m_PlayerOWins++;
            }
        }

        public static bool IsVaildGameFormat(int i_UserInput)
        {
            return Enum.IsDefined(typeof(eGameFormat), i_UserInput) && i_UserInput != 0;
        }

        public static bool IsValidSideForGameBoard(int i_UserInputSize)
        {
            return i_UserInputSize >= 4 && i_UserInputSize <= 8;
        }

        public ePlayerType GetValueInCell(int i_Row, int i_Col)
        {
            return r_GameBoard.GetValueInCell(i_Row, i_Col);
        }

        public int GetNumOfRows()
        {
            return r_GameBoard.Rows;
        }

        public int GetNumOfColumns()
        {
            return r_GameBoard.Columns;
        }
    }
}