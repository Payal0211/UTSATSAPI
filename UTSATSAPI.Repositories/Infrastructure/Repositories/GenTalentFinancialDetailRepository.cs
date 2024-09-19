using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentFinancialDetailRepository : GenericRepository<GenTalentFinancialDetail>, IGenTalentFinancialDetailRepository
{
public GenTalentFinancialDetailRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
