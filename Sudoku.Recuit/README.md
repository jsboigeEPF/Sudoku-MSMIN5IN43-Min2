Bienvenue sur notre dépôt Sudoku recuit simulé

## Présentation de notre solution

Comme évoqué lors de notre présentation, la méthode de résolution par recuit simulé n'est pas la plus performante ! En effet, notre algorythme utilise une fonction coût
qui va remplir chaque carré de manière aléatoire en ajoutant des valeurs dans les cases restantes sans oublier la règle que tous les numéros d'un carré doivent être
différents. Une fois la grille complète, la fonction coût compte les doublons de chaque ligne et de chaque colonne et évalue un taux d'erreur. A chaque itération, la
fonction essaye de diminuer ce taux d'erreur et à force d'essais atteint 0 et une résolution entière du sudoku. Ce modèle marche sur des sudokus simples mais devient
peu performant à mesure que l'on complexifie le niveau du sudoku !
