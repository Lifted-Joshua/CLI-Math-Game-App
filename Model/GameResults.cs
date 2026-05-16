using System;

namespace MathGame.Model;
public class GameResults
{
    /// <summary>
    /// Gets or sets the questions asked each game
    /// </summary>
    public List<string> _questions { get; private set; } = new List<string>();
    /// <summary>
    /// Gets or sets the score of the player for each game
    /// </summary>
    public int _score { get; private set; }
    /// <summary>
    /// Gets or sets total number of questions for each game
    /// </summary>
    public int _totalNumberOfQuestion {get; private set; }

    public GameResults(List<string> questions, int score, int totalNumberOfQuestions)
    {
        this._questions.AddRange(questions);
        this._score = score;
        this._totalNumberOfQuestion = totalNumberOfQuestions;
    }

}
