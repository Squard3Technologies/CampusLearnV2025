﻿namespace CampusLearn.DataModel.ViewModels;

public class DiscussionViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTime DateCreated { get; set; }

    public Guid CreatedByUserId { get; set; }

    public string CreatedByUserName { get; set; }

    public string CreatedByUserSurname { get; set; }

    public string CreatedByUserEmail { get; set; }

    public string LastComment { get; set; }

    public DateTime LastCommentDateCreated { get; set; }
}
