USE [DBMensualidadesSerinsisPC];
GO

IF OBJECT_ID('dbo.UsuarioAdmin', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.UsuarioAdmin
    (
        id INT IDENTITY(1,1) PRIMARY KEY,
        nombreUsuario VARCHAR(100) NOT NULL,
        loginUsuario VARCHAR(60) NOT NULL,
        passwordSalt VARCHAR(64) NOT NULL,
        passwordHash VARCHAR(64) NOT NULL,
        estado BIT NOT NULL CONSTRAINT DF_UsuarioAdmin_estado DEFAULT (1),
        fechaCreacion DATETIME NOT NULL CONSTRAINT DF_UsuarioAdmin_fechaCreacion DEFAULT (GETDATE()),
        fechaUltimoAcceso DATETIME NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_UsuarioAdmin_loginUsuario' AND object_id = OBJECT_ID('dbo.UsuarioAdmin'))
BEGIN
    CREATE UNIQUE INDEX UX_UsuarioAdmin_loginUsuario
    ON dbo.UsuarioAdmin(loginUsuario);
END
GO

CREATE OR ALTER VIEW dbo.V_UsuarioAdmin
AS
SELECT
    id,
    nombreUsuario,
    loginUsuario,
    estado,
    fechaCreacion,
    fechaUltimoAcceso
FROM dbo.UsuarioAdmin;
GO

CREATE OR ALTER PROCEDURE dbo.InsertInto_UsuarioAdmin
    @nombreUsuario VARCHAR(100),
    @loginUsuario VARCHAR(60),
    @passwordPlano VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @salt VARCHAR(64) = CONVERT(VARCHAR(64), NEWID()) + CONVERT(VARCHAR(32), ABS(CHECKSUM(NEWID())));
        DECLARE @hash VARCHAR(64) =
            CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', CONVERT(VARCHAR(64), @salt) + @passwordPlano), 2);

        INSERT INTO dbo.UsuarioAdmin
        (
            nombreUsuario,
            loginUsuario,
            passwordSalt,
            passwordHash,
            estado
        )
        VALUES
        (
            UPPER(LTRIM(RTRIM(@nombreUsuario))),
            LOWER(LTRIM(RTRIM(@loginUsuario))),
            @salt,
            @hash,
            1
        );

        SELECT 1 AS respuesta, CONVERT(INT, SCOPE_IDENTITY()) AS nuevoId;
    END TRY
    BEGIN CATCH
        SELECT 0 AS respuesta, 0 AS nuevoId;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.Validar_UsuarioAdmin
    @loginUsuario VARCHAR(60),
    @passwordPlano VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        id,
        nombreUsuario,
        loginUsuario,
        estado
    FROM dbo.UsuarioAdmin
    WHERE loginUsuario = LOWER(LTRIM(RTRIM(@loginUsuario)))
      AND estado = 1
      AND passwordHash = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', passwordSalt + @passwordPlano), 2);
END
GO

CREATE OR ALTER PROCEDURE dbo.UpdateUltimoAcceso_UsuarioAdmin
    @id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        UPDATE dbo.UsuarioAdmin
        SET fechaUltimoAcceso = GETDATE()
        WHERE id = @id;

        SELECT 1 AS respuesta;
    END TRY
    BEGIN CATCH
        SELECT 0 AS respuesta;
    END CATCH
END
GO

/*
Ejemplo de creacion de tu primer administrador:

EXEC dbo.InsertInto_UsuarioAdmin
    @nombreUsuario = 'EMILIANO',
    @loginUsuario = 'admin',
    @passwordPlano = 'CambiaEstaClaveSegura2026!';
*/
