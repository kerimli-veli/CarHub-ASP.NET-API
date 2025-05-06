CREATE DATABASE CarHubDb

CREATE TABLE Category (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(500),
    CreatedBy INT,
    UpdatedBy INT,
    DeletedBy INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME,
    DeletedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE Product (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(255) NOT NULL,
    SupplierId INT NOT NULL,
    QuantityPerUnit NVARCHAR(50),
    UnitPrice DECIMAL(18, 2),
    UnitsInStock SMALLINT,
    UnitsOnOrder SMALLINT,
    ReorderLevel SMALLINT,
    Discontinued BIT NOT NULL,
    CreatedBy INT,
    UpdatedBy INT,
    DeletedBy INT,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME,
    DeletedDate DATETIME,
    IsDeleted BIT DEFAULT 0
);

CREATE TABLE ProductCategory (
    ProductId INT NOT NULL,
    CategoryId INT NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Product(Id) ON DELETE CASCADE,
    FOREIGN KEY (CategoryId) REFERENCES Category(Id) ON DELETE CASCADE
);