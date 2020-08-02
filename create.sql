--@(#) script.ddl

CREATE TABLE [dbo].[graduates]
(
	id int IDENTITY (1, 1) NOT NULL,
	title varchar (255),
	year int,
	PRIMARY KEY(id)
);

CREATE TABLE [dbo].[teachers]
(
	id int IDENTITY (1, 1) NOT NULL,
	fullName NVARCHAR (255),
	PRIMARY KEY(id)
);

CREATE TABLE [dbo].[users]
(
	id int IDENTITY (1, 1) NOT NULL,
	fullName NVARCHAR (255),
	password NVARCHAR (255),
	email NVARCHAR (255),
	isActive TINYINT,
	isAdmin TINYINT,
	passwordHash VARBINARY (1024) NOT NULL,
    	passwordSalt VARBINARY (1024) NOT NULL,
	PRIMARY KEY(id)
);

CREATE TABLE [dbo].[chronicles]
(
	id int IDENTITY (1, 1) NOT NULL,
	title NVARCHAR (255),
	date date,
	folderUrl NVARCHAR (255),
	fkUser integer NULL,
	PRIMARY KEY(id),
	CONSTRAINT Creates FOREIGN KEY(fkUser) REFERENCES Users (id)
);

CREATE TABLE [dbo].[classes]
(
	id int IDENTITY (1, 1) NOT NULL,
	title NVARCHAR (255),
	fkTeacher integer NULL,
	PRIMARY KEY(id),
	CONSTRAINT Leads FOREIGN KEY(fkTeacher) REFERENCES Teachers (id)
);

CREATE TABLE [dbo].[reservations]
(
	id int IDENTITY (1, 1) NOT NULL,
	title NVARCHAR (255),
	startTime datetime,
	endTime datetime,
	email NVARCHAR (255),
	isDeleted TINYINT,
	reminderSent TINYINT,
	fkUser integer NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT Makes FOREIGN KEY(fkUser) REFERENCES Users (id)
);

CREATE TABLE [dbo].[Photos]
(
	id int IDENTITY (1, 1) NOT NULL,
	title nvarchar (255),
	url nvarchar (255),
	fkChronicle integer NOT NULL,
	PRIMARY KEY(id, fkChronicle),
	CONSTRAINT Consists_of FOREIGN KEY(fkChronicle) REFERENCES Chronicles (id)
);

CREATE TABLE [dbo].[students]
(
	id int IDENTITY (1, 1) NOT NULL,
	fullName nvarchar (255),
	surnameAfterMarriage nvarchar (255),
	birthdate date,
	workplace nvarchar (255),
	address nvarchar (255),
	phone nvarchar (255),
	email nvarchar (255),
	fkGraduates integer NULL,
	comment nvarchar (255),
	fkClass integer NULL,
	PRIMARY KEY(id),
	CONSTRAINT Has FOREIGN KEY(fkGraduates) REFERENCES Graduates (id),
	CONSTRAINT Include FOREIGN KEY(fkClass) REFERENCES Classes (id)
);
