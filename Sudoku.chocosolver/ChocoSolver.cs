using Sudoku.Shared;

namespace Sudoku.chocosolver;

public class ChocoSolver : ISudokuSolver
{
    public SudokuGrid Solve(SudokuGrid s)
    {
        return s.CloneSudoku();
    }

}