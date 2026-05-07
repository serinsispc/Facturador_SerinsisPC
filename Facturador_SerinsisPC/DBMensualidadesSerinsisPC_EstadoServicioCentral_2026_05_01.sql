USE [DBMensualidadesSerinsisPC];
GO

SET NOCOUNT ON;
GO

/* =========================================================
   Tabla central para que cada POS consulte el estado real
   0 = AL DIA
   1 = AVISO
   2 = SUSPENDIDO
   ========================================================= */
IF OBJECT_ID('dbo.EstadoServicioBaseDatos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.EstadoServicioBaseDatos
    (
        id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        idCliente INT NOT NULL,
        idDataBase INT NOT NULL,
        nombreDataBase VARCHAR(50) NOT NULL,
        estadoServicio INT NOT NULL,
        mensajeEstado VARCHAR(500) NOT NULL,
        saldoPendiente DECIMAL(18,2) NOT NULL,
        fechaUltimoPago DATE NULL,
        fechaProximoPago DATE NULL,
        fechaLimiteAviso DATE NULL,
        fechaLimiteSuspension DATE NULL,
        fechaEstado DATETIME NOT NULL,
        ultimaSincronizacion DATETIME NOT NULL,
        origenActualizacion VARCHAR(50) NOT NULL,
        observacionInterna VARCHAR(250) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_EstadoServicioBaseDatos_Clientes')
BEGIN
    ALTER TABLE dbo.EstadoServicioBaseDatos
    ADD CONSTRAINT FK_EstadoServicioBaseDatos_Clientes
        FOREIGN KEY (idCliente) REFERENCES dbo.Clientes(id);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_EstadoServicioBaseDatos_DatBases')
BEGIN
    ALTER TABLE dbo.EstadoServicioBaseDatos
    ADD CONSTRAINT FK_EstadoServicioBaseDatos_DatBases
        FOREIGN KEY (idDataBase) REFERENCES dbo.DatBases(id);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_EstadoServicioBaseDatos_idDataBase')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_EstadoServicioBaseDatos_idDataBase
        ON dbo.EstadoServicioBaseDatos(idDataBase);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_EstadoServicioBaseDatos_estadoServicio')
BEGIN
    ALTER TABLE dbo.EstadoServicioBaseDatos
    ADD CONSTRAINT CK_EstadoServicioBaseDatos_estadoServicio
        CHECK (estadoServicio IN (0, 1, 2));
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_EstadoServicioBaseDatos_estadoServicio')
BEGIN
    ALTER TABLE dbo.EstadoServicioBaseDatos
    ADD CONSTRAINT DF_EstadoServicioBaseDatos_estadoServicio DEFAULT (0) FOR estadoServicio;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_EstadoServicioBaseDatos_mensajeEstado')
BEGIN
    ALTER TABLE dbo.EstadoServicioBaseDatos
    ADD CONSTRAINT DF_EstadoServicioBaseDatos_mensajeEstado DEFAULT ('SERVICIO ACTIVO Y AL DIA.') FOR mensajeEstado;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_EstadoServicioBaseDatos_saldoPendiente')
BEGIN
    ALTER TABLE dbo.EstadoServicioBaseDatos
    ADD CONSTRAINT DF_EstadoServicioBaseDatos_saldoPendiente DEFAULT (0) FOR saldoPendiente;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_EstadoServicioBaseDatos_fechaEstado')
BEGIN
    ALTER TABLE dbo.EstadoServicioBaseDatos
    ADD CONSTRAINT DF_EstadoServicioBaseDatos_fechaEstado DEFAULT (GETDATE()) FOR fechaEstado;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_EstadoServicioBaseDatos_ultimaSincronizacion')
BEGIN
    ALTER TABLE dbo.EstadoServicioBaseDatos
    ADD CONSTRAINT DF_EstadoServicioBaseDatos_ultimaSincronizacion DEFAULT (GETDATE()) FOR ultimaSincronizacion;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_EstadoServicioBaseDatos_origenActualizacion')
BEGIN
    ALTER TABLE dbo.EstadoServicioBaseDatos
    ADD CONSTRAINT DF_EstadoServicioBaseDatos_origenActualizacion DEFAULT ('SISTEMA') FOR origenActualizacion;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_EstadoServicioBaseDatos_observacionInterna')
BEGIN
    ALTER TABLE dbo.EstadoServicioBaseDatos
    ADD CONSTRAINT DF_EstadoServicioBaseDatos_observacionInterna DEFAULT ('') FOR observacionInterna;
END
GO

/* =========================================================
   Vista lista para consumo del POS
   ========================================================= */
CREATE OR ALTER VIEW dbo.V_EstadoServicioPOS
AS
SELECT
    es.id,
    es.idCliente,
    es.idDataBase,
    es.nombreDataBase,
    es.estadoServicio,
    CASE es.estadoServicio
        WHEN 0 THEN 'AL_DIA'
        WHEN 1 THEN 'AVISO'
        WHEN 2 THEN 'SUSPENDIDO'
        ELSE 'DESCONOCIDO'
    END AS nombreEstadoServicio,
    es.mensajeEstado,
    es.saldoPendiente,
    es.fechaUltimoPago,
    es.fechaProximoPago,
    es.fechaLimiteAviso,
    es.fechaLimiteSuspension,
    es.fechaEstado,
    es.ultimaSincronizacion,
    c.nombreComercial,
    c.nombreRepresentate,
    c.celular,
    c.correo
FROM dbo.EstadoServicioBaseDatos es
INNER JOIN dbo.Clientes c ON c.id = es.idCliente;
GO

/* =========================================================
   Sincroniza una base puntual
   ========================================================= */
CREATE OR ALTER PROCEDURE dbo.sp_SincronizarEstadoServicioBase
    @idDataBase INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE
        @idCliente INT,
        @nombreDataBase VARCHAR(50),
        @estadoCliente INT,
        @saldoPendiente DECIMAL(18,2),
        @fechaUltimoPago DATE,
        @fechaProximoPago DATE,
        @diasGraciaSuspension INT,
        @fechaLimiteAviso DATE,
        @fechaLimiteSuspension DATE,
        @mensajeEstado VARCHAR(500);

    SELECT
        @idCliente = db.idCliente,
        @nombreDataBase = db.nameDataBase,
        @estadoCliente = ISNULL(c.estado, 0),
        @saldoPendiente = ISNULL(vcp.saldoPendienteTotal, 0),
        @fechaUltimoPago = c.fechaUltimoPago,
        @fechaProximoPago = c.fechaProximoPago,
        @diasGraciaSuspension = CASE WHEN ISNULL(c.diasGraciaSuspension, 0) <= 0 THEN 5 ELSE c.diasGraciaSuspension END,
        @fechaLimiteAviso = CASE WHEN c.fechaProximoPago IS NULL THEN NULL ELSE DATEADD(DAY, 1, c.fechaProximoPago) END,
        @fechaLimiteSuspension = CASE WHEN c.fechaProximoPago IS NULL THEN NULL ELSE DATEADD(DAY, CASE WHEN ISNULL(c.diasGraciaSuspension, 0) <= 0 THEN 5 ELSE c.diasGraciaSuspension END, c.fechaProximoPago) END
    FROM dbo.DatBases db
    INNER JOIN dbo.Clientes c ON c.id = db.idCliente
    LEFT JOIN dbo.V_ControlPagosClientes vcp ON vcp.id = c.id
    WHERE db.id = @idDataBase;

    IF @idCliente IS NULL
        RETURN;

    SET @mensajeEstado =
        CASE @estadoCliente
            WHEN 0 THEN
                'SERVICIO ACTIVO Y AL DIA. PUEDE CONTINUAR USANDO EL SISTEMA CON NORMALIDAD.'
            WHEN 1 THEN
                'AVISO DE PAGO PENDIENTE. SU SERVICIO PRESENTA MORA. POR FAVOR REALICE EL PAGO PARA EVITAR SUSPENSION.'
            WHEN 2 THEN
                'SERVICIO SUSPENDIDO POR FALTA DE PAGO. COMUNIQUESE CON SERINSIS PC Y REGISTRE SU PAGO PARA REACTIVAR EL SISTEMA.'
            ELSE
                'ESTADO DE SERVICIO NO CONFIGURADO.'
        END;

    MERGE dbo.EstadoServicioBaseDatos AS target
    USING
    (
        SELECT
            @idCliente AS idCliente,
            @idDataBase AS idDataBase,
            @nombreDataBase AS nombreDataBase
    ) AS source
    ON target.idDataBase = source.idDataBase
    WHEN MATCHED THEN
        UPDATE SET
            target.idCliente = @idCliente,
            target.nombreDataBase = @nombreDataBase,
            target.estadoServicio = @estadoCliente,
            target.mensajeEstado = @mensajeEstado,
            target.saldoPendiente = ISNULL(@saldoPendiente, 0),
            target.fechaUltimoPago = @fechaUltimoPago,
            target.fechaProximoPago = @fechaProximoPago,
            target.fechaLimiteAviso = @fechaLimiteAviso,
            target.fechaLimiteSuspension = @fechaLimiteSuspension,
            target.fechaEstado = GETDATE(),
            target.ultimaSincronizacion = GETDATE(),
            target.origenActualizacion = 'SISTEMA',
            target.observacionInterna = ''
    WHEN NOT MATCHED THEN
        INSERT
        (
            idCliente, idDataBase, nombreDataBase, estadoServicio, mensajeEstado,
            saldoPendiente, fechaUltimoPago, fechaProximoPago, fechaLimiteAviso,
            fechaLimiteSuspension, fechaEstado, ultimaSincronizacion, origenActualizacion, observacionInterna
        )
        VALUES
        (
            @idCliente, @idDataBase, @nombreDataBase, @estadoCliente, @mensajeEstado,
            ISNULL(@saldoPendiente, 0), @fechaUltimoPago, @fechaProximoPago, @fechaLimiteAviso,
            @fechaLimiteSuspension, GETDATE(), GETDATE(), 'SISTEMA', ''
        );
END
GO

/* =========================================================
   Sincroniza todas las bases registradas
   ========================================================= */
CREATE OR ALTER PROCEDURE dbo.sp_SincronizarEstadoServicioBases
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idDataBase INT;

    DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
        SELECT id
        FROM dbo.DatBases;

    OPEN cur;
    FETCH NEXT FROM cur INTO @idDataBase;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC dbo.sp_SincronizarEstadoServicioBase @idDataBase = @idDataBase;
        FETCH NEXT FROM cur INTO @idDataBase;
    END

    CLOSE cur;
    DEALLOCATE cur;
END
GO

/* =========================================================
   Permite actualizar manualmente el estado y mensaje central
   ========================================================= */
CREATE OR ALTER PROCEDURE dbo.sp_ActualizarEstadoServicioBaseManual
    @idDataBase INT,
    @estadoServicio INT,
    @mensajeEstado VARCHAR(500),
    @origenActualizacion VARCHAR(50) = 'MANUAL'
AS
BEGIN
    SET NOCOUNT ON;

    IF @estadoServicio NOT IN (0, 1, 2)
    BEGIN
        RAISERROR('El estadoServicio debe ser 0, 1 o 2.', 16, 1);
        RETURN;
    END

    UPDATE dbo.EstadoServicioBaseDatos
    SET
        estadoServicio = @estadoServicio,
        mensajeEstado = ISNULL(NULLIF(LTRIM(RTRIM(@mensajeEstado)), ''), mensajeEstado),
        fechaEstado = GETDATE(),
        ultimaSincronizacion = GETDATE(),
        origenActualizacion = ISNULL(NULLIF(LTRIM(RTRIM(@origenActualizacion)), ''), 'MANUAL')
    WHERE idDataBase = @idDataBase;
END
GO

/* =========================================================
   Inicialización rápida
   ========================================================= */
EXEC dbo.sp_SincronizarEstadoServicioBases;
GO
