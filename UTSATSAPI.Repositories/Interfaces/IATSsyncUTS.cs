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
        Sproc_UTS_AddEdit_ATSHR_Result Sproc_UTS_AddEdit_ATSHR(string param);
        Task<Sproc_Add_Company_Transactions_With_ATS_Result> Sproc_Add_Company_Transactions_With_ATS(string param);

        #region  Maintain UtsAts logs
        long InsertUtsAtsApiDetails(GenUtsAtsApiRecord gen_UtsAtsApi_Records);
        void UpdateUtsAtsApiDetails(long APIRecordInsertedID, string Message);
        #endregion
    }
}
