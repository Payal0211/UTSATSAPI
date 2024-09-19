using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ViewModel;

namespace UTSATSAPI.Models.ViewModels
{
    public class UserFeedBack
    {
        public int RatingStar { get; set; }
        public string Feedback { get; set; }
        public string PageUrl { get; set; }
        public string Browser { get; set; }
        public FileUploadModelBase64 fileUpload { get; set; }
    }
}
