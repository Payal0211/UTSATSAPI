using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class CompanyDetailViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long ID { get; set; }
        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        public string Company { get; set; }
        /// <summary>
        /// Gets or sets the linked in profile.
        /// </summary>
        /// <value>
        /// The linked in profile.
        /// </value>
        public string? LinkedInProfile { get; set; }
        /// <summary>
        /// Gets or sets the size of the company.
        /// </summary>
        /// <value>
        /// The size of the company.
        /// </value>
        public int CompanySize { get; set; }
        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
        public string? Phone { get; set; }
        /// <summary>
        /// Gets or sets the company domain.
        /// </summary>
        /// <value>
        /// The company domain.
        /// </value>
        public string? CompanyDomain { get; set; }
        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        /// <value>
        /// The total records.
        /// </value>
        public int TotalRecords { get; set; }
        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string? Location { get; set; }
        /// <summary>
        /// Gets or sets the contact status.
        /// </summary>
        /// <value>
        /// The contact status.
        /// </value>
        public string? Contact_Status { get; set; }
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string? Color { get; set; }
        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public decimal Score { get; set; }
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string? Category { get; set; }
        /// <summary>
        /// Gets or sets the company weighted average criteria identifier.
        /// </summary>
        /// <value>
        /// The company weighted average criteria identifier.
        /// </value>
        public long CompanyWeightedAverageCriteriaID { get; set; }
        /// <summary>
        /// Gets or sets the exists or not.
        /// </summary>
        /// <value>
        /// The exists or not.
        /// </value>
        public int ExistsOrNot { get; set; }
        /// <summary>
        /// Gets or sets the geo.
        /// </summary>
        /// <value>
        /// The geo.
        /// </value>
        public string? GEO { get; set; }
        /// <summary>
        /// Gets or sets the team lead.
        /// </summary>
        /// <value>
        /// The team lead.
        /// </value>
        public string? TeamLead { get; set; }
        /// <summary>
        /// Gets or sets the am sales person.
        /// </summary>
        /// <value>
        /// The am sales person.
        /// </value>
        public string? AM_SalesPerson { get; set; }
        /// <summary>
        /// Gets or sets the NBD sales person.
        /// </summary>
        /// <value>
        /// The NBD sales person.
        /// </value>
        public string? NBD_SalesPerson { get; set; }
        /// <summary>
        /// Gets or sets the type of the lead.
        /// </summary>
        /// <value>
        /// The type of the lead.
        /// </value>
        public string? Lead_Type { get; set; }
        /// <summary>
        /// Gets or sets the lead user.
        /// </summary>
        /// <value>
        /// The lead user.
        /// </value>
        public string? LeadUser { get; set; }
    }
}
