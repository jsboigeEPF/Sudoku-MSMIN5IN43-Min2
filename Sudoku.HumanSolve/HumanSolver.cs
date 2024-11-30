using Sudoku.Shared;
using Sudoku.SolverTechniques;

namespace Sudoku.HumanSolving
{
     partial class Solver
    {
        private static readonly string[] TupleStr = new string[5] { string.Empty, "single", "pair", "triple", "quadruple" };
        private readonly Func<SudokuGrid, bool>[] _techniques;
        private readonly SudokuGrid _grid; // Instance de la grille de Sudoku

        // Constructeur de Solver
        public Solver(SudokuGrid grid)
        {
            _grid = grid;
            _techniques = InitSolverTechniques();
        }

        // Fonction pour initialiser les techniques (références aux méthodes statiques dans les classes des techniques)
        private Func<SudokuGrid, bool>[] InitSolverTechniques()
        {
            return new Func<SudokuGrid, bool>[]
            {
                SolverTechniques.Solver_HiddenSingle.Apply,
                SolverTechniques.Solver_NakedPair.Apply,
                SolverTechniques.Solver_HiddenPair.Apply,
                SolverTechniques.Solver_LockedCandidate.Apply,
                SolverTechniques.Solver_PointingTuple.Apply,
                SolverTechniques.Solver_NakedTriple.Apply,
                SolverTechniques.Solver_HiddenTriple.Apply,
                SolverTechniques.Solver_XWing.Apply,
                SolverTechniques.Solver_Swordfish.Apply,
                SolverTechniques.Solver_YWing.Apply,
                SolverTechniques.Solver_XYZWing.Apply,
                SolverTechniques.Solver_XYChain.Apply,
                SolverTechniques.Solver_NakedQuadruple.Apply,
                SolverTechniques.Solver_HiddenQuadruple.Apply,
                SolverTechniques.Solver_Fish.Apply,
                SolverTechniques.Solver_UniqueRectangle.Apply,
                SolverTechniques.Solver_HiddenRectangle.Apply,
                SolverTechniques.Solver_AvoidableRectangle.Apply
            };
        }

        // Fonction qui applique les techniques au SudokuGrid et retourne vrai si le puzzle est résolu
        public bool Solve()
        {
            bool solved = false;

            // On applique chaque technique de résolution sur la grille de Sudoku
            foreach (var technique in _techniques)
            {
                if (technique(_grid)) // Appel de chaque fonction technique
                {
                    solved = true;
                }
            }

            return solved;
        }
    }
}
