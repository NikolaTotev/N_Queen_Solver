using System;
using System.Collections.Generic;
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

        public NQueenSolver(int queenNum)
        {

            m_QueenNum = queenNum;
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
            Random rand = new Random(42);
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
            //Iterate through columns
            for (int i = 0; i < m_Columns.Length; i++)
            {
                int currentQueenConflictCount = CalculateConflicts(i);


                if (currentQueenConflictCount > 0)
                {


                    if (minConflictRow != m_Columns[i])
                    {
                        m_Columns[i] = minConflictRow;
                        m_TotalMoves += 1;
                    }
                }

                //Check # of queens that are on the row
                //Get main diagonal
                //Get secondary diagonal
                //Calculate conflicts on both diagonals
            }
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
        public void RemoveQueenToRow(int row)
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
            int mainDiagConflicts = m_MainDiagonal[mainDiagIndex];
            int secondaryDiagConflicts = m_SecondaryDiagonal[mainDiagConflicts];
            return rowConflicts + mainDiagConflicts + secondaryDiagConflicts;
        }

        public void MoveQueen(int columnIndex, int newRow)
        {

        }
    }
}
