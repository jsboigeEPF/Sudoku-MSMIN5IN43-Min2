using Python.Runtime;
using Sudoku.Shared;

namespace Sudoku.Backtracking;

public class SimulatedAnnealingPythonSolver : PythonSolverBase
{
    public override SudokuGrid Solve(SudokuGrid sudoku)
    {
        using (Py.GIL())
        {
            // Ajouter le chemin du fichier sudoku_solver.py au sys.path
            string pythonScriptPath = @"C:\Users\Incha\programmation\IA\IA2\Sudoku-MSMIN5IN43-Min2\Sudoku.Backtracking";
            dynamic sys = Py.Import("sys");
            sys.path.append(pythonScriptPath);

            // Charger le module Python
            dynamic sudokuSolverModule = Py.Import("sudoku_solver");

            // Convertir la grille Sudoku (C#) en tableau Python (list of lists)
            var pySudokuInput = sudoku.Cells.ToPython(); // Extension pour convertir en PyObject

            // Appeler la fonction Python et récupérer le résultat
            dynamic pyResult = sudokuSolverModule.solveSudokuWrapper(pySudokuInput);

            // Convertir le résultat Python (list of lists) en tableau C#
            int[,] solvedGrid = PythonToCsArray(pyResult);

            // Retourner un SudokuGrid avec la grille résolue
            return new SudokuGrid { Cells = solvedGrid };
        }
    }

    private int[,] PythonToCsArray(PyObject pyResult)
    {
        int rows = (int)pyResult.Length(); // Nombre de lignes
        int cols = (int)pyResult[0].Length(); // Nombre de colonnes, supposées uniformes

        int[,] resultArray = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            PyObject row = pyResult[i];
            for (int j = 0; j < cols; j++)
            {
                resultArray[i, j] = row[j].As<int>();
            }
        }

        return resultArray;
    }

    protected override void InitializePythonComponents()
    {
        // Installer NumPy si nécessaire
        InstallPipModule("numpy");
        base.InitializePythonComponents();
    }
}
