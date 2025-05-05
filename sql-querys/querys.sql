SELECT TOP (1000) [Id]
      ,[Content]
  FROM [WebsitesWatcher].[dbo].[Snapshots]

  delete websites
  delete snapshots


  alter table snapshots
  add [Timestamp] DATETIME not null DEFAULT GETUTCDATE()

  
  alter table websites
  add [Timestamp] DATETIME not null DEFAULT GETUTCDATE()

alter table Snapshots
drop constraint PK_Snapshots

alter table Snapshots
add constraint PK_Snapshots PRIMARY KEY (Id, Timestamp)