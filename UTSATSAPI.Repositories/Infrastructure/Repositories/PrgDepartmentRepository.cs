using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgDepartmentRepository : GenericRepository<PrgDepartment>, IPrgDepartmentRepository
{
public PrgDepartmentRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
