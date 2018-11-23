-- ============================================================================
-- Author:      Matt Middleton
-- Create Date: 2018-11-23 
-- ============================================================================

CREATE PROCEDURE Create_Cookie
(
    @Person_Id BIGINT,
    @Created DATETIME,
	@Captured DATETIME,
	@CookieIdentifier NVARCHAR(128)
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Cookie (
		Person_Id,
		Created,
		Captured,
		CookieIdentifier
	)
	OUTPUT inserted.Id
	VALUES (
		@Person_Id,
		@Created,
		@Captured,
		@CookieIdentifier
	);

END