-- ============================================================================
-- Author:      Matt Middleton
-- Create Date: 2018-11-23 
-- ============================================================================
CREATE PROCEDURE Create_ContactDetail
(
    @Person_Id BIGINT,
    @Created DATETIME,
	@Captured DATETIME,
	@EmailAddress NVARCHAR(256),
	@EmailVerificationCompletion DATETIME
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO ContactDetail (
		Person_Id,
		Created,
		Captured,
		EmailAddress,
		EmailVerificationCompletion
	)
	OUTPUT inserted.Id
	VALUES (
		@Person_Id,
		@Created,
		@Captured,
		@EmailAddress,
		@EmailVerificationCompletion
	);

END