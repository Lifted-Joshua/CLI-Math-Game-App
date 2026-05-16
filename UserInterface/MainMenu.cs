using System.Diagnostics;
using System.Timers;
using MathGame.Enums;
using MathGame.Model;
using MathGame.Structs;
using Spectre.Console;
using Timer = System.Timers.Timer;

namespace MathGame.UserInterface;
public static class MainMenu
{
    /// <summary>
    /// Gets or sets whether main thread should read
    /// </summary>
    private static volatile bool _shouldRead;
    private static int _score = 0;
    private static bool _gameIsRunning = true;
    private static int _totalNumberOfQuestions = 0;
    public static Difficulty GameDifficulty { get; private set; }
    private static List<string> _questions = new List<string>();
    private static List<GameResults> _gameResultsList = new List<GameResults>();
    static Timer timer = new Timer(1000);
    static Random randomNum = new Random();
    private static int _countdownTime = 30;
    private static int _timeInMinutes;
    private readonly static Dictionary<string , Func<int, int, int>> _mathOperations = new Dictionary<string, Func<int, int, int>>()
    {
        {"+", Operations.Add},
        {"-", Operations.Subtract},
        {"*", Operations.Multiply},
        {"/", Operations.Divide},
    };
    private readonly static Dictionary<int, string> _mapNumberToOperation = new Dictionary<int, string>()
    {
        {0, "+"},
        {1, "-"},
        {2, "*"},
        {3, "/"},
    };

    public static void Intro()
    {
        var userChoice = AnsiConsole.Prompt(
            new SelectionPrompt<IntroChoice>()
                .Title("\nSelect [green]an option[/]?")
                .AddChoices(IntroChoice.StartGame, IntroChoice.StartRandomGame, IntroChoice.ViewResults, IntroChoice.ViewQuestions, IntroChoice.EndGame));

        switch(userChoice)
        {
            case IntroChoice.StartGame:
                ChooseDifficulty();
                break;
            case IntroChoice.StartRandomGame:
                RandomGame();
                break;
            case IntroChoice.ViewResults:
                DisplayResults();
                break;
            case IntroChoice.ViewQuestions:
                DisplayQuestions();
                break;
            case IntroChoice.EndGame:
                EndGame();
                break;
        }

    }

    private static void ChooseDifficulty()
    {
        GameDifficulty = AnsiConsole.Prompt(
                new SelectionPrompt<Difficulty>()
                    .Title("Select [red]difficulty?[/]")
                    .AddChoices(Difficulty.Easy, Difficulty.Medium, Difficulty.Hard));

        StartGame();
    }


    private static void StartGame()
    {
        var stopwatch = Stopwatch.StartNew();
        while(_gameIsRunning)
        {
            _totalNumberOfQuestions = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("Select [green]the number of question[/] you would you like in this game?")
                    .AddChoices(5, 6, 7, 8, 9, 10));
            ChooseOperation(_totalNumberOfQuestions);
            _gameIsRunning = false;
        }

        if(!_gameIsRunning)
        {
            stopwatch.Stop();
            AnsiConsole.MarkupLine($"Your score [green]{_score} /[/] [red]{_totalNumberOfQuestions} questions correct[/]");
            AnsiConsole.WriteLine($"Time taken: {stopwatch.Elapsed.ToString(@"mm\:ss")}");
            AnsiConsole.MarkupLine($"[Green]Math Game has ended![/] \n");

            GameResults gameResults = new GameResults(_questions, _score, _totalNumberOfQuestions);
            _gameResultsList.Add(gameResults);
            _questions.Clear();
            _gameIsRunning = true;
            _totalNumberOfQuestions = 0;
            _score = 0;
        }
    }

    // Method for running the game and generating the random inputs
    private static void ChooseOperation(int numOfQuestions)
    {
        while(numOfQuestions > 0)
        {
            var operation = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an [green]operation[/] ")
                    .AddChoices("+", "-", "*", "/"));

            PerformOperation(operation: operation, randomNum: null, false);

            numOfQuestions--;
        }
    }

    private static void PerformOperation(string? operation, int? randomNum, bool isRandomGameSelected)
    {
        if(!string.IsNullOrWhiteSpace(operation))
        {
            HandleOperationResults(Operations.Calculate(operation, _mathOperations[operation]), isRandomGameSelected);
        }

        else if(randomNum != null)
        {
            string operationFromDictionary = _mapNumberToOperation[randomNum!.Value];
            HandleOperationResults(Operations.Calculate(operationFromDictionary, _mathOperations[operationFromDictionary]), isRandomGameSelected);
        }
    }

    private static void RandomGame()
    {
        _timeInMinutes = _countdownTime;

        timer.Elapsed += OnTimedEvent!;
        timer.AutoReset = true;
        timer.Enabled = true;

        while(!_shouldRead)
        {
            var choice = randomNum.Next(0, 4);
            PerformOperation(operation: null, randomNum: choice, true);
        }

        if(_shouldRead)
        {
            EndRandomGame();
        }

    }

    private static void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        if(_countdownTime > 0)
        {
            _countdownTime--;
        } else
        {
            timer.Stop();
            _shouldRead = true;
        }
    }

    private static void EndRandomGame()
    {
        _shouldRead = false;

        AnsiConsole.MarkupLine($"Your score [green]{_score} /[/] [red]{_totalNumberOfQuestions} questions correct[/]");
        AnsiConsole.MarkupLine($"Total time [green]{ConvertSecondsToMins()} [/]");

        GameResults gameResults = new GameResults(_questions, _score, _totalNumberOfQuestions);
        _gameResultsList.Add(gameResults);
        _questions.Clear();
        EndGame();
    }

    private static void HandleOperationResults(OperationResults results, bool isRandomGameSelected)
    {
        _totalNumberOfQuestions = isRandomGameSelected ? _totalNumberOfQuestions +1 : _totalNumberOfQuestions;

        _questions.Add(results.Question);
        _score = results.QuestionAnsweredCorrectly ? _score + 1 : _score;

    }



    private static void DisplayResults()
    {
        if(!_gameResultsList.Any())
        {
            Console.WriteLine("Play a game");
            return;
        }

        Table table = new Table();

        table.AddColumn("GameID");
        table.AddColumn("Score");
        table.AddColumn("Total number of questions");

        for (int i = 0; i < _gameResultsList.Count; i++)
        {
            table.AddRow($"Game {i+1}", _gameResultsList[i]._score.ToString(), _gameResultsList[i]._totalNumberOfQuestion.ToString());
        }

        AnsiConsole.Write(table);
    }

    private static void EndGame()
    {
        AnsiConsole.MarkupLine("[red]Game Ended[/]");
        Program.mainLoop = false;
    }

    private static void DisplayQuestions()
    {
        if(!_gameResultsList.Any())
        {
            Console.WriteLine("Play a game");
            Task.Delay(3000);
            return;
        }

        List<string> gameSelectionChoice = new List<string>();
        int numberOfQuestions = _gameResultsList.Count;

        for(var i = 0; i < numberOfQuestions; i++)
        {
            gameSelectionChoice.Add($"Game {i+1}");
        }

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a [green]Game[/]:")
                .AddChoices(gameSelectionChoice));

        var index = getGameNumber(choice);
        var displayQuestionsTable = BuildTable(index);

        AnsiConsole.Write(displayQuestionsTable);

    }

    private static int getGameNumber(string userGameChoice)
    {
        string gameNumber = userGameChoice.Substring(5);
        return int.Parse(gameNumber)-1;
    }

    private static Table BuildTable(int index)
    {
        Table myTable = new Table();
        myTable.AddColumn($"Game {index + 1}");

        for(int i = 0; i < _gameResultsList[index]._questions.Count; i++)
        {
            myTable.AddRow(_gameResultsList[index]._questions[i]);
        }

        return myTable;
    }

    private static string ConvertSecondsToMins()
    {
        int seconds = _timeInMinutes % 60;
        int minutes = _timeInMinutes / 60;
        return $"{minutes}m : {seconds}s";
    }



}
