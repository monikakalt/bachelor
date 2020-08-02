--@(#) script.ddl

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
	id integer,
	fk_Userid integer NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT Creates FOREIGN KEY(fk_Userid) REFERENCES User (id)
);

CREATE TABLE Graduates
(
	title varchar (255),
	year int,
	id integer,
	fk_Userid integer NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT Creates FOREIGN KEY(fk_Userid) REFERENCES User (id)
);

CREATE TABLE Reservation
(
	title varchar (255),
	startTime date,
	endTime date,
	email varchar (255),
	id integer,
	fk_Userid integer NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT Makes FOREIGN KEY(fk_Userid) REFERENCES User (id)
);

CREATE TABLE Class
(
	title varchar (255),
	id integer,
	fk_Teacherid integer NOT NULL,
	fk_Graduatesid integer NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT Leads FOREIGN KEY(fk_Teacherid) REFERENCES Teacher (id),
	CONSTRAINT Has FOREIGN KEY(fk_Graduatesid) REFERENCES Graduates (id)
);

CREATE TABLE Photo
(
	photo smallint,
	id integer,
	fk_Chronicleid integer NOT NULL,
	PRIMARY KEY(id, fk_Chronicleid),
	CONSTRAINT Consists of FOREIGN KEY(fk_Chronicleid) REFERENCES Chronicle (id)
);

CREATE TABLE Student
(
	fullName,
	surnameAfterMarriage,
	birthdate,
	classId,
	workplace,
	address,
	phone,
	id integer,
	fk_Classid integer NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT Belongs to FOREIGN KEY(fk_Classid) REFERENCES Class (id)
);
