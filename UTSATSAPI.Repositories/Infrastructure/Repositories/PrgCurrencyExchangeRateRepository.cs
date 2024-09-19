using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
namespace UTSATSAPI.Repositories.Infrastructure.Repositories
{
public class PrgCurrencyExchangeRateRepository : GenericRepository<PrgCurrencyExchangeRate>, IPrgCurrencyExchangeRateRepository
{
public PrgCurrencyExchangeRateRepository(TalentConnectAdminDBContext dbContext) : base(dbContext){}
}
}
