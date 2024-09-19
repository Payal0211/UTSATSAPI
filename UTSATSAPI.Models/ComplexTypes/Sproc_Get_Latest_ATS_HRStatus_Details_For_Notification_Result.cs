using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public partial class Sproc_Get_Latest_ATS_HRStatus_Details_For_Notification_Result
    {
        public long ID { get; set; }
        public long StatusID { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public long createdByID { get; set; }
        public long HRID { get; set; }
        public long ATSHRStatusId { get; set; }
    }
}
