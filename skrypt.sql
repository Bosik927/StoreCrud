
use master;
drop database Warzywniak;
create database Warzywniak;

use Warzywniak;


create table Products(
ProductId int primary key identity,
ProductName varchar(69) not null,
ProductPrice decimal not null,
ProductUnit varchar(10) not null default('kg'),
Vat int not null,
ForDelete bit default(0),
constraint chk_vat check(Vat=5 or Vat=8 or Vat=23),
RowVersion timestamp
);

INSERT INTO Products (ProductName, ProductPrice, ProductUnit, Vat) VALUES 
('Jablka Biale',4,'kg',5),
('Gruszki male',5,'kg',5),
('Banany Afrykanskie',2.42,'kg',8),
('Kiwi z nikad',3.23,'sztuka',8),
('Pieprz mielony',3.2,'opakowanie',23),
('Herbata Zilona',2.2,'opakowanie',23),
('Jablka Czerwone',4.40,'kg',5),
('Gruszki duze',6,'kg',5),
('Banany Azjatyckie',3.42,'kg',8),
('Kiwi skads',4.23,'sztuka',8),
('Pieprz nie mielony',4.2,'opakowanie',23),
('Herbata Czarna',1.2,'opakowanie',23);

create table Users(
UserId int primary key identity,
Nick varchar(69) not null,
Password varchar(69) not null,
PhoneNumber int not null,
EmailAddress varchar(69) not null,
ForDelete bit default(0),
RowVersion timestamp default CURRENT_TIMESTAMP
);

INSERT INTO Users(Nick,Password,PhoneNumber,EmailAddress) VALUES
('Adam','adam1',123123123,'adam@wp.pl'),
('Stachu','stas1',456456456,'stas@wp.pl'),
('Marcin','marcin1',789789789,'marcin@wp.pl'),
('Filip','filip1',112121212,'filip@wp.pl');

Create table Adresses(
AdressId int primary key identity,
RoadName varchar(255),
HouseNumber int,
UserId int,
FOREIGN KEY (UserId) REFERENCES Users(UserId),
RowVersion timestamp
);

INSERT INTO Adresses(RoadName,HouseNumber,UserId) VALUES
('DrogaDoAdama',1,1),
('DrogaDoStacha',2,2),
('DrogaDoMarcina',3,3),
('DrogaDoFilipa',4,4);

create table Orders(
OrderId int primary key identity,
UserId int,
FOREIGN KEY (UserId) REFERENCES Users(UserId),
OrdareDate date,
Realized bit default(0),
RowVersion timestamp
);

INSERT INTO Orders(UserId, OrdareDate) VALUES
(1,'2017-01-14 08:03:00'),
(1,'2017-01-15 08:04:52'),
(2,'2017-01-16 09:03:52'),
(2,'2017-01-17 12:03:52'),
(2,'2017-01-18 18:03:52'),
(3,'2017-01-19 20:03:52'),
(2,'2017-01-20 08:00:00'),
(1,'2017-01-21 09:34:43');

create table OrderProduct(
OrderProductId int primary key identity,
ProductId int,
OrderId int,
FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
Quantity decimal not null,
RowVersion timestamp
);

INSERT INTO OrderProduct(ProductId,OrderId,Quantity) VALUES
--Order 1
(1,1,1),
(2,1,5),
(3,1,10),
(4,1,12),
--Order 2
(3,2,3.3),
(4,2,12),
(5,2,11),
(6,2,11),
(7,2,12),
(8,2,11),
(10,2,11),
--Order 3
(5,3,3.3),
(12,3,12),
(4,3,11),
--Order 4
(1,4,5),
--Order 5
(2,5,1),
--Order 6
(5,6,21),
(12,6,121),
--Order 7
(5,7,3),
--Order 8
(11,8,12);

create table Warehouse(
WarehouseId int primary key identity,
ProductId int,
FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
ExpiryDate date not null,
Quantity decimal not null,
RowVersion timestamp
);

INSERT INTO Warehouse(ProductId,ExpiryDate,Quantity) VALUES
(1,'2018-12-11',2),
(1,'2018-12-12',5),
(2,'2018-12-13',80),
(2,'2019-01-21',23),
(3,'2019-01-01',44),
(4,'2021-01-01',980),
(5,'2021-01-21',456),
(5,'2021-01-21',11),
(6,'2017-01-21',57),
(6,'2017-01-21',999),
(7,'2017-01-21',12),
(7,'2017-01-21',234),
(8,'2017-01-21',567),
(8,'2017-01-21',56),
(9,'2017-01-21',90),
(9,'2017-01-21',43),
(10,'2017-01-21',76),
(10,'2017-01-21',1337),
(11,'2017-01-21',69),
(11,'2017-01-21',84),
(11,'2017-01-21',32),
(11,'2017-01-21',12),
(12,'2017-01-21',87),
(12,'2017-01-21',44),
(12,'2017-01-21',33);

--Triggery
GO

create trigger DeletingUserTrigger
on Users
INSTEAD OF delete
as 
begin
	declare @IdUser int;
	select @IdUser=i.UserId from deleted i;
	IF EXISTS (Select * from Orders where UserId=@IdUser)
		UPDATE Users set ForDelete=1 where UserId=@IdUser;
	ELSE
	begin
		delete from Adresses where UserId=@IdUser;
		delete from Users where UserId=@IdUser;
	end
end
GO

create trigger CleanDbAfterDeletingOrder
on Orders
INSTEAD OF delete
as 
begin
	declare @IdOrder int;
	select @IdOrder=i.OrderId from deleted i;
	delete from OrderProduct where OrderId=@IdOrder;
	delete from Orders where OrderId=@IdOrder;
end
GO

create trigger CleanDbAfterDeletingProduct
on Products
INSTEAD OF delete
as 
begin
	declare @IdProduct int;
	select @IdProduct=i.ProductId from deleted i;
	IF EXISTS (Select * from OrderProduct where ProductId=@IdProduct)
		UPDATE Products set ForDelete=1 where ProductId=@IdProduct;
	ELSE
	begin
	delete from Warehouse where ProductId=@IdProduct;
	delete from Products where ProductId=@IdProduct;
	end
end
GO

create trigger AddWarehousesExceededExpiryDate
on Warehouse
for insert, update
as 
begin
	declare @expdate date;
	select @expdate=i.ExpiryDate from inserted i;

	if(@expdate < GETDATE())
	begin 
		rollback;
		raiserror('*** DATA WA¯NOŒCI ZOSTA£A PRZEKROCZONA ***',16,1);

	end
end
GO

create trigger AddOrderProductMoreThanWarehouse
on OrderProduct
for insert, update
as 
begin
	declare @quantity int;
	declare @IdProduct int;
	declare @quantityWH int;
	select @quantity=i.Quantity from inserted i;
	select @IdProduct=i.ProductId from inserted i;
	select @quantityWH = sum(w.Quantity) from Warehouse w where w.ProductId= @IdProduct;

	if(@quantity > @quantityWH)
	begin 
		rollback;
		raiserror('*** BRAK WYSTARCZAJ¥CEJ ILOŒCI TOWARU W MAGAZYNIE ***',16,1);

	end
end
GO

create procedure DailyExceededExpiryDate
as 
begin
	delete from Warehouse where ExpiryDate < GETDATE();
	delete from Users where ForDelete=1 and UserId not IN (select UserId from Orders);
	delete from Products where ForDelete=1 and ProductId not IN (select ProductId from OrderProduct);
end

GO

CREATE PROCEDURE GetAllProductsForUserId
@UserId INT
as
begin
	SELECT p.ProductName, p.ProductPrice, p.ProductUnit, op.Quantity FROM Products p 
	inner join OrderProduct op 
	ON p.ProductId = op.ProductId
	inner join Orders o 
	ON o.OrderId = op.OrderId
	WHERE UserId = @UserId;
end;
GO

CREATE PROCEDURE GetAllOrdersValueForUserId
@UserId INT
as
begin
	SELECT sum(op.Quantity * p.ProductPrice) FROM Products p 
	inner join OrderProduct op 
	ON p.ProductId = op.ProductId
	inner join Orders o 
	ON o.OrderId = op.OrderId
	WHERE UserId = @UserId;
end;
GO


CREATE TRIGGER  CannotAddRepitedUser
ON Users
INSTEAD OF INSERT
AS
BEGIN
   IF EXISTS (
      SELECT *
      FROM inserted i
      WHERE i.Nick in(Select Distinct Nick from Users)
   )
   BEGIN
      ROLLBACK TRANSACTION
	  RAISERROR ('Cannot create user with this name.' ,16,1)
   END 
   ELSE IF EXISTS (
      SELECT *
      FROM inserted i
      WHERE i.EmailAddress in(Select Distinct EmailAddress from Users)
   )
   BEGIN
      ROLLBACK TRANSACTION
	  RAISERROR ('User with this email already exist.' ,16,1)
   END 
   ELSE IF EXISTS (
      SELECT *
      FROM inserted i
      WHERE i.PhoneNumber in(Select Distinct PhoneNumber from Users)
   )
   BEGIN
      ROLLBACK TRANSACTION
	  RAISERROR ('User with this phone number already exist.' ,16,1)
   END 
   ELSE
	BEGIN
		INSERT INTO Users(Nick,Password,PhoneNumber,EmailAddress) 
			Select i.Nick, i.Password, i.PhoneNumber, i.EmailAddress From inserted i
   END
END
GO


CREATE TRIGGER  CannotAddRepitedProductName
ON Products
INSTEAD OF INSERT
AS
BEGIN
   IF EXISTS (
      SELECT *
      FROM inserted i
      WHERE i.ProductName in (Select Distinct ProductName from Products)
   )
   BEGIN
      ROLLBACK TRANSACTION
	  RAISERROR ('Product with this name already exist.' ,16,1);
   END 
	ELSE
	BEGIN
		INSERT INTO Products(ProductName, ProductPrice, ProductUnit, Vat) 
			Select i.ProductName, i.ProductPrice, i.ProductUnit, i.Vat From inserted i;
   END
END
GO

CREATE PROCEDURE SaleSummary
@BeginDate DATE,
@EndDate DATE
as
begin 
	select p.ProductName, sum(op.Quantity) Quantity, sum(Quantity*p.ProductPrice) Price from Products p
	join OrderProduct op on op.ProductId=p.ProductId
	join Orders o on o.OrderId=op.OrderId
	where o.OrdareDate between @BeginDate and @EndDate
	group by p.ProductName
end
GO

CREATE PROCEDURE SaleSummaryRealized
@BeginDate DATE,
@EndDate DATE
as
begin 
	select p.ProductName, sum(op.Quantity) Quantity, sum(Quantity*p.ProductPrice) Price from Products p
	join OrderProduct op on op.ProductId=p.ProductId
	join Orders o on o.OrderId=op.OrderId
	where (o.OrdareDate between @BeginDate and @EndDate)
	and o.Realized=1
	group by p.ProductName
end
GO
