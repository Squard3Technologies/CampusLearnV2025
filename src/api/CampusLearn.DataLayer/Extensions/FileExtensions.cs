namespace CampusLearn.DataLayer.Extensions;

public static class FileExtensions
{
    public static string GetEmailTemplate(string fileName)
    {
        var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Contants", "EmailTemplates");

        var filePath = Path.Combine(basePath, fileName);
        if (!File.Exists(filePath))
            throw new FileNotFoundException();

        return File.ReadAllText(filePath);
    }
}
