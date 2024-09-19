using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Models.ViewModels
{
    public class EsaleAMDetails
    {
        public EsaleAMDetails()
        {
            ClientDetails = new List<ClientDetail>();
        }
        public SP_UTS_ESales_Get_Client_AM_Details_Result EsalesClientAM { get; set; }

        public List<ClientDetail> ClientDetails { get; set; }
    }

    public class ClientDetail
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsPrimary { get; set; }
        public string EmailID { get; set; }
        public string regions { get; set; }
        public string skype { get; set; }
        public string city { get; set; }
    }
}
