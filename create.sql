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

create table Users(
UserId int primary key identity,
Nick varchar(69) not null,
Password varchar(69) not null,
PhoneNumber int not null,
EmailAddress varchar(69) not null,
ForDelete bit default(0),
RowVersion timestamp
);

Create table Adresses(
AdressId int primary key identity,
RoadName varchar(255),
HouseNumber int,
UserId int,
FOREIGN KEY (UserId) REFERENCES Users(UserId),
RowVersion timestamp
);

create table Orders(
OrderId int primary key identity,
UserId int,
FOREIGN KEY (UserId) REFERENCES Users(UserId),
OrdareDate date,
Realized bit default(0),
RowVersion timestamp
);

create table OrderProduct(
OrderProductId int primary key identity,
ProductId int,
OrderId int,
FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
Quantity decimal not null,
RowVersion timestamp
);

create table Warehouse(
WarehouseId int primary key identity,
ProductId int,
FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
ExpiryDate date not null,
Quantity decimal not null,
RowVersion timestamp
);