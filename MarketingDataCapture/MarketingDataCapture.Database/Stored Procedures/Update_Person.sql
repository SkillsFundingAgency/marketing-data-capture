-- ============================================================================
-- Author:      Matt Middleton
-- Create Date: 2018-11-29
-- ============================================================================
CREATE PROCEDURE Update_Person
(
    @Id BIGINT,
    @FirstName NVARCHAR(256),
    @LastName NVARCHAR(256)
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Person
    SET
        FirstName = ISNULL(@FirstName, FirstName),
        LastName = ISNULL(@LastName, LastName)
    WHERE
        Id = @Id;

END