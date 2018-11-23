-- ============================================================================
-- Author:      Matt Middleton
-- Create Date: 2018-11-23 
-- ============================================================================
CREATE PROCEDURE Create_Route
(
    @Person_Id BIGINT,
	@Created DATETIME, 
	@Captured DATETIME,
	@RouteIdentifier NVARCHAR(256)
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Route] (
		Person_Id,
		Created,
		Captured,
		RouteIdentifier
	)
	OUTPUT inserted.Id
	VALUES (
		@Person_Id,
		@Created,
		@Captured,
		@RouteIdentifier
	);

END