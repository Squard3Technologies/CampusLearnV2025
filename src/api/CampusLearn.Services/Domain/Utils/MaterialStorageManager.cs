using CampusLearn.DataModel.Models.LearningMaterial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.Services.Domain.Utils;

public class MaterialStorageManager
{
    public async Task<string> UploadLearningMaterialAsync(IConfiguration configuration, string baseUrl, AddLearningMaterialRequest request)
    {
        string returnStr = string.Empty;
        var uploadPath = Path.Combine(configuration["FileUploadPath"].ToString());
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var fileName = $"{Guid.NewGuid().ToString().Replace("-", "").ToUpper()}{Path.GetExtension(request.FileData.FileName)}";
        var filePath = Path.Combine(uploadPath, fileName);
        var dbFilePath = $"{baseUrl}/files/{fileName}";

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.FileData.CopyToAsync(stream);
        }

        if(File.Exists(filePath))
            returnStr = dbFilePath;
        return returnStr;
    }
}
