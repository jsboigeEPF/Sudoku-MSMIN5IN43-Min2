using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Sudoku.Shared;

namespace Sudoku.Solvers
{
    public class ChocoSolverPython : ISudokuSolver
    {
        public SudokuGrid Solve(SudokuGrid sudokuGrid)
        {
            // Assurez-vous que le chemin vers Python et le script est correct.
            string pythonPath = "python";  // Si Python n'est pas dans votre PATH, spécifiez le chemin complet ici.
            string scriptPath = @"..\..\..\..\Sudoku.chocosolver_Python\PythonChocoSolver_Python.py";  // Chemin vers le script Python

            // Préparer les données d'entrée sous forme JSON
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
                // Lancer le processus Python
                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();

                    // Envoyer les données JSON d'entrée au script Python
                    using (var writer = process.StandardInput)
                    {
                        writer.Write(inputJson);
                    }

                    // Lire la sortie JSON du script Python
                    string outputJson = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        string error = process.StandardError.ReadToEnd();
                        throw new Exception($"Erreur du script Python : {error}");
                    }

                    // Désérialiser la sortie JSON et retourner la solution sous forme de SudokuGrid
                    var result = JsonConvert.DeserializeObject<dynamic>(outputJson);
                    int[,] solution = JsonConvert.DeserializeObject<int[,]>(result.solution.ToString());

                    // Retourner la solution sous forme de SudokuGrid
                    return new SudokuGrid { Cells = solution };
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Échec de l'exécution du script Python : {ex.Message}", ex);
            }
        }
    }
}
