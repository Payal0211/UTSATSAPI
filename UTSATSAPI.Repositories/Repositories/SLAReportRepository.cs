using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Helpers;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Generic;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class SLAReportRepository : ControllerBase, ISLAReport
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        private IUnitOfWork _unitOfWork;
        #endregion
        public SLAReportRepository(TalentConnectAdminDBContext _db, IUnitOfWork unitOfWork)
        {
            db = _db;
            _unitOfWork = unitOfWork;
        }

        public SLAViewModel GetSLAReportDetail()
        {
            SLAViewModel reportViewModel = new SLAViewModel();

            reportViewModel.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
            reportViewModel.EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

            //reportViewModel.SalesManager = db.UsrUsers.Where(x => x.UserTypeId == 9).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.Id.ToString(),
            //    Text = x.FullName
            //});

            //reportViewModel.SV = db.UsrUsers.Where(x => x.UserTypeId == 10).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.Id.ToString(),
            //    Text = x.FullName
            //});

            //reportViewModel.SalesPerson = db.UsrUsers.Where(x => x.UserTypeId == 4).ToList().Select(x => new SelectListItem
            //{
            //    Value = x.Id.ToString(),
            //    Text = x.FullName
            //});

            reportViewModel.BindODR_Pool = new Dictionary<int, string>();
            reportViewModel.BindODR_Pool.Add(1, "Pool");
            reportViewModel.BindODR_Pool.Add(2, "ODR");

            //reportViewModel.ODRPoolDrop = reportViewModel.BindODR_Pool.ToList().Select(x => new SelectListItem
            //{
            //    Value = x.Key.ToString(),
            //    Text = x.Value
            //});
            reportViewModel.BindAMNDB = new Dictionary<int, string>();
            reportViewModel.BindAMNDB.Add(0, "Select");
            reportViewModel.BindAMNDB.Add(1, "AM");
            reportViewModel.BindAMNDB.Add(2, "NBD");

            //reportViewModel.AmBDRDrp = reportViewModel.BindAMNDB.ToList().Select(x => new SelectListItem
            //{
            //    Value = x.Key.ToString(),
            //    Text = x.Value
            //});
            reportViewModel.Stages = db.PrgSummaryStagesForReports.ToList().Where(x => x.Id >= 4).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.SummaryStage
            });

            //reportViewModel.Roles = db.PrgTalentRoles.ToList().Where(x => x.IsActive == true).OrderBy(x => x.TalentRole).Select(x => new SelectListItem
            //{
            //    Value = x.Id.ToString(),
            //    Text = x.TalentRole
            //});

            reportViewModel.ActionFilterDrop = db.PrgActionFilters.ToList().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.ActionFilter
            });

            reportViewModel.Companies = db.GenCompanies.Where(x => x.IsActive == true).Select(y => new SelectListItem
            {
                Text = y.Company,
                Value = y.Id.ToString()
            }).OrderBy(x => x.Value).ToList();



            return reportViewModel;
        }

        public async Task<List<Sproc_SLA_Report_For_Static_Stages_Result>> GetSLAReportData(string param)
        {
            return await db.Set<Sproc_SLA_Report_For_Static_Stages_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_SLA_Report_For_Static_Stages, param)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<Sproc_SLA_OverAll_Summary_Report_Static_Stages_Result>> OverAllSLASummary(string param)
        {
            return await db.Set<Sproc_SLA_OverAll_Summary_Report_Static_Stages_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_UTS_SLA_OverAll_Summary_Report_Static_Stages, param)).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<Sproc_SLA_Missed_Summary_Report_Result>> SLAMissedSummary(string param)
        {
            return await db.Set<Sproc_SLA_Missed_Summary_Report_Result>().FromSqlRaw(string.Format("{0} {1}", Constants.ProcConstant.Sproc_SLA_Missed_Summary_Report, param)).ToListAsync();
        }

        public IEnumerable<SelectListItem> FetchUserBasedonAmNBD(int? Id)
        {

            if (Id != null)
            {

                if (Id == 1)
                {
                    IEnumerable<SelectListItem> AMList = db.UsrUsers.Where(x => (x.UserTypeId == 4) && x.IsNewUser == false).ToList().Select(x => new SelectListItem
                    {
                        Text = x.FullName,
                        Value = x.Id.ToString()
                    });

                    return AMList;
                }

                if (Id == 0)
                {
                    IEnumerable<SelectListItem>  salesPerson = db.UsrUsers.Where(x => x.UserTypeId == 4).ToList().Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.FullName
                    });
                    return salesPerson;
                }
                if (Id == 2)
                {
                    IEnumerable<SelectListItem> NBDList = db.UsrUsers.Where(x => (x.UserTypeId == 4) && x.IsNewUser == true).ToList().Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.FullName
                    });

                    return NBDList;
                }
            }
            return null;
        }

        public IEnumerable<SelectListItem> FetchManagerBasedonAmNBD(int? Id)
        {
            if (Id != null)
            {

                if (Id == 1)
                {
                    IEnumerable<SelectListItem> AMList = db.UsrUsers.Where(x => (x.UserTypeId == 9) && x.IsNewUser == false).ToList().Select(x => new SelectListItem
                    {
                        Text = x.FullName,
                        Value = x.Id.ToString()
                    });

                    return AMList;
                }

                if (Id == 0)
                {
                    IEnumerable<SelectListItem> salesPerson = db.UsrUsers.Where(x => x.UserTypeId == 9).ToList().Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.FullName
                    });
                    return salesPerson;
                }
                if (Id == 2)
                {
                    IEnumerable<SelectListItem> NBDList = db.UsrUsers.Where(x => (x.UserTypeId == 9) && x.IsNewUser == true).ToList().Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.FullName
                    });

                    return NBDList;
                }
            }
            return null;
        }

        public async Task<List<Sproc_Get_SalesHead_Users_Result>> Sproc_Get_SalesHead_Users_Result()
        {
            return await db.Set<Sproc_Get_SalesHead_Users_Result>().FromSqlRaw(string.Format("{0}", Constants.ProcConstant.Sproc_Get_SalesHead_Users)).ToListAsync().ConfigureAwait(false);
        }
    }
}
