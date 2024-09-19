using Microsoft.AspNetCore.Mvc.Rendering;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.ViewModels;

namespace UTSATSAPI.Models.ViewModel
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string? EmployeeId { get; set; }
        public string? FullName { get; set; }
        public bool? IsNewUser { get; set; }
        public int? UserTypeId { get; set; } 
        public int? RoleId { get; set; } 
        public int[]? GeoIds { get; set; }
        public bool? IsOdr { get; set; }
        public long? ManagerID { get; set; }
        public int? PriorityCount { get; set; }
        public string? SkypeId { get; set; }
        public string? EmailId { get; set; }
        public string? ContactNumber { get; set; }
        public string? Designation { get; set; }
        public string? ProfilePic { get; set; }

        /// <summary>
        /// Gets or sets the user hierarchy parent identifier.
        /// </summary>
        /// <value>
        /// The user hierarchy parent identifier.
        /// </value>
        public long userHierarchyParentID { get; set; }

        /// <summary>
        /// Gets or sets the dept identifier.
        /// </summary>
        /// <value>
        /// The dept identifier.
        /// </value>
        public long? DeptID { get; set; }

        /// <summary>
        /// Gets or sets the team identifier.
        /// </summary>
        /// <value>
        /// The team identifier.
        /// </value>
        public string? TeamID { get; set; }

        /// <summary>
        /// Gets or sets the level identifier.
        /// </summary>
        /// <value>
        /// The level identifier.
        /// </value>
        public long? LevelID { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public FileUploadModelBase64 fileUpload { get; set; }
        
        public DetailList detail { get; set; }

        /// <summary>
        /// Gets or sets the reporting users.
        /// </summary>
        /// <value>
        /// The reporting users.
        /// </value>
        public IEnumerable<SelectListItem> ReportingUsers { get; set; }
        public List<sproc_GetUserHierarchy_Result> ReportingHierarchy { get; set; }
        public int? Another_UserTypeID { get; set; }

    }

    public class FileUploadModelBase64
    {
        public string Base64ProfilePic { get; set; }
        public string Extenstion { get; set; }
    }

    public class DetailList
    {
        public int? userTypeID { get; set; }
        public List<MastersResponseModel>? UserTypeList { get; set; }

        public int? userRoleID { get; set; }
        public List<MastersResponseModel>? UserRoleList { get; set; }

        public List<MastersResponseModel>? TalentRoleList { get; set; }

        public long teamManagerID { get; set; }
        public List<MastersResponseModel>? TeamManagerList { get; set; }
        
        public long reporteeUserID { get; set; }
        public List<MastersResponseModel>? ReporteeUserList { get; set; }

        public int[]? geoIDs { get; set; }
        public List<MastersResponseModel>? GeoList { get; set; } 
    }
}