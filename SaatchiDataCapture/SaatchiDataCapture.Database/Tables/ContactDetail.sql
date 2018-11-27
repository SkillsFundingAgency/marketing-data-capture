CREATE TABLE ContactDetail (
    Id BIGINT IDENTITY(1, 1) NOT NULL,
    Person_Id BIGINT NOT NULL,
    Created DATETIME NOT NULL,
    Captured DATETIME NOT NULL,
    EmailAddress NVARCHAR (256) NOT NULL,
    EmailVerificationCompletion DATETIME NULL,
    EmailVerified AS (CONVERT(BIT, CASE WHEN (EmailVerificationCompletion IS NULL) THEN 0 ELSE 1 END)) PERSISTED,
    CONSTRAINT PK_ContactDetail PRIMARY KEY NONCLUSTERED (Id ASC),
    CONSTRAINT FK_ContactDetail_Person FOREIGN KEY (Person_Id) REFERENCES Person (Id)
);

GO
CREATE UNIQUE CLUSTERED INDEX IX_ContactDetail_EmailAddress
    ON ContactDetail (EmailAddress ASC);