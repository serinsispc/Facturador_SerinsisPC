USE [DBMensualidadesSerinsisPC];
GO

IF COL_LENGTH('dbo.UsuarioAdmin', 'whatsApp') IS NULL
BEGIN
    ALTER TABLE dbo.UsuarioAdmin
    ADD whatsApp VARCHAR(20) NULL;
END
GO

CREATE OR ALTER VIEW dbo.V_UsuarioAdmin
AS
SELECT
    id,
    nombreUsuario,
    loginUsuario,
    whatsApp,
    estado,
    fechaCreacion,
    fechaUltimoAcceso
FROM dbo.UsuarioAdmin;
GO

CREATE OR ALTER PROCEDURE dbo.InsertInto_UsuarioAdmin
    @nombreUsuario VARCHAR(100),
    @loginUsuario VARCHAR(60),
    @passwordPlano VARCHAR(100),
    @whatsApp VARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @salt VARCHAR(64) =
            CONVERT(VARCHAR(64), NEWID()) + CONVERT(VARCHAR(32), ABS(CHECKSUM(NEWID())));

        DECLARE @hash VARCHAR(64) =
            CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @salt + @passwordPlano), 2);

        INSERT INTO dbo.UsuarioAdmin
        (
            nombreUsuario,
            loginUsuario,
            passwordSalt,
            passwordHash,
            whatsApp,
            estado
        )
        VALUES
        (
            UPPER(LTRIM(RTRIM(@nombreUsuario))),
            LOWER(LTRIM(RTRIM(@loginUsuario))),
            @salt,
            @hash,
            LTRIM(RTRIM(@whatsApp)),
            1
        );

        SELECT 1 AS respuesta, CONVERT(INT, SCOPE_IDENTITY()) AS nuevoId;
    END TRY
    BEGIN CATCH
        SELECT 0 AS respuesta, 0 AS nuevoId;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.Consultar_UsuarioAdmin_Recuperacion
    @loginUsuario VARCHAR(60),
    @whatsApp VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        id,
        nombreUsuario,
        loginUsuario,
        whatsApp,
        estado
    FROM dbo.UsuarioAdmin
    WHERE loginUsuario = LOWER(LTRIM(RTRIM(@loginUsuario)))
      AND whatsApp = LTRIM(RTRIM(@whatsApp))
      AND estado = 1;
END
GO

IF OBJECT_ID('dbo.TokenRecuperacionAdmin', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.TokenRecuperacionAdmin
    (
        id INT IDENTITY(1,1) PRIMARY KEY,
        idUsuarioAdmin INT NOT NULL,
        codigoHash VARCHAR(64) NOT NULL,
        fechaCreacion DATETIME NOT NULL CONSTRAINT DF_TokenRecuperacionAdmin_fechaCreacion DEFAULT (GETDATE()),
        fechaExpiracion DATETIME NOT NULL,
        usado BIT NOT NULL CONSTRAINT DF_TokenRecuperacionAdmin_usado DEFAULT (0),
        fechaUso DATETIME NULL,
        intentosFallidos INT NOT NULL CONSTRAINT DF_TokenRecuperacionAdmin_intentosFallidos DEFAULT (0)
    );

    ALTER TABLE dbo.TokenRecuperacionAdmin
    ADD CONSTRAINT FK_TokenRecuperacionAdmin_UsuarioAdmin
    FOREIGN KEY (idUsuarioAdmin) REFERENCES dbo.UsuarioAdmin(id);
END
GO

CREATE OR ALTER PROCEDURE dbo.GenerarTokenRecuperacion_UsuarioAdmin
    @idUsuarioAdmin INT,
    @codigoPlano VARCHAR(20),
    @minutosVigencia INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE dbo.TokenRecuperacionAdmin
        SET usado = 1,
            fechaUso = GETDATE()
        WHERE idUsuarioAdmin = @idUsuarioAdmin
          AND usado = 0;

        INSERT INTO dbo.TokenRecuperacionAdmin
        (
            idUsuarioAdmin,
            codigoHash,
            fechaExpiracion
        )
        VALUES
        (
            @idUsuarioAdmin,
            CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', LTRIM(RTRIM(@codigoPlano))), 2),
            DATEADD(MINUTE, @minutosVigencia, GETDATE())
        );

        SELECT 1 AS respuesta, CONVERT(INT, SCOPE_IDENTITY()) AS nuevoId;
    END TRY
    BEGIN CATCH
        SELECT 0 AS respuesta, 0 AS nuevoId;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.ConfirmarRecuperacion_UsuarioAdmin
    @loginUsuario VARCHAR(60),
    @whatsApp VARCHAR(20),
    @codigoPlano VARCHAR(20),
    @passwordNueva VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        DECLARE @idUsuarioAdmin INT;
        DECLARE @idToken INT;
        DECLARE @hashEsperado VARCHAR(64);
        DECLARE @salt VARCHAR(64);
        DECLARE @hashNuevaClave VARCHAR(64);

        SELECT TOP 1
            @idUsuarioAdmin = id
        FROM dbo.UsuarioAdmin
        WHERE loginUsuario = LOWER(LTRIM(RTRIM(@loginUsuario)))
          AND whatsApp = LTRIM(RTRIM(@whatsApp))
          AND estado = 1;

        IF @idUsuarioAdmin IS NULL
        BEGIN
            SELECT 0 AS respuesta, 0 AS nuevoId;
            RETURN;
        END

        SELECT TOP 1
            @idToken = id,
            @hashEsperado = codigoHash
        FROM dbo.TokenRecuperacionAdmin
        WHERE idUsuarioAdmin = @idUsuarioAdmin
          AND usado = 0
          AND fechaExpiracion >= GETDATE()
          AND intentosFallidos < 5
        ORDER BY id DESC;

        IF @idToken IS NULL
        BEGIN
            SELECT 0 AS respuesta, 0 AS nuevoId;
            RETURN;
        END

        IF @hashEsperado <> CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', LTRIM(RTRIM(@codigoPlano))), 2)
        BEGIN
            UPDATE dbo.TokenRecuperacionAdmin
            SET intentosFallidos = intentosFallidos + 1
            WHERE id = @idToken;

            SELECT 0 AS respuesta, 0 AS nuevoId;
            RETURN;
        END

        SET @salt = CONVERT(VARCHAR(64), NEWID()) + CONVERT(VARCHAR(32), ABS(CHECKSUM(NEWID())));
        SET @hashNuevaClave = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', @salt + @passwordNueva), 2);

        UPDATE dbo.UsuarioAdmin
        SET passwordSalt = @salt,
            passwordHash = @hashNuevaClave
        WHERE id = @idUsuarioAdmin;

        UPDATE dbo.TokenRecuperacionAdmin
        SET usado = 1,
            fechaUso = GETDATE()
        WHERE id = @idToken;

        SELECT 1 AS respuesta, @idUsuarioAdmin AS nuevoId;
    END TRY
    BEGIN CATCH
        SELECT 0 AS respuesta, 0 AS nuevoId;
    END CATCH
END
GO
