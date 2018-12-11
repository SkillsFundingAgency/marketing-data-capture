-- ============================================================================
-- Author:      Matt Middleton
-- Create Date: 2018-11-23 
-- ============================================================================
CREATE PROCEDURE Create_Person
(
    @Created DATETIME,
	@Enrolled DATETIME,
	@FirstName NVARCHAR(256),
	@LastName NVARCHAR(256)
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Person (
		Created,
		Enrolled,
		FirstName,
		LastName
	)
	OUTPUT inserted.Id
	VALUES (
		@Created,
		@Enrolled,
		@FirstName,
		@LastName
	);

END