using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ChatGPTResponseViewModel
    {
        
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? SortExpression { get; set; }
        public string? SortDirection { get; set; }
        public bool IsParsingURL { get; set; }
    }
}
