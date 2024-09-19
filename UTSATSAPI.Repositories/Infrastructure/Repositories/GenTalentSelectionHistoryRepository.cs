using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenTalentSelectionHistoryRepository : GenericRepository<GenTalentSelectionHistory>, IGenTalentSelectionHistoryRepository
{
public GenTalentSelectionHistoryRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
