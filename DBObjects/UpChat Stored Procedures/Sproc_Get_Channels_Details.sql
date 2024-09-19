
--exec Sproc_Get_Channels_Details 1
CREATE PROCEDURE [dbo].[Sproc_Get_Channels_Details] 
  @ChannelID			bigint = null
AS
BEGIN

			SELECT		ChannelID = ISNULL(HRC.ID,0),
						ChannelCreatedDateTime = Isnull(Convert(datetime,HRC.CreatedDateTime,103),''),
						ChannelModifiedDateTime = Isnull(Convert(datetime,HRC.ModifiedDateTime,103),''),
						ChannelMemberCount = Isnull(HRC.MemberCount,0),
						ChannelInviteLink = Isnull(HRC.ChannelInviteLink,''),
						IsChannelActive = Isnull(HRC.IsActive,0),
						ChannelIsPinned = Isnull(HRC.IsPinned,0),
						ChannelIsSnoozed = Isnull(HRC.IsSnoozed,0),
						ChannelLastMessageDateTime = Isnull(Convert(datetime,HRC.LastMessageDateTime,103),null)
			FROM		gen_SalesHiringRequest_Channel HRC with(nolock) 
			WHERE       HRC.ID = @ChannelID
			ORDER BY 	HRC.LastMessageDateTime DESC
END
