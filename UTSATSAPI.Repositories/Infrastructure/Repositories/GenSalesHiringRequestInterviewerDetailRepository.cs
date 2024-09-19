using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenSalesHiringRequestInterviewerDetailRepository : GenericRepository<GenSalesHiringRequestInterviewerDetail>, IGenSalesHiringRequestInterviewerDetailRepository
{
public GenSalesHiringRequestInterviewerDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
