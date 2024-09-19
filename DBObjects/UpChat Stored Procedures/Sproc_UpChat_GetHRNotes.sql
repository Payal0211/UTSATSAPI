CREATE PROCEDURE dbo.Sproc_UpChat_GetHRNotes 
	@HrID BIGINT NULL,
	@NoteID BIGINT NULL
AS
BEGIN
	SELECT 
	N.ID as NoteID, 
	N.HiringRequest_ID, 
	N.Notes, 
	U.FullName as UserName, 
	U.EmployeeID as UserEmpID, 
	U.Designation as UserDesignation,
	N.CreatedByID, 
	N.CreatedByDatetime 
	FROM gen_HRNotes (NOLOCK) as N INNER JOIN 
		 usr_User (NOLOCK) as U on U.ID = N.CreatedByID
	WHERE N.HiringRequest_ID = @HrID AND N.ID = @NoteID  
END

