/*
use [master];
IF EXISTS (SELECT * FROM SYS.DATABASES WHERE NAME='Project_6_semester')
BEGIN
	DROP DATABASE Project_6_semester;
END
CREATE DATABASE Project_6_semester;
*/

use Project_6_semester;
GO

drop table if exists Rent;
drop table if exists Employees;
drop table if exists Employees_roles;
drop table if exists Pavilion;
drop table if exists Pavilion_statuses;
drop table if exists Mall;
drop table if exists Rent_statuses;
drop table if exists Tenants;
GO

drop table if exists Employees_roles;
create table Employees_roles(
	[role_id] [bigint] NOT NULL PRIMARY KEY IDENTITY,
	[role_name] nvarchar(255) NOT NULL
);

drop table if exists Employees;
create table Employees(
	[employe_id] bigint NOT NULL PRIMARY KEY IDENTITY,
	[name] nvarchar(255) NOT NULL,
	[surname] nvarchar(255) NOT NULL,
	[patronymic] nvarchar(255) NOT NULL,
	[login] nvarchar(255) NOT NULL,
	[password] nvarchar(255) NOT NULL,
	[role] [bigint] NOT NULL,
	[phone] nvarchar(40) NOT NULL,
	[sex] nvarchar(1) NOT NULL
	check(sex = 'М' or sex = 'Ж'),
	[photo] varbinary(max) default null,
	FOREIGN KEY ([role]) REFERENCES Employees_roles ([role_id])
);

drop table if exists Mall_statuses;
create table Mall_statuses(
	[status_id] [bigint] NOT NULL PRIMARY KEY IDENTITY,
	[status_name] nvarchar(255) NOT NULL
);

drop table if exists Mall;
create table Mall(
	[mall_id] [bigint] NOT NULL PRIMARY KEY IDENTITY,
	[mall_name] nvarchar(255) NOT NULL,
	[status_id] bigint NOT NULL,
	[cost] decimal(38,5) NOT NULL,
	[number_of_pavilion] [int] NOT NULL,
	[city] nvarchar(255) NOT NULL,
	[value_added_factor] [float] NOT NULL,
	[number_of_storeys] [int] NOT NULL,
	[photo] varbinary(max) default null,
	FOREIGN KEY ([status_id]) REFERENCES Mall_statuses ([status_id])
);

drop table if exists Pavilion_statuses;
create table Pavilion_statuses(
	[status_id] [bigint] NOT NULL PRIMARY KEY IDENTITY,
	[status_name] nvarchar(255) NOT NULL
);

drop table if exists Pavilion;
create table Pavilion(
	[mall_id] [bigint] NOT NULL,
	[pavilion_number] nvarchar(20) NOT NULL,
	PRIMARY KEY ([mall_id], [pavilion_number]),
	[floor] [int] NOT NULL,
	[status_id] [bigint] NOT NULL,
	[square] decimal(20,5) NOT NULL,
	[cost_per_square_meter] decimal(20,5) NOT NULL,
	[value_added_factor] [float] NOT NULL,
	FOREIGN KEY ([status_id]) REFERENCES Pavilion_statuses ([status_id]),
	FOREIGN KEY ([mall_id]) REFERENCES Mall ([mall_id])
);

drop table if exists Tenants;
create table Tenants(
	[tenant_id] [bigint] NOT NULL PRIMARY KEY IDENTITY,
	[company_name] nvarchar(255) NOT NULL,
	[phone] nvarchar(40) NOT NULL,
	[address] nvarchar(255) NOT NULL
);

drop table if exists Rent_statuses;
create table Rent_statuses(
	[status_id] [bigint] NOT NULL PRIMARY KEY IDENTITY,
	[status_name] nvarchar(255) NOT NULL
);

drop table if exists Rent;
create table Rent(
	[rent_id] [bigint] NOT NULL PRIMARY KEY IDENTITY,
	[tenant_id] [bigint] NOT NULL,
	[mall_id] [bigint] NOT NULL,
	[pavilion_number] nvarchar(20) NOT NULL,
	[employee_id] [bigint] NOT NULL,
	[status_id] [bigint] NOT NULL,
	[start_date] [date] NOT NULL,
	[end_date] [date] NOT NULL,
	FOREIGN KEY ([employee_id]) REFERENCES Employees ([employe_id]),
	FOREIGN KEY ([tenant_id]) REFERENCES Tenants ([tenant_id]),
	FOREIGN KEY ([status_id]) REFERENCES Rent_statuses ([status_id]),
	FOREIGN KEY ([mall_id], [pavilion_number]) REFERENCES Pavilion ([mall_id], [pavilion_number])
);

insert into Employees_roles([role_name]) values
('Администратор'),
('Менеджер А'),
('Менеджер С'),
('Удалён');

insert into Employees(patronymic,surname, name,login,password,role,phone,sex) values
('Михеевич', 'Чашин', 'Елизар', 'Elizor@gmai.com', 'yntiRS', 1, '+7 (1070) 628 29 16', 'М'),
('Олеговна', 'Филенкова', 'Владлена', 'Vladlena@gmail.com', '07i7Lb', 2, '+7 (334) 146 01 51', 'Ж'),
('Владимирович', 'Ломовцев', 'Адам', 'Adam@gmail.com', '7SP9CV', 3, '+7 (8560) 519 32 99', 'М'),
('Федосиевич', 'Тепляков', 'Кир', 'Kar@gmail.com', '6QF1WB', 4, '+7 (824) 893 24 03', 'М'),
('Глебович', 'Рюриков', 'Юлий', 'Yli@gmail.com', 'GwffgE', 1, '+7 (6402) 701 31 09', 'М'),
('Тимофеевна', 'Беломестина', 'Василиса', 'Vasilisa@gmail.com', 'd7iKKV', 1, '+7 (92) 920 74 47', 'Ж'),
('Якововна', 'Панькива', 'Галина', 'Galina@gmail.com', '8KC7wJ', 2, '+7 (405) 088 73 89', 'Ж'),
('Мечиславович', 'Зарубин', 'Мирон', 'Miron@gmail.com', 'x58OA,', 1, '+7 (6010) 195 02 09', 'М'),
('Андрияновна', 'Веточкина', 'Всеслава', 'Vseslava@gmail.com', 'fhDSBf', 3, '+7 (078) 429 57 86', 'Ж'),
('Елизаровна', 'Рябова', 'Виктория', 'Victoria@gmail.com', '9mlPQJ', 4, '+7 (6394) 904 01 61', 'Ж'),
('Фёдорович', 'Федотов', 'Леон', 'Anisa@gmail.com', 'Wh4OYm', 2, ' +7 (22) 326 49 59', 'М'),
('Кириллович', 'Шарапов', 'Феофан', 'Feafan@gmail.com', 'Kc1PeS', 3, '+7 (789) 762 30 28', 'М');

insert into Mall_statuses(status_name) values
('План'),
('Строительсто'),
('Удалён'),
('Реализация');

insert into Mall(mall_name,status_id,number_of_pavilion,city,cost,value_added_factor,number_of_storeys) values
('Ривьера', 1, 0, 'Москва', 8260042, 0.5, 4),
('Авиапарк', 2, 9, 'Санкт-Петербург', 3297976, 0.1, 3),
('Мега Белая Дача', 3, 16, 'Новосибирск', 9006645, 1.7, 4),
('Рио', 4, 5, 'Екатеринбург', 1888653, 0.5, 1),
('Вегас', 1, 0, 'Нижний Новгород', 7567411, 0.4, 3),
('Лужайка', 2, 10, 'Казань', 4603336, 0.8, 2),
('Кунцево Плаза', 2, 8, 'Челябинск', 6797653, 1.1, 2),
('Мозаика', 2, 20, 'Самара', 1429419, 0.0, 4),
('Океания', 1, 0, 'Самара', 5192089, 1.8, 2),
('Гранд', 2, 20, 'Ростов-на-Дону', 2573981, 0.1, 4),
('Бутово Молл', 1, 0, 'Москва', 5327641, 1.7, 1),
('Рига Молл', 1, 0, 'Ростов-на-Дону', 9788316, 0.7, 3),
('Шоколад', 2, 12, 'Челябинск', 2217279, 1.1, 3),
('АфиМолл Сити', 4, 9, 'Санкт-Петербург', 8729160, 0.9, 3),
('Европейский', 2, 9, 'Москва', 5690500, 0.7, 3),
('Гагаринский', 1, 0, 'Екатеринбург', 1508807, 1.6, 1),
('Метрополис', 1, 0, 'Санкт-Петербург', 8117913, 0.0, 2),
('Мега Химки', 4, 3, 'Нижний Новгород', 3373234, 0.4, 1),
('Москва', 4, 12, 'Москва', 4226505, 0.3, 3),
('Вегас Кунцево', 4, 12, 'Москва', 3878254, 0.2, 4),
('Город Лефортово', 3, 12, 'Новосибирск', 1966214, 1.7, 4),
('Золотой Вавилон Ростокино', 4, 16, 'Екатеринбург', 1632702, 1.8, 4),
('Шереметьевский', 2, 16, 'Новосибирск', 2941379, 1.0, 4),
('Ханой-Москва', 1, 0, 'Самара', 9580221, 0.3, 4),
('Ашан Сити Капитолий', 2, 4, 'Екатеринбург', 5309117, 1.9, 1),
('Черемушки', 4, 15, 'Новосибирск', 7344604, 1.0, 3),
('Филион', 2, 8, 'Москва', 5343904, 0.3, 2),
('Весна', 3, 3, 'Нижний Новгород', 4687701, 0.8, 1),
('Гудзон', 4, 3, 'Екатеринбург', 7414460, 0.0, 1),
('Калейдоскоп', 4, 10, 'Новосибирск', 7847659, 0.7, 2),
('Новомосковский', 1, 0, 'Казань', 7161962.85, 0.4, 1),
('Хорошо', 4, 20, 'Ростов-на-Дону', 8306576, 1.9, 4),
('Щука', 2, 5, 'Нижний Новгород', 5428485, 0.4, 1),
('Атриум', 4, 4, 'Казань', 5059779, 0.2, 1),
('Принц Плаза', 4, 8, 'Самара', 1786688, 1.5, 2),
('Облака', 1, 0,'Санкт-Петербург',  1688905, 0.6, 3),
('Три Кита', 1, 0, 'Казань', 3089700, 1.7, 1),
('Реутов Парк', 2, 4,'Ростов-на-Дону',  1995904, 1.5, 1),
('ЕвроПарк', 3, 20, 'Новосибирск', 9391338, 0.7, 4),
('ГУМ', 4, 5, 'Санкт-Петербург', 6731491, 1.9, 1),
('Райкин Плаза', 2, 12, 'Санкт-Петербург', 8498470, 1.8, 3),
('Новинский пассаж', 4, 8, 'Екатеринбург', 3158957, 1.7, 2),
('Наш Гипермаркет', 1, 0, 'Ростов-на-Дону', 1088735, 1.2, 4),
('Красный Кит', 2, 9, 'Казань', 1912149, 1.1, 3),
('Мегаполис', 1, 0, 'Челябинск', 2175218.5, 0.5, 2),
('Отрада', 2, 4, 'Санкт-Петербург', 6760316, 0.9, 1),
('Твой дом', 1, 0, 'Челябинск', 6810865, 1.7, 4),
('Фестиваль', 3, 16, 'Новосибирск', 1828013, 0.2, 4),
('Времена Года', 4, 15, 'Екатеринбург', 2650484, 1.7, 3),
('Армада', 1, 0, 'Ростов-на-Дону ', 9172489, 0.9, 1);

insert into Pavilion_statuses(status_name) values
('Свободен'), -- в будущ нет ничего
('Забронировано'), -- есть интервал
('Удалён'), -- 
('Арендован'); -- сейчас идет

insert into Pavilion([mall_id], [pavilion_number], [floor], [status_id], [square], [cost_per_square_meter], [value_added_factor]) values
(50, '88А', 1, 4, 75, 3308, 0.1),
(36, '40А', 2, 3, 96, 690, 1.1),
(40, '76Б', 3, 2, 199, 4938, 0.9),
(27, '61А', 4, 1, 186, 2115, 0.9),
(40, '58В', 4, 4, 98, 1306, 1.9),
(28, '7А', 2, 2, 187, 2046, 1.0),
(27, '13Б', 2, 1, 141, 1131, 0.1),
(6, '60В', 2, 2, 94, 361, 0.3),
(42, '56А', 4, 1, 148, 1163, 0.6),
(45, '63Г', 2, 2, 153, 3486, 0.7),
(12, '8Г', 4, 1, 122, 2451, 1.5),
(31, '94В', 1, 3, 132, 4825, 2.0),
(14, '87Г', 2, 1, 174, 793, 1.5),
(9, '93В', 4, 1, 165, 4937, 0.8),
(40, '10А', 2, 3, 168, 1353, 1.7),
(13, '53Г', 4, 1, 99, 3924, 1.0),
(13, '73В', 1, 2, 116, 3418, 0.0),
(17, '89Б', 1, 1, 92, 562, 0.4),
(18, '44Г', 0, 2, 66.6, 870.3, 1.0),
(10, '65А', 0, 2, 95, 1381, 1.7),
(27, '16Г', 3, 2, 150, 747, 0.8),
(9, '61Б', 1, 1, 58, 1032, 1.7),
(35, '34Б', 2, 4, 102, 4715, 0.3),
(21, '91Г', 4, 3, 171, 1021, 1.2),
(39, '70Г', 2, 1, 83, 2246, 1.4),
(1, '10А', 0, 1, 187, 2067, 0.0),
(31, '80Г', 2, 1, 117, 1049, 1.3),
(17, '2Б', 3, 1, 176, 1673, 1.7),
(40, '41А', 1, 1, 108, 344, 0.0),
(29, '16Г', 4, 2, 125, 1037, 1.3),
(11, '13Б', 1, 2, 161.5, 2775.7, 0.4),
(20, '83Г', 4, 2, 85.5, 4584, 0.3),
(12, '9Г', 2, 1, 131, 2477, 1.5),
(6, '49Г', 4, 1, 164, 2728, 0.9),
(1, '1Г', 3, 1, 157, 1963, 1.6),
(50, '37А', 4, 3, 187, 3173, 0.3),
(46, '38Г', 4, 4, 97, 1364, 0.5),
(17, '27В', 2, 1, 96, 3134, 0.1),
(27, '67Б', 3, 1, 55, 4442, 0.8),
(25, '86Г', 1, 1, 58, 3707, 0.5),
(32, '98А', 2, 3, 172.5, 4951, 1.1),
(10, '11Г', 4, 2, 194, 827, 0.6),
(45, '42В', 1, 1, 166, 4216, 0.7),
(4, '55А', 2, 2, 127, 703, 1.0),
(8, '6Б', 1, 2, 119, 3262, 1.9),
(2, '15А', 2, 1, 90, 1569, 1.3),
(49, '27Б', 4, 3, 132, 627, 0.4),
(35, '65Б', 3, 4, 60, 3052, 0.6),
(2, '26А', 1, 1, 95, 670, 1.7),
(23, '53В', 4, 3, 132, 510, 1.2);


insert into Tenants(company_name, phone, address) values
('AG Marine', '+7 (495) 526 14 53', 'Москва, Бабаевская улица д.16'),
('Art-elle', '+7 (495) 250 58 94', 'Санкт-Петербург, Амбарная улица д.25 корп.1'),
('Atlantis', '+7 (495) 424 11 65', 'Новосибирск, Улица Каменская д.16'),
('Corporate Travel', '+7 (495) 245 39 05', 'Екатеринбург, Улица Антона Валека д.56'),
('GallaDance', '+7 (495) 316 77 94', 'Нижний Новгород, Улица Ларина д. 34');

insert into Rent_statuses(status_name) values
('Открыт'),
('Ожидание'),
('Закрыт');

insert into Rent([tenant_id], [mall_id], [employee_id], [pavilion_number], [status_id], [start_date], [end_date]) values
(2, 50, 2, '88А', 1, CONVERT(DATETIME,'24.01.2019', 103), CONVERT(DATETIME,'17.11.2020', 103)),
(4, 36, 7, '40А', 2, CONVERT(DATETIME,'21.11.2019', 103), CONVERT(DATETIME,'18.04.2020', 103)),
(1, 40, 11, '76Б', 3, CONVERT(DATETIME,'12.11.2018', 103), CONVERT(DATETIME,'25.06.2019', 103)),
(4, 27, 2, '61А', 3, CONVERT(DATETIME,'18.10.2018', 103), CONVERT(DATETIME,'16.09.2019', 103)),
(3, 40, 7, '58В', 2, CONVERT(DATETIME,'19.12.2019', 103), CONVERT(DATETIME,'26.06.2020', 103)),
(2, 28, 11, '7А', 2, CONVERT(DATETIME,'16.12.2019', 103), CONVERT(DATETIME,'12.05.2020', 103)),
(2, 27, 2, '13Б', 2, CONVERT(DATETIME,'06.12.2019', 103), CONVERT(DATETIME,'16.10.2020', 103)),
(3, 6, 11, '60В', 3, CONVERT(DATETIME,'03.09.2018', 103), CONVERT(DATETIME,'10.02.2019', 103)),
(5, 42, 2, '56А', 2, CONVERT(DATETIME,'04.11.2019', 103), CONVERT(DATETIME,'22.06.2020', 103)),
(3, 45, 7, '63Г', 3, CONVERT(DATETIME,'08.11.2018', 103), CONVERT(DATETIME,'16.01.2019', 103)),
(2, 12, 2, '8Г', 1, CONVERT(DATETIME,'07.06.2019', 103), CONVERT(DATETIME,'18.03.2020', 103)),
(3, 31, 2, '94В', 2, CONVERT(DATETIME,'15.11.2019', 103), CONVERT(DATETIME,'20.06.2020', 103)),
(3, 14, 11, '87Г', 3, CONVERT(DATETIME,'09.10.2018', 103), CONVERT(DATETIME,'22.09.2019', 103)),
(2, 9, 7, '93В', 2, CONVERT(DATETIME,'24.11.2019', 103), CONVERT(DATETIME,'17.07.2020', 103)),
(1, 40, 7, '10А', 3, CONVERT(DATETIME,'20.07.2018', 103), CONVERT(DATETIME,'07.06.2019', 103)),
(2, 13, 11, '53Г', 1, CONVERT(DATETIME,'04.02.2019', 103), CONVERT(DATETIME,'08.07.2020', 103)),
(5, 13, 2, '73В', 1, CONVERT(DATETIME,'06.08.2019', 103), CONVERT(DATETIME,'20.08.2020', 103)),
(2, 17, 7, '89Б', 1, CONVERT(DATETIME,'23.05.2019', 103), CONVERT(DATETIME,'13.05.2020', 103)),
(1, 18, 2, '44Г', 2, CONVERT(DATETIME,'16.12.2019', 103), CONVERT(DATETIME,'16.03.2020', 103)),
(4, 10, 11, '65А', 3, CONVERT(DATETIME,'24.08.2018', 103), CONVERT(DATETIME,'04.08.2019', 103)),
(3, 27, 2, '16Г', 2, CONVERT(DATETIME,'09.11.2019', 103), CONVERT(DATETIME,'17.08.2020', 103)),
(5, 9, 7, '61Б', 2, CONVERT(DATETIME,'02.12.2019', 103), CONVERT(DATETIME,'24.07.2020', 103)),
(1, 35, 11, '34Б', 2, CONVERT(DATETIME,'23.11.2019', 103), CONVERT(DATETIME,'06.08.2020', 103)),
(4, 21, 7, '91Г', 1, CONVERT(DATETIME,'02.05.2019', 103), CONVERT(DATETIME,'24.06.2020', 103)),
(4, 39, 2, '70Г', 2, CONVERT(DATETIME,'08.12.2019', 103), CONVERT(DATETIME,'17.08.2020', 103)),
(3, 1, 7, '10А', 3, CONVERT(DATETIME,'14.11.2018', 103), CONVERT(DATETIME,'19.04.2019', 103)),
(3, 31, 11, '80Г', 2, CONVERT(DATETIME,'26.12.2019', 103), CONVERT(DATETIME,'13.09.2020', 103)),
(3, 17, 2, '2Б', 3, CONVERT(DATETIME,'16.09.2018', 103), CONVERT(DATETIME,'12.02.2019', 103)),
(1, 40, 2, '41А', 3, CONVERT(DATETIME,'18.10.2018', 103), CONVERT(DATETIME,'22.06.2019', 103)),
(1, 29, 2, '16Г', 3, CONVERT(DATETIME,'23.06.2018', 103), CONVERT(DATETIME,'26.08.2019', 103)),
(4, 11, 11, '13Б', 2, CONVERT(DATETIME,'18.12.2019', 103), CONVERT(DATETIME,'23.05.2020', 103)),
(2, 20, 7, '83Г', 1, CONVERT(DATETIME,'01.04.2019', 103), CONVERT(DATETIME,'19.12.2020', 103)),
(2, 12, 11, '9Г', 2, CONVERT(DATETIME,'22.11.2019', 103), CONVERT(DATETIME,'15.08.2020', 103)),
(1, 6, 11, '49Г', 3, CONVERT(DATETIME,'08.10.2018', 103), CONVERT(DATETIME,'21.07.2019', 103)),
(1, 1, 2, '1Г', 1, CONVERT(DATETIME,'07.04.2019', 103), CONVERT(DATETIME,'05.03.2020', 103)),
(4, 50, 7, '37А', 2, CONVERT(DATETIME,'07.11.2019', 103), CONVERT(DATETIME,'08.03.2020', 103)),
(2, 46, 2, '38Г', 1, CONVERT(DATETIME,'15.07.2019', 103), CONVERT(DATETIME,'25.04.2020', 103)),
(4, 27, 11, '67Б', 3, CONVERT(DATETIME,'05.08.2018', 103), CONVERT(DATETIME,'14.06.2019', 103)),
(3, 25, 11, '86Г', 1, CONVERT(DATETIME,'19.08.2019', 103), CONVERT(DATETIME,'02.09.2020', 103)),
(5, 32, 7, '98А', 3, CONVERT(DATETIME,'20.09.2018', 103), CONVERT(DATETIME,'12.02.2019', 103));

/*
select * from Tenants

select * from Employees

select em.* from Employees_roles em

select em.*, er.role_name from Employees em
join Employees_roles er on em.role = er.role_id

select * from mall
where mall_name like '%рио%'

select * from mall m
join mall_statuses ms on ms.status_id = m.status_id

select * from Pavilion
select * from Pavilion_statuses

select * from Rent

select 
	p.mall_id,
	p.pavilion_number,
	ps.status_name [статус павильона],
	re.rent_id,
	re.start_date,
	re.end_date,
	rs.status_name [статус аренды]
from Pavilion p
join Pavilion_statuses ps on 
	ps.status_id = p.status_id
join Rent re on 
	re.mall_id = p.mall_id and 
	re.pavilion_number = p.pavilion_number
join Rent_statuses rs on 
	rs.status_id = re.status_id
where ps.[status_id] <> 3

*/

--************************************************************************
GO
IF EXISTS (SELECT name FROM sysobjects WHERE name = 'UpdateRentsAndPavilions' AND type = 'P')
BEGIN
   DROP PROCEDURE UpdateRentsAndPavilions;
END;
GO

/*
ПЕРЕНОС СРОКА АРЕНДЫ
Создайте  и выполните хранимую процедуру, которая перенесет срок аренды всех павильонов на год и обновит статусы павильонов в связи с переносом даты. 
*/

GO
CREATE PROCEDURE UpdateRentsAndPavilions AS
BEGIN
	declare @months int = 12 * 3 + 2;

	UPDATE rent
	SET 
		[start_date] = DATEADD(MONTH, @months, [start_date]), 
		[end_date] = DATEADD(MONTH, @months, [end_date]);

	UPDATE rent
	SET status_id = 
	CASE
		WHEN [start_date] <= GETDATE() AND [end_date] >= GETDATE()
		THEN 1 --открыт
		WHEN [start_date] > GETDATE()
		THEN 2 --ожидание
		WHEN [end_date] < GETDATE()
		THEN 3 --закрыт
	END;

	/*
	('Свободен'), -- в будущ нет ничего
	('Забронировано'), -- есть интервал
	('Удалён'), -- 
	('Арендован'); -- сейчас идет
	*/

	UPDATE Pavilion
	SET status_id = 
	CASE
		WHEN EXISTS (SELECT rent_id FROM rent WHERE [start_date] <= GETDATE() AND [end_date] >= GETDATE() AND pavilion_number = p.pavilion_number)
		THEN 4 --арендован
		WHEN EXISTS (SELECT rent_id FROM rent WHERE [start_date] > GETDATE() AND pavilion_number = p.pavilion_number)
		THEN 2 --забронирован
		ELSE 1 --свободен
	END
	FROM Pavilion p 
	JOIN rent r ON 
		r.pavilion_number = p.pavilion_number
END;
GO

print('=======UpdateRentsAndPavilions=======')
exec UpdateRentsAndPavilions;

--************************************************************************
IF EXISTS (SELECT name FROM sys.objects WHERE name = 'CheckerMalls' AND type = 'TR')  
   DROP TRIGGER CheckerMalls;  
GO  

CREATE TRIGGER CheckerMalls
ON Mall
FOR UPDATE
AS
BEGIN
	IF EXISTS (
		select 
			*
		from inserted i
		join deleted d on
			d.mall_id = i.mall_id
		where i.status_id = 1 and
			  d.status_id <> 1 and
			  (select 
				count(*)
			  from pavilion p
			  where status_id = 2 and 
				p.mall_id = i.mall_id) > 0
	) BEGIN
		RAISERROR ('Невозможно изменить статус ТЦ', 16, 1);
		ROLLBACK;
	END;
END
GO 

--************************************************************************
IF EXISTS (SELECT name FROM sys.objects WHERE name = 'CheckerPavilionsUpdate' AND type = 'TR')  
   DROP TRIGGER CheckerPavilionsUpdate;  
GO  

CREATE TRIGGER CheckerPavilionsUpdate
ON Pavilion
FOR UPDATE
AS
BEGIN
/*
Исключите возможность удаления и редактирования павильонов, имеющих статус 
«забронирован» или «арендован».
select 
	p.*,
	p.mall_id,
	p.pavilion_number,
	ps.status_id, -- [статус павильона]
	ps.status_name [статус павильона]
from Pavilion p
join Pavilion_statuses ps on 
	ps.status_id = p.status_id
*/

	IF EXISTS (
		select 
			*
		from deleted d
		where d.status_id in (2, 4) --and
			  --update((select status_id from inserted) )
	) BEGIN
		RAISERROR ('Невозможно редактировать павильон.', 16, 1);
		ROLLBACK;
	END;
END
GO 

--************************************************************************
IF EXISTS (SELECT name FROM sys.objects WHERE name = 'CheckerPavilionsDelete' AND type = 'TR')  
   DROP TRIGGER CheckerPavilionsDelete;  
GO  

CREATE TRIGGER CheckerPavilionsDelete
ON Pavilion
FOR DELETE
AS
BEGIN
	IF EXISTS (
		select
			*
		from deleted d
		where d.status_id in (2, 4)
	) BEGIN
		RAISERROR ('Невозможно удалить павильон.', 16, 1);
		ROLLBACK;
	END;
END
GO 

--************************************************************************
GO
IF EXISTS (SELECT name FROM sysobjects WHERE name = 'RentOrBookPavilionInMall' AND type = 'P')
BEGIN
   DROP PROCEDURE RentOrBookPavilionInMall;
END;
GO

GO
CREATE PROCEDURE RentOrBookPavilionInMall(
	@status_action bit,
	@pavilion_number nvarchar(255),
	@mall_id bigint,
	@start_date date,
	@end_date date,
	@tenant_id bigint,
	@employee_id bigint
) AS
BEGIN -- проверка по датам / статус тц смотреть
	declare 
		@status_id bigint,
		@found_booked_rents bigint;
	set @status_id = (select status_id from Mall
					  where mall_id = @mall_id);
	set @found_booked_rents = (select count(*) from Rent r
							   where not((start_date < @start_date and end_date < @start_date) or (start_date > @end_date and end_date > @end_date)) and
									 r.mall_id = @mall_id and 
									 r.pavilion_number = @pavilion_number)
	--print(@found_booked_rents)
	IF (@found_booked_rents > 0) BEGIN
		RAISERROR ('Нельзя арендовать/забронировать павильон на заданную дату.', 16, 1);
	END ELSE BEGIN
		IF (@status_action = 0) BEGIN -- аренда
			IF (@status_id = 2) BEGIN
				RAISERROR ('Нельзя арендовать павильон в статусе строительства.', 16, 1);
			END ELSE BEGIN
				insert into Rent([tenant_id], [mall_id], [employee_id], [pavilion_number], [status_id], [start_date], [end_date]) values
				(@tenant_id, @mall_id, @employee_id, @pavilion_number, 1, @start_date, @end_date);
			END;
		END ELSE BEGIN -- бронь
			IF (@status_id = 2) BEGIN -- бронь если незабронировано
				RAISERROR ('Нельзя забронировать павильон в статусе строительства.', 16, 1);
			END ELSE BEGIN
				insert into Rent([tenant_id], [mall_id], [employee_id], [pavilion_number], [status_id], [start_date], [end_date]) values
				(@tenant_id, @mall_id, @employee_id, @pavilion_number, 2, @start_date, @end_date);
			END;
		END;
	END;
END;
GO

print('=======RentOrBookPavilionInMall=======')
declare 
	@start date = CAST('12.07.2022' as date),
	@end date = CAST('13.07.2022' as date);
exec RentOrBookPavilionInMall 0, '55А', 4, @start, @end, 1, 4;
/*
select * from Rent
where pavilion_number = '55А'
*/
--************************************************************************
IF EXISTS (SELECT name FROM sys.objects WHERE name = 'CheckerRentStatuses' AND type = 'TR')  
   DROP TRIGGER CheckerRentStatuses;  
GO  

CREATE TRIGGER CheckerRentStatuses
ON Rent
AFTER INSERT
AS
BEGIN
	declare 
		@mall_id bigint,
		@pavilion_number nvarchar(255),
		@start_date date,
		@end_date date,
		@new_rent_status_id bigint;

	DECLARE cursor_rents CURSOR FOR 
	select 
		mall_id, 
		pavilion_number,
		start_date,
		end_date
	from inserted;

	OPEN cursor_rents
	FETCH NEXT FROM cursor_rents INTO @mall_id, @pavilion_number, @start_date, @end_date;

	-- по всем статусам аренды

	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		IF (getdate() between @start_date and @end_date) BEGIN
			set @new_rent_status_id = 4; -- Арендован
		END ELSE IF (@start_date < getdate() and @end_date < getdate()) BEGIN 
			set @new_rent_status_id = 1; -- Свободен
		END ELSE IF (@start_date > getdate() and @end_date > getdate()) BEGIN 
			set @new_rent_status_id = 2; -- Забронировано
		END;
		update Pavilion
		set status_id = @new_rent_status_id
		where mall_id = @mall_id and 
			  pavilion_number = @pavilion_number;
		FETCH NEXT FROM cursor_rents INTO @mall_id, @pavilion_number, @start_date, @end_date;
	END 
	CLOSE cursor_rents;
	DEALLOCATE cursor_rents;
END
GO 
