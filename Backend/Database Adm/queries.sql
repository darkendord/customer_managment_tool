CREATE DATABASE CRMDB

USE CRMDB

--TABLE--

CREATE TABLE customer(
IdCustomer INT PRIMARY KEY IDENTITY(1,1),
Name VARCHAR(50),
Email VARCHAR(100),
Phone VARCHAR(20),
Adress VARCHAR(200)
);


CREATE TABLE card(
IdCard INT PRIMARY KEY IDENTITY(1,1),
NumberCard VARBINARY(16),
ExprirationDate DATE,
IdCustomer INT,
FOREIGN KEY (IdCustomer) REFERENCES customer(IdCustomer)

);

CREATE TABLE NOTE(
IdNote INT PRIMARY KEY IDENTITY(1,1),
Detail TEXT,
CreationDate DATETIME DEFAULT GETDATE(),
IdCustomer int
FOREIGN KEY (IdCustomer) REFERENCES customer(IdCustomer)
);

--PROCEDURE INSERT--

CREATE PROCEDURE SP_CustomerInsert
@Name VARCHAR(50),
@Email VARCHAR(100),
@Phone VARCHAR(20),
@Adress VARCHAR(200)
AS
BEGIN
  INSERT INTO customer(Name,Email,Phone,Adress)
  Values (@Name,@Email,@Phone,@Adress)
END


CREATE PROCEDURE SP_CardInsert
@NumberCard VARBINARY(16),
@ExprirationDate DATE,
@IdCustomer INT
AS
BEGIN
  DECLARE @CardEncrypt VARBINARY(256);
  SET @CardEncrypt = ENCRYPTBYPASSPHRASE('root', @NumberCard);

  INSERT INTO card(NumberCard,ExprirationDate,IdCard)
  Values (@CardEncrypt,@ExprirationDate,@IdCustomer)
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
  AS NumberCard, ExprirationDate FROM card WHERE IdCard = @IdCustomer
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
@Email VARCHAR(100),
@Phone VARCHAR(20),
@Adress VARCHAR(200)

AS
 BEGIN
  UPDATE customer 
   SET 
    Name = @Name,
    Email = @Email,
    Phone = @Phone,
    Adress = @Adress
 END



--DELETE NOTE--

CREATE PROCEDURE SP_DeleteNote
@IdNote INT
AS
 BEGIN
  DELETE FROM NOTE WHERE IdNote = @IdNote;
End