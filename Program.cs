// See https://aka.ms/new-console-template for more information
using MathGame.UserInterface;
using Spectre.Console;

namespace MathGame;
internal static partial class Program
{
    public static bool mainLoop = true;

    public static void Main(string[] args)
    {
        AnsiConsole.MarkupLine($"[green]Math Game! [/] \n");

        while(mainLoop)
        {
            MainMenu.Intro();
        }
    }
}



