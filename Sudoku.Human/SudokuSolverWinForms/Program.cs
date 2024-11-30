using System;
using Sudoku.Shared;
using Kermalis.SudokuSolver;

class Program
{
    static void Main(string[] args)
    {
        // Charger un puzzle depuis un fichier ou crée-le manuellement
        var puzzles = SudokuGrid.ReadSudokuFile("path/to/your/sudoku_file.txt"); // Ajuste ce chemin pour le fichier de puzzle
        var puzzleToSolve = puzzles[0]; // Utilise le premier puzzle pour le test

        Console.WriteLine("Puzzle initial:");
        Console.WriteLine(puzzleToSolve.ToString());

        // Créer une instance du wrapper
        var humanSolverWrapper = new HumanSolverWrapper();

        // Résoudre le puzzle
        try
        {
            var solvedPuzzle = humanSolverWrapper.SolveSudoku(puzzleToSolve);
            Console.WriteLine("Puzzle résolu:");
            Console.WriteLine(solvedPuzzle.ToString());

            // Vérifier la validité de la solution
            if (solvedPuzzle.IsValid(puzzleToSolve)) // Assurez-vous d'avoir une méthode IsValid
            {
                Console.WriteLine("La solution est valide.");
            }
            else
            {
                Console.WriteLine("La solution n'est pas valide.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur lors de la résolution du puzzle : " + ex.Message);
        }
    }
}