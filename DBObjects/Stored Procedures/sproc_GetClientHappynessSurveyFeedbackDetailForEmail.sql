ALTER PROCEDURE [dbo].[sproc_GetClientHappynessSurveyFeedbackDetailForEmail]
@HappinessSurvey_FeedbackID  BIGINT
AS
BEGIN
							SELECT  
									Company = CASE WHEN isnull(chs.Other_Company_Name,'') <> '' THEN isnull(chs.Other_Company_Name,'') ELSE  ISNULL(co.Company,'') END, 
									Client = CASE WHEN ISNULL(chs.Other_Client_Name,'') <> '' THEN ISNULL(chs.Other_Client_Name,'') ELSE ISNULL(con.FullName,'') END, 
									Email = CASE WHEN ISNULL(chs.Other_ClientEmailID,'') <> '' THEN ISNULL(chs.Other_ClientEmailID,'') ELSE ISNULL(con.EmailID,'') END,
									ISNULL(Rating,0) Rating,
									ISNULL((SELECT STUFF((SELECT TOP 1 ',' + ISNULL(L.HappinessSurvey_Question,'')  FROM prg_ClientHappinessSurvey_FeedbackQuestions L WITH(NOLOCK) inner join gen_ClientHappinessSurvey_Questions CHO WITH(NOLOCK) On CHO.HappinessSurvey_QuestionID = L.ID WHERE CHO.HappinessSurvey_FeedbackID  = chs.ID ORDER BY CHO.ID DESC FOR XML PATH('')),1,1,'')),'') AS Question, 
									ISNULL((SELECT STUFF((SELECT '|' + ISNULL(L.HappynessSurvay_Option,'')  FROM prg_ClientHappinessSurvey_FeedbackOptions L WITH(NOLOCK) inner join gen_ClientHappinessSurvey_Options CHO WITH(NOLOCK) ON CHO.HappynessSurvay_OptionID = L.ID  WHERE CHO.HappynessSurvay_Feedback_ID = chs.ID ORDER BY CHO.ID DESC FOR XML PATH('')),1,1,'')),'') AS Options, 
									ISNULL(chs.Feedback_Comments,'') AS Comments,
									ISNULL(chs.TestimonialOptions,'') as TestimonialOptions
							FROM	gen_ClientHappinessSurvey chs WITH(NOLOCK)									  
									LEFT JOIN gen_Company co WITH(NOLOCK) ON chs.Company_ID = co.ID
									LEFT JOIN gen_Contact con WITH(NOLOCK) ON chs.Contact_ID = con.ID
									WHERE chs.ID = @HappinessSurvey_FeedbackID
END