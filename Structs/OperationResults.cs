using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathGame.Structs;
public struct OperationResults
{
    public string Question { get; }
    public bool QuestionAnsweredCorrectly { get; }

    public OperationResults(string question, bool QuestionAnsweredCorrectly)
    {
        this.Question = question;
        this.QuestionAnsweredCorrectly = QuestionAnsweredCorrectly;
    }

}
