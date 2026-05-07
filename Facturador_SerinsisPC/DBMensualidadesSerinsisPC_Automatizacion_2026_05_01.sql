USE [DBMensualidadesSerinsisPC];
GO

SET NOCOUNT ON;
GO

/* =========================================================
   1. Catálogo de estados del servicio del cliente
   0 = Al día
   1 = Aviso
   2 = Suspendido
   ========================================================= */
IF OBJECT_ID('dbo.EstadoServicioCliente', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.EstadoServicioCliente
    (
        id INT NOT NULL PRIMARY KEY,
        nombreEstado VARCHAR(30) NOT NULL,
        descripcion VARCHAR(150) NOT NULL
    );
END
GO

MERGE dbo.EstadoServicioCliente AS target
USING
(
    SELECT 0 AS id, 'AL_DIA' AS nombreEstado, 'Cliente al día y con servicio habilitado' AS descripcion
    UNION ALL
    SELECT 1, 'AVISO', 'Cliente vencido en etapa de aviso'
    UNION ALL
    SELECT 2, 'SUSPENDIDO', 'Cliente vencido y con servicio suspendido'
) AS source
ON target.id = source.id
WHEN MATCHED THEN
    UPDATE SET
        nombreEstado = source.nombreEstado,
        descripcion = source.descripcion
WHEN NOT MATCHED THEN
    INSERT (id, nombreEstado, descripcion)
    VALUES (source.id, source.nombreEstado, source.descripcion);
GO

/* =========================================================
   2. Enriquecimiento de clientes para automatización
   ========================================================= */
IF COL_LENGTH('dbo.Clientes', 'cobroAutomatico') IS NULL
    ALTER TABLE dbo.Clientes ADD cobroAutomatico BIT NULL;
GO
IF COL_LENGTH('dbo.Clientes', 'facturacionAutomatica') IS NULL
    ALTER TABLE dbo.Clientes ADD facturacionAutomatica BIT NULL;
GO
IF COL_LENGTH('dbo.Clientes', 'diasGraciaSuspension') IS NULL
    ALTER TABLE dbo.Clientes ADD diasGraciaSuspension INT NULL;
GO
IF COL_LENGTH('dbo.Clientes', 'fechaUltimoCobroEnviado') IS NULL
    ALTER TABLE dbo.Clientes ADD fechaUltimoCobroEnviado DATETIME NULL;
GO
IF COL_LENGTH('dbo.Clientes', 'fechaUltimoAviso') IS NULL
    ALTER TABLE dbo.Clientes ADD fechaUltimoAviso DATETIME NULL;
GO
IF COL_LENGTH('dbo.Clientes', 'fechaUltimaSuspension') IS NULL
    ALTER TABLE dbo.Clientes ADD fechaUltimaSuspension DATETIME NULL;
GO
IF COL_LENGTH('dbo.Clientes', 'fechaUltimaReactivacion') IS NULL
    ALTER TABLE dbo.Clientes ADD fechaUltimaReactivacion DATETIME NULL;
GO
IF COL_LENGTH('dbo.Clientes', 'observacionEstadoServicio') IS NULL
    ALTER TABLE dbo.Clientes ADD observacionEstadoServicio VARCHAR(250) NULL;
GO

UPDATE dbo.Clientes
SET
    estado = CASE WHEN estado IS NULL OR estado NOT IN (0, 1, 2) THEN 0 ELSE estado END,
    cobroAutomatico = ISNULL(cobroAutomatico, 1),
    facturacionAutomatica = ISNULL(facturacionAutomatica, 1),
    diasGraciaSuspension = CASE WHEN ISNULL(diasGraciaSuspension, 0) <= 0 THEN 5 ELSE diasGraciaSuspension END,
    observacionEstadoServicio = ISNULL(observacionEstadoServicio, '');
GO

ALTER TABLE dbo.Clientes ALTER COLUMN estado INT NOT NULL;
GO
ALTER TABLE dbo.Clientes ALTER COLUMN cobroAutomatico BIT NOT NULL;
GO
ALTER TABLE dbo.Clientes ALTER COLUMN facturacionAutomatica BIT NOT NULL;
GO
ALTER TABLE dbo.Clientes ALTER COLUMN diasGraciaSuspension INT NOT NULL;
GO
ALTER TABLE dbo.Clientes ALTER COLUMN observacionEstadoServicio VARCHAR(250) NOT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_Clientes_estado_servicio')
BEGIN
    ALTER TABLE dbo.Clientes
    ADD CONSTRAINT DF_Clientes_estado_servicio DEFAULT (0) FOR estado;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_Clientes_cobroAutomatico')
BEGIN
    ALTER TABLE dbo.Clientes
    ADD CONSTRAINT DF_Clientes_cobroAutomatico DEFAULT (1) FOR cobroAutomatico;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_Clientes_facturacionAutomatica')
BEGIN
    ALTER TABLE dbo.Clientes
    ADD CONSTRAINT DF_Clientes_facturacionAutomatica DEFAULT (1) FOR facturacionAutomatica;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_Clientes_diasGraciaSuspension')
BEGIN
    ALTER TABLE dbo.Clientes
    ADD CONSTRAINT DF_Clientes_diasGraciaSuspension DEFAULT (5) FOR diasGraciaSuspension;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_Clientes_observacionEstadoServicio')
BEGIN
    ALTER TABLE dbo.Clientes
    ADD CONSTRAINT DF_Clientes_observacionEstadoServicio DEFAULT ('') FOR observacionEstadoServicio;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Clientes_EstadoServicioCliente')
BEGIN
    ALTER TABLE dbo.Clientes
    ADD CONSTRAINT FK_Clientes_EstadoServicioCliente
        FOREIGN KEY (estado) REFERENCES dbo.EstadoServicioCliente(id);
END
GO

/* =========================================================
   3. Programación de ciclos de cobro
   ========================================================= */
IF OBJECT_ID('dbo.ProgramacionCobro', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProgramacionCobro
    (
        id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        idCliente INT NOT NULL,
        fechaProgramada DATE NOT NULL,
        periodoDesde DATE NOT NULL,
        periodoHasta DATE NOT NULL,
        periodicidadMeses INT NOT NULL,
        valorProgramado DECIMAL(18,2) NOT NULL,
        estadoProgramacion VARCHAR(30) NOT NULL,
        idFactura INT NULL,
        fechaFacturaGenerada DATETIME NULL,
        fechaCobroEnviado DATETIME NULL,
        fechaAviso DATETIME NULL,
        fechaSuspension DATETIME NULL,
        fechaPagoConfirmado DATETIME NULL,
        intentosCobro INT NOT NULL,
        mensajeUltimoProceso VARCHAR(250) NOT NULL,
        fechaCreacion DATETIME NOT NULL,
        fechaActualizacion DATETIME NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_ProgramacionCobro_estadoProgramacion')
BEGIN
    ALTER TABLE dbo.ProgramacionCobro
    ADD CONSTRAINT DF_ProgramacionCobro_estadoProgramacion DEFAULT ('PROGRAMADO') FOR estadoProgramacion;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_ProgramacionCobro_intentosCobro')
BEGIN
    ALTER TABLE dbo.ProgramacionCobro
    ADD CONSTRAINT DF_ProgramacionCobro_intentosCobro DEFAULT (0) FOR intentosCobro;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_ProgramacionCobro_mensajeUltimoProceso')
BEGIN
    ALTER TABLE dbo.ProgramacionCobro
    ADD CONSTRAINT DF_ProgramacionCobro_mensajeUltimoProceso DEFAULT ('') FOR mensajeUltimoProceso;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_ProgramacionCobro_fechaCreacion')
BEGIN
    ALTER TABLE dbo.ProgramacionCobro
    ADD CONSTRAINT DF_ProgramacionCobro_fechaCreacion DEFAULT (GETDATE()) FOR fechaCreacion;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_ProgramacionCobro_fechaActualizacion')
BEGIN
    ALTER TABLE dbo.ProgramacionCobro
    ADD CONSTRAINT DF_ProgramacionCobro_fechaActualizacion DEFAULT (GETDATE()) FOR fechaActualizacion;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_ProgramacionCobro_Clientes')
BEGIN
    ALTER TABLE dbo.ProgramacionCobro
    ADD CONSTRAINT FK_ProgramacionCobro_Clientes
        FOREIGN KEY (idCliente) REFERENCES dbo.Clientes(id);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_ProgramacionCobro_Facturas')
BEGIN
    ALTER TABLE dbo.ProgramacionCobro
    ADD CONSTRAINT FK_ProgramacionCobro_Facturas
        FOREIGN KEY (idFactura) REFERENCES dbo.Facturas(id);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_ProgramacionCobro_Estado')
BEGIN
    ALTER TABLE dbo.ProgramacionCobro
    ADD CONSTRAINT CK_ProgramacionCobro_Estado
    CHECK (estadoProgramacion IN ('PROGRAMADO', 'FACTURADO', 'COBRO_ENVIADO', 'AVISO', 'SUSPENDIDO', 'PAGO_PARCIAL', 'PAGADO', 'CANCELADO'));
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UX_ProgramacionCobro_Cliente_Fecha')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_ProgramacionCobro_Cliente_Fecha
        ON dbo.ProgramacionCobro(idCliente, fechaProgramada);
END
GO

/* =========================================================
   4. Bitácoras operativas
   ========================================================= */
IF OBJECT_ID('dbo.BitacoraProcesoAutomatico', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.BitacoraProcesoAutomatico
    (
        id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        nombreProceso VARCHAR(80) NOT NULL,
        fechaEjecucion DATETIME NOT NULL,
        totalProcesados INT NOT NULL,
        totalExitosos INT NOT NULL,
        totalErrores INT NOT NULL,
        detalle VARCHAR(500) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_BitacoraProcesoAutomatico_fechaEjecucion')
BEGIN
    ALTER TABLE dbo.BitacoraProcesoAutomatico
    ADD CONSTRAINT DF_BitacoraProcesoAutomatico_fechaEjecucion DEFAULT (GETDATE()) FOR fechaEjecucion;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_BitacoraProcesoAutomatico_totalProcesados')
BEGIN
    ALTER TABLE dbo.BitacoraProcesoAutomatico
    ADD CONSTRAINT DF_BitacoraProcesoAutomatico_totalProcesados DEFAULT (0) FOR totalProcesados;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_BitacoraProcesoAutomatico_totalExitosos')
BEGIN
    ALTER TABLE dbo.BitacoraProcesoAutomatico
    ADD CONSTRAINT DF_BitacoraProcesoAutomatico_totalExitosos DEFAULT (0) FOR totalExitosos;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_BitacoraProcesoAutomatico_totalErrores')
BEGIN
    ALTER TABLE dbo.BitacoraProcesoAutomatico
    ADD CONSTRAINT DF_BitacoraProcesoAutomatico_totalErrores DEFAULT (0) FOR totalErrores;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_BitacoraProcesoAutomatico_detalle')
BEGIN
    ALTER TABLE dbo.BitacoraProcesoAutomatico
    ADD CONSTRAINT DF_BitacoraProcesoAutomatico_detalle DEFAULT ('') FOR detalle;
END
GO

IF OBJECT_ID('dbo.HistorialEstadoCliente', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.HistorialEstadoCliente
    (
        id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        idCliente INT NOT NULL,
        estadoAnterior INT NOT NULL,
        estadoNuevo INT NOT NULL,
        motivo VARCHAR(200) NOT NULL,
        fechaCambio DATETIME NOT NULL,
        usuarioProceso VARCHAR(100) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_HistorialEstadoCliente_Clientes')
BEGIN
    ALTER TABLE dbo.HistorialEstadoCliente
    ADD CONSTRAINT FK_HistorialEstadoCliente_Clientes
        FOREIGN KEY (idCliente) REFERENCES dbo.Clientes(id);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_HistorialEstadoCliente_fechaCambio')
BEGIN
    ALTER TABLE dbo.HistorialEstadoCliente
    ADD CONSTRAINT DF_HistorialEstadoCliente_fechaCambio DEFAULT (GETDATE()) FOR fechaCambio;
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.default_constraints WHERE name = 'DF_HistorialEstadoCliente_usuarioProceso')
BEGIN
    ALTER TABLE dbo.HistorialEstadoCliente
    ADD CONSTRAINT DF_HistorialEstadoCliente_usuarioProceso DEFAULT ('SISTEMA') FOR usuarioProceso;
END
GO

/* =========================================================
   5. Vistas operativas y de ingresos
   ========================================================= */
CREATE OR ALTER VIEW dbo.V_ClientesOperacionCobro
AS
SELECT
    c.id,
    c.nombreComercial,
    c.nombreRepresentate,
    c.celular,
    c.correo,
    tp.nombrePlan,
    tp.periodicidadMeses,
    esc.nombreEstado AS nombreEstadoServicio,
    c.estado AS idEstadoServicio,
    c.diaPago,
    c.fechaInicioPlan,
    c.fechaUltimoPago,
    c.fechaProximoPago,
    c.cobroAutomatico,
    c.facturacionAutomatica,
    c.diasGraciaSuspension,
    ISNULL(pc.estadoProgramacion, 'SIN_PROGRAMACION') AS estadoProgramacion,
    pc.fechaProgramada,
    pc.periodoDesde,
    pc.periodoHasta,
    pc.idFactura,
    ISNULL(vcp.saldoPendienteTotal, 0) AS saldoPendienteTotal,
    vcp.proximoVencimiento
FROM dbo.Clientes c
INNER JOIN dbo.TipoPlan tp ON tp.id = c.idTipoPlan
INNER JOIN dbo.EstadoServicioCliente esc ON esc.id = c.estado
LEFT JOIN dbo.V_ControlPagosClientes vcp ON vcp.id = c.id
LEFT JOIN
(
    SELECT *
    FROM
    (
        SELECT
            pc.*,
            ROW_NUMBER() OVER (PARTITION BY pc.idCliente ORDER BY pc.fechaProgramada DESC, pc.id DESC DESC) AS rn
        FROM dbo.ProgramacionCobro pc
        WHERE pc.estadoProgramacion <> 'CANCELADO'
    ) x
    WHERE x.rn = 1
) pc ON pc.idCliente = c.id;
GO

CREATE OR ALTER VIEW dbo.V_ClientesParaFacturarHoy
AS
SELECT *
FROM dbo.V_ClientesOperacionCobro
WHERE facturacionAutomatica = 1
  AND cobroAutomatico = 1
  AND fechaProgramada = CAST(GETDATE() AS DATE)
  AND idFactura IS NULL
  AND estadoProgramacion = 'PROGRAMADO';
GO

CREATE OR ALTER VIEW dbo.V_ClientesParaCobrarHoy
AS
SELECT *
FROM dbo.V_ClientesOperacionCobro
WHERE cobroAutomatico = 1
  AND idFactura IS NOT NULL
  AND estadoProgramacion = 'FACTURADO';
GO

CREATE OR ALTER VIEW dbo.V_ClientesParaAviso
AS
SELECT *
FROM dbo.V_ClientesOperacionCobro
WHERE saldoPendienteTotal > 0
  AND idEstadoServicio = 0
  AND proximoVencimiento IS NOT NULL
  AND CAST(GETDATE() AS DATE) > proximoVencimiento
  AND DATEDIFF(DAY, proximoVencimiento, CAST(GETDATE() AS DATE)) BETWEEN 1 AND 4;
GO

CREATE OR ALTER VIEW dbo.V_ClientesParaSuspender
AS
SELECT *
FROM dbo.V_ClientesOperacionCobro
WHERE saldoPendienteTotal > 0
  AND idEstadoServicio <> 2
  AND proximoVencimiento IS NOT NULL
  AND DATEDIFF(DAY, proximoVencimiento, CAST(GETDATE() AS DATE)) >= diasGraciaSuspension;
GO

CREATE OR ALTER VIEW dbo.V_ResumenIngresosMensuales
AS
SELECT
    YEAR(pr.fechaPago) AS anio,
    MONTH(pr.fechaPago) AS mes,
    COUNT(DISTINCT pr.id) AS cantidadPagos,
    COUNT(DISTINCT pr.idCliente) AS clientesQuePagaron,
    SUM(pr.valorRecibido) AS totalIngresos,
    AVG(CAST(pr.valorRecibido AS DECIMAL(18,2))) AS promedioPago
FROM dbo.PagosRecibidos pr
GROUP BY YEAR(pr.fechaPago), MONTH(pr.fechaPago);
GO

CREATE OR ALTER VIEW dbo.V_CarteraMensual
AS
SELECT
    f.yearFactura AS anio,
    f.idMes AS mes,
    COUNT(*) AS cantidadFacturas,
    SUM(f.valorAPagar) AS totalFacturado,
    SUM(f.saldoPendiente) AS totalPendiente,
    SUM(CASE WHEN f.saldoPendiente <= 0 THEN f.valorAPagar ELSE 0 END) AS totalRecaudadoPorFacturasPagas
FROM dbo.Facturas f
GROUP BY f.yearFactura, f.idMes;
GO

/* =========================================================
   6. Procedimientos operativos
   ========================================================= */
CREATE OR ALTER PROCEDURE dbo.sp_ProgramarSiguienteCobroCliente
    @idCliente INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @fechaBase DATE,
            @diaPago INT,
            @periodicidad INT,
            @valorProgramado DECIMAL(18,2),
            @sedes INT,
            @fechaProgramada DATE,
            @periodoDesde DATE,
            @periodoHasta DATE;

    SELECT
        @fechaBase = c.fechaProximoPago,
        @diaPago = c.diaPago,
        @periodicidad = CASE WHEN ISNULL(tp.periodicidadMeses, 0) <= 0 THEN 1 ELSE tp.periodicidadMeses END,
        @valorProgramado = c.valorPlan * c.sedes,
        @sedes = c.sedes
    FROM dbo.Clientes c
    INNER JOIN dbo.TipoPlan tp ON tp.id = c.idTipoPlan
    WHERE c.id = @idCliente;

    IF @fechaBase IS NULL
        RETURN;

    SET @fechaProgramada = @fechaBase;
    SET @periodoHasta = @fechaProgramada;
    SET @periodoDesde = DATEADD(DAY, 1, DATEADD(MONTH, -@periodicidad, @periodoHasta));

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.ProgramacionCobro
        WHERE idCliente = @idCliente
          AND fechaProgramada = @fechaProgramada
    )
    BEGIN
        INSERT INTO dbo.ProgramacionCobro
        (
            idCliente, fechaProgramada, periodoDesde, periodoHasta,
            periodicidadMeses, valorProgramado, estadoProgramacion,
            intentosCobro, mensajeUltimoProceso, fechaCreacion, fechaActualizacion
        )
        VALUES
        (
            @idCliente, @fechaProgramada, @periodoDesde, @periodoHasta,
            @periodicidad, @valorProgramado, 'PROGRAMADO',
            0, '', GETDATE(), GETDATE()
        );
    END
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ReconstruirProgramacionCobros
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idCliente INT;

    DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
        SELECT id
        FROM dbo.Clientes
        WHERE cobroAutomatico = 1;

    OPEN cur;
    FETCH NEXT FROM cur INTO @idCliente;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC dbo.sp_ProgramarSiguienteCobroCliente @idCliente;
        FETCH NEXT FROM cur INTO @idCliente;
    END

    CLOSE cur;
    DEALLOCATE cur;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_ActualizarEstadosServicio
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @cambios TABLE
    (
        idCliente INT,
        estadoAnterior INT,
        estadoNuevo INT,
        motivo VARCHAR(200)
    );

    UPDATE c
    SET
        estado =
            CASE
                WHEN v.proximoVencimiento IS NOT NULL
                 AND DATEDIFF(DAY, v.proximoVencimiento, CAST(GETDATE() AS DATE)) >= c.diasGraciaSuspension
                 AND ISNULL(v.saldoPendienteTotal, 0) > 0 THEN 2
                WHEN v.proximoVencimiento IS NOT NULL
                 AND DATEDIFF(DAY, v.proximoVencimiento, CAST(GETDATE() AS DATE)) BETWEEN 1 AND c.diasGraciaSuspension - 1
                 AND ISNULL(v.saldoPendienteTotal, 0) > 0 THEN 1
                ELSE 0
            END,
        fechaUltimoAviso =
            CASE
                WHEN v.proximoVencimiento IS NOT NULL
                 AND DATEDIFF(DAY, v.proximoVencimiento, CAST(GETDATE() AS DATE)) BETWEEN 1 AND c.diasGraciaSuspension - 1
                 AND ISNULL(v.saldoPendienteTotal, 0) > 0 THEN ISNULL(c.fechaUltimoAviso, GETDATE())
                ELSE c.fechaUltimoAviso
            END,
        fechaUltimaSuspension =
            CASE
                WHEN v.proximoVencimiento IS NOT NULL
                 AND DATEDIFF(DAY, v.proximoVencimiento, CAST(GETDATE() AS DATE)) >= c.diasGraciaSuspension
                 AND ISNULL(v.saldoPendienteTotal, 0) > 0 THEN ISNULL(c.fechaUltimaSuspension, GETDATE())
                ELSE c.fechaUltimaSuspension
            END,
        fechaUltimaReactivacion =
            CASE
                WHEN ISNULL(v.saldoPendienteTotal, 0) <= 0 THEN GETDATE()
                ELSE c.fechaUltimaReactivacion
            END
    OUTPUT
        inserted.id,
        deleted.estado,
        inserted.estado,
        CASE
            WHEN inserted.estado = 2 THEN 'Suspensión automática por mora'
            WHEN inserted.estado = 1 THEN 'Aviso automático por vencimiento'
            ELSE 'Reactivación automática por cartera al día'
        END
    INTO @cambios(idCliente, estadoAnterior, estadoNuevo, motivo)
    FROM dbo.Clientes c
    LEFT JOIN dbo.V_ControlPagosClientes v ON v.id = c.id;

    INSERT INTO dbo.HistorialEstadoCliente
    (
        idCliente, estadoAnterior, estadoNuevo, motivo, fechaCambio, usuarioProceso
    )
    SELECT
        idCliente, estadoAnterior, estadoNuevo, motivo, GETDATE(), 'SISTEMA'
    FROM @cambios
    WHERE estadoAnterior <> estadoNuevo;

    UPDATE db
    SET db.estado = CASE WHEN c.estado = 2 THEN 0 ELSE 1 END
    FROM dbo.DatBases db
    INNER JOIN dbo.Clientes c ON c.id = db.idCliente;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_MarcarProgramacionFacturada
    @idCliente INT,
    @idFactura INT,
    @fechaProgramada DATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.ProgramacionCobro
    SET
        idFactura = @idFactura,
        fechaFacturaGenerada = GETDATE(),
        estadoProgramacion = 'FACTURADO',
        fechaActualizacion = GETDATE(),
        mensajeUltimoProceso = 'Factura generada automáticamente'
    WHERE idCliente = @idCliente
      AND fechaProgramada = @fechaProgramada;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_MarcarProgramacionCobroEnviado
    @idCliente INT,
    @fechaProgramada DATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.ProgramacionCobro
    SET
        fechaCobroEnviado = GETDATE(),
        intentosCobro = intentosCobro + 1,
        estadoProgramacion = 'COBRO_ENVIADO',
        fechaActualizacion = GETDATE(),
        mensajeUltimoProceso = 'Cobro enviado al cliente'
    WHERE idCliente = @idCliente
      AND fechaProgramada = @fechaProgramada;

    UPDATE dbo.Clientes
    SET fechaUltimoCobroEnviado = GETDATE()
    WHERE id = @idCliente;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_RegistrarPagoYReactivarServicio
    @idFactura INT,
    @fechaPago DATETIME,
    @idMetodoPago INT,
    @valorRecibido DECIMAL(18,2),
    @numeroComprobante VARCHAR(100),
    @referenciaPago VARCHAR(100),
    @observacion VARCHAR(250),
    @usuarioRegistro VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @idCliente INT, @valorAplicado DECIMAL(18,2), @fechaProgramada DATE;

    SELECT
        @idCliente = idCliente,
        @valorAplicado = CASE WHEN saldoPendiente > 0 THEN saldoPendiente ELSE valorAPagar END,
        @fechaProgramada = fechaVencimiento
    FROM dbo.Facturas
    WHERE id = @idFactura;

    EXEC dbo.InsertInto_PagoRecibidoFactura
        @fechaPago = @fechaPago,
        @idCliente = @idCliente,
        @idMetodoPago = @idMetodoPago,
        @valorRecibido = @valorRecibido,
        @numeroComprobante = @numeroComprobante,
        @referenciaPago = @referenciaPago,
        @observacion = @observacion,
        @usuarioRegistro = @usuarioRegistro,
        @idFactura = @idFactura,
        @valorAplicado = @valorAplicado;

    UPDATE dbo.Clientes
    SET
        estado = 0,
        fechaUltimaReactivacion = GETDATE(),
        observacionEstadoServicio = 'Reactivado por pago registrado'
    WHERE id = @idCliente;

    UPDATE dbo.DatBases
    SET estado = 1
    WHERE idCliente = @idCliente;

    UPDATE dbo.ProgramacionCobro
    SET
        estadoProgramacion = 'PAGADO',
        fechaPagoConfirmado = GETDATE(),
        fechaActualizacion = GETDATE(),
        mensajeUltimoProceso = 'Pago confirmado y servicio reactivado'
    WHERE idCliente = @idCliente
      AND fechaProgramada = @fechaProgramada;

    EXEC dbo.sp_ProgramarSiguienteCobroCliente @idCliente;
END
GO
