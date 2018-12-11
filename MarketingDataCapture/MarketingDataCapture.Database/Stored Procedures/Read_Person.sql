-- ============================================================================
-- Author:      Matt Middleton
-- Create Date: 2018-11-27
-- ============================================================================
CREATE PROCEDURE Read_Person
(
    @EmailAddress NVARCHAR(256)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
		p.Id,
        cd.Id AS ContactDetail_Id
	FROM Person AS p
	INNER JOIN ContactDetail AS cd ON cd.Person_Id = p.Id
	WHERE cd.EmailAddress = @EmailAddress;

END