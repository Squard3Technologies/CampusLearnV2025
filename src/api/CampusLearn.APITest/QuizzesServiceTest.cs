using CampusLearn.API.Controllers;
using CampusLearn.DataLayer.IRepositoryService;
using CampusLearn.DataModel.Models.Quizzes;
using CampusLearn.DataModel.ViewModels;
using CampusLearn.Services.Domain.Quizzes;
using CampusLearn.Services.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace CampusLearn.APITest;

public class QuizzesServiceTest
{
    private readonly Mock<IQuizRepository> quizRepositoryMock;
    private readonly IQuizService quizService;

    public QuizzesServiceTest()
    {
        quizRepositoryMock = new Mock<IQuizRepository>();
        quizService = new QuizService(quizRepositoryMock.Object);
    }


    [Fact]
    public async Task GetQuizzesByTopic_ReturnsNothing_WhenTopicDoesNotExists()
    {
        Guid topicId = Guid.Parse("4E1D6B75-5934-4C15-AE7F-C3EEAB6D313B");
        var expectedQuizzesResults = new List<QuizViewModel>();
        var token = CancellationToken.None;

        quizRepositoryMock.Setup(x => x.GetQuizzesByTopicAsync(topicId, token)).ReturnsAsync(expectedQuizzesResults);

        //Act
        var quizzesResponse = await quizService.GetQuizzesByTopic(topicId, token);

        //Assert
        Assert.NotNull(quizzesResponse);
        Assert.Equal(expectedQuizzesResults, quizzesResponse);
    }


}
