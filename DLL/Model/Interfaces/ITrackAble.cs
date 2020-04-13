using System;

namespace DLL.Model.Interfaces
{
    public interface ITrackable
    {
        DateTimeOffset CreatedAt { get; set; }
        string CreatedBy { get; set; }
        DateTimeOffset UpdatedAt { get; set; }
        string UpdatedBy { get; set; }
        
    }
}