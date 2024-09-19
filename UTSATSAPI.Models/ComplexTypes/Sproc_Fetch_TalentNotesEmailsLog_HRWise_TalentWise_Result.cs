using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Fetch_TalentNotesEmailsLog_HRWise_TalentWise_Result
    {
        public string? Note { get; set; }
        public string? NotesAddedby { get; set; }
        public string? ATSNoteID { get; set; }
    }
}
