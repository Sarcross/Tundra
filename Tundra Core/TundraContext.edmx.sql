
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/26/2016 15:01:46
-- Generated from EDMX file: C:\Users\antzy_000\RiderProjects\Tundra\Tundra Core\TundraContext.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TundraCore];
GO
IF SCHEMA_ID(N'TundraCore') IS NULL EXECUTE(N'CREATE SCHEMA [TundraCore]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [TundraCore].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [RequiresValidation] bit  NOT NULL
);
GO

-- Creating table 'Accounts'
CREATE TABLE [TundraCore].[Accounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Balance] decimal(18,2)  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Goals'
CREATE TABLE [TundraCore].[Goals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [TargetAmount] decimal(18,2)  NOT NULL,
    [AcruedAmount] decimal(18,2)  NOT NULL,
    [ContributionAmount] decimal(18,2)  NOT NULL,
    [Accomplished] bit  NOT NULL,
    [Created] datetime  NOT NULL,
    [Deadline] datetime  NOT NULL,
    [Modified] datetime  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Transactions'
CREATE TABLE [TundraCore].[Transactions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Amount] decimal(18,2)  NOT NULL,
    [UserId] int  NOT NULL
);
GO

-- Creating table 'Accounts_SavingsAccount'
CREATE TABLE [TundraCore].[Accounts_SavingsAccount] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Accounts_CheckingAccount'
CREATE TABLE [TundraCore].[Accounts_CheckingAccount] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Transactions_Income'
CREATE TABLE [TundraCore].[Transactions_Income] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'Transactions_Expense'
CREATE TABLE [TundraCore].[Transactions_Expense] (
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [TundraCore].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Accounts'
ALTER TABLE [TundraCore].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Goals'
ALTER TABLE [TundraCore].[Goals]
ADD CONSTRAINT [PK_Goals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions'
ALTER TABLE [TundraCore].[Transactions]
ADD CONSTRAINT [PK_Transactions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Accounts_SavingsAccount'
ALTER TABLE [TundraCore].[Accounts_SavingsAccount]
ADD CONSTRAINT [PK_Accounts_SavingsAccount]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Accounts_CheckingAccount'
ALTER TABLE [TundraCore].[Accounts_CheckingAccount]
ADD CONSTRAINT [PK_Accounts_CheckingAccount]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_Income'
ALTER TABLE [TundraCore].[Transactions_Income]
ADD CONSTRAINT [PK_Transactions_Income]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Transactions_Expense'
ALTER TABLE [TundraCore].[Transactions_Expense]
ADD CONSTRAINT [PK_Transactions_Expense]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'Accounts'
ALTER TABLE [TundraCore].[Accounts]
ADD CONSTRAINT [FK_UserAccount]
    FOREIGN KEY ([UserId])
    REFERENCES [TundraCore].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccount'
CREATE INDEX [IX_FK_UserAccount]
ON [TundraCore].[Accounts]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'Goals'
ALTER TABLE [TundraCore].[Goals]
ADD CONSTRAINT [FK_UserGoal]
    FOREIGN KEY ([UserId])
    REFERENCES [TundraCore].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserGoal'
CREATE INDEX [IX_FK_UserGoal]
ON [TundraCore].[Goals]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'Transactions'
ALTER TABLE [TundraCore].[Transactions]
ADD CONSTRAINT [FK_UserTransaction]
    FOREIGN KEY ([UserId])
    REFERENCES [TundraCore].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTransaction'
CREATE INDEX [IX_FK_UserTransaction]
ON [TundraCore].[Transactions]
    ([UserId]);
GO

-- Creating foreign key on [Id] in table 'Accounts_SavingsAccount'
ALTER TABLE [TundraCore].[Accounts_SavingsAccount]
ADD CONSTRAINT [FK_SavingsAccount_inherits_Account]
    FOREIGN KEY ([Id])
    REFERENCES [TundraCore].[Accounts]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Accounts_CheckingAccount'
ALTER TABLE [TundraCore].[Accounts_CheckingAccount]
ADD CONSTRAINT [FK_CheckingAccount_inherits_Account]
    FOREIGN KEY ([Id])
    REFERENCES [TundraCore].[Accounts]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Transactions_Income'
ALTER TABLE [TundraCore].[Transactions_Income]
ADD CONSTRAINT [FK_Income_inherits_Transaction]
    FOREIGN KEY ([Id])
    REFERENCES [TundraCore].[Transactions]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Transactions_Expense'
ALTER TABLE [TundraCore].[Transactions_Expense]
ADD CONSTRAINT [FK_Expense_inherits_Transaction]
    FOREIGN KEY ([Id])
    REFERENCES [TundraCore].[Transactions]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------