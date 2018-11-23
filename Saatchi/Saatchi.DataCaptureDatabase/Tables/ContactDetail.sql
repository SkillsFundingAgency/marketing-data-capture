CREATE TABLE [dbo].[ContactDetail] (
    [Id]                          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Person_Id]                   BIGINT         NOT NULL,
    [EmailAddress]                NVARCHAR (256) NOT NULL,
    [EmailVerificationCompletion] DATETIME       NULL,
    CONSTRAINT [PK_ContactDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ContactDetail_Person] FOREIGN KEY ([Person_Id]) REFERENCES [dbo].[Person] ([Id])
);

