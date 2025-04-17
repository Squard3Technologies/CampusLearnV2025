using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearnLib.Models;

public class MaterialModel
{
    public Guid Id { get; set; }
    public MaterialFileType MyProperty { get; set; }
    public string FilePath { get; set; }
    public UserModel UploadedBy { get; set; }
    public DateTime UploadedDate { get; set; }
}



public class MaterialFileType
{
    public int Id { get; set; }
    public string Description { get; set; }
}