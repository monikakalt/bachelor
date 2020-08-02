--@(#) script.ddl

CREATE TABLE Graduates
(
	title varchar (255),
	year int,
	id integer,
	PRIMARY KEY(id)
);

CREATE TABLE Teacher
(
	fullName varchar (255),
	id integer,
	PRIMARY KEY(id)
);

CREATE TABLE User
(
	fullName varchar (255),
	password varchar (255),
	email varchar (255),
	isActive boolean,
	isAdmin boolean,
	id integer,
	PRIMARY KEY(id)
);

CREATE TABLE Chronicle
(
	title varchar (255),
	date date,
	folderUrl varchar (255),
	id integer,
	fkUser integer NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT Creates FOREIGN KEY(fkUser) REFERENCES User (id)
);

CREATE TABLE Class
(
	title varchar (255),
	id integer,
	fkTeacher integer NOT NULL,
	fkUser integer NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT Leads FOREIGN KEY(fkTeacher) REFERENCES Teacher (id),
	CONSTRAINT Creates FOREIGN KEY(fkUser) REFERENCES User (id)
);

CREATE TABLE Reservation
(
	title varchar (255),
	start timestamp,
	end timestamp,
	email varchar (255),
	isDeleted boolean,
	reminderSent boolean,
	id integer,
	fkUser integer NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT Makes FOREIGN KEY(fkUser) REFERENCES User (id)
);

CREATE TABLE Photo
(
	title varchar (255),
	url varchar (255),
	id integer,
	fkChronicle integer NOT NULL,
	PRIMARY KEY(id, fkChronicle),
	CONSTRAINT Consists_of FOREIGN KEY(fkChronicle) REFERENCES Chronicle (id)
);

CREATE TABLE Student
(
	fullName varchar (255),
	surnameAfterMarriage varchar (255),
	birthdate date,
	workplace varchar (255),
	address varchar (255),
	phone varchar (255),
	email varchar (255),
	comment varchar (255),
	id integer,
	fkGraduates integer NOT NULL,
	fkClass integer NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT Has FOREIGN KEY(fkGraduates) REFERENCES Graduates (id),
	CONSTRAINT Include FOREIGN KEY(fkClass) REFERENCES Class (id)
);
