# Math Game

Console math game in C# with difficulty levels, timed random mode, and session history. Built with Spectre.Console.

## Features

- **Four operations** — addition, subtraction, multiplication, division
- **Three difficulty levels** — Easy (1–9), Medium (1–99), Hard (1–999)
- **Standard mode** — choose an operation and number of questions per game between 5 and 10 questions.
- **Random mode** — 30-second timed mode with randomly selected operations
- **Session history** — view scores and individual questions for each game played

## Tech Stack

- C# / .NET 10
- [Spectre.Console](https://spectreconsole.net/) for styled terminal UI

## Getting Started

**Prerequisites:** .NET 10 SDK

```bash
git clone https://github.com/YOUR_USERNAME/MathGame.git
cd MathGame
dotnet run
```

## Project Structure

```
MathGame/
├── Enums/
│   ├── Difficulty.cs       # Easy, Medium, Hard
│   └── IntroChoice.cs      # Menu options
├── Model/
│   └── GameResults.cs      # Stores score and questions per game
├── Structs/
│   └── OperationResults.cs # Result of a single question
├── UserInterface/
│   ├── MainMenu.cs         # Game loop, menus, scoring, results display
│   └── Operations.cs       # Math logic and user input per question
└── Program.cs
```

## Part of

[C# Academy](https://www.thecsharpacademy.com/) — beginner project track.
