using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    [Keyless]
    public class ValidationMessageForIncentiveReport
    {
        public string Message { get; set; }
    }
}
