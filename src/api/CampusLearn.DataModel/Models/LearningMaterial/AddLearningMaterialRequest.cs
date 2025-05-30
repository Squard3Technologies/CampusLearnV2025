using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataModel.Models.LearningMaterial;

public class AddLearningMaterialRequest
{
    public Guid TopicId { get; set; }
    public string FileType { get; set; }
    public IFormFile FileData { get; set; }
}
