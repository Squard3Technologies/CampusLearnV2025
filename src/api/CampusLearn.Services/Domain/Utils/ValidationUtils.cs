namespace CampusLearn.Services.Domain.Utils;

public static class ValidationUtils
{
    public static bool IsValidEmailAddress(string emailAddress)
    {
        try
        {
            var email = new MailAddress(emailAddress);
            return email.Address == emailAddress.Trim();
        }
        catch
        {
            return false;
        }
    }
}
