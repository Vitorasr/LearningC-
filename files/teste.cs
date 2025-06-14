using System;
namespace tictactoeFile
{
    public class tictactoe
    {
        static char[,] board = {
        { '1', '2', '3' },
        { '4', '5', '6' },
        { '7', '8', '9' }
    };
        static int turns = 0;
        static char currentPlayer = 'X';

        public static void start()
        {
            while (true)
            {
                Console.Clear();
                PrintBoard();
                PlayerMove();
                if (CheckWin())
                {
                    Console.Clear();
                    PrintBoard();
                    Console.WriteLine($"\nJogador {currentPlayer} venceu!");
                    break;
                }
                else if (turns == 9)
                {
                    Console.Clear();
                    PrintBoard();
                    Console.WriteLine("\nEmpate!");
                    break;
                }
                SwitchPlayer();
            }
            Console.WriteLine("\nFim de jogo. Pressione qualquer tecla para sair.");
            Console.ReadKey();
        }

        static void PrintBoard()
        {
            Console.WriteLine(" JOGO DA VELHA\n");
            Console.WriteLine("-------------");
            for (int i = 0; i < 3; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(board[i, j] + " | ");
                }
                Console.WriteLine("\n-------------");
            }
        }

        static void PlayerMove()
        {
            int choice;
            bool validInput = false;

            do
            {
                Console.Write($"\nJogador {currentPlayer}, escolha um número (1-9): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out choice) && choice >= 1 && choice <= 9)
                {
                    int row = (choice - 1) / 3;
                    int col = (choice - 1) % 3;

                    if (board[row, col] != 'X' && board[row, col] != 'O')
                    {
                        board[row, col] = currentPlayer;
                        turns++;
                        validInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Essa posição já está ocupada. Tente outra.");
                    }
                }
                else
                {
                    Console.WriteLine("Entrada inválida. Digite um número de 1 a 9.");
                }
            } while (!validInput);
        }

        static void SwitchPlayer()
        {
            currentPlayer = currentPlayer == 'X' ? 'O' : 'X';
        }

        static bool CheckWin()
        {
            // Linhas e colunas
            for (int i = 0; i < 3; i++)
            {
                if ((board[i, 0] == currentPlayer && board[i, 1] == currentPlayer && board[i, 2] == currentPlayer) ||
                    (board[0, i] == currentPlayer && board[1, i] == currentPlayer && board[2, i] == currentPlayer))
                    return true;
            }

            // Diagonais
            if ((board[0, 0] == currentPlayer && board[1, 1] == currentPlayer && board[2, 2] == currentPlayer) ||
                (board[0, 2] == currentPlayer && board[1, 1] == currentPlayer && board[2, 0] == currentPlayer))
                return true;
//FEITO POR LucasPaixaoCL
            return false;
        }
    }
}