using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_CreateJDFromPrompt_ClientPortal_Result
    {
        public string? PromptBody { get; set; }
        public bool IsHrExists { get; set; }
    }
}
