using System;
using System.Text;
using Sudoku.Shared; 
using Kermalis.SudokuSolver; 

namespace Kermalis.SudokuSolver
{
   public class HumanSolverWrapper
    {
        public SudokuGrid SolveSudoku(SudokuGrid grid)
        {
            // Convertir SudokuGrid en format de string pour le Human Solver
            var puzzleString = ConvertToStringFormat(grid);
            Puzzle puzzle = Puzzle.Parse(puzzleString);
            var solver = new Solver(puzzle);

            // Essayer de résoudre le puzzle
            if (solver.TrySolve())
            {
                return ConvertToSudokuGrid(solver.Puzzle);
            }

            throw new Exception("Le solveur n'a pas pu résoudre le puzzle.");
        }

        private ReadOnlySpan<string> ConvertToStringFormat(SudokuGrid grid)
        {
            string[] rows = new string[9];
            for (int row = 0; row < 9; row++)
            {
                StringBuilder sb = new StringBuilder();
                for (int col = 0; col < 9; col++)
                {
                    int value = grid.Cells[row, col]; // Accède directement au tableau Cells
                    sb.Append(value == 0 ? '-' : value.ToString()[0]); // Utilise '-' pour les cellules vides
                }
                rows[row] = sb.ToString();
            }
            return rows;
        }

        private SudokuGrid ConvertToSudokuGrid(Puzzle puzzle)
        {
            var grid = new SudokuGrid();
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    grid.Cells[row, col] = puzzle[col, row].Value; // Récupérer la valeur du puzzle
                }
            }
            return grid;
        }
    }
}