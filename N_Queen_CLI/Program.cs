using System;
using System.Diagnostics;

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
                //Console.WriteLine("What is the max step count?");
                //int maxSteps = int.Parse(Console.ReadLine());
                NQueenSolver solver = new NQueenSolver(queenNum, 10000000);

                Console.WriteLine("Do you want to draw the board?");
                drawFlag = Console.ReadLine();



                solver.Setup();
                if (drawFlag == "Y")
                {
                    solver.PrintBoard();
                }
                //solver.PrintStats();
                solver.Solve();
                if (drawFlag == "Y")
                {
                    solver.PrintBoard();
                    //solver.PrintStats(); 
                }
            }
           
        }
    }
}
