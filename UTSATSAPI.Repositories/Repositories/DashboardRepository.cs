using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModel;
using UTSATSAPI.Repositories.Interfaces;

namespace UTSATSAPI.Repositories.Repositories
{
    public class DashboardRepository : IDashboard
    {
        #region Variables
        private TalentConnectAdminDBContext db;
        #endregion

        #region Constructor
        public DashboardRepository(TalentConnectAdminDBContext _db)
        {
            this.db = _db;
        }
        #endregion

        #region Public Method
        //public List<PieChartOutput> GetPieChartOne(PieChartInput pieChartInput)
        //{
        //    try
        //    {
        //        var isManaged = new SqlParameter("@IsManaged", pieChartInput.IsManaged);
        //        var Month = new SqlParameter("@Month", pieChartInput.Month);
        //        var Year = new SqlParameter("@Year", pieChartInput.Year);
        //        List<PieChartOutput> pieChartData = db.PieChartOutputs.FromSqlRaw(Helpers.Constants.ProcConstant.sproc_Dashboard_GetHRsCountByManagers, isManaged, Month, Year).ToList();
        //        return pieChartData;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public List<PieChartOutput> GetPieChartTwo(PieChartInput pieChartInput)
        //{
        //    try
        //    {
        //        var isManaged = new SqlParameter("@IsManaged", pieChartInput.IsManaged);
        //        var Month = new SqlParameter("@Month", pieChartInput.Month);
        //        var Year = new SqlParameter("@Year", pieChartInput.Year);
        //        List<PieChartOutput> pieChartData = db.PieChartOutputs.FromSqlRaw(Helpers.Constants.ProcConstant.sproc_Dashboard_GetHRsCountbyOpsMAnagers, isManaged, Month, Year).ToList();
        //        return pieChartData;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        #endregion
    }
}
