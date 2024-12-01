import pychoco
import json

def solve_sudoku(sudoku_grid):
    model = pychoco.Model()

    # Créer une grille de variables (9x9) avec des valeurs entre 1 et 9
    cells = [[model.intvar(1, 9) for _ in range(9)] for _ in range(9)]

    # Contraintes de ligne, colonne et boîte 3x3
    for i in range(9):
        model.all_different([cells[i][j] for j in range(9)]).post()  # Lignes
        model.all_different([cells[j][i] for j in range(9)]).post()  # Colonnes

    for boxRow in range(0, 9, 3):
        for boxCol in range(0, 9, 3):
            model.all_different([ 
                cells[row][col]
                for row in range(boxRow, boxRow + 3)
                for col in range(boxCol, boxCol + 3)
            ]).post()  # Boîtes 3x3

    # Contraintes pour les cases pré-remplies
    for i in range(9):
        for j in range(9):
            if sudoku_grid[i][j] != 0:
                model.arithm(cells[i][j], "=", sudoku_grid[i][j]).post()

    # Résolution du modèle
    solver = model.get_solver()
    if solver.solve():
        # Retourne la solution sous forme de grille 9x9
        return [[cells[i][j].get_value() for j in range(9)] for i in range(9)]
    else:
        return None

# Fonction principale pour l'appel
if __name__ == "__main__":
    import sys
    input_data = json.loads(sys.stdin.read())
    sudoku_grid = input_data["sudoku"]

    solution = solve_sudoku(sudoku_grid)
    
    # Affichage du résultat sous forme de JSON
    print(json.dumps({"solution": solution}))
