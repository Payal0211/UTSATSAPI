Create Procedure [dbo].[Sproc_Get_Email_SubjectList]
AS
BEGIN
		select ID,DisplayName
		from   gen_AWS_Tracking_Email_Subject WITH(NOLOCK)

END
