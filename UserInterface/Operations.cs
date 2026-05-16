using System;
using MathGame.Enums;
using MathGame.Structs;
using Spectre.Console;

namespace MathGame.UserInterface;
internal static class Operations
{
    private static Random rnd1 = new Random();
    public static OperationResults Calculate(string operationSymbol, Func<int, int, int> performOperation)
    {
        if (performOperation == null)
            throw new ArgumentNullException(nameof(performOperation), "Operation function cannot be null.");

        if(operationSymbol == "/")
        {
            var number = Operations.GenerateRandomNumber();
            var multipleOfNumer = number * Operations.GenerateRandomNumber();

            int resultDivide = performOperation(multipleOfNumer, number);

            return ValidateUserChoice(multipleOfNumer, number, resultDivide, operationSymbol);
        }

        var num1 = GenerateRandomNumber();
        var num2 = GenerateRandomNumber();

        int result = performOperation(num1, num2);

        return ValidateUserChoice(num1, num2, result, operationSymbol);

    }

    private static OperationResults ValidateUserChoice(int num1, int num2, int result, string operationSymbol)
    {
        int userChoice = AnsiConsole.Ask<int>($"{num1} {operationSymbol} {num2} = ");

        if(userChoice == result)
        {
            AnsiConsole.MarkupLine($"[green]Correct! [/] \n");
            Task.Delay(1000);
            return new OperationResults($"{num1} {operationSymbol} {num2} = {userChoice}", true);
        } else
        {
            AnsiConsole.MarkupLine($"[red]Incorrect! [/] \n");
            Task.Delay(1000);
            return new OperationResults($"{num1} {operationSymbol} {num2} = {userChoice}", false);
        }
    }


    public static int Add(int num1, int num2)
    {
        return num1 + num2;
    }
    public static int Subtract(int num1, int num2)
    {
        return num1 - num2;
    }
    public static int Multiply(int num1, int num2)
    {
        return num1 * num2;
    }

    public static int Divide(int num1, int num2)
    {
        if(num1 == 0) throw new DivideByZeroException();
        return num1 / num2;
    }


    private static int GenerateRandomNumber()
    {
        switch (MainMenu.GameDifficulty)
        {
            case Difficulty.Easy:
                return rnd1.Next(1, 10);
            case Difficulty.Medium:
                return rnd1.Next(1, 100);
            case Difficulty.Hard:
                return rnd1.Next(1, 1000);
            default:
                throw new ArgumentOutOfRangeException("Invalid enum value");
        }
    }

}
