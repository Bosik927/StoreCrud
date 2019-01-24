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
