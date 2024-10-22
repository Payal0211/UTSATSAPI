using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenContactRepository : GenericRepository<GenContact>, IGenContactRepository
{
public GenContactRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
