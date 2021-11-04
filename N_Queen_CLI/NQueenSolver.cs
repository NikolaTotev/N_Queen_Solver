using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Queen_CLI
{
    class NQueenSolver
    {
        private int m_QueenNum = 0;
        private int[] m_Columns;
        private int[] m_Rows;
        private int[] m_MainDiagonal;
        private int[] m_SecondaryDiagonal;
        private int m_TotalMoves = 0;
        private bool m_IsSolved;
        private int m_MaxSteps = 0;
        public NQueenSolver(int queenNum, int maxSteps)
        {

            m_QueenNum = queenNum;
            m_MaxSteps = maxSteps;
            int diagCount = (queenNum * 2) + 1;
            m_Columns = new int[queenNum];
            m_Rows = new int[queenNum];
            m_MainDiagonal = new int[diagCount];
            m_SecondaryDiagonal = new int[diagCount];

            Array.Clear(m_Columns, 0, m_Columns.Length);
            Array.Clear(m_Rows, 0, m_Rows.Length);
            Array.Clear(m_MainDiagonal, 0, m_MainDiagonal.Length);
            Array.Clear(m_SecondaryDiagonal, 0, m_SecondaryDiagonal.Length);
        }

        public void Setup()
        {
            int startRow = 0;
            int numberOfPlacedQueens = 0;
            bool onOdd = true;
            while (numberOfPlacedQueens < m_QueenNum)
            {
                for (int i = 0; i < m_Columns.Length; i++)
                {
                    if (onOdd)
                    {
                        if (i % 2 != 0)
                        {
                            m_Columns[i] = startRow;
                            AddQueenToRow(startRow);
                            AddQueenToMainDiagonal(startRow, i);
                            AddQueenToSecondaryDiagonal(startRow, i);
                            startRow++;
                            numberOfPlacedQueens++;
                        }
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            m_Columns[i] = startRow;
                            AddQueenToRow(startRow);
                            AddQueenToMainDiagonal(startRow, i);
                            AddQueenToSecondaryDiagonal(startRow, i);
                            startRow++;
                            numberOfPlacedQueens++;
                        }
                    }
                }

                onOdd = !onOdd;
            }

        }

        public void PrintBoard()
        {
            for (int i = 0; i < m_Rows.Length; i++)
            {
                for (int j = 0; j < m_Columns.Length; j++)
                {
                    if (m_Columns[j] == i)
                    {
                        Console.Write("Q");
                    }
                    else
                    {
                        Console.Write("-");
                    }
                }
                Console.WriteLine();
            }
        }

        public void PrintStats()
        {
            Console.WriteLine("Column Array Data:");
            for (int i = 0; i < m_Columns.Length; i++)
            {
                Console.WriteLine($"{i}: {m_Columns[i]} ");
            }
            Console.WriteLine();

            Console.WriteLine("Row Array Data:");
            for (int i = 0; i < m_Rows.Length; i++)
            {
                Console.WriteLine($"{i}: {m_Rows[i]} ");
            }
            Console.WriteLine();

            Console.WriteLine("Main Diagonal Array Data:");
            for (int i = 0; i < m_MainDiagonal.Length; i++)
            {
                Console.WriteLine($"{i}: {m_MainDiagonal[i]} ");
            }
            Console.WriteLine();

            Console.WriteLine("Secondary Diagonal Array Data:");
            for (int i = 0; i < m_SecondaryDiagonal.Length; i++)
            {
                Console.WriteLine($"{i}: {m_SecondaryDiagonal[i]} ");
            }
            Console.WriteLine();

        }

        public void Solve()
        {
            Console.WriteLine("Starting search.");
            Stopwatch sw = new Stopwatch();
            Console.WriteLine("Starting stopwatch.");
            sw.Start();
            Random milRand = new Random();
            while (!m_IsSolved)
            {


                m_IsSolved = true;

                //int maxConflictQueen = 0;
                //int maxConflictCount = CalculateConflicts(0);
                //for (int i = 1; i < m_Columns.Length; i++)
                //{
                //    int newMaxConflictCount = CalculateConflicts(i);

                //    if (newMaxConflictCount > maxConflictCount)
                //    {
                //        maxConflictCount = newMaxConflictCount;
                //        maxConflictQueen = i;
                //    }
                //}

                //Iterate through columns
                int randQueen = milRand.Next(0, m_QueenNum);
                //Console.WriteLine($"Max conflict queen {randQueen}");
                //Console.WriteLine($"Moving queen {randQueen}");
                int queenConflict = CalculateConflicts(randQueen);

                if (queenConflict != 0)
                {
                    int newMinConflictRow = FindMinConflictRow(queenConflict, m_Columns[randQueen]);
                    if (newMinConflictRow != m_Columns[randQueen])
                    {
                        MoveQueen(randQueen, newMinConflictRow);
                        m_TotalMoves += 1;
                    }
                }

                for (int i = 0; i < m_Columns.Length; i++)
                {
                    int currentQueenConflictCount = CalculateConflicts(i);


                    if (currentQueenConflictCount > 0)
                    {
                        m_IsSolved = false;
                    }
                }
            }

            sw.Stop();
            if (m_TotalMoves >= m_MaxSteps)
            {
                Console.WriteLine("Failed to eliminate conflicts within the given max steps.");
            }
            Console.WriteLine($"Number of moves required: {m_TotalMoves}");
            Console.WriteLine($"Elapsed time:{sw.Elapsed}");
        }


        public void AddQueenToMainDiagonal(int row, int column)
        {
            int index = FindMainDiagonalIndex(row, column);
            m_MainDiagonal[index]++;
        }

        public void AddQueenToSecondaryDiagonal(int row, int column)
        {
            int index = FindSecondDiagonal(row, column);
            m_SecondaryDiagonal[index]++;
        }

        public void RemoveQueenFromMainDiagonal(int row, int column)
        {
            int index = FindMainDiagonalIndex(row, column);
            m_MainDiagonal[index]--;
        }

        public void RemoveQueenFromSecondaryDiagonal(int row, int column)
        {
            int index = FindSecondDiagonal(row, column);
            m_SecondaryDiagonal[index]--;
        }

        public void AddQueenToRow(int row)
        {
            m_Rows[row]++;
        }
        public void RemoveQueenFromRow(int row)
        {
            m_Rows[row]--;
        }

        public int FindMainDiagonalIndex(int row, int column)
        {
            int diff = row - column;
            int index = m_QueenNum - diff;
            return index;
        }

        public int FindSecondDiagonal(int row, int column)
        {
            int index = column + row;
            return index;
        }

        public int CalculateConflicts(int columnArrayIndex)
        {
            int mainDiagIndex = FindMainDiagonalIndex(m_Columns[columnArrayIndex], columnArrayIndex);
            int secondaryDiagIndex = FindSecondDiagonal(m_Columns[columnArrayIndex], columnArrayIndex);

            int rowConflicts = m_Rows[m_Columns[columnArrayIndex]];
            if (rowConflicts == 1)
            {
                rowConflicts = 0;
            }

            int mainDiagConflicts = m_MainDiagonal[mainDiagIndex];
            if (mainDiagConflicts == 1)
            {
                mainDiagConflicts = 0;
            }

            int secondaryDiagConflicts = m_SecondaryDiagonal[secondaryDiagIndex];
            if (secondaryDiagConflicts == 1)
            {
                secondaryDiagConflicts = 0;
            }
            return rowConflicts + mainDiagConflicts + secondaryDiagConflicts;
        }

        public int FindMinConflictRow(int currentConflictCount, int currentRow)
        {
            int newMinConflictRow = 0;
            int newMinConflictCount = currentConflictCount;

            for (int i = 0; i < m_Rows.Length; i++)
            {
                int rowConflicts = m_Rows[i];
                if (rowConflicts < newMinConflictCount)
                {
                    newMinConflictCount = rowConflicts;
                    newMinConflictRow = i;
                }
            }

            if (newMinConflictCount == currentConflictCount)
            {
                return currentRow;
            }

            return newMinConflictRow;


        }

        public void MoveQueen(int columnIndex, int newRow)
        {
            int oldRow = m_Columns[columnIndex];
            m_Columns[columnIndex] = newRow;

            RemoveQueenFromRow(oldRow);
            RemoveQueenFromMainDiagonal(oldRow, columnIndex);
            RemoveQueenFromSecondaryDiagonal(oldRow, columnIndex);

            AddQueenToRow(newRow);
            AddQueenToMainDiagonal(newRow, columnIndex);
            AddQueenToSecondaryDiagonal(newRow, columnIndex);
        }
    }
}
