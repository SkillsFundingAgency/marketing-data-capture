CREATE TABLE [dbo].[Person] (
    [Id]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Created]   DATETIME       NOT NULL,
    [Enrolled]  DATETIME       NOT NULL,
    [FirstName] NVARCHAR (256) NOT NULL,
    [LastName]  NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED ([Id] ASC)
);



