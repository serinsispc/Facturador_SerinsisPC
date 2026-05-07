IF DB_ID('DBMensualidadesSerinsisPC_V2') IS NULL
BEGIN
    CREATE DATABASE DBMensualidadesSerinsisPC_V2;
END
GO

USE DBMensualidadesSerinsisPC_V2;
GO

SET NOCOUNT ON;
GO

CREATE TABLE ServiceStatuses
(
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(30) NOT NULL,
    Description VARCHAR(150) NOT NULL
);
GO

MERGE ServiceStatuses AS target
USING
(
    SELECT 0 AS Id, 'AL_DIA' AS Name, 'Cliente al dia y servicio habilitado' AS Description
    UNION ALL
    SELECT 1, 'AVISO', 'Cliente en mora dentro del periodo de aviso'
    UNION ALL
    SELECT 2, 'SUSPENDIDO', 'Cliente suspendido por falta de pago'
) AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET Name = source.Name, Description = source.Description
WHEN NOT MATCHED THEN
    INSERT (Id, Name, Description) VALUES (source.Id, source.Name, source.Description);
GO

CREATE TABLE InvoiceStatuses
(
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(30) NOT NULL
);
GO

MERGE InvoiceStatuses AS target
USING
(
    SELECT 0 AS Id, 'DRAFT' AS Name
    UNION ALL SELECT 1, 'ISSUED'
    UNION ALL SELECT 2, 'PARTIAL'
    UNION ALL SELECT 3, 'PAID'
    UNION ALL SELECT 4, 'OVERDUE'
    UNION ALL SELECT 5, 'CANCELLED'
) AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET Name = source.Name
WHEN NOT MATCHED THEN
    INSERT (Id, Name) VALUES (source.Id, source.Name);
GO

CREATE TABLE PaymentMethods
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    IsActive BIT NOT NULL DEFAULT (1)
);
GO

IF NOT EXISTS (SELECT 1 FROM PaymentMethods WHERE Name = 'EFECTIVO') INSERT INTO PaymentMethods(Name) VALUES ('EFECTIVO');
IF NOT EXISTS (SELECT 1 FROM PaymentMethods WHERE Name = 'TRANSFERENCIA') INSERT INTO PaymentMethods(Name) VALUES ('TRANSFERENCIA');
IF NOT EXISTS (SELECT 1 FROM PaymentMethods WHERE Name = 'CONSIGNACION') INSERT INTO PaymentMethods(Name) VALUES ('CONSIGNACION');
IF NOT EXISTS (SELECT 1 FROM PaymentMethods WHERE Name = 'NEQUI') INSERT INTO PaymentMethods(Name) VALUES ('NEQUI');
IF NOT EXISTS (SELECT 1 FROM PaymentMethods WHERE Name = 'DAVIPLATA') INSERT INTO PaymentMethods(Name) VALUES ('DAVIPLATA');
GO

CREATE TABLE ServicePlans
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(80) NOT NULL,
    BillingPeriodMonths INT NOT NULL,
    BasePrice DECIMAL(18,2) NOT NULL,
    IsActive BIT NOT NULL DEFAULT (1)
);
GO

IF NOT EXISTS (SELECT 1 FROM ServicePlans WHERE Name = 'MENSUAL')
    INSERT INTO ServicePlans (Name, BillingPeriodMonths, BasePrice, IsActive) VALUES ('MENSUAL', 1, 50000, 1);
IF NOT EXISTS (SELECT 1 FROM ServicePlans WHERE Name = 'SEMESTRAL')
    INSERT INTO ServicePlans (Name, BillingPeriodMonths, BasePrice, IsActive) VALUES ('SEMESTRAL', 6, 250000, 1);
IF NOT EXISTS (SELECT 1 FROM ServicePlans WHERE Name = 'ANUAL')
    INSERT INTO ServicePlans (Name, BillingPeriodMonths, BasePrice, IsActive) VALUES ('ANUAL', 12, 500000, 1);
GO

CREATE TABLE Customers
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DocumentNumber VARCHAR(20) NOT NULL,
    BusinessName VARCHAR(150) NOT NULL,
    CommercialName VARCHAR(150) NOT NULL,
    ContactName VARCHAR(150) NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    Address VARCHAR(200) NOT NULL DEFAULT (''),
    IsActive BIT NOT NULL DEFAULT (1),
    ServiceStatusId INT NOT NULL DEFAULT (0),
    Notes VARCHAR(500) NOT NULL DEFAULT (''),
    CreatedAt DATETIME NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT FK_Customers_ServiceStatuses FOREIGN KEY (ServiceStatusId) REFERENCES ServiceStatuses(Id)
);
GO

CREATE UNIQUE INDEX UX_Customers_DocumentNumber ON Customers(DocumentNumber);
GO

CREATE TABLE CustomerSubscriptions
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    ServicePlanId INT NOT NULL,
    StartDate DATE NOT NULL,
    PaymentDay TINYINT NOT NULL,
    NextBillingDate DATE NOT NULL,
    LastPaymentDate DATE NULL,
    GraceDays INT NOT NULL DEFAULT (5),
    AutomaticBillingEnabled BIT NOT NULL DEFAULT (1),
    AutomaticCollectionEnabled BIT NOT NULL DEFAULT (1),
    ServiceStatusId INT NOT NULL DEFAULT (0),
    Notes VARCHAR(250) NOT NULL DEFAULT (''),
    CONSTRAINT FK_CustomerSubscriptions_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    CONSTRAINT FK_CustomerSubscriptions_ServicePlans FOREIGN KEY (ServicePlanId) REFERENCES ServicePlans(Id),
    CONSTRAINT FK_CustomerSubscriptions_ServiceStatuses FOREIGN KEY (ServiceStatusId) REFERENCES ServiceStatuses(Id),
    CONSTRAINT CK_CustomerSubscriptions_PaymentDay CHECK (PaymentDay BETWEEN 1 AND 31)
);
GO

CREATE TABLE CustomerDatabases
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    DatabaseName VARCHAR(80) NOT NULL,
    ServerName VARCHAR(120) NOT NULL,
    IsOnline BIT NOT NULL DEFAULT (1),
    ServiceStatusId INT NOT NULL DEFAULT (0),
    CurrentMessage VARCHAR(500) NOT NULL DEFAULT ('SERVICIO ACTIVO Y AL DIA.'),
    LastSynchronizedAt DATETIME NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT FK_CustomerDatabases_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    CONSTRAINT FK_CustomerDatabases_ServiceStatuses FOREIGN KEY (ServiceStatusId) REFERENCES ServiceStatuses(Id)
);
GO

CREATE UNIQUE INDEX UX_CustomerDatabases_DatabaseName ON CustomerDatabases(DatabaseName);
GO

CREATE TABLE Invoices
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    CustomerSubscriptionId INT NOT NULL,
    IssuedAt DATETIME NOT NULL,
    DueDate DATE NOT NULL,
    PeriodStart DATE NOT NULL,
    PeriodEnd DATE NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    PendingAmount DECIMAL(18,2) NOT NULL,
    InvoiceStatusId INT NOT NULL DEFAULT (1),
    CONSTRAINT FK_Invoices_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    CONSTRAINT FK_Invoices_CustomerSubscriptions FOREIGN KEY (CustomerSubscriptionId) REFERENCES CustomerSubscriptions(Id),
    CONSTRAINT FK_Invoices_InvoiceStatuses FOREIGN KEY (InvoiceStatusId) REFERENCES InvoiceStatuses(Id)
);
GO

CREATE INDEX IX_Invoices_Customer_DueDate ON Invoices(CustomerId, DueDate);
GO

CREATE TABLE PaymentReceipts
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    PaymentMethodId INT NOT NULL,
    PaidAt DATETIME NOT NULL,
    ReceivedAmount DECIMAL(18,2) NOT NULL,
    ReceiptNumber VARCHAR(100) NOT NULL DEFAULT (''),
    Reference VARCHAR(100) NOT NULL DEFAULT (''),
    Notes VARCHAR(250) NOT NULL DEFAULT (''),
    RegisteredBy VARCHAR(100) NOT NULL DEFAULT ('SISTEMA'),
    CONSTRAINT FK_PaymentReceipts_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    CONSTRAINT FK_PaymentReceipts_PaymentMethods FOREIGN KEY (PaymentMethodId) REFERENCES PaymentMethods(Id)
);
GO

CREATE TABLE PaymentAllocations
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PaymentReceiptId INT NOT NULL,
    InvoiceId INT NOT NULL,
    AppliedAmount DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_PaymentAllocations_PaymentReceipts FOREIGN KEY (PaymentReceiptId) REFERENCES PaymentReceipts(Id),
    CONSTRAINT FK_PaymentAllocations_Invoices FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id)
);
GO

CREATE TABLE BillingSchedules
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    CustomerSubscriptionId INT NOT NULL,
    ScheduledDate DATE NOT NULL,
    PeriodStart DATE NOT NULL,
    PeriodEnd DATE NOT NULL,
    AmountToBill DECIMAL(18,2) NOT NULL,
    ScheduleState VARCHAR(40) NOT NULL DEFAULT ('SCHEDULED'),
    InvoiceId INT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT (GETDATE()),
    ProcessedAt DATETIME NULL,
    LastMessage VARCHAR(250) NOT NULL DEFAULT (''),
    CONSTRAINT FK_BillingSchedules_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id),
    CONSTRAINT FK_BillingSchedules_CustomerSubscriptions FOREIGN KEY (CustomerSubscriptionId) REFERENCES CustomerSubscriptions(Id),
    CONSTRAINT FK_BillingSchedules_Invoices FOREIGN KEY (InvoiceId) REFERENCES Invoices(Id)
);
GO

CREATE UNIQUE INDEX UX_BillingSchedules_Subscription_Date ON BillingSchedules(CustomerSubscriptionId, ScheduledDate);
GO

CREATE TABLE NotificationTemplates
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Channel VARCHAR(30) NOT NULL,
    Subject VARCHAR(150) NOT NULL DEFAULT (''),
    Body VARCHAR(MAX) NOT NULL,
    IsActive BIT NOT NULL DEFAULT (1)
);
GO

CREATE TABLE NotificationLogs
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    Channel VARCHAR(30) NOT NULL,
    Recipient VARCHAR(150) NOT NULL,
    Subject VARCHAR(150) NOT NULL DEFAULT (''),
    MessageBody VARCHAR(MAX) NOT NULL,
    SentAt DATETIME NOT NULL DEFAULT (GETDATE()),
    DeliveryStatus VARCHAR(40) NOT NULL DEFAULT ('PENDING'),
    CONSTRAINT FK_NotificationLogs_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
GO

CREATE TABLE ServiceStatusHistories
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    PreviousStatusId INT NOT NULL,
    NewStatusId INT NOT NULL,
    Reason VARCHAR(250) NOT NULL,
    ChangedAt DATETIME NOT NULL DEFAULT (GETDATE()),
    CONSTRAINT FK_ServiceStatusHistories_Customers FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
GO

CREATE TABLE AutomationProcessLogs
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProcessName VARCHAR(100) NOT NULL,
    ExecutedAt DATETIME NOT NULL DEFAULT (GETDATE()),
    ProcessedCount INT NOT NULL DEFAULT (0),
    SuccessfulCount INT NOT NULL DEFAULT (0),
    ErrorCount INT NOT NULL DEFAULT (0),
    Details VARCHAR(500) NOT NULL DEFAULT ('')
);
GO

CREATE VIEW V_CustomerOperations
AS
SELECT
    c.Id,
    c.DocumentNumber,
    c.BusinessName,
    c.CommercialName,
    c.ContactName,
    c.PhoneNumber,
    c.Email,
    c.IsActive,
    c.ServiceStatusId,
    ss.Name AS ServiceStatusName,
    cs.Id AS SubscriptionId,
    sp.Name AS PlanName,
    sp.BillingPeriodMonths,
    cs.PaymentDay,
    cs.NextBillingDate,
    cs.LastPaymentDate,
    cs.GraceDays,
    ISNULL(SUM(i.PendingAmount), 0) AS PendingAmount,
    MAX(i.DueDate) AS LatestDueDate
FROM Customers c
LEFT JOIN ServiceStatuses ss ON ss.Id = c.ServiceStatusId
LEFT JOIN CustomerSubscriptions cs ON cs.CustomerId = c.Id
LEFT JOIN ServicePlans sp ON sp.Id = cs.ServicePlanId
LEFT JOIN Invoices i ON i.CustomerId = c.Id AND i.InvoiceStatusId IN (1, 2, 4)
GROUP BY
    c.Id, c.DocumentNumber, c.BusinessName, c.CommercialName, c.ContactName, c.PhoneNumber, c.Email,
    c.IsActive, c.ServiceStatusId, ss.Name, cs.Id, sp.Name, sp.BillingPeriodMonths, cs.PaymentDay,
    cs.NextBillingDate, cs.LastPaymentDate, cs.GraceDays;
GO

CREATE VIEW V_MonthlyIncome
AS
SELECT
    YEAR(PaidAt) AS [Year],
    MONTH(PaidAt) AS [Month],
    COUNT(*) AS PaymentsCount,
    COUNT(DISTINCT CustomerId) AS CustomersPaid,
    SUM(ReceivedAmount) AS TotalIncome
FROM PaymentReceipts
GROUP BY YEAR(PaidAt), MONTH(PaidAt);
GO

CREATE VIEW V_PortfolioSummary
AS
SELECT
    YEAR(DueDate) AS [Year],
    MONTH(DueDate) AS [Month],
    COUNT(*) AS InvoicesCount,
    SUM(TotalAmount) AS TotalBilled,
    SUM(PendingAmount) AS TotalPending
FROM Invoices
GROUP BY YEAR(DueDate), MONTH(DueDate);
GO

CREATE VIEW V_ServiceStatusPOS
AS
SELECT
    db.Id AS CustomerDatabaseId,
    db.DatabaseName,
    db.ServerName,
    db.IsOnline,
    db.ServiceStatusId,
    ss.Name AS ServiceStatusName,
    db.CurrentMessage,
    db.LastSynchronizedAt,
    c.Id AS CustomerId,
    c.CommercialName,
    c.ContactName,
    c.PhoneNumber,
    c.Email
FROM CustomerDatabases db
INNER JOIN Customers c ON c.Id = db.CustomerId
INNER JOIN ServiceStatuses ss ON ss.Id = db.ServiceStatusId;
GO

CREATE OR ALTER PROCEDURE sp_RegisterPayment
    @InvoiceId INT,
    @PaymentMethodId INT,
    @PaidAt DATETIME,
    @ReceivedAmount DECIMAL(18,2),
    @ReceiptNumber VARCHAR(100),
    @Reference VARCHAR(100),
    @Notes VARCHAR(250),
    @RegisteredBy VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CustomerId INT, @PaymentReceiptId INT, @CurrentPending DECIMAL(18,2);

    SELECT
        @CustomerId = CustomerId,
        @CurrentPending = PendingAmount
    FROM Invoices
    WHERE Id = @InvoiceId;

    INSERT INTO PaymentReceipts
    (
        CustomerId, PaymentMethodId, PaidAt, ReceivedAmount,
        ReceiptNumber, Reference, Notes, RegisteredBy
    )
    VALUES
    (
        @CustomerId, @PaymentMethodId, @PaidAt, @ReceivedAmount,
        @ReceiptNumber, @Reference, @Notes, @RegisteredBy
    );

    SET @PaymentReceiptId = SCOPE_IDENTITY();

    INSERT INTO PaymentAllocations (PaymentReceiptId, InvoiceId, AppliedAmount)
    VALUES (@PaymentReceiptId, @InvoiceId, @ReceivedAmount);

    UPDATE Invoices
    SET
        PendingAmount = CASE WHEN PendingAmount - @ReceivedAmount < 0 THEN 0 ELSE PendingAmount - @ReceivedAmount END,
        InvoiceStatusId =
            CASE
                WHEN PendingAmount - @ReceivedAmount <= 0 THEN 3
                WHEN PendingAmount - @ReceivedAmount < TotalAmount THEN 2
                ELSE InvoiceStatusId
            END
    WHERE Id = @InvoiceId;
END
GO

CREATE OR ALTER PROCEDURE sp_RefreshCustomerServiceStatus
    @CustomerId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE
        @PendingAmount DECIMAL(18,2),
        @NextBillingDate DATE,
        @GraceDays INT,
        @NewStatusId INT,
        @PreviousStatusId INT;

    SELECT
        @PendingAmount = ISNULL(SUM(PendingAmount), 0)
    FROM Invoices
    WHERE CustomerId = @CustomerId
      AND InvoiceStatusId IN (1, 2, 4);

    SELECT TOP 1
        @NextBillingDate = NextBillingDate,
        @GraceDays = GraceDays
    FROM CustomerSubscriptions
    WHERE CustomerId = @CustomerId
    ORDER BY Id DESC;

    SELECT @PreviousStatusId = ServiceStatusId
    FROM Customers
    WHERE Id = @CustomerId;

    SET @NewStatusId =
        CASE
            WHEN ISNULL(@PendingAmount, 0) <= 0 THEN 0
            WHEN @NextBillingDate IS NOT NULL AND DATEDIFF(DAY, @NextBillingDate, CAST(GETDATE() AS DATE)) >= ISNULL(@GraceDays, 5) THEN 2
            WHEN @NextBillingDate IS NOT NULL AND DATEDIFF(DAY, @NextBillingDate, CAST(GETDATE() AS DATE)) >= 1 THEN 1
            ELSE 0
        END;

    UPDATE Customers
    SET ServiceStatusId = @NewStatusId
    WHERE Id = @CustomerId;

    UPDATE CustomerSubscriptions
    SET ServiceStatusId = @NewStatusId
    WHERE CustomerId = @CustomerId;

    UPDATE CustomerDatabases
    SET
        ServiceStatusId = @NewStatusId,
        IsOnline = CASE WHEN @NewStatusId = 2 THEN 0 ELSE 1 END,
        CurrentMessage =
            CASE @NewStatusId
                WHEN 0 THEN 'SERVICIO ACTIVO Y AL DIA.'
                WHEN 1 THEN 'AVISO DE PAGO PENDIENTE. EVITE LA SUSPENSION.'
                WHEN 2 THEN 'SERVICIO SUSPENDIDO POR FALTA DE PAGO.'
                ELSE CurrentMessage
            END,
        LastSynchronizedAt = GETDATE()
    WHERE CustomerId = @CustomerId;

    IF ISNULL(@PreviousStatusId, -1) <> @NewStatusId
    BEGIN
        INSERT INTO ServiceStatusHistories (CustomerId, PreviousStatusId, NewStatusId, Reason)
        VALUES (@CustomerId, ISNULL(@PreviousStatusId, 0), @NewStatusId, 'Actualizacion automatica de estado del servicio');
    END
END
GO

CREATE OR ALTER PROCEDURE sp_RefreshAllServiceStatuses
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CustomerId INT;

    DECLARE customer_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT Id FROM Customers;

    OPEN customer_cursor;
    FETCH NEXT FROM customer_cursor INTO @CustomerId;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        EXEC sp_RefreshCustomerServiceStatus @CustomerId = @CustomerId;
        FETCH NEXT FROM customer_cursor INTO @CustomerId;
    END

    CLOSE customer_cursor;
    DEALLOCATE customer_cursor;
END
GO

CREATE OR ALTER PROCEDURE sp_ScheduleBillingForCustomer
    @CustomerSubscriptionId INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE
        @CustomerId INT,
        @NextBillingDate DATE,
        @BillingPeriodMonths INT,
        @BasePrice DECIMAL(18,2),
        @PeriodStart DATE,
        @PeriodEnd DATE;

    SELECT
        @CustomerId = cs.CustomerId,
        @NextBillingDate = cs.NextBillingDate,
        @BillingPeriodMonths = sp.BillingPeriodMonths,
        @BasePrice = sp.BasePrice
    FROM CustomerSubscriptions cs
    INNER JOIN ServicePlans sp ON sp.Id = cs.ServicePlanId
    WHERE cs.Id = @CustomerSubscriptionId;

    SET @PeriodEnd = @NextBillingDate;
    SET @PeriodStart = DATEADD(DAY, 1, DATEADD(MONTH, -ISNULL(@BillingPeriodMonths, 1), @PeriodEnd));

    IF NOT EXISTS
    (
        SELECT 1
        FROM BillingSchedules
        WHERE CustomerSubscriptionId = @CustomerSubscriptionId
          AND ScheduledDate = @NextBillingDate
    )
    BEGIN
        INSERT INTO BillingSchedules
        (
            CustomerId, CustomerSubscriptionId, ScheduledDate, PeriodStart, PeriodEnd, AmountToBill
        )
        VALUES
        (
            @CustomerId, @CustomerSubscriptionId, @NextBillingDate, @PeriodStart, @PeriodEnd, @BasePrice
        );
    END
END
GO
