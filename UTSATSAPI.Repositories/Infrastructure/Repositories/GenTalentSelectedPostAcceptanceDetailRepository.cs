using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentSelectedPostAcceptanceDetailRepository : GenericRepository<GenTalentSelectedPostAcceptanceDetail>, IGenTalentSelectedPostAcceptanceDetailRepository
{
public GenTalentSelectedPostAcceptanceDetailRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
