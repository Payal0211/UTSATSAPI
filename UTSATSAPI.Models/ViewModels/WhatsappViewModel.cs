using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class Participant
    {
        public string phone { get; set; }
        public bool admin { get; set; }
    }

    public class Permissions
    {
        public string edit { get; set; }
        public string send { get; set; }
        public string invite { get; set; }
        public bool approval { get; set; }
    }

    public class WhatsappViewModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public List<Participant> participants { get; set; }
        public Permissions permissions { get; set; }
    }

    public class WhatsappViewModelForMessage
    {
        public string group { get; set; }
        public string message { get; set; }
    }

    public class CompanyWhatsappDetails
    {
        public long? CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public long? POCUserID { get; set; }
    }
}
