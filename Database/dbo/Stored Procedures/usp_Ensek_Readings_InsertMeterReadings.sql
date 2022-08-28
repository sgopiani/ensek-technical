-- =============================================
-- Author:		Sumeet Gopiani
-- Create date: 27-Aug-2022
-- Description:	INSERT Meter Readings into Readings table
-- =============================================
CREATE PROCEDURE [dbo].[usp_Ensek_Readings_InsertMeterReadings] 
	@MeterReadings [dbo].[dt_MeterReadings] READONLY
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	
	DECLARE @ExistingAccounts AS TABLE(AccountId int);
	DECLARE @InsertedRows INT = 0,
			@UpdatedRows INT = 0;

	INSERT INTO @ExistingAccounts(AccountId)
	SELECT a.AccountId
	FROM Accounts a
	INNER JOIN @MeterReadings mr ON mr.AccountId = a.AccountId

	INSERT INTO Readings(AccountId, ReadingDateTime, ReadingValue)
	SELECT mr.AccountId, mr.MeterReadingDateTime, mr.MeterReadValue
	FROM @MeterReadings mr
	INNER JOIN @ExistingAccounts ea ON ea.AccountId = mr.AccountId
	FULL OUTER JOIN Readings r ON r.AccountId = mr.AccountId 
	WHERE r.AccountId IS NULL;

	SELECT @InsertedRows = @@ROWCOUNT;

	UPDATE Readings
	SET ReadingDateTime = mr.MeterReadingDateTime,
		ReadingValue = mr.MeterReadValue
	FROM @MeterReadings mr
	INNER JOIN Readings r ON mr.AccountId = r.AccountId AND mr.MeterReadingDateTime > r.ReadingDateTime;

	SELECT @UpdatedRows = @@ROWCOUNT;

	select @InsertedRows + @UpdatedRows;
END
GO
