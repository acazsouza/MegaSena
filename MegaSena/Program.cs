using MegaSena.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaSena
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = 0;
            var intro = true;

            var program = new MegaSenaLotteryProgram();

            do
            {
                input = ShowMenu(intro);
                intro = false;

                switch (input)
                {
                    case 1:
                        RealizarJogo(program);
                        break;
                    case 2:
                        RealizarJogoAutomaticamente(program);
                        break;
                    case 3:
                        RealizarSorteio(program);
                        break;
                    case 4:
                        Historico(program);
                        break;
                    default:
                        break;
                }
            } while (input != 5);
        }

        private static void Historico(MegaSenaLotteryProgram program)
        {
            var result = program.GetHistory();

            if (result.Success)
            {
                Console.WriteLine();
                Console.WriteLine(result.Content);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(result.Error);
                Console.WriteLine();
                return;
            }
        }

        private static void RealizarSorteio(MegaSenaLotteryProgram program)
        {
            var result = program.DoLottery();

            if (result.Success)
            {
                Console.WriteLine();
                Console.WriteLine(result.Content);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(result.Error);
                Console.WriteLine();
                return;
            }
        }

        private static void RealizarJogoAutomaticamente(MegaSenaLotteryProgram program)
        {
            var result = program.RegisterAutomaticGame();

            if (result.Success)
            {
                Console.WriteLine();
                Console.WriteLine(result.Content);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(result.Error);
                Console.WriteLine();
                return;
            }
        }

        static void RealizarJogo(MegaSenaLotteryProgram program)
        {
            Console.WriteLine();
            Console.WriteLine("Digite os 6 números separados por ponto e virgula, por favor:");
            var input = Console.ReadLine();

            int[] numbers = null;
            try
            {
                numbers = input.Split(';')
                                        .Select(x => Convert.ToInt32(x.TrimStart().TrimEnd()))
                                        .ToArray();
            }
            catch (Exception)
            {
                Console.WriteLine();
                Console.WriteLine("Jogo inválido. Tente novamente.");
                Console.WriteLine();
                return;
            }

            var newGame = new MegaSenaGame(numbers);

            var result = program.TryRegisterGame(newGame);

            if (result.Success)
            {
                Console.WriteLine();
                Console.WriteLine(result.Content);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(result.Error);
                Console.WriteLine();
                return;
            }
        }

        static public int ShowMenu(bool intro = false)
        {
            if (intro)
            {
                Console.WriteLine("Bem vindo ao sistema da Mega Sena! Escolha uma das opções abaixo:");
                Console.WriteLine();
            }

            var result = ShowMainMenu();

            int inputInt;
            if (int.TryParse(result, out inputInt))
            {
                return inputInt;
            }
            else
            {
                Console.WriteLine("Não entendi. Digite apenas o número de uma das opções abaixo:");
                ShowMenu(false);
            }

            return inputInt;
        }

        private static string ShowMainMenu()
        {
            Console.WriteLine("1 - Registrar jogo");
            Console.WriteLine("2 - Registrar jogo automaticamente");
            Console.WriteLine("3 - Realizar sorteio");
            Console.WriteLine("4 - Historico de sorteios");
            Console.WriteLine("5 - Sair");
            return Console.ReadLine();
        }
    }
}
