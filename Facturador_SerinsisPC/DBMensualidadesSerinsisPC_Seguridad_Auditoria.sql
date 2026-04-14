USE [DBMensualidadesSerinsisPC];
GO

IF OBJECT_ID('dbo.BitacoraSeguridadAdmin', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.BitacoraSeguridadAdmin
    (
        id INT IDENTITY(1,1) PRIMARY KEY,
        idUsuarioAdmin INT NULL,
        loginUsuario VARCHAR(60) NULL,
        accion VARCHAR(60) NOT NULL,
        detalle VARCHAR(250) NULL,
        fechaEvento DATETIME NOT NULL CONSTRAINT DF_BitacoraSeguridadAdmin_fechaEvento DEFAULT (GETDATE())
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE name = 'FK_BitacoraSeguridadAdmin_UsuarioAdmin'
)
BEGIN
    ALTER TABLE dbo.BitacoraSeguridadAdmin
    ADD CONSTRAINT FK_BitacoraSeguridadAdmin_UsuarioAdmin
    FOREIGN KEY (idUsuarioAdmin) REFERENCES dbo.UsuarioAdmin(id);
END
GO

CREATE OR ALTER PROCEDURE dbo.InsertInto_BitacoraSeguridadAdmin
    @idUsuarioAdmin INT = NULL,
    @loginUsuario VARCHAR(60) = NULL,
    @accion VARCHAR(60),
    @detalle VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO dbo.BitacoraSeguridadAdmin
        (
            idUsuarioAdmin,
            loginUsuario,
            accion,
            detalle
        )
        VALUES
        (
            @idUsuarioAdmin,
            @loginUsuario,
            @accion,
            @detalle
        );

        SELECT 1 AS respuesta, CONVERT(INT, SCOPE_IDENTITY()) AS nuevoId;
    END TRY
    BEGIN CATCH
        SELECT 0 AS respuesta, 0 AS nuevoId;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.UpdatePassword_UsuarioAdmin_Interna
    @id INT,
    @passwordNueva VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @salt VARCHAR(64) =
            CONVERT(VARCHAR(64), NEWID()) + CONVERT(VARCHAR(32), ABS(CHECKSUM(NEWID())));

        DECLARE @hash VARCHAR(64) =
            CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @salt + @passwordNueva), 2);

        UPDATE dbo.UsuarioAdmin
        SET
            passwordSalt = @salt,
            passwordHash = @hash
        WHERE id = @id
          AND estado = 1;

        IF @@ROWCOUNT > 0
            SELECT 1 AS respuesta;
        ELSE
            SELECT 0 AS respuesta;
    END TRY
    BEGIN CATCH
        SELECT 0 AS respuesta;
    END CATCH
END
GO

/*
Registro manual de eventos si lo necesitas:

EXEC dbo.InsertInto_BitacoraSeguridadAdmin
    @idUsuarioAdmin = 1,
    @loginUsuario = 'admin',
    @accion = 'LOGIN_OK',
    @detalle = 'Ingreso correcto al panel';
*/
