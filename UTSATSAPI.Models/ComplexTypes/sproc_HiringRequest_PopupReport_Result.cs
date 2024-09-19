using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    /// <summary>
    /// sproc_HiringRequest_PopupReport_Result
    /// </summary>
    [Keyless]
    public class sproc_HiringRequest_PopupReport_Result
    {
        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        public string Company { get; set; }
        /// <summary>
        /// Gets or sets the hr number.
        /// </summary>
        /// <value>
        /// The hr number.
        /// </value>
        public string HR_NUMBER { get; set; }
        public string HR_Status { get; set; }
        public string Role { get; set; }
        public string SalesPerson { get; set; }
        public int NoOfProfileShared { get; set; }
        public string? HRRaisedDate { get; set; }
        public string? HRAcceptedDateTime { get; set; }
        public string? SalesPersonHead { get; set; }
        public string? FirstProfileSharedDate { get; set; }
    }
}
