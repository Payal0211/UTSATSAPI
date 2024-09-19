CREATE PROCEDURE [dbo].[Sproc_AddUpdate_Location]
  @LocationID				bigint = null,
  @city						Nvarchar(Max) = null,
  @state					Nvarchar(Max) = null,
  @revised_city				Nvarchar(Max) = null
AS
BEGIN

		IF EXISTS(Select 1 from prg_State_District WITH(NOLOCK) where ID = @LocationID)	
			BEGIN
					Update prg_State_District
					SET	   District = Isnull(@City,District),
						   RevisedCity = Isnull(@revised_city,RevisedCity)
					where  ID = @LocationID

			END
		ELSE
			BEGIN
					Insert into prg_State_District(State,District,Country,Status,RevisedCity)
					Values (@state,@city,'India',1,@revised_city)

					SET @LocationID = @@IDENTITY
			END
			
			Select @LocationID as ID

END