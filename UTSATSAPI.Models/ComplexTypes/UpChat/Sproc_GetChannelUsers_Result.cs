using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes.UpChat
{
    [Keyless]
    public class Sproc_GetChannelUsers_Result
    {
        public string? photoUrl { get; set; }
        public string? userDesignation { get; set; }
        public string? userEmpId { get; set; }
        public string? userInitial { get; set; }
        public string? userName { get; set; }
    }
}
