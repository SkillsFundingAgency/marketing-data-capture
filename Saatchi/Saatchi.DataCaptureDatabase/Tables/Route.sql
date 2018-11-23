CREATE TABLE [dbo].[Route] (
    [Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [Person_Id]       BIGINT         NOT NULL,
    [RouteDate]       DATETIME       NOT NULL,
    [RouteIdentifier] NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_Route] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Route_Person] FOREIGN KEY ([Person_Id]) REFERENCES [dbo].[Person] ([Id])
);

