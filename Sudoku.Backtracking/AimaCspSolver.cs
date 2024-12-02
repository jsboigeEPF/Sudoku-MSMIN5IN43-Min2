using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Shared;

namespace Sudoku.Backtracking
{
    public class AimaCspSolver : ISudokuSolver
    {
        // Variable: Each cell in Sudoku grid is represented as a variable (row, col).
        private class Variable
        {
            public int Row { get; set; }
            public int Col { get; set; }
        }

        // Domain: Possible values for each cell (1-9 in standard Sudoku).
        private readonly List<int> _domain = Enumerable.Range(1, 9).ToList();

        // CSP Variables
        private readonly Variable[,] _variables = new Variable[9, 9];
        private readonly Dictionary<Variable, List<int>> _domains = new();

        public AimaCspSolver()
        {
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    var variable = new Variable { Row = row, Col = col };
                    _variables[row, col] = variable;
                    _domains[variable] = new List<int>(_domain);
                }
            }
        }

        // Constraint: No duplicates in rows, columns, or 3x3 grids.
        private bool IsValidAssignment(Variable variable, int value, int[,] grid)
        {
            // Row constraint
            for (int col = 0; col < 9; col++)
            {
                if (grid[variable.Row, col] == value)
                    return false;
            }

            // Column constraint
            for (int row = 0; row < 9; row++)
            {
                if (grid[row, variable.Col] == value)
                    return false;
            }

            // 3x3 grid constraint
            int startRow = (variable.Row / 3) * 3;
            int startCol = (variable.Col / 3) * 3;

            for (int r = startRow; r < startRow + 3; r++)
            {
                for (int c = startCol; c < startCol + 3; c++)
                {
                    if (grid[r, c] == value)
                        return false;
                }
            }

            return true;
        }

        // Backtracking Solver
        public bool SolveSudoku(int[,] grid)
        {
            var variable = SelectUnassignedVariable(grid);

            if (variable == null) // All variables are assigned
                return true;

            foreach (var value in _domains[variable])
            {
                if (IsValidAssignment(variable, value, grid))
                {
                    grid[variable.Row, variable.Col] = value;

                    if (SolveSudoku(grid)) // Recursive call
                        return true;

                    grid[variable.Row, variable.Col] = 0; // Backtrack
                }
            }

            return false;
        }

        private Variable SelectUnassignedVariable(int[,] grid)
        {
            foreach (var variable in _domains.Keys)
            {
                if (grid[variable.Row, variable.Col] == 0)
                    return variable;
            }
            return null;
        }

        // Implementation of ISudokuSolver
        public SudokuGrid Solve(SudokuGrid sudokuGrid)
        {
            // The Cells property is already a 2D array (int[,])
            int[,] grid = sudokuGrid.Cells;

            // Solve the Sudoku
            SolveSudoku(grid);

            // The grid is modified in-place, so no need for conversion
            return sudokuGrid;
        }
    }
}
