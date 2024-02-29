using System;
using System.Text;
using Ex02.ConsoleUtils;
using static Ex02.GameFormat;
using static Ex02.PlayerType;
using static Ex02.GameSituation;
using static Ex02.GameLogic;

namespace Ex02
{
    public class ConsoleUI
    {
        private GameLogic m_GameLogic;

        public void GetInsideTheGame()
        {
            bool isAnotherRound = false;

            Console.WriteLine("Hello welcome to connect four game.");
            Console.WriteLine("Firstly,please define game settings.");
            defineGameSettings();
            Console.WriteLine("Press enter to start the game.");
            Console.ReadKey();
            do
            {
                Screen.Clear();
                m_GameLogic.PrepareNewRound();
                startRoundGame();
                Console.WriteLine("Do you want to play another game? answer in Y/N");
                isAnotherRound = askForAnotherGame();
            } while (isAnotherRound);

            Console.WriteLine(@"The game is over.
Press enter to exit");
        }

        private void startRoundGame()
        {
            do
            {
                Screen.Clear();
                drawGameBoard();
                playTurn();
            } while (m_GameLogic.CurrentGameSituation == eGameSituation.RoundContinues);

            Screen.Clear();
            drawGameBoard();
            if (m_GameLogic.CurrentGameSituation == eGameSituation.WasVictory)
            {
                Console.WriteLine("congratulations,{0} won.", m_GameLogic.Winner);
            }
            else if (m_GameLogic.CurrentGameSituation == eGameSituation.WasRetirement)
            {
                Console.WriteLine("The other player retired from the game. {0},you got the win.", m_GameLogic.Winner);
            }
            else
            {
                Console.WriteLine("This is a draw.");

            }

            Console.WriteLine("Result : player1  [{0}] - [{1}] : player2", m_GameLogic.PlayerXWins, m_GameLogic.PlayerOWins);
        }

        private void playTurn()
        {
            if (m_GameLogic.CurrentPlayer == ePlayerType.PlayerOne)
            {
                Console.WriteLine("Player 1 it's your turn,please choose number between 1 to {0}", m_GameLogic.GetNumOfColumns());

            }
            else
            {
                Console.WriteLine("Player 2 it's your turn,please choose number between 1 to {0}", m_GameLogic.GetNumOfColumns());
            }

            do
            {
                string input = Console.ReadLine();
                int intInput;
                bool isNumeric = int.TryParse(input, out intInput);
                if (isNumeric && isValidMove(intInput))
                {
                    bool moveSucceeded = moveSucceeded = m_GameLogic.MakeMove(intInput);
                    if (moveSucceeded)
                    {
                        break;
                    }

                    Console.WriteLine("The column is Full!!");
                }
                else if (input.ToUpper() == "Q")
                {
                    m_GameLogic.RetierdFromRound();
                    break;
                }
                else
                {
                    Console.WriteLine("Invaild number,please choose number between 1 to {0}!", m_GameLogic.GetNumOfColumns());
                }
            } while (true);
        }

        private bool askForAnotherGame()
        {
            bool isAnotherGame = false;
            bool vaildInput = false;

            do
            {
                string userInput = Console.ReadLine();
                switch (userInput)
                {
                    case "Y":
                    case "y":
                        isAnotherGame = true;
                        vaildInput = true;
                        break;
                    case "N":
                    case "n":
                        isAnotherGame = false;
                        vaildInput = true;
                        break;
                    default:
                        Console.WriteLine("Please enter a vaild input!!");
                        break;
                }
            } while (!vaildInput);

            return isAnotherGame;
        }

        private void defineGameSettings()
        {
            int row;
            int col;
            eGameFormat gameFormat;

            Console.WriteLine("Please enter number of rows (4<=row<=8)");
            row = getValidNumberForBoard();
            Console.WriteLine("Please enter number of columns (4<=row<=8)");
            col = getValidNumberForBoard();
            gameFormat = chooseGameFormat();
            m_GameLogic = new GameLogic(row, col, gameFormat);
        }

        private int getValidNumberForBoard()
        {
            int userInput;

            while (true)
            {
                bool isNumber = int.TryParse(Console.ReadLine(), out userInput);
                if (isNumber && IsValidSideForGameBoard(userInput))
                {
                    return userInput;
                }
                else
                {
                    Console.WriteLine("Please enter a valid number between 4 and 8.");
                }
            }
        }

        private bool isValidMove(int i_Column)
        {
            return i_Column >= 1 && i_Column <= m_GameLogic.GetNumOfColumns();
        }

        private eGameFormat chooseGameFormat()
        {
            bool isValidFormatGame = false;
            int userInput;

            Console.WriteLine(@"Please select game format
1 For playing against the computer.
2 For a game with two players.");
            do
            {
                if (int.TryParse(Console.ReadLine(), out userInput))
                {
                    isValidFormatGame = IsVaildGameFormat(userInput);
                    if (!isValidFormatGame)
                    {
                        Console.WriteLine("Invalid input. Please select a valid game format.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric option.");
                }
            } while (!isValidFormatGame);

            return (eGameFormat)userInput;
        }

        private void drawGameBoard()
        {
            StringBuilder boardString = new StringBuilder();

            for (int row = -1; row < m_GameLogic.GetNumOfRows(); row++)
            {
                for (int col = 0; col < m_GameLogic.GetNumOfColumns(); col++)
                {
                    if (row == -1)
                    {
                        boardString.AppendFormat("  {0} ", col + 1);
                    }
                    else
                    {
                        boardString.AppendFormat("| {0} ", (char)m_GameLogic.GetValueInCell(row, col));
                        if (col == m_GameLogic.GetNumOfColumns() - 1)
                        {
                            boardString.Append("|");
                        }
                    }
                }

                boardString.AppendLine();
                if (row == -1)
                {
                    continue;
                }

                for (int i = 0; i < m_GameLogic.GetNumOfColumns(); i++)
                {
                    boardString.Append("====");
                }

                boardString.AppendLine("=");
            }

            Console.WriteLine(boardString.ToString());
        }
    }
}