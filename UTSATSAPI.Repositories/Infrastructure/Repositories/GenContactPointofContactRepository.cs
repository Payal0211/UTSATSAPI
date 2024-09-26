using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenContactPointofContactRepository : GenericRepository<GenContactPointofContact>, IGenContactPointofContactRepository
{
public GenContactPointofContactRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
