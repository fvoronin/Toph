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

create table dbo.Customer (
    Id int identity(1,1) not null constraint PK_Customer primary key,
    Version int not null,
    Name nvarchar(255) not null,
    OwnerId int not null constraint FK_Customer_OwnerId foreign key references UserProfile (Id)
);

create table dbo.Invoice (
	Id int identity(1,1) not null constraint PK_Invoice primary key,
	Version int not null,
	InvoiceDate datetimeoffset(7) not null,
	CustomerId int not null constraint FK_Invoice_CustomerId foreign key references Customer (Id)
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

CREATE TABLE dbo.Engagement (
	Id int identity(1,1) not null constraint PK_Engagement primary key,
	Version int not null,
	CustomerId int not null constraint FK_Engagement_CustomerId foreign key references Customer (Id)
);

CREATE TABLE dbo.TimeEntry (
	Id int identity(1,1) not null constraint PK_TimeEntry primary key,
	Version int not null,
	TimeEntryStart datetimeoffset(7) not null,
	TimeEntryEnd datetimeoffset(7) not null,
	EngagementId int not null constraint FK_TimeEntry_EngagementId foreign key references Engagement (Id),
	InvoiceLineItemId int null constraint FK_TimeEntry_InvoiceLineItemId foreign key references InvoiceLineItem (Id)
);

GO
