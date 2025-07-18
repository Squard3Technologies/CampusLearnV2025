﻿namespace CampusLearn.DataModel.ViewModels;

public class QuizDetailViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public TimeSpan Duration { get; set; }

    public List<QuizQuestionViewModel> Questions { get; set; }
}
