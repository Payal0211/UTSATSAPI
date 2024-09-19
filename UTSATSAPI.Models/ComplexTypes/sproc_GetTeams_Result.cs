using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GetTeams_Result
    {
        public long ID { get; set; }
        public string Team { get; set; }
        public string DeptName { get; set; }
        public string UserType { get; set; }
        public string CreatedByDateTime { get; set; }
        public string TeamForNewUser { get; set; }
        public int TotalRecords { get; set; }
        public string CreatedBy { get; set; }
        public string Is_Active { get; set; }
        public string Head { get; set; }
    }
}
