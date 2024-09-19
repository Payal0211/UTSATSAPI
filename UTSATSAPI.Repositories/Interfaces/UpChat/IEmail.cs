using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Repositories.Interfaces.UpChat
{
    public interface IEmail
    {
        Task<string> SendEmail_UserTagInChat(long? hrID, string assignedUsers, string notes, string userEmployeeID);
    }
}
