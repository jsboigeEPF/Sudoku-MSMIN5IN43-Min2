using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json; 
using Sudoku.Shared;

namespace Sudoku.Solvers
{
    public class ChocoSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid sudokuGrid)
        {
            string pythonPath = @"C:\Users\elea1\AppData\Local\Programs\Python\Python313\Python.exe"; // Chemin vers l'exécutable Python
            string scriptPath = @"..\..\..\..\Sudoku.chocosolver\PythonChocoSolver.py";

           
            var inputGrid = new { sudoku = sudokuGrid.Cells };
            string inputJson = JsonConvert.SerializeObject(inputGrid);

            var processStartInfo = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = scriptPath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    using (var writer = process.StandardInput)
                    {
                        writer.Write(inputJson);
                    }
                    string outputJson = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        string error = process.StandardError.ReadToEnd();
                        throw new Exception($"Python script error: {error}");
                    }
                    var result = JsonConvert.DeserializeObject<dynamic>(outputJson);
                    int[,] solution = JsonConvert.DeserializeObject<int[,]>(result.solution.ToString());
                    return new SudokuGrid { Cells = solution };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to execute Python script: {ex.Message}", ex);
            }
        }
    }
}
