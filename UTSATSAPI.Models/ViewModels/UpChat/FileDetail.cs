using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels.UpChat
{
    public class FileDetail
    {
        public string? Name { get; set; }
        public string? ContentType { get; set; }
        public ulong? Size { get; set; }
        public string? FileUrl { get; set; }
        public string? UploadedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }

    }
}
