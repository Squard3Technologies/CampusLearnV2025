using CampusLearn.DataLayer.IRepositoryService;
using CampusLearn.DataModel.ViewModels;
using CampusLearn.Services.Domain.Enquiries;
using Xunit;
using Assert = Xunit.Assert;

namespace CampusLearn.APITest;

public class EnquiryServiceTest
{
    private readonly Mock<IEnquiryRepository> enquiryRepositoryMock;
    private readonly IEnquiryService enquiryService;

    public EnquiryServiceTest()
    {
        enquiryRepositoryMock = new Mock<IEnquiryRepository>();
        enquiryService = new EnquiryService(enquiryRepositoryMock.Object, null, null);
    }

    [Fact]
    public async Task GetEnquiries_ForUserWithNoEnquiries_And_ReturnsEmptyList()
    {
        var userId = Guid.NewGuid();
        var token = CancellationToken.None;

        enquiryRepositoryMock.Setup(repo => repo.GetEnquiriesAsync(userId, token))
                .ReturnsAsync(new List<EnquiryViewModel>());

        // Act
        var result = await enquiryService.GetEnquiries(userId, token);

        // Assert
        // The result should not be null
        Assert.NotNull(result);

        //The resut should be of array type
        Assert.IsType<List<EnquiryViewModel>>(result);

        //The result should be empty
        Assert.Empty(result);
    }
}