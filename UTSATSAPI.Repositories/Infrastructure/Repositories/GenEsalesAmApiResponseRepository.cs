using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenEsalesAmApiResponseRepository : GenericRepository<GenEsalesAmApiResponse>, IGenEsalesAmApiResponseRepository
{
public GenEsalesAmApiResponseRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
