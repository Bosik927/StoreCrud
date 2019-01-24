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

INSERT INTO Users(Nick,Password,PhoneNumber,EmailAddress) VALUES
('Adam','adam1',123123123,'adam@wp.pl'),
('Stachu','stas1',456456456,'stas@wp.pl'),
('Marcin','marcin1',789789789,'marcin@wp.pl'),
('Filip','filip1',112121212,'filip@wp.pl');

INSERT INTO Adresses(RoadName,HouseNumber,UserId) VALUES
('DrogaDoAdama',1,1),
('DrogaDoStacha',2,2),
('DrogaDoMarcina',3,3),
('DrogaDoFilipa',4,4);

INSERT INTO Orders(UserId, OrdareDate) VALUES
(1,'2017-01-14 08:03:00'),
(1,'2017-01-15 08:04:52'),
(2,'2017-01-16 09:03:52'),
(2,'2017-01-17 12:03:52'),
(2,'2017-01-18 18:03:52'),
(3,'2017-01-19 20:03:52'),
(2,'2017-01-20 08:00:00'),
(1,'2017-01-21 09:34:43');

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
