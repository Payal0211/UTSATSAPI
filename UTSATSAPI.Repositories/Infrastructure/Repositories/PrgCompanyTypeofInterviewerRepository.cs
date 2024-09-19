using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgCompanyTypeofInterviewerRepository : GenericRepository<PrgCompanyTypeofInterviewer>, IPrgCompanyTypeofInterviewerRepository
{
public PrgCompanyTypeofInterviewerRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
