using System;

namespace RegistrationAPI.Models
{
    public class EmployeeImage
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string ImagePath { get; set; }
        public string ImageType { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public int FileSize { get; set; }
        public DateTime UploadDate { get; set; }
        public string Description { get; set; }
        public bool IsProfilePicture { get; set; }

        // Navigation property
        public virtual Employee Employee { get; set; }
    }
}