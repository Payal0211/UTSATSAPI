using Microsoft.AspNetCore.Mvc.Rendering;
using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Models.ViewModels
{
    public class NotesViewModel
    {
        /// <summary>
        /// Gets or sets the hiring request identifier.
        /// </summary>
        /// <value>
        /// The hiring request identifier.
        /// </value>
        public Nullable<long> HiringRequest_ID { get; set; }

        /// <summary>
        /// Gets or sets the add from.
        /// </summary>
        /// <value>
        /// The add from.
        /// </value>
        public string AddFrom { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the hr notes list.
        /// </summary>
        /// <value>
        /// The hr notes list.
        /// </value>
        public List<sproc_GetAllNotes_Result> HRNotesList { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user list data.
        /// </summary>
        /// <value>
        /// The user list data.
        /// </value>
        public List<SelectListItem> UserListData { get; set; }

        /// <summary>
        /// Gets or sets the HDN user values.
        /// </summary>
        /// <value>
        /// The HDN user values.
        /// </value>
        public string hdnUserValues { get; set; }

        /// <summary>
        /// Gets or sets the HDN notes.
        /// </summary>
        /// <value>
        /// The HDN notes.
        /// </value>
        public string hdnNotes { get; set; }
    }

    public class UserOptionVM
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string ID { get; set; }
    }

    public class HRTalentNotesViewModel
    {
        public long? ContactID { get; set; }
        public long? HRID { get; set; }
        public long? ATSTalentID { get; set; }
        public long? UTSTalentID { get; set; }
        public string? Notes { get; set; }
        public string? ATSNoteID { get; set; }
        public string? CreatedByDateTime { get; set; }
        public string? Flag { get; set; }
    }
}
