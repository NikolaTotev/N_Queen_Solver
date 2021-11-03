using System;
using System.Diagnostics;

namespace N_Queen_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("How many queens to you want to place?");
            int queenNum = int.Parse(Console.ReadLine());
            NQueenSolver solver = new NQueenSolver(queenNum);
            solver.Setup();
            solver.PrintBoard();
            solver.PrintStats();
        }
    }
}
