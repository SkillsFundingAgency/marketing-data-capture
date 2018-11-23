CREATE TABLE Consent (
    Id BIGINT IDENTITY(1, 1) NOT NULL,
    Person_Id BIGINT NOT NULL,
    Created DATETIME NOT NULL,
    GdprConsentDeclared DATETIME NOT NULL,
    GdprConsentGiven BIT NULL,
    CONSTRAINT PK_Consent PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_Consent_Person FOREIGN KEY (Person_Id) REFERENCES Person (Id)
);