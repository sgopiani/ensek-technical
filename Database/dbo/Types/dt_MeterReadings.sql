CREATE TYPE dt_MeterReadings AS TABLE 
(
	AccountId int NOT NULL,
	MeterReadingDateTime DateTime NOT NULL,
	MeterReadValue nvarchar(5) NOT NULL

)