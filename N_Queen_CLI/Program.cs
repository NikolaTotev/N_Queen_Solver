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
            Console.WriteLine("Select mode, A/N");
            drawFlag = Console.ReadLine();

            if (drawFlag == "A")
            {

                for (int i = 5; i < 100; i++)
                {
                    Console.WriteLine("==========================================");
                    Console.WriteLine($"Testing {i}");
                    Console.WriteLine("==========================================");
                    NQueenSolver solver = new NQueenSolver(i, 10000000, false);
                    solver.HorsePatternSetup();

                    //solver.PrintStats();
                    solver.Solve();
                    Thread.Sleep(500);
                }
            }
            else
            {
                while (drawFlag != "C")
                {
                    Console.WriteLine("How many queens to you want to place?");
                    int queenNum = int.Parse(Console.ReadLine());
             
                    Console.WriteLine("Do you want advanced debug output? (Y/N)");
                    string debugFlag = Console.ReadLine();
                    
                    NQueenSolver solver = new NQueenSolver(queenNum, 0, debugFlag=="Y");

                    Console.WriteLine("Do you want to draw the board? (Y/N)");
                    drawFlag = Console.ReadLine();


                    bool restart = true;

                    while (restart)
                    {
                        solver.HorsePatternSetup();
                        if (drawFlag == "Y")
                        {
                            solver.PrintBoard();
                        }
                        if (solver.Solve() != -1)
                        {
                            restart = false;
                            if (drawFlag == "Y")
                            {
                                solver.PrintBoard();
                            }
                        }
                        else
                        {
                            Console.WriteLine("===========================");
                            Console.WriteLine("Restarting");
                            Console.WriteLine("===========================");
                        }
                        
                    }

                }
            }
        }
    }
}
