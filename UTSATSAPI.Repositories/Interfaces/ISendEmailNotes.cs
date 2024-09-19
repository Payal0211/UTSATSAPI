namespace UTSATSAPI.Repositories.Interfaces
{
    public interface ISendEmailNotes
    {
        Task<string> SendEmailForAddingNotes(long? hrID, string assignedUsers, string notes, string userEmployeeID);
        string GetCCEmailIdValues(long? salesUSerID);
        string GetCCEmailNameValues(long? salesUSerID);
        string GetAssignedUsersCCEmailIdValues(string assignedUsersIds);
        string GetAssignedUsersCCEmailNameValues(string assignedUsersIds);
        string SendEmailForHRAcceptanceToInternalTeam(long hrID, int acceptValue, string reason);
        string SendEmailForTRAcceptanceToInternalTeam(long HRID, int AcceptanceValue, int? TRParked);
        string SendEmailForTalentAcceptedHR(Int64 TalentID, Int64? HRID);
        string SendEmailtoSalesDirectPlacement(Int64 TalentID, long? HiringRequest_ID);
        string SendEmailForTalentShowtoclientbySalesTeam(Int64 TalentID, Int64? HRID, string encryptedHRID, string encryptedRole_ID);
    }
}