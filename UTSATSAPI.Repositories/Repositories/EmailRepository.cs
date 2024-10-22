using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class EmailRepository : IEmail
    {
        #region Variables

        private UTSATSAPIDBConnection _db;  

        #endregion

        #region Constructors
        public EmailRepository(UTSATSAPIDBConnection db, IUnitOfWork unitOfWork)
        {
            _db = db; 
        }
        #endregion         
      
    }
}
