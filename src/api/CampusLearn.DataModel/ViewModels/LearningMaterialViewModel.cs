using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataModel.ViewModels;

public class LearningMaterialViewModel
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public Guid UserId { get; set; }
    public string FileType { get; set; }
    public string FilePath { get; set; }
    public DateTime? UploadedDate { get; set; }
}