----------------------------------------------------------------------------------------------------------------------------

ALTER PROCEDURE [dbo].[Sproc_Update_ShortistedTalent_ForCreditFlow_ClientPortal]  
   @ATSTalentId   bigint = 0,  
   @HrId		  bigint = 0,  
   @StatusID	  bigint = 0,  
   @GUID		  Nvarchar(50) = null,  
   @IsMyList	  bit = 0,  
   @IsUnLock      bit = 0,  
   @ProfileCredit decimal(18,2) = 0,  
   @IsVetted	  bit = 0,
   @SSOUserID BIGINT = NULL,
   @RejectionReason NVARCHAR(MAX) = NULL
AS  
BEGIN  
   
  --DECLARE @xml XML  
  --SET @xml = @XmlData  
  
  --SELECT     
  -- cast(c.value('(isMyList/@xsi:nil)[1]', 'bit') as int) AS IsMyList,  
  -- cast(0 as int) as IsUnLock,  
  -- c.value('(statusId)[1]', 'int') AS StatusId,  
  -- c.value('(atsTalentId)[1]', 'bigint') AS ATSTalentId,  
  -- --t.c.value('(HrId)[1]', 'int') AS HrId,     
  -- cast(1 as decimal(18,2)) as ProfileCredit,  
  -- c.value('(guid)[1]', 'Nvarchar(50)') AS Guid     
  --FROM @xml.nodes('/UpdateTalentDataNew/Talent/*') AS XTbl(c)  
      
  
		  Declare @CreditOptionID as int = 0  
		  Declare @ContactID as bigint = 0  
		  Declare @ActionID as int = 0
		  Declare @StatusCount as int = 0
		  Declare @HRTypeId as INT = 0
		  DECLARE @TalentStatusID_BasedOnHR as int = 0
		  DECLARE @Actionname as varchar(500) = '',
		  @ActionPerformedBy BIGINT = 0,
		  @ActionManagedByClient BIT = 1
		
		  select TOP 1 
		  @ContactID = ContactID,
		  @HRTypeId = HRTypeId
		  from   gen_SalesHiringRequest With(Nolock)  
		  where  ID = @HrId 
  
		  If @IsVetted = 1  
		   SET @CreditOptionID = 2  
		  ELse   
		   SET @CreditOptionID = 3  
  
		  select @ProfileCredit = CreditUsed  
		  from   prg_CreditOption_ClientPortal WITH(NOLOCK)  
		  Where  ID = @CreditOptionID  

		   DECLARE @UtsTalentID Bigint = 0, @CTPID BIGINT, @TalentStatus_BasedOnHR INT
		   SELECT TOP 1 @UtsTalentID = ID FROM gen_Talent WITH(NOLOCK) WHERE ATS_Talent_ID = @ATSTalentId	
		   
		   -- Set action performed by user based on SSO login UTS-7223
			SET @ActionPerformedBy =  (SELECT IIF(ISNULL(@SSOUserId,0) = 0, @ContactID, @SSOUserId))
			SET @ActionManagedByClient = (SELECT IIF(ISNULL(@SSOUserId,0) = 0, 1, 0))   

		  IF(@HRTypeId IN (3, 6))
		  BEGIN
			  IF Not Exists(Select 1 from gen_CRCTP_ShortlistedTalents_ClientPortal With(Nolock) where HRID = @HrId and ATS_TalentID = @ATSTalentId and GUID = @GUID)  
			   BEGIN  
				 Insert into gen_CRCTP_ShortlistedTalents_ClientPortal(ATS_TalentID,HRID,GUID,StatusID, StatusChangeDateTime, IsVetted,ProfileCredit, IsMyList, MyListDateTime)  
				 Values(@ATSTalentId,@HrId,@GUID,@StatusID,IIF(@StatusID = null, null,GETDATE()),@IsVetted,@ProfileCredit, @IsMyList, IIF(@IsMyList = null, null,GETDATE()) )      
			   ENd  
			  ELSE  
			   BEGIN  
				 UPDATE gen_CRCTP_ShortlistedTalents_ClientPortal  
				 SET    IsMyList = Isnull(@IsMyList,IsMyList),  
					 MyListDateTime = case when Isnull(@IsMyList,0) <> 0 then GETDATE() else MyListDateTime end,  
					 StatusID = Isnull(@StatusID,StatusID),  
					 StatusChangeDateTime = case when Isnull(@StatusID,0) <> 0 then GETDATE() else StatusChangeDateTime end,  
					 IsVetted = ISNULL(@IsVetted, IsVetted)  
				 Where  HRID = @HrId and ATS_TalentID = @ATSTalentId   
				 --and GUID = @GUID  
  
				 UPDATE gen_SalesHiringRequest SET LastActivityDate = GETDATE() WHERE ID = @HrId  
			   END 
  
						 Select   IsMyList = cast(Isnull(IsMyList,0) as int),  
						   IsUnLock = cast(Isnull(IsUnLock,0) as int),  
						   StatusID = Isnull(StatusID,0),  
						   AtsTalentId = ATS_TalentID,  
						   ProfileCredit = Isnull(ProfileCredit,0),  
						   GUID = ISNULL(@GUID,GUID)  
				  from     gen_CRCTP_ShortlistedTalents_ClientPortal With(Nolock)   
				  where   HRID = @HrId and ATS_TalentID = @ATSTalentId and GUID = @GUID  
			END
			ELSE
			BEGIN
				Select   IsMyList = cast(Isnull(STC.IsBookMarked,0) as int),  
						   IsUnLock = 1,  
						   StatusID = Isnull(TalentStatusID_ClientPortal,0),  
						   AtsTalentId = @ATSTalentId,  
						   ProfileCredit = CAST(0 AS DECIMAL(18,2)),  
						   GUID = ISNULL(@GUID,HRTalentGUID)  
				  from     gen_ContactTalentPriority CTP With(Nolock) 
						   LEFT JOIN gen_ShortlistedTalents_ClientPortal STC With(Nolock) ON CTP.ID = STC.CtpId
				  where   HiringRequestID = @HrId and TalentID = @UtsTalentID
			END

			 --------If the Talent data is there in CTP then update------------------				

			 IF EXISTS(SELECT 1 FROM gen_ContactTalentPriority WITH(NOLOCK) WHERE HiringRequestID = @HrId AND TalentID = @UtsTalentID)
			 BEGIN
				
				SELECT TOP 1 @CTPID = ID FROM gen_ContactTalentPriority WITH(NOLOCK) WHERE HiringRequestID = @HrId AND TalentID = @UtsTalentID	
				
					UPDATE gen_ContactTalentPriority SET 					
					HRTalentGUID = @Guid,
					ModifiedByID = @ActionPerformedBy,
					ModifiedByDatetime = GETDATE()
					WHERE ID = @CTPID


				IF(ISNULL(@StatusID,0) <> 0)
				BEGIN

					SELECT TOP 1 @TalentStatus_BasedOnHR = TalentStatusID_BasedOnHR FROM prg_TalentStatus_ClientPortal WITH(NOLOCK) WHERE ID = @StatusID

					UPDATE gen_ContactTalentPriority SET 
					TalentStatusID_ClientPortal = @StatusID,
					TalentStatusID_BasedOnHR = @TalentStatus_BasedOnHR,
					RejectReasonID = IIF(@StatusID = 8, -1, NULL),
					OtherRejectReason = IIF(@StatusID = 8, @RejectionReason, NULL),
					ModifiedByID = @ActionPerformedBy,
					ModifiedByDatetime = GETDATE()
					WHERE ID = @CTPID

					IF(@StatusID = 2 )
					BEGIN
						SET @Actionname = 'New_Talent_Status_InContacted'
					END
					ELSE IF(@StatusID = 3 )
					BEGIN
						SET @Actionname = 'New_Talent_Status_InSubmitted'
					END
					ELSE IF(@StatusID = 4 )
					BEGIN
						SET @Actionname = 'New_Talent_Status_InAssessment'
					END
					ELSE IF(@StatusID = 5 )
					BEGIN
						SET @Actionname = 'Interview_in_Process'
					END
					ELSE IF(@StatusID = 6 )
					BEGIN
						SET @Actionname = 'New_Talent_Status_Offered'
					END
					ELSE IF(@StatusID = 7 )
					BEGIN
						SET @Actionname = 'New_Talent_Status_Hired'
					END
					ELSE IF(@StatusID = 8 )
					BEGIN
						SET @Actionname = 'Talent_Status_Rejected'
					END

					EXEC sproc_HiringRequest_History_Insert @Action = @Actionname,@HiringRequest_ID= @HRID,@Talent_ID= @UtsTalentID,
								@Created_From =0, @CreatedById =@ActionPerformedBy,@ContactTalentPriority_ID= @CTPID,@InterviewMaster_ID= 0, @HR_AcceptedDateTime='', 
								@OnBoard_ID=0,@IsManagedByClient= @ActionManagedByClient,@IsManagedByTalent= 0,@SalesUserID= 0,@OldSalesUserID= 0,@AppActionDoneBy= 4

				END

				-- If added in my list then update the tables related to CTP.
				IF(@IsMyList IS NOT NULL)
				BEGIN
					EXEC sproc_Add_Update_BookMarked_Talents_ClientPortal @HRID = @HRID,@ContactId = @ContactID, @CTPId = @CTPID, @Flag = 'Add', @AddBookeMarked = @IsMyList, @SSOUserID = @SSOUserID
				END			

			 END
			 
			 IF(ISNULL(@StatusID,0) <> 0)
			 BEGIN
				 -----------Update the zomato tracker------------------
				 IF(@HRTypeId IN (3, 6))
				 BEGIN
 
					  SET @ActionID = 8
					  SET @StatusID = 5

						If not Exists(select 1 from gen_ATS_HRStatus_Details with(Nolock) where ATSHRStatusId = @ActionID and HRID = @HrId)
							BEGIN
								INSERT INTO gen_ATS_HRStatus_Details 
								(HRID,ATSHRStatusId,Published_Datetime,createdByID,CreatedByDateTime,ModifiedByID,ModifiedByDateTime,Total_Talent)
								VALUES(@HrId,@ActionID,null,@ActionPerformedBy,getdate(),null,null,1);									
							END
						 
							BEGIN
									select @StatusCount = Count(ID)
									from   gen_CRCTP_ShortlistedTalents_ClientPortal WITH(NOLOCK)
									WHERE  HRID = @HrId and StatusID = @StatusID

									UPdate gen_ATS_HRStatus_Details
									set    Total_Talent = @StatusCount,ModifiedByDateTime = getdate(),ModifiedByID = @ActionPerformedBy
									where  ATSHRStatusId = @ActionID and HRID = @HrId
							END
		
					  SET @ActionID = 9
					  SET @StatusID = 6

					  If not Exists(select 1 from gen_ATS_HRStatus_Details with(Nolock) where ATSHRStatusId = @ActionID and HRID = @HrId)
							BEGIN
								INSERT INTO gen_ATS_HRStatus_Details 
								(HRID,ATSHRStatusId,Published_Datetime,createdByID,CreatedByDateTime,ModifiedByID,ModifiedByDateTime,Total_Talent)
								VALUES(@HrId,@ActionID,null,@ActionPerformedBy,getdate(),null,null,1);									
							END
						 
							BEGIN
									select @StatusCount = Count(ID)
									from   gen_CRCTP_ShortlistedTalents_ClientPortal WITH(NOLOCK)
									WHERE  HRID = @HrId and StatusID = @StatusID

									UPdate gen_ATS_HRStatus_Details
									set    Total_Talent = @StatusCount,ModifiedByDateTime = getdate(),ModifiedByID = @ActionPerformedBy
									where  ATSHRStatusId = @ActionID and HRID = @HrId
							END

					  SET @ActionID = 10
					  SET @StatusID = 7

					  If not Exists(select 1 from gen_ATS_HRStatus_Details with(Nolock) where ATSHRStatusId = @ActionID and HRID = @HrId)
							BEGIN
								INSERT INTO gen_ATS_HRStatus_Details 
								(HRID,ATSHRStatusId,Published_Datetime,createdByID,CreatedByDateTime,ModifiedByID,ModifiedByDateTime,Total_Talent)
								VALUES(@HrId,@ActionID,null,@ActionPerformedBy,getdate(),null,null,1);									
							END
						 
							BEGIN
									select @StatusCount = Count(ID)
									from   gen_CRCTP_ShortlistedTalents_ClientPortal WITH(NOLOCK)
									WHERE  HRID = @HrId and StatusID = @StatusID

									UPdate gen_ATS_HRStatus_Details
									set    Total_Talent = @StatusCount,ModifiedByDateTime = getdate(),ModifiedByID = @ActionPerformedBy
									where  ATSHRStatusId = @ActionID and HRID = @HrId
							END

						-----------Block Added by RIYA (04-04-2024) IF no of hired talent is equal to no of TR then complete the HR ------------------
						---- Change in salesHiringdetail table also RolestatusID, JobStatusID ------

						--select @StatusCount = Count(ID)
						--			from   gen_CRCTP_ShortlistedTalents_ClientPortal WITH(NOLOCK)
						--			WHERE  HRID = @HrId and StatusID = 7

						--DECLARE @HRTR INT
						--SELECT TOP 1 @HRTR = NoOfTalents FROM gen_SalesHiringRequest WITH(NOLOCK) WHERE ID = @HrId

						--IF(@StatusCount = @HRTR)
						--BEGIN
						--	UPDATE gen_SalesHiringRequest SET Status_ID = 3, LastModifiedByID = @ContactID, LastModifiedDatetime = GETDATE() 
						--	WHERE ID = @HrId
						--END
						--ELSE
						--BEGIN
						--	UPDATE gen_SalesHiringRequest SET Status_ID = 2, LastModifiedByID = @ContactID, LastModifiedDatetime = GETDATE() 
						--	WHERE ID = @HrId
						--END

				  END
				 ELSE
				 BEGIN
						SET @ActionID = 8
						SET @TalentStatusID_BasedOnHR = 3
					
						If not Exists(select 1 from gen_ATS_HRStatus_Details with(Nolock) where ATSHRStatusId = @ActionID and HRID = @HrId)
										BEGIN
											INSERT INTO gen_ATS_HRStatus_Details 
											(HRID,ATSHRStatusId,Published_Datetime,createdByID,CreatedByDateTime,ModifiedByID,ModifiedByDateTime,Total_Talent)
											VALUES(@HrId,@ActionID,null,@ActionPerformedBy,getdate(),null,null,1);									
										END
									 
										BEGIN
												select @StatusCount = Count(ID)
												from   gen_ContactTalentPriority WITH(NOLOCK)
												WHERE  HiringRequestID = @HrId and TalentStatusID_BasedOnHR = @TalentStatusID_BasedOnHR

												UPdate gen_ATS_HRStatus_Details
												set    Total_Talent = @StatusCount,ModifiedByDateTime = getdate(),ModifiedByID = @ActionPerformedBy
												where  ATSHRStatusId = @ActionID and HRID = @HrId
										END
					

						SET @ActionID = 9
						SET @TalentStatusID_BasedOnHR = 4

					
						If not Exists(select 1 from gen_ATS_HRStatus_Details with(Nolock) where ATSHRStatusId = @ActionID and HRID = @HrId)
										BEGIN
											INSERT INTO gen_ATS_HRStatus_Details 
											(HRID,ATSHRStatusId,Published_Datetime,createdByID,CreatedByDateTime,ModifiedByID,ModifiedByDateTime,Total_Talent)
											VALUES(@HrId,@ActionID,null,@ActionPerformedBy,getdate(),null,null,1);									
										END
									 
										BEGIN
												select @StatusCount = Count(ID)
												from   gen_ContactTalentPriority WITH(NOLOCK)
												WHERE  HiringRequestID = @HrId and TalentStatusID_BasedOnHR = @TalentStatusID_BasedOnHR

												UPdate gen_ATS_HRStatus_Details
												set    Total_Talent = @StatusCount,
												ModifiedByDateTime = getdate(),
												ModifiedByID = @ActionPerformedBy
												where  ATSHRStatusId = @ActionID and HRID = @HrId
										END
					

						SET @ActionID = 10
						SET @TalentStatusID_BasedOnHR = 10

					
						If not Exists(select 1 from gen_ATS_HRStatus_Details with(Nolock) where ATSHRStatusId = @ActionID and HRID = @HrId)
										BEGIN
											INSERT INTO gen_ATS_HRStatus_Details 
											(HRID,ATSHRStatusId,Published_Datetime,createdByID,CreatedByDateTime,ModifiedByID,ModifiedByDateTime,Total_Talent)
											VALUES(@HrId,@ActionID,null,@ActionPerformedBy,getdate(),null,null,1);									
										END
									 
										BEGIN
												select @StatusCount = Count(ID)
												from   gen_ContactTalentPriority WITH(NOLOCK)
												WHERE  HiringRequestID = @HrId and TalentStatusID_BasedOnHR = @TalentStatusID_BasedOnHR

												UPdate gen_ATS_HRStatus_Details
												set    Total_Talent = @StatusCount,ModifiedByDateTime = getdate(),ModifiedByID = @ActionPerformedBy
												where  ATSHRStatusId = @ActionID and HRID = @HrId
										END
					
				  END

					---- Riya(04-04-2024) Mark  HR  as Completed / won Status based on Qty & CTP (No of Talents) Matched
					--	EXEC Sproc_Update_HRStatus_BasedonQty 
					--		@ContactID = @ContactID, 
					--		@HR_ID = @HRID, 
					--		@Talent_Id = @UtsTalentID, 
					--		@CreatedModifiedId = @ContactID

					--	--Riya(4-4-2024), after reject talent if hr is completed than based on tr, again HR will be in process
					--	EXEC Sproc_UTS_UpdateHRActive_BasedonQty
					--		@HR_ID = @HRID,
					--		@Talent_ID = @UtsTalentID,
					--		@CreatedModifiedId =  @ContactID
			 END	
			 

			 --Added Jimit For Talent BackOut Case Mark HR in InProcess if TR > 1 otherwise Close - Lost.  20-05-24
				--DECLARE @NoofTalents as int = 0
				--IF @StatusID <> 0
				--	BEGIN
				--			SELECT	@NoofTalents = NoofTalents 
				--			from	gen_SalesHiringRequest WITH(NOLOCK) WHERE ID = @HRID

				--			IF @NoofTalents = 1
				--				BEGIN
				--					  UPDATE  gen_SalesHiringRequest
				--					  SET	  Status_ID = 6,JobStatusID = 2
				--					  WHERE   ID = @HRID
				--				END
				--			ELSE
				--				BEGIN
				--						 UPDATE  gen_SalesHiringRequest
				--						  SET	  Status_ID = 2,JobStatusID = 1
				--						  WHERE   ID = @HRID
				--				END

				--	END
END

