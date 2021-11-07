using System;
using System.Diagnostics;
using System.Threading;

namespace N_Queen_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            string drawFlag = "";

            while (drawFlag != "C")
            {
                Console.WriteLine("How many queens to you want to place?");
                int queenNum = int.Parse(Console.ReadLine());

                NQueenSolver solver = new NQueenSolver(queenNum, 0, false);

                Console.WriteLine("Do you want to draw the board? (Y/N)");
                drawFlag = Console.ReadLine();

                if (queenNum == 10000)
                {
                    solver.Initialize(mode: NQueenSolver.InitMode.Horse);
                    if (solver.Solve()==-1)
                    {
                        Console.WriteLine("Failed to solve within 20 seconds.");
                    }
                    if (drawFlag == "Y")
                    {
                        solver.PrintBoard();
                    }
                }
                else
                {
                    solver.Initialize(mode: NQueenSolver.InitMode.Min);
                    solver.Solve();
                    if (solver.Solve() == -1)
                    {
                        Console.WriteLine("Failed to solve within 20 seconds.");
                    }
                    if (drawFlag == "Y")
                    {
                        solver.PrintBoard();
                    }
                }
            }
        }
    }
}
