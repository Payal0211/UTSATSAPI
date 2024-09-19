--exec Sproc_Get_Channels_Message_Details 1
CREATE PROCEDURE [dbo].[Sproc_Get_Channels_Message_Details]
  @ChannelID			bigint = null
AS
BEGIN

			SELECT		ChannelID = ISNULL(HRC.ID,0),
						MessageID = ISNULL(CU.ID,0),
						MessageSentFrom = Isnull(MessageSender.FullName,''),
						IsMessageReplied     =  Isnull(CU.IsMessageReplied,0) ,
						MessageRepliedTo	 =  Isnull(MessageReplied.FullName,''),
						MessageRepliedID	 =  Isnull(CU.MessageRepliedID,0) ,
						IsMessageBookmarked	 =  ISNULL(CU.IsMessageBookmarked,0) ,
						Message				 =  ISNULL(CU.Message,'') ,
						IsSent				 =  ISNULL(CU.IsSent,0) ,
						MessageSentDateTime	 =  Isnull(Convert(datetime,CU.MessageSentDateTime,103),''),
						IsMessageActive		 =  ISnull(CU.IsACtive,0),
						IsActivity			 = Isnull(CU.IsActivity,0),
						DesignationOfMessageSender = ISnull(MessageSender.Designation,''),
						MessageDay =  Case when Convert(date,CU.MessageSentDateTime,103) = Convert(date,Getdate(),103) then 'Today'
									  when Convert(date,CU.MessageSentDateTime,103) = Convert(date,DAteAdd(Day,-1,getdate())) then 'Yesterday'
									  else 'Earlier'  end,
						MessageReactions = ISNULL((SELECT 
												STUFF((
														select ',' + CONVERT(nvarchar(100),CMR.UserID) + '#' + CONVERT(nvarchar(50),CMR.Emoticon) from gen_Channel_MessageReactions CMR with(nolock) 
														INNER JOIN gen_SalesHiringRequest_Channel HRC1 with(nolock) on CMR.ChannelID = HRC1.ID
														INNER JOIN gen_Channel_UserMessages CU1 WITH(NOLOCK) ON CU1.ChannelID = HRC1.ID and CMR.MessageID = CU1.ID
														WHERE HRC1.ID = HRC.ID and CU1.ID = CU.ID
														for xml path('')
													  ),1,1,''
													 )
											),'')
			FROM		gen_SalesHiringRequest_Channel HRC with(nolock) 
						INNER JOIN gen_Channel_UserMessages CU WITH(NOLOCK) ON CU.ChannelID = HRC.ID
						INNER JOIN usr_User MessageSender WITH(NOLOCK) ON MessageSender.ID = CU.MessageSentFrom
						LEFT JOIN usr_User MessageReplied WITH(NOLOCK) ON MessageReplied.ID = CU.MessageRepliedTo
						
			WHERE       HRC.ID = @ChannelID
			ORDER BY 	HRC.LastMessageDateTime DESC

END