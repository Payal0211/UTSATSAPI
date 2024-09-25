using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IATSsyncUTS
    {
        void SaveHRPOCDetails(string param);
        long InsertUtsAtsApiDetails(GenUtsAtsApiRecord gen_UtsAtsApi_Records);
        void UpdateUtsAtsApiDetails(long APIRecordInsertedID, string Message);
    }
}
