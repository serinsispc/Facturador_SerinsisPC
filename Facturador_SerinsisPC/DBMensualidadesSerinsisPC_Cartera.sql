USE [DBMensualidadesSerinsisPC];
GO

IF COL_LENGTH('dbo.TipoPlan', 'periodicidadMeses') IS NULL
BEGIN
    ALTER TABLE dbo.TipoPlan
    ADD periodicidadMeses INT NOT NULL CONSTRAINT DF_TipoPlan_periodicidadMeses DEFAULT (1);
END
GO

UPDATE dbo.Facturas
SET
    saldoPendiente = 0,
    fechaPagoCompleto = ISNULL(fechaPagoCompleto, CAST(GETDATE() AS DATE)),
    idEstado = 3
WHERE saldoPendiente <= 0 AND idEstado <> 3;
GO

UPDATE f
SET f.saldoPendiente = f.valorAPagar
FROM dbo.Facturas f
INNER JOIN dbo.EstadoMensualidad e ON e.id = f.idEstado
WHERE UPPER(ISNULL(e.nombreEstado, '')) = 'ACTIVA'
  AND ISNULL(f.saldoPendiente, 0) <= 0;
GO

UPDATE dbo.TipoPlan
SET periodicidadMeses =
    CASE UPPER(LTRIM(RTRIM(nombrePlan)))
        WHEN 'MENSUAL' THEN 1
        WHEN 'SEMESTRAL' THEN 6
        WHEN 'ANUAL' THEN 12
        ELSE 1
    END;
GO

IF COL_LENGTH('dbo.Clientes', 'diaPago') IS NULL ALTER TABLE dbo.Clientes ADD diaPago TINYINT NULL;
GO
IF COL_LENGTH('dbo.Clientes', 'fechaInicioPlan') IS NULL ALTER TABLE dbo.Clientes ADD fechaInicioPlan DATE NULL;
GO
IF COL_LENGTH('dbo.Clientes', 'fechaUltimoPago') IS NULL ALTER TABLE dbo.Clientes ADD fechaUltimoPago DATE NULL;
GO
IF COL_LENGTH('dbo.Clientes', 'fechaProximoPago') IS NULL ALTER TABLE dbo.Clientes ADD fechaProximoPago DATE NULL;
GO
IF COL_LENGTH('dbo.Clientes', 'observacionCartera') IS NULL ALTER TABLE dbo.Clientes ADD observacionCartera VARCHAR(250) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_Clientes_diaPago')
BEGIN
    ALTER TABLE dbo.Clientes
    ADD CONSTRAINT CK_Clientes_diaPago CHECK (diaPago IS NULL OR (diaPago BETWEEN 1 AND 31));
END
GO

IF COL_LENGTH('dbo.Facturas', 'fechaVencimiento') IS NULL ALTER TABLE dbo.Facturas ADD fechaVencimiento DATE NULL;
GO
IF COL_LENGTH('dbo.Facturas', 'periodoDesde') IS NULL ALTER TABLE dbo.Facturas ADD periodoDesde DATE NULL;
GO
IF COL_LENGTH('dbo.Facturas', 'periodoHasta') IS NULL ALTER TABLE dbo.Facturas ADD periodoHasta DATE NULL;
GO
IF COL_LENGTH('dbo.Facturas', 'saldoPendiente') IS NULL ALTER TABLE dbo.Facturas ADD saldoPendiente DECIMAL(18,2) NULL;
GO
IF COL_LENGTH('dbo.Facturas', 'fechaPagoCompleto') IS NULL ALTER TABLE dbo.Facturas ADD fechaPagoCompleto DATE NULL;
GO

UPDATE dbo.Facturas SET saldoPendiente = valorAPagar WHERE saldoPendiente IS NULL;
GO

ALTER TABLE dbo.Facturas ALTER COLUMN saldoPendiente DECIMAL(18,2) NOT NULL;
GO

IF OBJECT_ID('dbo.MetodoPago', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.MetodoPago
    (
        id INT IDENTITY(1,1) PRIMARY KEY,
        nombreMetodo VARCHAR(50) NOT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM dbo.MetodoPago WHERE nombreMetodo = 'EFECTIVO') INSERT INTO dbo.MetodoPago(nombreMetodo) VALUES ('EFECTIVO');
IF NOT EXISTS (SELECT 1 FROM dbo.MetodoPago WHERE nombreMetodo = 'TRANSFERENCIA') INSERT INTO dbo.MetodoPago(nombreMetodo) VALUES ('TRANSFERENCIA');
IF NOT EXISTS (SELECT 1 FROM dbo.MetodoPago WHERE nombreMetodo = 'CONSIGNACION') INSERT INTO dbo.MetodoPago(nombreMetodo) VALUES ('CONSIGNACION');
IF NOT EXISTS (SELECT 1 FROM dbo.MetodoPago WHERE nombreMetodo = 'NEQUI') INSERT INTO dbo.MetodoPago(nombreMetodo) VALUES ('NEQUI');
IF NOT EXISTS (SELECT 1 FROM dbo.MetodoPago WHERE nombreMetodo = 'DAVIPLATA') INSERT INTO dbo.MetodoPago(nombreMetodo) VALUES ('DAVIPLATA');
GO

IF OBJECT_ID('dbo.PagosRecibidos', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PagosRecibidos
    (
        id INT IDENTITY(1,1) PRIMARY KEY,
        fechaPago DATETIME NOT NULL,
        idCliente INT NOT NULL,
        idMetodoPago INT NOT NULL,
        valorRecibido DECIMAL(18,2) NOT NULL,
        numeroComprobante VARCHAR(100) NULL,
        referenciaPago VARCHAR(100) NULL,
        observacion VARCHAR(250) NULL,
        usuarioRegistro VARCHAR(100) NULL,
        fechaRegistro DATETIME NOT NULL CONSTRAINT DF_PagosRecibidos_fechaRegistro DEFAULT (GETDATE())
    );

    ALTER TABLE dbo.PagosRecibidos
    ADD CONSTRAINT FK_PagosRecibidos_Clientes FOREIGN KEY (idCliente) REFERENCES dbo.Clientes(id);

    ALTER TABLE dbo.PagosRecibidos
    ADD CONSTRAINT FK_PagosRecibidos_MetodoPago FOREIGN KEY (idMetodoPago) REFERENCES dbo.MetodoPago(id);
END
GO

IF OBJECT_ID('dbo.PagoDetalleFactura', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PagoDetalleFactura
    (
        id INT IDENTITY(1,1) PRIMARY KEY,
        idPagoRecibido INT NOT NULL,
        idFactura INT NOT NULL,
        valorAplicado DECIMAL(18,2) NOT NULL
    );

    ALTER TABLE dbo.PagoDetalleFactura
    ADD CONSTRAINT FK_PagoDetalleFactura_PagosRecibidos FOREIGN KEY (idPagoRecibido) REFERENCES dbo.PagosRecibidos(id);

    ALTER TABLE dbo.PagoDetalleFactura
    ADD CONSTRAINT FK_PagoDetalleFactura_Facturas FOREIGN KEY (idFactura) REFERENCES dbo.Facturas(id);
END
GO

CREATE OR ALTER VIEW dbo.V_Clientes
AS
SELECT
    c.id,
    c.idTipoPlan,
    t.nombrePlan,
    c.nit,
    c.nombreComercial,
    c.nombreRepresentate,
    c.celular,
    c.correo,
    c.sedes,
    c.valorPlan,
    c.estado,
    c.diaPago,
    c.fechaInicioPlan,
    c.fechaUltimoPago,
    c.fechaProximoPago,
    c.observacionCartera,
    t.periodicidadMeses
FROM dbo.Clientes c
INNER JOIN dbo.TipoPlan t ON c.idTipoPlan = t.id;
GO

CREATE OR ALTER VIEW dbo.V_Facturas
AS
SELECT
    f.id,
    f.fechaFactura,
    f.idCliente,
    c.celular,
    c.nombreRepresentate,
    c.nombreComercial,
    c.correo,
    f.yearFactura,
    f.idMes,
    m.nombreMes,
    f.valorPlan,
    f.sedes,
    f.valorAPagar,
    f.idEstado,
    e.nombreEstado,
    f.contador,
    f.fechaVencimiento,
    f.periodoDesde,
    f.periodoHasta,
    f.saldoPendiente,
    f.fechaPagoCompleto
FROM dbo.Facturas f
INNER JOIN dbo.Clientes c ON f.idCliente = c.id
INNER JOIN dbo.Meses m ON f.idMes = m.id
INNER JOIN dbo.EstadoMensualidad e ON f.idEstado = e.id;
GO

CREATE OR ALTER VIEW dbo.V_ControlPagosClientes
AS
SELECT
    c.id,
    c.nombreComercial,
    c.nombreRepresentate,
    c.celular,
    tp.nombrePlan,
    tp.periodicidadMeses,
    c.diaPago,
    c.fechaInicioPlan,
    c.fechaUltimoPago,
    c.fechaProximoPago,
    ISNULL(SUM(CASE WHEN f.idEstado <> 3 THEN f.saldoPendiente ELSE 0 END), 0) AS saldoPendienteTotal,
    MIN(CASE WHEN f.idEstado <> 3 THEN f.fechaVencimiento END) AS proximoVencimiento
FROM dbo.Clientes c
INNER JOIN dbo.TipoPlan tp ON tp.id = c.idTipoPlan
LEFT JOIN dbo.Facturas f ON f.idCliente = c.id
GROUP BY
    c.id, c.nombreComercial, c.nombreRepresentate, c.celular,
    tp.nombrePlan, tp.periodicidadMeses, c.diaPago,
    c.fechaInicioPlan, c.fechaUltimoPago, c.fechaProximoPago;
GO

CREATE OR ALTER VIEW dbo.V_IngresosMensuales
AS
SELECT
    YEAR(fechaPago) AS anio,
    MONTH(fechaPago) AS mes,
    COUNT(*) AS cantidadPagos,
    SUM(valorRecibido) AS totalIngresos
FROM dbo.PagosRecibidos
GROUP BY YEAR(fechaPago), MONTH(fechaPago);
GO

CREATE OR ALTER VIEW dbo.V_PagosRecibidos
AS
SELECT
    pr.id,
    pr.fechaPago,
    c.nombreComercial,
    c.nombreRepresentate,
    mp.nombreMetodo,
    pr.valorRecibido,
    pr.numeroComprobante,
    pr.referenciaPago,
    pr.observacion
FROM dbo.PagosRecibidos pr
INNER JOIN dbo.Clientes c ON c.id = pr.idCliente
INNER JOIN dbo.MetodoPago mp ON mp.id = pr.idMetodoPago;
GO

CREATE OR ALTER PROCEDURE dbo.InsertInto_Clientes
    @idTipoPlan INT,
    @nombreComercial VARCHAR(50),
    @nombreRepresentate VARCHAR(50),
    @celular VARCHAR(10),
    @correo VARCHAR(50),
    @sedes INT,
    @valorPlan DECIMAL(18,2),
    @nit VARCHAR(10),
    @estado INT,
    @diaPago TINYINT = NULL,
    @fechaInicioPlan DATE = NULL,
    @fechaUltimoPago DATE = NULL,
    @fechaProximoPago DATE = NULL,
    @observacionCartera VARCHAR(250) = NULL
AS
BEGIN
    BEGIN TRY
        INSERT INTO dbo.Clientes
        (
            idTipoPlan, nombreComercial, nombreRepresentate, celular, correo,
            sedes, valorPlan, nit, estado, diaPago, fechaInicioPlan,
            fechaUltimoPago, fechaProximoPago, observacionCartera
        )
        VALUES
        (
            @idTipoPlan, @nombreComercial, @nombreRepresentate, @celular, @correo,
            @sedes, @valorPlan, @nit, @estado, @diaPago, @fechaInicioPlan,
            @fechaUltimoPago, @fechaProximoPago, @observacionCartera
        );
        SELECT 1 AS respuesta, CONVERT(INT, SCOPE_IDENTITY()) AS nuevoId;
    END TRY
    BEGIN CATCH
        SELECT 0 AS respuesta, 0 AS nuevoId;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.Update_Clientes
    @id INT,
    @idTipoPlan INT,
    @nombreComercial VARCHAR(50),
    @nombreRepresentate VARCHAR(50),
    @celular VARCHAR(10),
    @correo VARCHAR(50),
    @sedes INT,
    @valorPlan DECIMAL(18,2),
    @nit VARCHAR(10),
    @estado INT,
    @diaPago TINYINT = NULL,
    @fechaInicioPlan DATE = NULL,
    @fechaUltimoPago DATE = NULL,
    @fechaProximoPago DATE = NULL,
    @observacionCartera VARCHAR(250) = NULL
AS
BEGIN
    BEGIN TRY
        UPDATE dbo.Clientes
        SET
            idTipoPlan = @idTipoPlan,
            nombreComercial = @nombreComercial,
            nombreRepresentate = @nombreRepresentate,
            celular = @celular,
            correo = @correo,
            sedes = @sedes,
            valorPlan = @valorPlan,
            nit = @nit,
            estado = @estado,
            diaPago = @diaPago,
            fechaInicioPlan = @fechaInicioPlan,
            fechaUltimoPago = @fechaUltimoPago,
            fechaProximoPago = @fechaProximoPago,
            observacionCartera = @observacionCartera
        WHERE id = @id;
        SELECT 1 AS respuesta;
    END TRY
    BEGIN CATCH
        SELECT 0 AS respuesta;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.InsertInto_Facturas
    @fechaFactura DATE,
    @idCliente INT,
    @idMes INT,
    @valorPlan DECIMAL(18,2),
    @sedes INT,
    @valorAPagar DECIMAL(18,2),
    @idEstado INT,
    @contador INT,
    @yearFactura INT,
    @fechaVencimiento DATE = NULL,
    @periodoDesde DATE = NULL,
    @periodoHasta DATE = NULL,
    @saldoPendiente DECIMAL(18,2) = NULL,
    @fechaPagoCompleto DATE = NULL
AS
BEGIN
    BEGIN TRY
        INSERT INTO dbo.Facturas
        (
            fechaFactura, idCliente, idMes, valorPlan, sedes, valorAPagar,
            idEstado, contador, yearFactura, fechaVencimiento, periodoDesde,
            periodoHasta, saldoPendiente, fechaPagoCompleto
        )
        VALUES
        (
            @fechaFactura, @idCliente, @idMes, @valorPlan, @sedes, @valorAPagar,
            @idEstado, @contador, @yearFactura, @fechaVencimiento, @periodoDesde,
            @periodoHasta, ISNULL(@saldoPendiente, @valorAPagar), @fechaPagoCompleto
        );
        SELECT 1 AS respuesta, CONVERT(INT, SCOPE_IDENTITY()) AS nuevoId;
    END TRY
    BEGIN CATCH
        SELECT 0 AS respuesta, 0 AS nuevoId;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.Update_Facturas
    @id INT,
    @fechaFactura DATE,
    @idCliente INT,
    @idMes INT,
    @valorPlan DECIMAL(18,2),
    @sedes INT,
    @valorAPagar DECIMAL(18,2),
    @idEstado INT,
    @contador INT,
    @yearFactura INT,
    @fechaVencimiento DATE = NULL,
    @periodoDesde DATE = NULL,
    @periodoHasta DATE = NULL,
    @saldoPendiente DECIMAL(18,2) = NULL,
    @fechaPagoCompleto DATE = NULL
AS
BEGIN
    BEGIN TRY
        UPDATE dbo.Facturas
        SET
            fechaFactura = @fechaFactura,
            idCliente = @idCliente,
            idMes = @idMes,
            valorPlan = @valorPlan,
            sedes = @sedes,
            valorAPagar = @valorAPagar,
            idEstado = @idEstado,
            contador = @contador,
            yearFactura = @yearFactura,
            fechaVencimiento = @fechaVencimiento,
            periodoDesde = @periodoDesde,
            periodoHasta = @periodoHasta,
            saldoPendiente = ISNULL(@saldoPendiente, valorAPagar),
            fechaPagoCompleto = @fechaPagoCompleto
        WHERE id = @id;
        SELECT 1 AS respuesta;
    END TRY
    BEGIN CATCH
        SELECT 0 AS respuesta;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE dbo.InsertInto_PagoRecibidoFactura
    @fechaPago DATETIME,
    @idCliente INT,
    @idMetodoPago INT,
    @valorRecibido DECIMAL(18,2),
    @numeroComprobante VARCHAR(100),
    @referenciaPago VARCHAR(100),
    @observacion VARCHAR(250),
    @usuarioRegistro VARCHAR(100),
    @idFactura INT,
    @valorAplicado DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @idPago INT;

        INSERT INTO dbo.PagosRecibidos
        (
            fechaPago, idCliente, idMetodoPago, valorRecibido, numeroComprobante,
            referenciaPago, observacion, usuarioRegistro
        )
        VALUES
        (
            @fechaPago, @idCliente, @idMetodoPago, @valorRecibido, @numeroComprobante,
            @referenciaPago, @observacion, @usuarioRegistro
        );

        SET @idPago = SCOPE_IDENTITY();

        INSERT INTO dbo.PagoDetalleFactura
        (
            idPagoRecibido, idFactura, valorAplicado
        )
        VALUES
        (
            @idPago, @idFactura, @valorAplicado
        );

        UPDATE dbo.Facturas
        SET
            saldoPendiente = CASE WHEN saldoPendiente - @valorAplicado < 0 THEN 0 ELSE saldoPendiente - @valorAplicado END,
            fechaPagoCompleto = CASE WHEN saldoPendiente - @valorAplicado <= 0 THEN CAST(@fechaPago AS DATE) ELSE fechaPagoCompleto END,
            idEstado = CASE WHEN saldoPendiente - @valorAplicado <= 0 THEN 3 ELSE idEstado END
        WHERE id = @idFactura;

        COMMIT TRANSACTION;

        SELECT 1 AS respuesta, @idPago AS nuevoId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SELECT 0 AS respuesta, 0 AS nuevoId;
    END CATCH
END
GO
