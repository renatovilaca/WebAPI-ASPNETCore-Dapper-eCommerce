CREATE TABLE [dbo].[Users] (
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] VARCHAR(70) NOT NULL,
	[Email] VARCHAR(100) NOT NULL,
	[Gender] CHAR(1) NULL,
	[RG] VARCHAR(15) NULL,
	[CPF] CHAR(14) NULL,
	[Mother] VARCHAR(70) NULL,
	[Status] CHAR(1) NOT NULL,
	[CreatedAt] DATETIMEOFFSET NOT NULL,

	CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

/*One-To-One*/
CREATE TABLE [dbo].[Contacts] (
	[Id] INT IDENTITY(1,1) NOT NULL,
	[UserId] INT NOT NULL,
	[Phone] VARCHAR(15) NULL,
	[CellPhone] VARCHAR(15) NULL,

	CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Contacts_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

/*One-To-Many*/
CREATE TABLE [dbo].[DeliveryAddress] (
	[Id] INT IDENTITY(1,1) NOT NULL,
	[UserId] INT NOT NULL,
	[AddressTitle] VARCHAR(100) NOT NULL,
	[Zipcode] CHAR(10) NOT NULL,
	[State] CHAR(2) NOT NULL,
	[City] VARCHAR(120) NOT NULL,
	[District] VARCHAR(200) NOT NULL,
	[Address] VARCHAR(200) NOT NULL,
	[Number] VARCHAR(20) NULL,
	[Address2] VARCHAR(30) NULL,
	
	CONSTRAINT [PK_DeliveryAddress] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_DeliveryAddress_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE

);

/*Many-To-Many*/
CREATE TABLE [dbo].[Departments] (
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] VARCHAR(100) NOT NULL,
	CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED ([Id] ASC),
);

CREATE TABLE [dbo].[UsersDepartments] (
	[Id] INT IDENTITY(1,1) NOT NULL,
	[UserId] INT NOT NULL,
	[DepartmentId] INT NOT NULL,

	CONSTRAINT [PK_UsersDepartments] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_UsersDepartments_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_UsersDepartments_Departments] FOREIGN KEY ([DepartmentId]) REFERENCES [dbo].[Departments] ([Id]) ON DELETE CASCADE
);
go


-- Store Procedures na tabela de Users
CREATE PROCEDURE dbo.GetUsers
AS
    SELECT * FROM [dbo].[Users]
go

CREATE PROCEDURE dbo.GetUserById
(
	@id int
)
AS
    SELECT * FROM [dbo].[Users] WHERE Id = @id
go

CREATE PROCEDURE dbo.InsertUser
(
	@name varchar(70),
	@email varchar(100),
	@gender char(1),
	@rg varchar(15),
	@cpf char(14),
	@mother varchar(70),
	@status char(1),
	@createdat datetimeoffset
)
AS
	INSERT INTO [dbo].[Users] (Name, Email, Gender, RG, CPF, Mother, Status, CreatedAt) VALUES
	(@name, @email, @gender, @rg, @cpf, @mother, @status, @createdat); SELECT CAST(scope_identity() AS int)
go



CREATE PROCEDURE dbo.UpdateUser 
(
	@id int,
	@name varchar(70),
	@email varchar(100),
	@gender char(1),
	@rg varchar(15),
	@cpf char(14),
	@mother varchar(70),
	@status char(1),
	@createdat datetimeoffset
)
AS
	UPDATE [dbo].[Users] SET 
	Name = @name,
	Email = @email,
	Gender = @gender,
	RG = @rg,
	CPF = @cpf,
	Mother = @mother,
	Status = @status, 
	CreatedAt = @createdat
	WHERE Id = @id
go

CREATE PROCEDURE dbo.DeleteUser
(
	@id int
)
AS
	DELETE FROM [dbo].[Users] WHERE Id = @id
go
