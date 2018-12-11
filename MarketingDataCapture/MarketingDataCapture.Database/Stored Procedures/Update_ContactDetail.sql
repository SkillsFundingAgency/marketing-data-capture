-- ============================================================================
-- Author:      Matt Middleton
-- Create Date: 2018-11-29
-- ============================================================================
CREATE PROCEDURE Update_ContactDetail
(
    @Id BIGINT,
    @EmailVerificationCompletion DATETIME
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ContactDetail
    SET
        EmailVerificationCompletion = @EmailVerificationCompletion
    WHERE
        Id = @Id;

END