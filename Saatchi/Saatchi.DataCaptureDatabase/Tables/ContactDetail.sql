CREATE TABLE [dbo].[ContactDetail] (
    [Id]                          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Person_Id]                   BIGINT         NOT NULL,
    [Created]                     DATETIME       NOT NULL,
    [Captured]                    DATETIME       NOT NULL,
    [EmailAddress]                NVARCHAR (256) NOT NULL,
    [EmailVerificationCompletion] DATETIME       NULL,
    [EmailVerified]               AS             (CONVERT([bit],case when [EmailVerificationCompletion] IS NULL then (0) else (1) end)) PERSISTED,
    CONSTRAINT [PK_ContactDetail] PRIMARY KEY NONCLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ContactDetail_Person] FOREIGN KEY ([Person_Id]) REFERENCES [dbo].[Person] ([Id])
);




GO
CREATE CLUSTERED INDEX [IX_ContactDetail_EmailAddress]
    ON [dbo].[ContactDetail]([EmailAddress] ASC);

