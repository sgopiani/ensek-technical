-- =============================================
-- Author:		Sumeet Gopiani
-- Create date: 27-Aug-2022
-- Description:	INSERT Meter Readings into Readings table
-- =============================================
CREATE PROCEDURE [dbo].[usp_Ensek_Readings_InsertMeterReadings] 
	@ReadingsTable [dbo].[dt_MeterReadings] READONLY
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT @ReadingsTable
END
GO
