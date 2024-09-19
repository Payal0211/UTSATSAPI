using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenCrctpShortlistedTalentsClientPortal
    {
        public long Id { get; set; }
        public string? Guid { get; set; }
        public long? AtsTalentId { get; set; }
        public long? UtstalentId { get; set; }
        public long? Hrid { get; set; }
        public int? StatusId { get; set; }
        public DateTime? StatusChangeDateTime { get; set; }
        public bool? IsMyList { get; set; }
        public DateTime? MyListDateTime { get; set; }
        public bool? IsUnLock { get; set; }
        public DateTime? UnLockDateTime { get; set; }
        public decimal? ProfileCredit { get; set; }
        public bool? IsVetted { get; set; }
    }
}
