using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ConvertToDP
    {
        public long HRID { get; set; }
        public long ContactTalentID { get; set; }
        public long TalentID { get; set; }
        public decimal DPAmount { get; set; }
        public decimal DpPercentage { get; set; }
        public decimal CurrentCTC { get; set; }
        public decimal ExpectedCTC { get; set; }
    }

    public class ConvertToContractual
    {
        public long HRID { get; set; }
        public long ContactTalentID { get; set; }
        public long TalentID { get; set; }
        public decimal NRPercentage { get; set; }
        public decimal HR_Cost { get; set; }
        public bool IsHiringLimited { get; set; }
        public string? durationType { get; set; }
        public int SpecificMonth { get; set; }
    }
}
