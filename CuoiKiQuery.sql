CREATE DATABASE QLBaiXe
GO

Use QLBaiXe	
Go
CREATE TABLE dbo.ChucVu(
	MaCV int PRIMARY KEY,
	ChucVu nvarchar(50) Null
)
insert into ChucVu values(1,N'Nhân viên');
insert into ChucVu values(2,N'Quản lý');
Go
CREATE TABLE dbo.NhanVien(
	MaNV INT IDENTITY(1,1) PRIMARY KEY,
	TenNV nvarchar(50) NULL,
	NamSinh char(4) NULL,
	DiaChi nvarchar(500) Null,
	GioiTinh nvarchar(4) Null,
	CMND nvarchar(15) Null,
	Sdt nvarchar(12) Null,
	Email nvarchar(100) Null,
	ChucVu int references ChucVu(MaCV)
)
Go
insert into NhanVien values(N'Nguyễn Văn A','2000',N'So 1 Vo Van Ngan',N'Nam','0842020026712','0987261662','ngvana@gmail.com',1);
insert into NhanVien values(N'Nguyễn Văn B','2002',N'So 2 Vo Van Ngan',N'Nam','0842020026711','0987261661','ngvanb@gmail.com',1);
insert into NhanVien values('Phan Gia huy','2001','So 2 Vo Van Ngan','Nam','0842020026711','0987261661','ngvanb@gmail.com',1);
GO
CREATE TABLE dbo.TaiKhoan(
	MaTk INT IDENTITY(1,1) PRIMARY KEY,
	Username nvarchar(30) NULL,
	MatKhau nvarchar(30) NULL,
	MaNV int references NhanVien(MaNV) ON DELETE CASCADE ON UPDATE CASCADE,
	Quyen int references ChucVu(MaCV) ON UPDATE CASCADE ON DELETE SET NULL
)
insert into TaiKhoan values('client','12345',1,1);
insert into TaiKhoan values('admin','12345',1,2);
GO
CREATE TABLE dbo.KhuVuc(
	MaKV int PRIMARY KEY,
	TenKV nvarchar(30) NULL,
	SoCho int NULL
)
Go
insert into KhuVuc values(1,'A',200);
insert into KhuVuc values(2,'B',100);
insert into KhuVuc values(3,'C',50);
insert into KhuVuc values(4,'D',20);

GO
CREATE TABLE dbo.TinhTrang(
	MaTT int PRIMARY KEY,
	TinhTrang nvarchar(10) NULL
)
insert into TinhTrang values(1,N'Đang giữ');
insert into TinhTrang values(2,N'Mất');
insert into TinhTrang values(3,N'Đã trả');
insert into TinhTrang values(4,N'Hư hại');

GO
CREATE TABLE dbo.BangGia(
	LoaiXe nvarchar(20) PRIMARY KEY,
	Gia bigint NULL,
	MaKV int references KhuVuc(MaKV)
)
go
insert into BangGia values(N'Xe máy',3000,1);
insert into BangGia values(N'Ô tô',15000,2);
insert into BangGia values(N'Xe tải',20000,3);
insert into BangGia values(N'Xe Container',30000,4);

GO
CREATE TABLE dbo.GiuXe(
	MaGX INT IDENTITY(1,1) PRIMARY KEY,
	MaNV int references NhanVien(MaNV),
	NgayVao datetime Null,
	NgayRa datetime Null,
	BienSoXe nvarchar(30) NULL,
	LoaiXe int references KhuVuc(MaKV),
	MaTT int references TinhTrang(MaTT),
	CONSTRAINT c_ngay CHECK(NgayRa>NgayVao),
)
go
insert into GiuXe values(1,'2022-10-10',null,'49H1-27182',1,1);
insert into GiuXe values(2,'2022-10-10',null,'49H1-22182',1,1);

---------Views
Go
create view BangGia_View AS
SELECT LoaiXe,Gia,SoCho
FROM BangGia join KhuVuc
on BangGia.MaKV = KhuVuc.MaKV;

--
go
create view Home_View AS
SELECT MaGX, NgayVao, BienSoXe, BangGia.LoaiXe, TinhTrang.TinhTrang
FROM dbo.GiuXe join BangGia
on GiuXe.LoaiXe = BangGia.MaKV
join TinhTrang
on GiuXe.MaTT = TinhTrang.MaTT
Where GiuXe.MaTT = 1

go
create view XeHuMat_View AS
SELECT MaGX, NgayVao, BienSoXe, BangGia.LoaiXe, TinhTrang.TinhTrang
FROM GiuXe join BangGia
on GiuXe.LoaiXe = BangGia.MaKV
join TinhTrang
on GiuXe.MaTT = TinhTrang.MaTT
Where GiuXe.MaTT = 2 or GiuXe.MaTT = 4

go
create view BangGia_info AS
SELECT LoaiXe, Gia, kv.TenKV , bg.MaKV
FROM BangGia bg join KhuVuc kv
on bg.MaKV = kv.MaKV

---------Functions
Go
CREATE FUNCTION TinhTien (@MaGiuXe INT)
RETURNS bigint AS
BEGIN
   DECLARE @tien bigint;
   SELECT @tien= (datediff(DD,NgayVao,getdate())+1)*BangGia.Gia FROM GiuXe 
   join BangGia On BangGia.MaKV = GiuXe.LoaiXe 
   WHERE MaGX=@MaGiuXe;
   RETURN @tien;
END

go
CREATE FUNCTION XemTaiKhoan (@Username nvarchar(30))
RETURNS TABLE AS
	return 
		(SELECT 
			TaiKhoan.Username as Username,
			TaiKhoan.MaNV as MaNV,
			NhanVien.TenNV as TenNV,
			NhanVien.NamSinh as NamSinh,
			NhanVien.DiaChi as DiaChi,
			NhanVien.GioiTinh as GioiTinh,
			NhanVien.CMND as CMND,
			NhanVien.Sdt as Sdt,
			NhanVien.Email as Email,
			ChucVu.ChucVu as ChucVu
		FROM dbo.TaiKhoan Inner join dbo.NhanVien 
		on dbo.TaiKhoan.MaNV = dbo.NhanVien.MaNV Inner join dbo.ChucVu 
		on dbo.NhanVien.ChucVu = dbo.ChucVu.MaCV
		WHERE @Username = '' or dbo.TaiKhoan.Username = @Username)
go

CREATE FUNCTION getMaNhanVien (@Username nvarchar(30))
RETURNS int AS
	BEGIN
		DECLARE @maNV int;
		SELECT @maNV = NhanVien.MaNV from NhanVien join TaiKhoan on NhanVien.MaNV = TaiKhoan.MaNV where TaiKhoan.Username = @Username;
		RETURN @maNV;
	END
----------Procedures
go
Create procedure sp_PhanQuyenUser
        @login nvarchar(30),
        @pass nvarchar(30)
as
declare @sql nvarchar(max)
set @sql = 'use QLBaiXe;' +
           'create login ' + @login + 
               ' with password = ''' + @pass + '''; ' +
           'create user '+ + @login + ' from login ' + @login + ';'
		   +'Grant select,insert,update on GiuXe to '+@login+';'
		   +'Grant select on BangGia to '+@login+';'
		   +'Grant select on NhanVien to '+@login+';'
		   +'Grant exec on sp_addGiuXe to '+ @login+';'
		   +'Grant exec on sp_updateRaBaiXe to '+ @login+';'
		   +'Grant exec on sp_updateMatXe to '+ @login+';'
		   +'Grant exec on sp_updateHuXe to '+ @login+';'
		   +'Grant exec on TinhTien to '+ @login+';'
		   +'Grant select on Home_View to '+ @login+';'
		   +'Grant select on BangGia_View to '+ @login+';'
exec (@sql)

go
Create procedure sp_XoaUser
        @login nvarchar(30)
as
declare @sql nvarchar(max)
set @sql = 'use QLBaiXe;' +
           +'drop user ' + @login + '; '
		   +'drop login ' + @login + '; '

exec (@sql)

---TaiKhoan
go
create proc sp_addAccount
@Username nvarchar(30), @Password nvarchar(30), @MaNV int, @Quyen int
as
	BEGIN
		INSERT INTO dbo.TaiKhoan(Username,MatKhau,MaNV,Quyen) values (@Username,@Password,@MaNV,@Quyen)
	END

go
create proc sp_updateAccount(@Username nvarchar(30), @Password nvarchar(30), @Quyen int)
as
	begin
		if (len(@Password) > 0)
			UPDATE dbo.TaiKhoan set
			MatKhau = @Password,
			Quyen = @Quyen
			WHERE Username = @Username
		else
			UPDATE dbo.TaiKhoan set
			Quyen = @Quyen
			WHERE Username = @Username
	end

go
create proc sp_deleteAccount(@Username nvarchar(30))
as 
	begin
		delete from dbo.TaiKhoan where Username = @Username
	end


----Bang Gia
go
create proc sp_addBangGia(@LoaiXe nvarchar(20),@Gia bigint,@MaKV int)
as
	begin
		INSERT INTO BangGia(LoaiXe,Gia,MaKV) values (@LoaiXe,@Gia,@MaKV)
	end;

go
create proc sp_updateBangGia(@LoaiXe nvarchar(20), @Gia bigint, @MaKV int)
as
	BEGIN
		UPDATE dbo.BangGia set
		Gia = @Gia,
		MaKV = @MaKV
		WHERE LoaiXe = @LoaiXe
	END

go
create proc sp_deleteBangGia(@LoaiXe nvarchar(20))
as
     begin
              DELETE FROM BangGia WHERE LoaiXe = @LoaiXe
     end;
----Nhan Vien
GO
create proc [dbo].[sp_addNhanVien](
@TenNV NVARCHAR(50),
@NamSinh char(4),
@DiaChi nvarchar(500),
@GioiTinh nvarchar(4),
@CMND nvarchar(15),
@Sdt nvarchar(12),
@Email nvarchar(100),
@ChucVu int
)
as
	BEGIN
		INSERT INTO dbo.NhanVien(TenNV,NamSinh,DiaChi,GioiTinh,CMND,Sdt,Email,ChucVu) values(@TenNV,@NamSinh,@DiaChi,@GioiTinh,@CMND,@Sdt,@Email,@ChucVu)
	END

GO
create proc [dbo].[sp_updateNhanVien](
@MaNV int,
@TenNV NVARCHAR(50),
@NamSinh char(4),
@DiaChi nvarchar(500),
@GioiTinh nvarchar(4),
@CMND nvarchar(15),
@Sdt nvarchar(12),
@Email nvarchar(100),
@ChucVu int
)
as
	BEGIN 
    UPDATE dbo.NhanVien SET 
    TenNV=@TenNV,
    NamSinh=@NamSinh,
	DiaChi=@DiaChi,
	GioiTinh=@GioiTinh,
	CMND=@CMND,
	Sdt=@Sdt,
	Email=@Email,
	ChucVu=@ChucVu
    WHERE MaNV=@MaNV 
END



go
create proc sp_addGiuXe
@MaNV int, @BienSoXe nvarchar(30), @LoaiXe int
as
	BEGIN
		INSERT INTO dbo.GiuXe(MaNV,NgayVao,NgayRa,BienSoXe,LoaiXe,MaTT) values (@MaNV, getdate(), null, @BienSoXe, @LoaiXe, 1)
	END

go
create proc sp_updateRaBaiXe
@MaGX int
as
	BEGIN
		update GiuXe
		set MaTT = 3,
		NgayRa=getdate()
		where MaGX = @MaGX
	END

go
create proc sp_updateMatXe
@MaGX int
as
	BEGIN
		update GiuXe
		set MaTT = 2
		where MaGX = @MaGX
	END
go

create proc sp_updateHuXe
@MaGX int
as
	BEGIN
		update GiuXe
		set MaTT = 4
		where MaGX = @MaGX
	END
go

----Login
GO
CREATE PROCEDURE sp_login
@Username nvarchar(20),
@Password nvarchar(20),
@ChkQuyen int
as
begin
	if exists (select * from TaiKhoan where Username = @Username and MatKhau = @Password and @ChkQuyen = Quyen and Quyen=1)
		select 1 as code
	else if exists (select * from TaiKhoan where Username = @Username and MatKhau = @Password and @ChkQuyen = Quyen and Quyen=2)
		select 0 as code
	else if exists(select * from TaiKhoan where Username = @Username and MatKhau != @Password and @ChkQuyen != Quyen) 
		select 2 as code
	else if exists(select * from TaiKhoan where Username = @Username and MatKhau = @Password) 
		select 2 as code
	else select 3 as code
end


-----------Triggers
go
Create trigger trg_GiuXe on GiuXe after insert as
begin
	update KhuVuc
	set SoCho = SoCho - 1
	From KhuVuc join inserted i  
	On KhuVuc.MaKV = i.LoaiXe
End

--
Go
Create trigger trg_TraXe on GiuXe after update as
begin
	update KhuVuc
	set SoCho = SoCho + 1
	From KhuVuc join inserted i
	On KhuVuc.MaKV = i.LoaiXe
End

Go
Create trigger trg_PhanQuyen on TaiKhoan after insert as
declare @Username nvarchar(30), @MatKhau nvarchar(30), @Quyen int
select @Username = Username from inserted
select @MatKhau = MatKhau from inserted
select @Quyen = Quyen from inserted
begin
	if @Quyen = 1
	begin
		exec sp_PhanQuyenUser @Username,@MatKhau
	end
End

Go
Create trigger trg_XoaUser on TaiKhoan after delete, update as
declare @Username nvarchar(30), @Quyen int
select @Username = Username from inserted
select @Quyen = Quyen from inserted
begin
	if @Quyen = 1
	begin
		exec sp_XoaUser @Username
	end
End
go
insert into TaiKhoan values('duong','12345',1,1);
