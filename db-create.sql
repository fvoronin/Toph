use master;

if exists (select * from sys.databases where name = N'Toph')
begin
    exec msdb.dbo.sp_delete_database_backuphistory @database_name = N'Toph';
    alter database Toph set single_user with rollback immediate;
    drop database Toph;
end

create database Toph;

GO



use Toph;

create table dbo.UserProfile (
    Id int identity(1,1) not null constraint PK_UserProfile primary key,
    Version int not null,
    Username nvarchar(255) not null
);

create table dbo.Invoice (
	Id int identity(1,1) not null constraint PK_Invoice primary key,
	Version int not null,
	InvoiceDate datetimeoffset(7) not null,
	InvoiceNumber nvarchar(255) not null,
	Name nvarchar(255) null,
	Line1 nvarchar(255) null,
	Line2 nvarchar(255) null,
	City nvarchar(255) null,
	State nvarchar(255) null,
	PostalCode nvarchar(255) null,
	UserProfileId int not null constraint FK_Invoice_UserProfileId foreign key references UserProfile (Id)
);

CREATE TABLE dbo.InvoiceLineItem (
	Id int identity(1,1) not null constraint PK_InvoiceLineItem primary key,
	Version int not null,
	LineItemDate datetimeoffset(7) null,
	Description nvarchar(255) null,
	Quantity float not null,
	Price float not null,
	InvoiceId int not null constraint FK_InvoiceLineItem_InvoiceId foreign key references Invoice (Id)
);

GO
