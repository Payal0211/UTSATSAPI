using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class UpdateTRModel
    {
        [Required]
        public decimal NoOfTR { get; set; }
        [Required]
        public int HiringRequestId { get; set; }
        [Required]
        public string AddtionalRemarks { get; set; }
        [Required]
        public string ReasonForLossCancelled { get; set; }
        [Required]
        public bool IsFinalSubmit { get; set; } = false;
    }
}
