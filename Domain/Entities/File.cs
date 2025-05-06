using Domain.BaseEntities;

namespace Domain.Entities;

public class File : BaseEntity
{
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public long FileSize { get; set; }
    public string FileType { get; set; }
}
