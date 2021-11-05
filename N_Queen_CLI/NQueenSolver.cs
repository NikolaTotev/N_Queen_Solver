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
        private long m_TotalMoves = 0;
        private bool m_IsSolved = false;
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

        public void HorsePatternSetup()
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

        public void RandomSetup()
        {
            Random rand = new Random();
            for (int i = 0; i < m_Columns.Length - 1; i++)
            {
                int row = rand.Next(0, m_QueenNum - 1);
                m_Columns[i] = row;
                AddQueenToRow(row);
                AddQueenToMainDiagonal(row, i);
                AddQueenToSecondaryDiagonal(row, i);
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
            List<int> maxIndexes = new List<int>();

            while (!m_IsSolved)
            {
                //Clear previous max conflict indexes.
                maxIndexes.Clear();

                Dictionary<int, int> queenConflictPairs = new Dictionary<int, int>();

                for (int i = 0; i < m_Columns.Length; i++)
                {
                    int conflictCount = CalculateExistingConflicts(m_Columns[i], i);

                    queenConflictPairs.Add(i, conflictCount);
                }


                var items = from pair in queenConflictPairs
                    orderby pair.Value ascending
                    select pair;

                KeyValuePair<int,int> maxConflict = items.Last();
                
                foreach (KeyValuePair<int, int> pair in items)
                {
                    if (pair.Value == maxConflict.Value)
                    {
                        maxIndexes.Add(pair.Key);
                    }
                }


                int maxConflictQueen;

                if (maxIndexes.Count > 1)
                {
                    maxConflictQueen = maxIndexes[milRand.Next(0, maxIndexes.Count)];
                }
                else
                {
                    maxConflictQueen = maxIndexes[0];
                }

                int randQueen = maxConflictQueen;
                int queenConflict = CalculateExistingConflicts(m_Columns[randQueen], randQueen);

                if (queenConflict != 0)
                {
                    int newMinConflictRow = FindMinConflictPosition(queenConflict, m_Columns[randQueen], randQueen);
                    if (newMinConflictRow != m_Columns[randQueen])
                    {
                        MoveQueen(randQueen, newMinConflictRow);
                        m_TotalMoves += 1;
                    }
                    m_IsSolved = true;
                    for (int i = 0; i < m_Columns.Length; i++)
                    {
                        int currentQueenConflictCount = CalculatePotentialConflicts(m_Columns[i], i);


                        if (currentQueenConflictCount > 0)
                        {
                            m_IsSolved = false;
                        }
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
            return index - 1;
        }

        public int FindSecondDiagonal(int row, int column)
        {
            int index = column + row;
            return index;
        }

        public int CalculatePotentialConflicts(int rowArrayIndex, int columnArrayIndex)
        {
            int mainDiagIndex = FindMainDiagonalIndex(rowArrayIndex, columnArrayIndex);
            int secondaryDiagIndex = FindSecondDiagonal(rowArrayIndex, columnArrayIndex);

            int rowConflicts = m_Rows[rowArrayIndex];

            int mainDiagConflicts = m_MainDiagonal[mainDiagIndex];

            int secondaryDiagConflicts = m_SecondaryDiagonal[secondaryDiagIndex];

            return rowConflicts + mainDiagConflicts + secondaryDiagConflicts;
        }

        public int CalculateExistingConflicts(int rowArrayIndex, int columnArrayIndex)
        {
            int mainDiagIndex = FindMainDiagonalIndex(rowArrayIndex, columnArrayIndex);
            int secondaryDiagIndex = FindSecondDiagonal(rowArrayIndex, columnArrayIndex);

            int rowConflicts = m_Rows[rowArrayIndex] - 1;

            int mainDiagConflicts = m_MainDiagonal[mainDiagIndex] - 1;

            int secondaryDiagConflicts = m_SecondaryDiagonal[secondaryDiagIndex] - 1;

            return rowConflicts + mainDiagConflicts + secondaryDiagConflicts;
        }

        public int FindMinConflictPosition(int currentConflictCount, int currentRow, int currentColumn)
        {
            int newMinConflictPos = 0;
            int newMinConflictCount = currentConflictCount;

            for (int i = 0; i < m_Rows.Length; i++)
            {
                int posConflicts;
                if (i == currentRow)
                {
                    posConflicts = CalculateExistingConflicts(i, currentColumn);
                }
                else
                {
                    posConflicts = CalculatePotentialConflicts(i, currentColumn);
                }

                if (posConflicts <= newMinConflictCount)
                {
                    newMinConflictCount = posConflicts;
                    if (i != currentRow)
                    {
                        newMinConflictPos = i;
                    }
                }

                if (newMinConflictCount == 0)
                {
                    break;
                }
            }

            return newMinConflictPos;
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
