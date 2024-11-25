import pychoco
import numpy as np

def solve_sudoku(sudoku_grid):
    model = pychoco.Model()

    cells = [[model.intVar(1, 9) for _ in range(9)] for _ in range(9)]

    # Contraintes de lignes, colonnes et blocs
    for i in range(9):
        model.allDifferent([cells[i][j] for j in range(9)]).post()  # Lignes
        model.allDifferent([cells[j][i] for j in range(9)]).post()  # Colonnes

    for boxRow in range(0, 9, 3):
        for boxCol in range(0, 9, 3):
            model.allDifferent([
                cells[row][col]
                for row in range(boxRow, boxRow + 3)
                for col in range(boxCol, boxCol + 3)
            ]).post()  # Blocs

    # Contraintes des valeurs initiales
    for i in range(9):
        for j in range(9):
            if sudoku_grid[i][j] != 0:
                model.arithm(cells[i][j], "=", sudoku_grid[i][j]).post()

    solver = model.getSolver()
    if solver.solve():
        return [[cells[i][j].getValue() for j in range(9)] for i in range(9)]
    else:
        return None

if __name__ == "__main__":
    import json
    import sys

    # Lire la grille Sudoku en entrée (JSON)
    input_data = json.loads(sys.stdin.read())
    sudoku_grid = input_data["sudoku"]

    # Résoudre le Sudoku
    solution = solve_sudoku(sudoku_grid)

    # Retourner le résultat
    print(json.dumps({"solution": solution}))
