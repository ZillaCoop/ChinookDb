USE SuperheroesDb;

CREATE TABLE Superhero (
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] nvarchar(50) NOT NULL,
	[Alias] nvarchar(50) NOT NULL,
	[Origin] nvarchar(50) NOT NULL
);

CREATE TABLE Assistant (
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] nvarchar(50) NOT NULL,
);

CREATE TABLE Power (
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] nvarchar(50) NOT NULL,
	[Description] nvarchar(200) NOT NULL,
);