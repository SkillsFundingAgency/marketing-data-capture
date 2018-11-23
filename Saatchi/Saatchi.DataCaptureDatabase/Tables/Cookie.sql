CREATE TABLE [dbo].[Cookie] (
    [Id]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [Person_Id]        BIGINT         NOT NULL,
    [CookieIdentifier] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_Cookie] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Cookie_Person] FOREIGN KEY ([Person_Id]) REFERENCES [dbo].[Person] ([Id])
);

