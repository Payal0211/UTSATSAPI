namespace UTSATSAPI.Repositories.Interfaces
{
    public interface IUpChatCall
    {
        Task CreateChannel(long HrID);
        Task UpdateChannel(long HrID);
        Task AddHRActivityInChats(long HrId ,long HistoryId);
        Task AddNotesInChats(long HrId ,long NoteID);
        Task<string> GetChannelLibrary(string Type, string ChannelID);
    }
}
