using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class AMAssigmentRepository : IAMAssignment
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructors
        public AMAssigmentRepository(TalentConnectAdminDBContext _db)
        {
            db = _db;
        }

        #endregion

        #region public Methods

        public List<sproc_UTS_GetAMAssignments_Result> GetAMAssignmentList(string paramasString)
        {
            return db.Set<sproc_UTS_GetAMAssignments_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_GetAMAssignments, paramasString)).ToList();
        }

        public SP_UTS_ESales_Get_Client_AM_Details_Result SP_UTS_ESales_Get_Client_AM_Details(string paramasString)
        {
            return db.Set<SP_UTS_ESales_Get_Client_AM_Details_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.SP_UTS_ESales_Get_Client_AM_Details, paramasString)).AsEnumerable().FirstOrDefault();
        }

        public void sproc_UTS_Update_EmployeeID_FromInvoiceAPIResponse(string paramasString)
        {
            db.Database.ExecuteSqlRaw(String.Format("{0} {1}", Constants.ProcConstant.sproc_UTS_Update_EmployeeID_FromInvoiceAPIResponse, paramasString));
        }

        public async Task<List<SelectListItem>> GetAMUser()
        {
            List<Sproc_Get_AM_User_Result> data = await db.Set<Sproc_Get_AM_User_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.Sproc_Get_AM_User)).ToListAsync();

            return data.Select(x => new SelectListItem
            {
                Value = x.FullName.ToString(),
                Text = x.ID.ToString()
            }).OrderBy(x => x.Text).ToList();
        }

        public async Task<bool> ChangeAssignmentTeamDistributionPriority(int id, int priority, int UserId)
        {
            GenTeamDistribution gen_Team_Distribution = new GenTeamDistribution();
            gen_Team_Distribution = db.GenTeamDistributions.Where(xy => xy.Id == id && xy.IsDeleted == false).FirstOrDefault();
            if (gen_Team_Distribution == null)
            {
                return await Task.FromResult(false).ConfigureAwait(false);
            }
            if (gen_Team_Distribution != null && gen_Team_Distribution.Id > 0)
            {
                if (db.GenTeamDistributions.Any(g => g.Geoid == gen_Team_Distribution.Geoid && g.SortNo == priority && g.Id != id && g.IsDeleted == false))
                {

                    return await Task.FromResult(false).ConfigureAwait(false);
                }
                gen_Team_Distribution.ModifiedById = UserId;
                gen_Team_Distribution.SortNo = priority;
                db.Entry(gen_Team_Distribution).State = EntityState.Modified;
                db.SaveChanges();
            }
            return await Task.FromResult(true).ConfigureAwait(false);
        }

        #endregion
    }
}