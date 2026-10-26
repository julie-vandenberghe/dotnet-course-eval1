# VotePreferentiel

Worker Service C# (.NET 10) calcule le gagnant d'une élection au **vote préférentiel**.

## Fonctionnement
1. Le Worker télécharge le fichier JSON de votes (URL dans `appsettings.json`).
2. Le fichier est désérialisé en `VoteModel`.
3. `VoteCounter.GetWinner` compte les premiers choix, élimine le ou les candidats les moins votés, reporte leurs voix sur le choix suivant, et recommence jusqu'à ce qu'il reste un seul candidat.
4. Le nom du gagnant est affiché dans la console.

## Structure
| Projet | Rôle |
|---|---|
| `VotePreferentiel` | Bibliothèque : `VoteModel`, `VoteProvider`, `IVoteCounter`, `VoteCounter` |
| `VotePreferentiel.Worker` | Worker Service : appelle le provider et le compteur (injection de dépendances) |
| `VotePreferentiel.Tests` | Tests unitaires TUnit de `GetWinner` |

## Configuration
Dans `VotePreferentiel.Worker/appsettings.json` :
```json
"VoteSettings": { "Url": "https://raw.githubusercontent.com/JLou/dotnet-course/refs/heads/main/eval-1/votes.json" }
```
Pour un plus petit jeu d'exemple : remplacer `votes.json` par `votes.example.json` à la place dans l'URL.

## Lancer
```bash
dotnet run --project VotePreferentiel.Worker
dotnet test
```

## Tests couverts
Cas nominal, un seul candidat, report des préférences, égalité en dernière position, bulletin vide (exception).