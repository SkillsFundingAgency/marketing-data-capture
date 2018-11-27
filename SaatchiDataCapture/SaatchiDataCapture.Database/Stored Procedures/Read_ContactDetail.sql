-- ============================================================================
-- Author:      Matt Middleton
-- Create Date: 2018-11-27
-- ============================================================================
CREATE PROCEDURE Read_ContactDetail
(
    @EmailAddress NVARCHAR(256)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
		cd.Id
	FROM ContactDetail AS cd
	WHERE cd.EmailAddress = @EmailAddress;

END