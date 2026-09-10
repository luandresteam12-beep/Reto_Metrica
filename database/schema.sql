IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Pedidos] (
    [Id] int NOT NULL IDENTITY,
    [NumeroPedido] nvarchar(50) NOT NULL,
    [Cliente] nvarchar(150) NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [Estado] nvarchar(30) NOT NULL,
    [Eliminado] bit NOT NULL,
    [FechaEliminacion] datetime2 NULL,
    [CreadoEn] datetime2 NOT NULL,
    [ActualizadoEn] datetime2 NULL,
    [Version] rowversion NOT NULL,
    CONSTRAINT [PK_Pedidos] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_Pedidos_Eliminado_Fecha] ON [Pedidos] ([Eliminado], [Fecha]);
GO

CREATE UNIQUE INDEX [IX_Pedidos_NumeroPedido] ON [Pedidos] ([NumeroPedido]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260910031006_InitialCreate', N'8.0.5');
GO

COMMIT;
GO

