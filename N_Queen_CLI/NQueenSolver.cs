using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
            //Initialize and Start stopwatch for timing purposes
            Console.WriteLine("Starting search.");
            Stopwatch sw = new Stopwatch();
            Console.WriteLine("Starting stopwatch.");
            sw.Start();

            //Create instance of Random class to be used in queen selection
            Random milRand = new Random();
            
            //Create a list to hold all column indexes that have a maximum number of conflicts
            List<int> maxIndexes = new List<int>();

            //While loop will run until m_IsSolved flag is not true
            while (!m_IsSolved)
            {
                //Clear previous max conflict indexes.
                maxIndexes.Clear();

                //Create a dictionary to hold <colIndex, conflict> pairs. This is used to sort queens by number of conflicts.
                Dictionary<int, int> queenConflictPairs = new Dictionary<int, int>();

                //Iterate through all queens and put the queenConflict pair in the dictionary
                for (int i = 0; i < m_Columns.Length; i++)
                {
                    int conflictCount = CalculateExistingConflicts(m_Columns[i], i);
                    queenConflictPairs.Add(i, conflictCount);
                }

                //Order the dictionary by value
                var items = from pair in queenConflictPairs
                    orderby pair.Value ascending
                    select pair;

                //Extract the last value from the items variable, this should be the maximum number of conflicts
                KeyValuePair<int,int> maxConflict = items.Last();
                
                //Iterate through the items variable to check if there are any other indexes that have the same max number of conflicts
                foreach (KeyValuePair<int, int> pair in items)
                {
                    //If a pair has a value equal to maxConflict.value it is added to the list.
                    if (pair.Value == maxConflict.Value)
                    {
                        maxIndexes.Add(pair.Key);
                    }
                }
                
                int maxConflictQueen;

                //If there is more than one index in the maxIndexes list, a random index is selected from the list. 
                //Else just the first index is used.
                if (maxIndexes.Count > 1)
                {
                    maxConflictQueen = maxIndexes[milRand.Next(0, maxIndexes.Count)];
                }
                else
                {
                    maxConflictQueen = maxIndexes[0];
                }
                
                //Assign max conflict queen index. This is for easy of editing.
                int randQueen = maxConflictQueen;

                //Get the queen conflict, again used for eas of editing.
                int queenConflict = CalculateExistingConflicts(m_Columns[randQueen], randQueen);

                //The the chosen queen has conflicts we try to move it to a row with the least amount of conflicts
                if (queenConflict != 0)
                {
                    //Find min conflict position in the array.
                    int newMinConflictRow = FindMinConflictPosition(queenConflict, m_Columns[randQueen], randQueen);

                    //This might be a redundant check considering there is a similar one in the FindMinConflictPosition function.
                    if (newMinConflictRow != m_Columns[randQueen])
                    {
                        //Move the selected queen to the new row.
                        MoveQueen(randQueen, newMinConflictRow);
                        m_TotalMoves += 1;
                    }

                    //Assume that the queens satisfy the constraints and have 0 conflicts.
                    m_IsSolved = true;

                    //Iterate through all columns and check how many conflicts exist.
                    for (int i = 0; i < m_Columns.Length; i++)
                    {
                        //The check uses the function CalculateExistingConflicts because we know that there will be at least one queen.
                        int currentQueenConflictCount = CalculateExistingConflicts(m_Columns[i], i);

                        //If any queen has a conflict the flag is set to false and the algorithm will start again from the top.
                        if (currentQueenConflictCount > 0)
                        {
                            m_IsSolved = false;
                        }
                    }
                }

            }

            //Stop the stopwatch right after the algorithm has found a solution
            sw.Stop();
            
            //Redundant check that uses a max steps variable
            if (m_TotalMoves >= m_MaxSteps)
            {
                Console.WriteLine("Failed to eliminate conflicts within the given max steps.");
            }
            
            //Print the stats for the solution.
            Console.WriteLine($"Number of moves required: {m_TotalMoves}");
            Console.WriteLine($"Elapsed time:{sw.Elapsed}");
        }

        public void AddQueenToMainDiagonal(int row, int column)
        {
            //First find in which main diagonal the queen is.
            int index = FindMainDiagonalIndex(row, column);
            //Add 1 to that diagonal
            m_MainDiagonal[index]++;
        }

        public void AddQueenToSecondaryDiagonal(int row, int column)
        {
            //First find in which secondary diagonal the queen is.
            int index = FindSecondDiagonal(row, column);
            //Add 1 to that diagonal
            m_SecondaryDiagonal[index]++;
        }

        public void RemoveQueenFromMainDiagonal(int row, int column)
        {
            //Same as AddQueenToMainDiagonal, but just removing 1
            int index = FindMainDiagonalIndex(row, column);
            m_MainDiagonal[index]--;
        }

        public void RemoveQueenFromSecondaryDiagonal(int row, int column)
        {
            //Same as AddQueenToSecondaryDiagonal, but just removing 1
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

        //See excel table in repo for diagonal calculations.
        //There is a sheet that shows everything very nicely.
        //No need to figure it out just by looking at this code :)
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

        public int CalculatePotentialConflicts(int rowArrayIndex, int columnArrayIndex)
        {
            //Find the main diagonal index
            int mainDiagIndex = FindMainDiagonalIndex(rowArrayIndex, columnArrayIndex);
            //Find the secondary diagonal index.
            int secondaryDiagIndex = FindSecondDiagonal(rowArrayIndex, columnArrayIndex);

            //Get conflicts for row
            int rowConflicts = m_Rows[rowArrayIndex];

            //Conflicts for main diagonal
            int mainDiagConflicts = m_MainDiagonal[mainDiagIndex];

            //Conflicts for secondary diagonal
            int secondaryDiagConflicts = m_SecondaryDiagonal[secondaryDiagIndex];


            //Return the sum of all conflicts
            return rowConflicts + mainDiagConflicts + secondaryDiagConflicts;
        }

        public int CalculateExistingConflicts(int rowArrayIndex, int columnArrayIndex)
        {
            //Same as in the CalculatePotentialConflicts function
            int mainDiagIndex = FindMainDiagonalIndex(rowArrayIndex, columnArrayIndex);
            int secondaryDiagIndex = FindSecondDiagonal(rowArrayIndex, columnArrayIndex);

            //Just remove 1 from row, main and secondary diagonal conflicts.
            //We do this because we know that the queen we selected is there.
            int rowConflicts = m_Rows[rowArrayIndex] - 1;

            int mainDiagConflicts = m_MainDiagonal[mainDiagIndex] - 1;

            int secondaryDiagConflicts = m_SecondaryDiagonal[secondaryDiagIndex] - 1;

            return rowConflicts + mainDiagConflicts + secondaryDiagConflicts;
        }

        public int FindMinConflictPosition(int currentConflictCount, int currentRow, int currentColumn)
        {
            //Set variables by default.
            int newMinConflictPos = 0;
            //minConflictCount is set to the current number of conflicts as a base.
            int newMinConflictCount = currentConflictCount;

            //Iterate through all of the rows in the current column.
            for (int i = 0; i < m_Rows.Length; i++)
            {
                int posConflicts;

                //If the current row is the one that contains the current queen, we use the CalculateExistingConflicts function.
                //This function takes into account that the given rows/diagonals will have at least 1 queen.
                if (i == currentRow)
                {
                    posConflicts = CalculateExistingConflicts(i, currentColumn);
                }
                //If the row is not the one that contains the current queen, we use the CalculatePotentialConflicts.
                //This function just calculates the number of conflicts for a position by summing the number of queens on the  row, main and secondary diagonals.
                else
                {
                    posConflicts = CalculatePotentialConflicts(i, currentColumn);
                }

                //If the conflicts at current position is smaller than or equal to the current minimum, update the mimimum.
                if (posConflicts <= newMinConflictCount)
                {
                    newMinConflictCount = posConflicts;
                    //Update the row only if the row is different from the row on which the queen is.
                    //This is to avoid the situation where the queen doesn't move even though there are valid moves that might lead to a solution.
                    if (i != currentRow)
                    {
                        newMinConflictPos = i;
                    }
                }

                //If the new minimum is zero, stop the loop because there can't be a smaller value than 0.
                if (newMinConflictCount == 0)
                {
                    break;
                }
            }

            //Return the position.
            return newMinConflictPos;
        }

        public void MoveQueen(int columnIndex, int newRow)
        {
            //Save the old row number
            int oldRow = m_Columns[columnIndex];
            //Update the column array
            m_Columns[columnIndex] = newRow;

            //Remove the queen from the old row, main and second diagonals.
            RemoveQueenFromRow(oldRow);
            //The convention for all functions accepting row and column args is to have the row first than then the column
            RemoveQueenFromMainDiagonal(oldRow, columnIndex);
            RemoveQueenFromSecondaryDiagonal(oldRow, columnIndex);

            //Add a queen to the new row, main and second diagonals.
            AddQueenToRow(newRow);
            AddQueenToMainDiagonal(newRow, columnIndex);
            AddQueenToSecondaryDiagonal(newRow, columnIndex);
        }
    }
}
