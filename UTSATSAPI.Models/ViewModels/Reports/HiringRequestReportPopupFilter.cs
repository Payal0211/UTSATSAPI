namespace UTSATSAPI.Models.ViewModels.Reports
{
    /// <summary>
    /// HiringRequestReportPopupFilter
    /// </summary>
    public class HiringRequestReportPopupFilter
    {
        /// <summary>
        /// Gets or sets from date.
        /// </summary>
        /// <value>
        /// From date.
        /// </value>
        public string FromDate { get; set; }
        /// <summary>
        /// Converts to date.
        /// </summary>
        /// <value>
        /// To date.
        /// </value>
        public string ToDate { get; set; }
        /// <summary>
        /// Gets or sets the type of hr.
        /// </summary>
        /// <value>
        /// The type of hr.
        /// </value>
        public string? TypeOfHR { get; set; }
        /// <summary>
        /// Gets or sets the mode of work identifier.
        /// </summary>
        /// <value>
        /// The mode of work identifier.
        /// </value>
        public string? ModeOfWorkId { get; set; }
        /// <summary>
        /// Gets or sets the heads.
        /// </summary>
        /// <value>
        /// The heads.
        /// </value>
        public string? Heads { get; set; }
        /// <summary>
        /// Gets or sets the hr status identifier.
        /// </summary>
        /// <value>
        /// The hr status identifier.
        /// </value>
        public string? HRStatusID { get; set; }
        /// <summary>
        /// Gets or sets the stages.
        /// </summary>
        /// <value>
        /// The stages.
        /// </value>
        public string Stages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is export.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is export; otherwise, <c>false</c>.
        /// </value>
        public bool IsExport { get; set; } = false;

        /// <summary>
        /// Gets or sets the is hr focused.
        /// </summary>
        /// <value>
        /// The is hr focused.
        /// </value>
        public bool? IsHRFocused { get; set; } = false;
        public int ClientType { get; set; }
        public string Geos { get; set; }
        public string? Sales_ManagerIDs { get; set; }
    }

    /// <summary>
    /// HiringRequestReportPopup_Filter
    /// </summary>
    public class HiringRequestReportPopup_Filter
    {
        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int? PageIndex { get; set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int? PageSize { get; set; }
        /// <summary>
        /// Gets or sets the hiring request report popup filter.
        /// </summary>
        /// <value>
        /// The hiring request report popup filter.
        /// </value>
        public HiringRequestReportPopupFilter hiringRequestReportPopupFilter { get; set; }
    }
}