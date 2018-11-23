CREATE TABLE [dbo].[Consent] (
    [Id]        BIGINT   IDENTITY (1, 1) NOT NULL,
    [Person_Id] BIGINT   NOT NULL,
    [Given]     DATETIME NULL,
    CONSTRAINT [PK_Consent] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Consent_Person] FOREIGN KEY ([Person_Id]) REFERENCES [dbo].[Person] ([Id])
);

