using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class SaveInvoiceDetailsFromEngagementDashboard
    {
        public long OnBoardID { get; set; }
        public DateTime InvoiceSentdate { get; set; }
        public string InvoiceNumber { get; set; }
        public int InvoiceStatusId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
    }
}
