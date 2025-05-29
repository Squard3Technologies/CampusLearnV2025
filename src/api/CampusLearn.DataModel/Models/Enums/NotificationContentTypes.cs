namespace CampusLearn.DataModel.Models.Enums;

public enum NotificationContentTypes
{
    None = 0,

    AccountApproved = 1,
    AccountRejected = 2,
    AccountDeactivated = 3,

    TopicCreated = 4,
    TopicQuizCreated = 5,
    TopicQuizUpdated = 6,
    TopicDiscussionCreated = 7,
    TopicCommentCreated = 8,
    TopicLearningMaterialUploaded = 9,
    TopicQuizAssigned = 10,

    EnquiryCreated = 11,
    EnquiryResolved = 12,

    ChatMessageCreated = 13,
}