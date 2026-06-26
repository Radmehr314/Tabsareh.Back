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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624202334_Initial'
)
BEGIN
    CREATE TABLE [Admins] (
        [Id] nvarchar(64) NOT NULL,
        [UserName] nvarchar(256) NOT NULL,
        [FirstName] nvarchar(256) NOT NULL,
        [LastName] nvarchar(256) NOT NULL,
        [Phone] nvarchar(32) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [Salt] nvarchar(max) NOT NULL,
        [IsBan] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [RoleId] nvarchar(64) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Admins] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624202334_Initial'
)
BEGIN
    CREATE TABLE [Categories] (
        [Id] nvarchar(64) NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [ParentId] nvarchar(64) NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624202334_Initial'
)
BEGIN
    CREATE TABLE [ContentOwners] (
        [Id] nvarchar(64) NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [UserName] nvarchar(256) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [Salt] nvarchar(max) NOT NULL,
        [IsBan] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_ContentOwners] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624202334_Initial'
)
BEGIN
    CREATE TABLE [Roles] (
        [Id] nvarchar(64) NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [Permissions] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624202334_Initial'
)
BEGIN
    CREATE TABLE [Teachers] (
        [Id] nvarchar(64) NOT NULL,
        [FirstName] nvarchar(256) NOT NULL,
        [LastName] nvarchar(256) NOT NULL,
        [ProfileImage] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Teachers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624202334_Initial'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] nvarchar(64) NOT NULL,
        [FirstName] nvarchar(256) NOT NULL,
        [LastName] nvarchar(256) NOT NULL,
        [UserName] nvarchar(256) NOT NULL,
        [Phone] nvarchar(32) NOT NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624202334_Initial'
)
BEGIN
    CREATE INDEX [IX_Admins_UserName] ON [Admins] ([UserName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624202334_Initial'
)
BEGIN
    CREATE INDEX [IX_Categories_Name] ON [Categories] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624202334_Initial'
)
BEGIN
    CREATE INDEX [IX_Categories_ParentId] ON [Categories] ([ParentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624202334_Initial'
)
BEGIN
    CREATE INDEX [IX_ContentOwners_UserName] ON [ContentOwners] ([UserName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624202334_Initial'
)
BEGIN
    CREATE INDEX [IX_Users_UserName] ON [Users] ([UserName]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624202334_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260624202334_Initial', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624204644_blog'
)
BEGIN
    CREATE TABLE [Blogs] (
        [Id] nvarchar(64) NOT NULL,
        [Title] nvarchar(512) NOT NULL,
        [Slug] nvarchar(512) NOT NULL,
        [TitleImage] nvarchar(1024) NULL,
        [Body] nvarchar(max) NOT NULL,
        [Excerpt] nvarchar(1024) NULL,
        [CategoryId] nvarchar(64) NULL,
        [MetaTitle] nvarchar(512) NULL,
        [MetaDescription] nvarchar(1024) NULL,
        [MetaKeywords] nvarchar(1024) NULL,
        [IsPublished] bit NOT NULL,
        [PublishedAt] datetime2 NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Blogs] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624204644_blog'
)
BEGIN
    CREATE INDEX [IX_Blogs_CategoryId] ON [Blogs] ([CategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624204644_blog'
)
BEGIN
    CREATE INDEX [IX_Blogs_IsPublished] ON [Blogs] ([IsPublished]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624204644_blog'
)
BEGIN
    CREATE INDEX [IX_Blogs_Slug] ON [Blogs] ([Slug]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260624204644_blog'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260624204644_blog', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625092448_AddForeignKeys'
)
BEGIN
    CREATE INDEX [IX_Admins_RoleId] ON [Admins] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625092448_AddForeignKeys'
)
BEGIN
    ALTER TABLE [Admins] ADD CONSTRAINT [FK_Admins_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625092448_AddForeignKeys'
)
BEGIN
    ALTER TABLE [Blogs] ADD CONSTRAINT [FK_Blogs_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625092448_AddForeignKeys'
)
BEGIN
    ALTER TABLE [Categories] ADD CONSTRAINT [FK_Categories_Categories_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [Categories] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625092448_AddForeignKeys'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260625092448_AddForeignKeys', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    CREATE TABLE [Courses] (
        [Id] nvarchar(64) NOT NULL,
        [Title] nvarchar(512) NOT NULL,
        [BannerImage] nvarchar(1024) NULL,
        [DurationMinutes] int NOT NULL,
        [CategoryId] nvarchar(64) NULL,
        [TeacherId] nvarchar(64) NOT NULL,
        [ContentOwnerId] nvarchar(64) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Price] decimal(18,2) NOT NULL,
        [ContentOwnerSharePercent] decimal(5,2) NOT NULL,
        [IsActive] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Courses] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Courses_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]),
        CONSTRAINT [FK_Courses_ContentOwners_ContentOwnerId] FOREIGN KEY ([ContentOwnerId]) REFERENCES [ContentOwners] ([Id]),
        CONSTRAINT [FK_Courses_Teachers_TeacherId] FOREIGN KEY ([TeacherId]) REFERENCES [Teachers] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    CREATE TABLE [CourseChapters] (
        [Id] nvarchar(64) NOT NULL,
        [CourseId] nvarchar(64) NOT NULL,
        [Title] nvarchar(512) NOT NULL,
        [Duration] nvarchar(32) NOT NULL,
        [DisplayOrder] int NOT NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_CourseChapters] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CourseChapters_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    CREATE TABLE [CoursePdfFiles] (
        [Id] nvarchar(64) NOT NULL,
        [CourseId] nvarchar(64) NOT NULL,
        [Title] nvarchar(512) NOT NULL,
        [FileUrl] nvarchar(1024) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_CoursePdfFiles] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CoursePdfFiles_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    CREATE TABLE [CourseSampleVideos] (
        [Id] nvarchar(64) NOT NULL,
        [CourseId] nvarchar(64) NOT NULL,
        [Title] nvarchar(512) NOT NULL,
        [FileUrl] nvarchar(1024) NOT NULL,
        [Duration] nvarchar(16) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_CourseSampleVideos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CourseSampleVideos_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    CREATE INDEX [IX_CourseChapters_CourseId] ON [CourseChapters] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    CREATE INDEX [IX_CourseChapters_DisplayOrder] ON [CourseChapters] ([DisplayOrder]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    CREATE INDEX [IX_CoursePdfFiles_CourseId] ON [CoursePdfFiles] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    CREATE INDEX [IX_Courses_CategoryId] ON [Courses] ([CategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    CREATE INDEX [IX_Courses_ContentOwnerId] ON [Courses] ([ContentOwnerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    CREATE INDEX [IX_Courses_IsActive] ON [Courses] ([IsActive]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    CREATE INDEX [IX_Courses_TeacherId] ON [Courses] ([TeacherId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    CREATE INDEX [IX_CourseSampleVideos_CourseId] ON [CourseSampleVideos] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625163001_AddCoursesAndChapters'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260625163001_AddCoursesAndChapters', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625164752_AddCourseSettlementPriceAndChapterVideos'
)
BEGIN
    ALTER TABLE [Courses] ADD [SettlementBasePrice] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625164752_AddCourseSettlementPriceAndChapterVideos'
)
BEGIN
    UPDATE [Courses] SET [SettlementBasePrice] = [Price] WHERE [SettlementBasePrice] = 0
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625164752_AddCourseSettlementPriceAndChapterVideos'
)
BEGIN
    CREATE TABLE [CourseChapterVideos] (
        [Id] nvarchar(64) NOT NULL,
        [CourseChapterId] nvarchar(64) NOT NULL,
        [Title] nvarchar(512) NOT NULL,
        [Duration] nvarchar(32) NOT NULL,
        [ExternalVideoId] nvarchar(256) NULL,
        [VideoUrl] nvarchar(1024) NULL,
        [UploadStatus] nvarchar(64) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_CourseChapterVideos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CourseChapterVideos_CourseChapters_CourseChapterId] FOREIGN KEY ([CourseChapterId]) REFERENCES [CourseChapters] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625164752_AddCourseSettlementPriceAndChapterVideos'
)
BEGIN
    CREATE INDEX [IX_CourseChapterVideos_CourseChapterId] ON [CourseChapterVideos] ([CourseChapterId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625164752_AddCourseSettlementPriceAndChapterVideos'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260625164752_AddCourseSettlementPriceAndChapterVideos', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625182507_AddDiscounts'
)
BEGIN
    ALTER TABLE [Courses] ADD [DiscountEndDate] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625182507_AddDiscounts'
)
BEGIN
    ALTER TABLE [Courses] ADD [DiscountPercent] decimal(5,2) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625182507_AddDiscounts'
)
BEGIN
    ALTER TABLE [Courses] ADD [DiscountStartDate] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625182507_AddDiscounts'
)
BEGIN
    CREATE TABLE [DiscountCodes] (
        [Id] nvarchar(64) NOT NULL,
        [Title] nvarchar(256) NOT NULL,
        [Code] nvarchar(64) NOT NULL,
        [UsageLimit] int NOT NULL,
        [UsedCount] int NOT NULL,
        [DiscountPercent] decimal(5,2) NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NOT NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_DiscountCodes] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625182507_AddDiscounts'
)
BEGIN
    CREATE UNIQUE INDEX [IX_DiscountCodes_Code] ON [DiscountCodes] ([Code]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625182507_AddDiscounts'
)
BEGIN
    CREATE INDEX [IX_DiscountCodes_IsDeleted] ON [DiscountCodes] ([IsDeleted]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625182507_AddDiscounts'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260625182507_AddDiscounts', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625203909_AddOrders'
)
BEGIN
    CREATE TABLE [Orders] (
        [Id] nvarchar(64) NOT NULL,
        [UserId] nvarchar(64) NOT NULL,
        [CourseId] nvarchar(64) NOT NULL,
        [PaymentMethod] nvarchar(32) NOT NULL,
        [Status] nvarchar(32) NOT NULL,
        [CoursePrice] decimal(18,2) NOT NULL,
        [DiscountPercentSnapshot] decimal(5,2) NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [SettlementBasePriceSnapshot] decimal(18,2) NOT NULL,
        [ContentOwnerSharePercentSnapshot] decimal(5,2) NOT NULL,
        [PaidAt] datetime2 NULL,
        [CardToCardTrackingNumber] nvarchar(128) NULL,
        [CardToCardDescription] nvarchar(1024) NULL,
        [RejectionReason] nvarchar(1024) NULL,
        [LicenseCode] nvarchar(128) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_Orders] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Orders_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]),
        CONSTRAINT [FK_Orders_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625203909_AddOrders'
)
BEGIN
    CREATE INDEX [IX_Orders_CourseId] ON [Orders] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625203909_AddOrders'
)
BEGIN
    CREATE INDEX [IX_Orders_PaymentMethod] ON [Orders] ([PaymentMethod]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625203909_AddOrders'
)
BEGIN
    CREATE INDEX [IX_Orders_Status] ON [Orders] ([Status]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625203909_AddOrders'
)
BEGIN
    CREATE INDEX [IX_Orders_UserId] ON [Orders] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625203909_AddOrders'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260625203909_AddOrders', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625210702_AddSmsOtpLogin'
)
BEGIN
    CREATE TABLE [SmsOtps] (
        [Id] nvarchar(64) NOT NULL,
        [Phone] nvarchar(32) NOT NULL,
        [Code] nvarchar(16) NOT NULL,
        [ExpiresAt] datetime2 NOT NULL,
        [IsUsed] bit NOT NULL,
        [UsedAt] datetime2 NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_SmsOtps] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625210702_AddSmsOtpLogin'
)
BEGIN
    CREATE INDEX [IX_SmsOtps_Phone] ON [SmsOtps] ([Phone]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625210702_AddSmsOtpLogin'
)
BEGIN
    CREATE INDEX [IX_SmsOtps_Phone_Code_IsUsed_ExpiresAt] ON [SmsOtps] ([Phone], [Code], [IsUsed], [ExpiresAt]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625210702_AddSmsOtpLogin'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260625210702_AddSmsOtpLogin', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625213408_AddOrderItemsAndInvoice'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Orders]') AND [c].[name] = N'CourseId');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [Orders] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [Orders] ALTER COLUMN [CourseId] nvarchar(64) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625213408_AddOrderItemsAndInvoice'
)
BEGIN
    ALTER TABLE [Orders] ADD [CourseDiscountAmount] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625213408_AddOrderItemsAndInvoice'
)
BEGIN
    ALTER TABLE [Orders] ADD [DiscountCode] nvarchar(64) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625213408_AddOrderItemsAndInvoice'
)
BEGIN
    ALTER TABLE [Orders] ADD [DiscountCodeAmount] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625213408_AddOrderItemsAndInvoice'
)
BEGIN
    ALTER TABLE [Orders] ADD [DiscountCodePercentSnapshot] decimal(5,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625213408_AddOrderItemsAndInvoice'
)
BEGIN
    ALTER TABLE [Orders] ADD [PayableAmount] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625213408_AddOrderItemsAndInvoice'
)
BEGIN
    ALTER TABLE [Orders] ADD [SubtotalAmount] decimal(18,2) NOT NULL DEFAULT 0.0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625213408_AddOrderItemsAndInvoice'
)
BEGIN
    CREATE TABLE [OrderItems] (
        [Id] nvarchar(64) NOT NULL,
        [OrderId] nvarchar(64) NOT NULL,
        [CourseId] nvarchar(64) NOT NULL,
        [CourseTitleSnapshot] nvarchar(512) NOT NULL,
        [CoursePriceSnapshot] decimal(18,2) NOT NULL,
        [CourseDiscountPercentSnapshot] decimal(5,2) NOT NULL,
        [CourseDiscountAmountSnapshot] decimal(18,2) NOT NULL,
        [DiscountCodePercentSnapshot] decimal(5,2) NOT NULL,
        [DiscountCodeAmountSnapshot] decimal(18,2) NOT NULL,
        [FinalAmount] decimal(18,2) NOT NULL,
        [SettlementBasePriceSnapshot] decimal(18,2) NOT NULL,
        [ContentOwnerSharePercentSnapshot] decimal(5,2) NOT NULL,
        [LicenseCode] nvarchar(128) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_OrderItems] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_OrderItems_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]),
        CONSTRAINT [FK_OrderItems_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625213408_AddOrderItemsAndInvoice'
)
BEGIN
    CREATE INDEX [IX_OrderItems_CourseId] ON [OrderItems] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625213408_AddOrderItemsAndInvoice'
)
BEGIN
    CREATE INDEX [IX_OrderItems_OrderId] ON [OrderItems] ([OrderId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625213408_AddOrderItemsAndInvoice'
)
BEGIN

    UPDATE Orders
    SET SubtotalAmount = CoursePrice,
        CourseDiscountAmount = CASE WHEN CoursePrice > Amount THEN CoursePrice - Amount ELSE 0 END,
        PayableAmount = Amount
    WHERE CourseId IS NOT NULL;

    INSERT INTO OrderItems (
        Id,
        OrderId,
        CourseId,
        CourseTitleSnapshot,
        CoursePriceSnapshot,
        CourseDiscountPercentSnapshot,
        CourseDiscountAmountSnapshot,
        DiscountCodePercentSnapshot,
        DiscountCodeAmountSnapshot,
        FinalAmount,
        SettlementBasePriceSnapshot,
        ContentOwnerSharePercentSnapshot,
        LicenseCode,
        CreatedAt,
        UpdatedAt)
    SELECT
        REPLACE(CONVERT(nvarchar(36), NEWID()), '-', ''),
        o.Id,
        o.CourseId,
        c.Title,
        o.CoursePrice,
        o.DiscountPercentSnapshot,
        CASE WHEN o.CoursePrice > o.Amount THEN o.CoursePrice - o.Amount ELSE 0 END,
        0,
        0,
        o.Amount,
        o.SettlementBasePriceSnapshot,
        o.ContentOwnerSharePercentSnapshot,
        o.LicenseCode,
        o.CreatedAt,
        o.UpdatedAt
    FROM Orders o
    JOIN Courses c ON c.Id = o.CourseId
    WHERE o.CourseId IS NOT NULL
      AND NOT EXISTS (SELECT 1 FROM OrderItems oi WHERE oi.OrderId = o.Id);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625213408_AddOrderItemsAndInvoice'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260625213408_AddOrderItemsAndInvoice', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625215208_AddOrderItemContentOwnerSnapshot'
)
BEGIN
    ALTER TABLE [OrderItems] ADD [ContentOwnerIdSnapshot] nvarchar(64) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625215208_AddOrderItemContentOwnerSnapshot'
)
BEGIN
    ALTER TABLE [OrderItems] ADD [ContentOwnerNameSnapshot] nvarchar(256) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625215208_AddOrderItemContentOwnerSnapshot'
)
BEGIN
    CREATE INDEX [IX_OrderItems_ContentOwnerIdSnapshot] ON [OrderItems] ([ContentOwnerIdSnapshot]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625215208_AddOrderItemContentOwnerSnapshot'
)
BEGIN

    UPDATE oi
    SET oi.ContentOwnerIdSnapshot = c.ContentOwnerId,
        oi.ContentOwnerNameSnapshot = COALESCE(co.Name, '')
    FROM OrderItems oi
    JOIN Courses c ON c.Id = oi.CourseId
    LEFT JOIN ContentOwners co ON co.Id = c.ContentOwnerId;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260625215208_AddOrderItemContentOwnerSnapshot'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260625215208_AddOrderItemContentOwnerSnapshot', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260626140234_AddContentOwnerPayments'
)
BEGIN
    CREATE TABLE [ContentOwnerPayments] (
        [Id] nvarchar(64) NOT NULL,
        [ContentOwnerId] nvarchar(64) NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [PaymentDate] datetime2 NOT NULL,
        [ReceiptImage] nvarchar(1024) NOT NULL,
        [TrackingNumber] nvarchar(128) NULL,
        [Description] nvarchar(1024) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_ContentOwnerPayments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ContentOwnerPayments_ContentOwners_ContentOwnerId] FOREIGN KEY ([ContentOwnerId]) REFERENCES [ContentOwners] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260626140234_AddContentOwnerPayments'
)
BEGIN
    CREATE INDEX [IX_ContentOwnerPayments_ContentOwnerId] ON [ContentOwnerPayments] ([ContentOwnerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260626140234_AddContentOwnerPayments'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260626140234_AddContentOwnerPayments', N'10.0.0');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260626144219_AddCourseCommentsAndRatingStats'
)
BEGIN
    ALTER TABLE [Courses] ADD [AverageRating] float NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260626144219_AddCourseCommentsAndRatingStats'
)
BEGIN
    ALTER TABLE [Courses] ADD [CommentCount] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260626144219_AddCourseCommentsAndRatingStats'
)
BEGIN
    CREATE TABLE [CourseComments] (
        [Id] nvarchar(64) NOT NULL,
        [CourseId] nvarchar(64) NOT NULL,
        [AuthorName] nvarchar(256) NOT NULL,
        [AuthorPhone] nvarchar(20) NULL,
        [Content] nvarchar(2000) NOT NULL,
        [Rating] int NOT NULL,
        [IsApproved] bit NOT NULL,
        [IsDeleted] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        CONSTRAINT [PK_CourseComments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CourseComments_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260626144219_AddCourseCommentsAndRatingStats'
)
BEGIN
    CREATE INDEX [IX_CourseComments_CourseId] ON [CourseComments] ([CourseId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260626144219_AddCourseCommentsAndRatingStats'
)
BEGIN
    CREATE INDEX [IX_CourseComments_IsApproved] ON [CourseComments] ([IsApproved]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260626144219_AddCourseCommentsAndRatingStats'
)
BEGIN
    CREATE INDEX [IX_CourseComments_IsDeleted] ON [CourseComments] ([IsDeleted]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260626144219_AddCourseCommentsAndRatingStats'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260626144219_AddCourseCommentsAndRatingStats', N'10.0.0');
END;

COMMIT;
GO

