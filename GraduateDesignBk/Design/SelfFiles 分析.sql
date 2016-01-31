select * from DownUploads
select * from Files

select * from Files F,AspNetUsers A where F.FromUID =A.Id
--上传了一个文件
insert into Files values(NEWID(),NEWID(),'kxz毕业设计.doc','14b6b9be-e7d0-4e7b-86f4-6ace3bdf509e',0,GETDATE())  --FID 1
--选择接收人
--找到所有学生或者老师列表，勾选，上传文件，发送
select Id from AspNetUsers
--发送
select * from DownUploads

insert into DownUploads values(NEWID(),GETDATE(),'14b6b9be-e7d0-4e7b-86f4-6ace3bdf509e',1,0)
insert into DownUploads values(NEWID(),GETDATE(),'4eafd41b-b76c-43bf-ae74-dabf854ab462',1,0)
insert into DownUploads values(NEWID(),GETDATE(),'742b33e9-07e0-462b-ac6f-3486cc5b7ef4',1,0)
insert into DownUploads values(NEWID(),GETDATE(),'7d0-4e7b-86be-e7d0-4e7b-86f4-6ace3bd',1,0)
insert into DownUploads values(NEWID(),GETDATE(),'6f4b9be-e7d0-4e7b-86f4-6ace3bdf509e',1,0)

select * from Files where FromUID = '14b6b9be-e7d0-4e7b-86f4-6ace3bdf509e'

select D.DID,D.Time,D.ToUID,A.RealName,D.FID,D.Readed from DownUploads D,AspNetUsers A where D.ToUID=A.Id
--当用户读了就将reader设为1



delete from Files