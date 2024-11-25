using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json; // Assurez-vous d'installer le package NuGet Newtonsoft.Json
using Sudoku.Shared;

namespace Sudoku.Solvers
{
    public class ChocoSolver : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid sudokuGrid)
        {
            string pythonPath = @"C:\Users\elea1\AppData\Local\Programs\Python\Python313\Python.exe"; // Chemin vers l'exécutable Python
            string scriptPath = @"Sudoku.chocosolver\PythonChocoSolver.py"; // Chemin vers le script Python

            // Convertir la grille en JSON
            var inputGrid = new { sudoku = sudokuGrid.Cells };
            string inputJson = JsonConvert.SerializeObject(inputGrid);

            // Configurer le processus
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

                    // Écrire les données JSON dans l'entrée standard
                    using (var writer = process.StandardInput)
                    {
                        writer.Write(inputJson);
                    }

                    // Lire la sortie standard
                    string outputJson = process.StandardOutput.ReadToEnd();

                    // Attendre la fin du processus
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        string error = process.StandardError.ReadToEnd();
                        throw new Exception($"Python script error: {error}");
                    }

                    // Désérialiser la sortie JSON
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
