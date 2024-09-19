using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgRolesAndResponsilbityRepository : GenericRepository<PrgRolesAndResponsilbity>, IPrgRolesAndResponsilbityRepository
{
public PrgRolesAndResponsilbityRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
