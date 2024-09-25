using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IUniversalProcRunner
    {
        public object Manipulation(string proName, object[] args);
        public object ManipulationWithNULL(string proName, object[] args);
        public void InsertReactPayload(GenUtsadminReactPayload genUtsadminReactPayload);
    }
}
