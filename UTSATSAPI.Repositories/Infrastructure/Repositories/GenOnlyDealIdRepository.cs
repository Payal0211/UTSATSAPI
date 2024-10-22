using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenOnlyDealIdRepository : GenericRepository<GenOnlyDealId>, IGenOnlyDealIdRepository
{
public GenOnlyDealIdRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
