using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IInterviewer
    {
        List<sproc_UTS_FetchInterviewerDetails_Result> FetchInterviewerDetails(string param);
        void AddUpdateDeleteInterviewerDetails(string param);
    }
}
