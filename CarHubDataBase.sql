CREATE TABLE [dbo].[Users] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (100) NOT NULL,
    [Surname]       NVARCHAR (100) NOT NULL,
    [Email]         NVARCHAR (255) NOT NULL,
    [Phone]         NVARCHAR (20)  NULL,
    [PasswordHash]  NVARCHAR (255) NOT NULL,
    [UserImagePath] NVARCHAR (500) NULL,
    [UserRole]      INT            NOT NULL,
    [CreatedBy]     INT            NULL,
    [UpdatedBy]     INT            NULL,
    [DeletedBy]     INT            NULL,
    [CreatedDate]   DATETIME       NULL,
    [UpdatedDate]   DATETIME       NULL,
    [DeletedDate]   DATETIME       NULL,
    [IsDeleted]     BIT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);


CREATE TABLE [dbo].[Cars] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Brand]          NVARCHAR (255) NOT NULL,
    [BrandImagePath] NVARCHAR (300) NULL,
    [Model]          NVARCHAR (255) NOT NULL,
    [Year]           INT            NOT NULL,
    [Price]          INT            NOT NULL,
    [Fuel]           INT            NOT NULL,
    [Transmission]   INT            NOT NULL,
    [Miles]          FLOAT (53)     NOT NULL,
    [Body]           INT            NOT NULL,
    [BodyTypeImage]  NVARCHAR (300) NULL,
    [Color]          NVARCHAR (255) NOT NULL,
    [VIN]            NVARCHAR (255) NOT NULL,
    [Text]           NVARCHAR (MAX) NOT NULL,
    [IsDeleted]      BIT            DEFAULT ((0)) NOT NULL,
    [CreatedDate]    DATETIME       NOT NULL,
    [CreatedBy]      INT            NULL,
    [UpdatedDate]    DATETIME       NULL,
    [UpdatedBy]      INT            NULL,
    [DeletedDate]    DATETIME       NULL,
    [DeletedBy]      INT            NULL,
    [UserId]         INT            NOT NULL,
    CONSTRAINT [PK_Cars] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Cars_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);



CREATE TABLE [dbo].[UserFavorites] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [UserId]     INT NOT NULL,
    [CarId]      INT NOT NULL,
    [IsFavorite] BIT DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC, [CarId] ASC),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]),
    FOREIGN KEY ([CarId]) REFERENCES [dbo].[Cars] ([Id])
);

CREATE TABLE [dbo].[CarImage] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [CarId]     INT            NOT NULL,
    [ImagePath] NVARCHAR (500) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([CarId]) REFERENCES [dbo].[Cars] ([Id])
);


CREATE TABLE [dbo].[RefreshTokens] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Token]          NVARCHAR (255) NOT NULL,
    [UserId]         INT            NOT NULL,
    [ExpirationDate] DATETIME2 (7)  NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
