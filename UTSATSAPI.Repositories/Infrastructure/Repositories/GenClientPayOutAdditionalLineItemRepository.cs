using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class GenClientPayOutAdditionalLineItemRepository : GenericRepository<GenClientPayOutAdditionalLineItem>, IGenClientPayOutAdditionalLineItemRepository
{
public GenClientPayOutAdditionalLineItemRepository(UTSATSAPIDBConnection dbContext) : base(dbContext){}
}
}
