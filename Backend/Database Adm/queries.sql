CREATE DATABASE CRMDB

USE CRMDB

--TABLE EMPLOYYE--

CREATE TABLE Employee(
IdEmployee INT  IDENTITY(1,1),
username VARCHAR(100),
IsActive BIT NOT NULL,
EmployeeNumber INT PRIMARY KEY,
EmployeeName VARCHAR(100),
EmployeeLastName VARCHAR(100),
)


--TABLE CUSTOMER--

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

--TABLE CARD--

CREATE TABLE Card(
IdCard INT PRIMARY KEY IDENTITY(1,1),
NumberCard VARBINARY(16),
OpenDate DATETIME DEFAULT GETDATE(),
ExprirationDate DATE,
BillingCycle VARCHAR(50),
Balance DECIMAL(18,2),
IsCardActive BIT NOT NULL,
IdCustomer INT,
FOREIGN KEY (IdCustomer) REFERENCES customer(IdCustomer)
);


--TABLE NOTE--

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

--TABLE ESCALATION--

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

--PROCEDURE EMPLOYEE INSERT--

CREATE PROCEDURE SP_EmployeeInsert
@username VARCHAR(100),
@IsActive BIT,
@EmployeeNumber INT,
@EmployeeName VARCHAR(100),
@EmployeeLastName VARCHAR(100)
AS
 begin
   INSERT INTO Employee(username,IsActive,EmployeeNumber,EmployeeName,EmployeeLastName)
   Values (@username,@IsActive,@EmployeeNumber,@EmployeeName,@EmployeeLastName)
end 


--PROCEDURE CUSTOMER INSERT--

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

--PROCEDURE CARD INSERT--

CREATE PROCEDURE SP_CardInsert
@NumberCard VARBINARY(16),
@OpenDate DATETIME,
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

--PROCEDURE NOTE INSERT--

CREATE PROCEDURE SP_NoteInsert
@Detail TEXT,
@IdCustomer INT,
@CreatedBy VARCHAR(100),
@EmployeeNumber INT

AS
 BEGIN
  INSERT INTO NOTE (Detail,IdCustomer,CreatedBy,EmployeeNumber)
  VALUES(@Detail,@IdCustomer,@CreatedBy,@EmployeeNumber)
END

--PROCEDURE ESCALATION INSERT
CREATE PROCEDURE SP_Escalation
@Departament VARCHAR(100),
@Notes TEXT,
@IdCustomer INT,
@EmployeeNumber INT
AS
 BEGIN
  INSERT INTO escalation(Departament,Notes,IdCustomer,EmployeeNumber)
  VALUES (@Departament,@Notes,@IdCustomer,@EmployeeNumber)
END


--PROCEDURE CUSTOMER OBTAIN--

CREATE PROCEDURE SP_CustomerObtain
@IdCustomer int
as
BEGIN
SELECT * FROM Customer
Where IdCustomer = @IdCustomer
end

--PROCEDURE CARD OBTAIN--

CREATE PROCEDURE SP_CardObtain
@IdCustomer int

AS
 BEGIN
  SELECT IdCard, CONVERT(VARCHAR(16), DECRYPTBYPASSPHRASE('root',NumberCard ))AS
   NumberCard, 
   OpenDate,ExprirationDate,
   BillingCycle,
   Balance, 
   IsCardActive,
   IdCustomer 
   FROM card WHERE IdCustomer = @IdCustomer;
END

--PROCEDURE NOTE OBTAIN--

CREATE PROCEDURE SP_NoteObtain
@IdCustomer int

AS
 BEGIN
  SELECT * FROM NOTE 
  WHERE IdCustomer = @IdCustomer
END

--PROCEDURE ESCALATION OBTAIN--
CREATE PROCEDURE SP_EscalationObtain
@IdCustomer int
AS
BEGIN 
 SELECT * FROM escalation
 WHERE IdCustomer = @IdCustomer
END



--UPDATE  CUSTOMER UPDATE--

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
	TypeOfCustomer = @TypeOfCustomer;
 END



--UPDATE  EMPLOYEE UPDATE--

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
   EmployeeLastName = @EmployeeLastName;
END


--PROCEDURE ESCALATION UPDATE

CREATE PROCEDURE SP_EscalationUpdate
@IdEscalation INT,
@Departament VARCHAR(100),
@Notes TEXT,
@IdCustomer INT,
@EmployeeNumber INT
AS
 BEGIN
  UPDATE escalation
  SET Departament = @Departament,
  Notes = @Notes
END

--DELETE NOTE--

CREATE PROCEDURE SP_DeleteNote
@IdNote INT
AS
 BEGIN
  DELETE FROM NOTE WHERE IdNote = @IdNote;
End



--INSERT INFO	---


EXEC SP_EmployeeInsert
@username  = 'uriel809',
@IsActive = 1,
@EmployeeNumber = 1,
@EmployeeName = 'uriel',
@EmployeeLastName = 'Aquino'


EXEC SP_CustomerInsert
   @Name ='Prueba',
   @LastName = 'pruebin',
   @Email ='prueba@GMAIL.COM',
   @Phone ='809566651',
   @Adress ='San luis',
   @IdentificationNumber = 255655,
   @IsCustomerActicve =1,
   @TypeOfCustomer ='GOld'



EXEC SP_CardInsert
    @NumberCard = 5658881598455685,  
    @OpenDate = '30/4/25',      
    @ExprirationDate = '2025-06-28',  
    @BillingCycle = 'Monthly',        
    @Balance = 4000.50,               
    @IsCardActive = 1,                
    @IdCustomer = 2     
	

EXEC SP_NoteInsert
    @Detail = 'HELLO, WELCOME BY HOME',
    @IdCustomer = 2,
    @CreatedBy = 'uriel',
    @EmployeeNumber = 1


EXEC SP_Escalation
   @Departament = 'Administration',
   @Notes = 'Hello world',
   @IdCustomer = 2,
   @EmployeeNumber = 1

 --OBTAIN INFO CARD--

EXEC SP_CardObtain @IdCustomer =1;
EXEC SP_CardObtain @IdCustomer =2;

 --OBTAIN INFO NOTE--

EXEC SP_NoteObtain @IdCustomer = 1;

--OBTAIN INFO CUSTOMER--
EXEC SP_CustomerObtain @IdCustomer = 1;


--OBTAIN INFO ESCALATION
EXEC SP_EscalationObtain @IdCustomer = 2;



select * from Employee
select * from Customer
select * from Card
select * from Note
select * from escalation
