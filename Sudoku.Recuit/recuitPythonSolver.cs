using Python.Runtime;
using Sudoku.Shared;


namespace Sudoku.Recuit;

public class RecuitPythonSolver : PythonSolverBase
{

    public override SudokuGrid Solve(SudokuGrid sudokuGrid)
        {
            string scriptPath = @".\sudoku_solver.py";


			using (PyModule scope = Py.CreateScope())
			{
                // Injectez le script de conversion
				AddNumpyConverterScript(scope);

				// Convertissez le tableau .NET en tableau NumPy
				var pyCells = AsNumpyArray(sudokuGrid.Cells, scope);
				
				// create a Python variable "instance"
				scope.Set("sudoku_grid", pyCells);

				// run the Python script
				string code = File.ReadAllText(scriptPath);
				scope.Exec(code);

				PyObject result = scope.Get("solution");


				// Convertissez le résultat NumPy en tableau .NET
				var managedResult = AsManagedArray(scope, result);

				return new SudokuGrid() { Cells = managedResult };
			}
		}


    protected override void InitializePythonComponents()
    {
        // Installer NumPy si nécessaire
        InstallPipModule("numpy");
        base.InitializePythonComponents();
    }
}

