CREATE DATABASE CRMDB

USE CRMDB

--TABLE--

CREATE TABLE Employee(
IdEmployee INT  IDENTITY(1,1),
username VARCHAR(100),
IsActive BIT NOT NULL,
EmployeeNumber INT PRIMARY KEY,
EmployeeName VARCHAR(100),
EmployeeLastName VARCHAR(100),
)



CREATE TABLE Customer(
IdCustomer INT PRIMARY KEY IDENTITY(1,1),
Name VARCHAR(50),
LastNamer VARCHAR(50),
Email VARCHAR(100),
Phone VARCHAR(20),
Adress VARCHAR(200),
IdentificationNumber INT,
IsCustomerActicve BIT NOT NULL,
TypeOfCustomer VARCHAR(50)
);


CREATE TABLE Card(
IdCard INT PRIMARY KEY IDENTITY(1,1),
NumberCard VARBINARY(16),
OpenDate VARCHAR(200),
ExprirationDate DATE,
BillingCycle VARCHAR(50),
Balance DECIMAL(18,2),
IsCardActive BIT NOT NULL,
IdCustomer INT,
FOREIGN KEY (IdCustomer) REFERENCES customer(IdCustomer)
);

CREATE TABLE Note(
IdNote INT PRIMARY KEY IDENTITY(1,1),
Detail TEXT,
CreationDate DATETIME DEFAULT GETDATE(),
CreatedBy VARCHAR(100),
IdCustomer int,
EmployeeNumber INT,
FOREIGN KEY (EmployeeNumber) REFERENCES Employee(EmployeeNumber),
FOREIGN KEY (IdCustomer) REFERENCES customer(IdCustomer)
);



CREATE TABLE escalation(
IdEscalation INT PRIMARY KEY IDENTITY(1,1),
Departament VARCHAR(100),
Notes TEXT,
CreationDate DATETIME DEFAULT GETDATE(),
IdCustomer INT,
EmployeeNumber INT,
FOREIGN KEY (IdCustomer) REFERENCES Customer(IdCustomer),
FOREIGN KEY (EmployeeNumber) REFERENCES Employee(EmployeeNumber)
);

--PROCEDURE INSERT--

CREATE PROCEDURE SP_CustomerInsert
@Name VARCHAR(50),
@LastName VARCHAR(50),
@Email VARCHAR(100),
@Phone VARCHAR(20),
@Adress VARCHAR(200),
@IdentificationNumber INT,
@IsCustomerActicve BIT,
@TypeOfCustomer VARCHAR(50)
AS
BEGIN
  INSERT INTO customer(Name,LastNamer,Email,Phone,Adress,IdentificationNumber,IsCustomerActicve,TypeOfCustomer)
  Values (@Name,@LastName,@Email,@Phone,@Adress,@IdentificationNumber,@IsCustomerActicve,@TypeOfCustomer)
END


CREATE PROCEDURE SP_CardInsert
@NumberCard VARBINARY(16),
@OpenDate VARCHAR(200),
@ExprirationDate DATE,
@BillingCycle VARCHAR(50),
@Balance DECIMAL(18,2),
@IsCardActive BIT,
@IdCustomer INT
AS
BEGIN
  DECLARE @CardEncrypt VARBINARY(256);
  SET @CardEncrypt = ENCRYPTBYPASSPHRASE('root', @NumberCard);

  INSERT INTO card(NumberCard,OpenDate,ExprirationDate,BillingCycle,Balance,IsCardActive,IdCustomer)
  Values (@CardEncrypt,@OpenDate,@ExprirationDate,@BillingCycle,@Balance,@IsCardActive,@IdCustomer)
END


CREATE PROCEDURE SP_NoteInsert
@Detail TEXT,
@IdCustomer INT
AS
 BEGIN
  INSERT INTO NOTE (Detail,IdCustomer)
  VALUES(@Detail,@IdCustomer)
END

--PROCEDURE OBTAIN--

CREATE PROCEDURE SP_CardObtain
@IdCustomer int

AS
 BEGIN
  SELECT IdCard, CONVERT(VARCHAR(16), DECRYPTBYPASSPHRASE('root',NumberCard ))
   NumberCard, 
   OpenDate,ExprirationDate,
   BillingCycle,
   Balance, 
   IsCardActive,
   IdCustomer 
   FROM card WHERE IdCard = @IdCustomer
END


CREATE PROCEDURE SP_NoteObtain
@IdCustomer int

AS
 BEGIN
  SELECT * FROM NOTE 
  WHERE IdCustomer = @IdCustomer
END


--UPDATE CUSTOMER--

CREATE PROCEDURE SP_UpdateCustomer
@IdCustomer INT,
@Name VARCHAR(50),
@LastName VARCHAR(50),
@Email VARCHAR(100),
@Phone VARCHAR(20),
@Adress VARCHAR(200),
@IdentificationNumber INT,
@IsCustomerActicve BIT,
@TypeOfCustomer VARCHAR(50)

AS
 BEGIN
  UPDATE customer 
   SET 
    Name = @Name,
    LastNamer = @LastName,
    Email = @Email,
	Phone = @Phone,
    Adress = @Adress,
	IdentificationNumber = @IdentificationNumber,
	IsCustomerActicve = @IsCustomerActicve,
	TypeOfCustomer = @TypeOfCustomer
 END



--UPDATE Employee

CREATE PROCEDURE SP_UpdateEmployee
@IdEmployee INT,
@username VARCHAR(100),
@IsActive BIT,
@EmployeeNumber INT,
@EmployeeName VARCHAR(100),
@EmployeeLastName VARCHAR(100)
AS
 BEGIN
   UPDATE Employee
   SET
   username = @username,
   IsActive = @IsActive,
   EmployeeNumber = @EmployeeNumber,
   EmployeeName  = @EmployeeName,
   EmployeeLastName = @EmployeeLastName
END

--DELETE NOTE--

CREATE PROCEDURE SP_DeleteNote
@IdNote INT
AS
 BEGIN
  DELETE FROM NOTE WHERE IdNote = @IdNote;
End

