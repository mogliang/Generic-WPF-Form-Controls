insert Users([Username],[Password],[Name],[IdentificationNumber],[PhoneNumber],[Company],[Department],[PermissionGroup],[Memo])
values('seed','123456',N'希德','410402197127470921','13564881231','Garden Crop','Research & Design', 1, N'研发部老大')

insert Users([Username],[Password],[Name],[IdentificationNumber],[PhoneNumber],[Company],[Department],[PermissionGroup],[Memo])
values('squall','123456',N'大力','310402197127470921','13564881231','Garden Crop','Marketing', 1, N'市场部总监')

insert Users([Username],[Password],[Name],[IdentificationNumber],[PhoneNumber],[Company],[Department],[PermissionGroup],[Memo])
values('seifer','123456',N'二娃','210402197127470921','13564881231','Garden Crop','Marketing', 1, N'市场部小弟,刚刚入职')

insert [dbo].[Products]([ScanCode],[Name],[Unit],[Memo])
values('111',N'21天网站开发速成教程',N'本',N'好书!葵花宝典')

insert [dbo].[Products]([ScanCode],[Name],[Unit],[Memo])
values('112',N'乐事薯片100g（清新黄瓜）',N'包',N'真好吃')

insert [dbo].[Products]([ScanCode],[Name],[Unit],[Memo])
values('113',N'菠菜罐头',N'听',N'神奇！吃完感觉很有干劲')

-- clear
delete from warehouseinbounds;
delete from users;
delete from products;
delete from goods;
delete from tags;
delete from taggroups;


-- query
select * from users