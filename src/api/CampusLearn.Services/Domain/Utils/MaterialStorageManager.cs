using CampusLearn.DataModel.Models.LearningMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.Services.Domain.Utils;

public class MaterialStorageManager
{
    protected string basePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "material");



    
    public async Task<string> UploadLearningMaterialAsync(AddLearningMaterialRequest request)
    {
        string returnStr = string.Empty;
        var topicDir = Path.Combine(basePath, request.TopicId.ToString());
        if(!Directory.Exists(topicDir))
        {
            Directory.CreateDirectory(topicDir);
        }

        var userDir = Path.Combine(topicDir, request.UploadedByUserId.ToString());
        if (!Directory.Exists(userDir))
        {
            Directory.CreateDirectory(userDir);
        }
        var fileExtension = ".pdf";
        if(request.FileType.Equals("pdf", comparisonType: StringComparison.OrdinalIgnoreCase))
        {
            fileExtension = ".pdf";
        }
        else if (request.FileType.Equals("docx", comparisonType: StringComparison.OrdinalIgnoreCase))
        {
            fileExtension = ".docx";
        }
        else if (request.FileType.Equals("mp4", comparisonType: StringComparison.OrdinalIgnoreCase))
        {
            fileExtension = ".mp4";
        }
        else
        {
            fileExtension = ".pdf";
        }
        var filePath = Path.Combine(userDir, $"{DateTime.Now.ToString("yyyyMMddHHmmssfff")}{fileExtension}");
        var fileData = Convert.FromBase64String(request.FileData);
        await File.WriteAllBytesAsync(filePath, fileData);
        returnStr = filePath;
        return returnStr;
    }
}
