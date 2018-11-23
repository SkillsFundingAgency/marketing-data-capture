-- ============================================================================
-- Author:      Matt Middleton
-- Create Date: 2018-11-23 
-- ============================================================================
CREATE PROCEDURE Create_Consent
(
    @Person_Id BIGINT,
    @Created DATETIME,
	@GdprConsentDeclared DATETIME,
	@GdprConsentGiven BIT
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Consent (
		Person_Id,
		Created,
		GdprConsentDeclared,
		GdprConsentGiven
	)
	OUTPUT inserted.Id
	VALUES (
		@Person_Id,
		@Created,
		@GdprConsentDeclared,
		@GdprConsentGiven
	);

END