using Microsoft.AspNetCore.Http;

namespace UTSATSAPI.Models.ViewModels
{
    public class FileUploadVModel
    {
        public IFormFile _file { get; set; }
        public string FileUploadFor { get; set; }
    }
}
