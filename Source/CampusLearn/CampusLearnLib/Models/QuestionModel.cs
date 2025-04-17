using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearnLib.Models;

public class QuestionModel
{
    public Guid Id { get; set; }

    public string Caption { get; set; }

    /// <summary>
    /// Can be either (true/false) | (multi-select) | (Dropdown) etc.
    /// </summary>
    public QuestionTypeModel QuestionType { get; set; }

    public IList<string> PotentialAnswers { get; set; }

    public IList<string> CorrectAnswers { get; set; }
}



public class QuestionTypeModel
{
    public int Id { get; set; }
    public string Description { get; set; }
}