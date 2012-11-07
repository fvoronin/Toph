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

create table dbo.UserProfile
(
    Id int identity(1,1) not null constraint PK_UserProfile primary key,
    Username nvarchar(max) not null
);

GO
