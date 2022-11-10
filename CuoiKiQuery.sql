CREATE DATABASE QLBaiXe
GO

Use QLBaiXe	
Go
CREATE TABLE dbo.ChucVu(
	MaCV int PRIMARY KEY,
	ChucVu nvarchar(50) Null
)
insert into ChucVu values(1,'Nhân viên');
insert into ChucVu values(2,'Quản lý');
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
insert into NhanVien values('Nguyễn Văn A','2000','So 1 Vo Van Ngan','Nam','0842020026712','0987261662','ngvana@gmail.com',1);
insert into NhanVien values('Nguyễn Văn B','2002','So 2 Vo Van Ngan','Nam','0842020026711','0987261661','ngvanb@gmail.com',1);
insert into NhanVien values('Phan Gia huy','2001','So 2 Vo Van Ngan','Nam','0842020026711','0987261661','ngvanb@gmail.com',1);
GO
CREATE TABLE dbo.TaiKhoan(
	MaTk INT IDENTITY(1,1) PRIMARY KEY,
	Username nvarchar(30) not null unique,
	MatKhau nvarchar(30) not null,
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
insert into TinhTrang values(1,'Đang giữ');
insert into TinhTrang values(2,'Mất');
insert into TinhTrang values(3,'Đã trả');

GO
CREATE TABLE dbo.BangGia(
	LoaiXe nvarchar(20) PRIMARY KEY,
	Gia bigint NULL,
	MaKV int references KhuVuc(MaKV)
)
go
insert into BangGia values('Xe máy',3000,1);
insert into BangGia values('Ô tô',15000,2);
insert into BangGia values('Xe tải',20000,3);
insert into BangGia values('Xe Container',30000,4);

GO
CREATE TABLE dbo.GiuXe(
	MaGX INT IDENTITY(1,1) PRIMARY KEY,
	MaNV int references NhanVien(MaNV),
	NgayGiu date Null,
	BienSoXe nvarchar(30) NULL,
	LoaiXe int references KhuVuc(MaKV),
	MaTT int references TinhTrang(MaTT)
)
insert into GiuXe values(1,'2022-10-10','49H1-27182',1,1);

-----------Triggers
Go
Create trigger trg_GiuXe on GiuXe after insert as
begin
	update KhuVuc
	set SoCho = SoCho - 1
	From KhuVuc join GiuXe 
	On KhuVuc.MaKV = GiuXe.LoaiXe
	Where GiuXe.MaTT=1
End

--
Go
Create trigger trg_TraXe on GiuXe after insert as
begin
	update KhuVuc
	set SoCho = SoCho + 1
	From KhuVuc join GiuXe 
	On KhuVuc.MaKV = GiuXe.LoaiXe
	Where GiuXe.MaTT=3
End

---------Views
Go
create view BangGia_View AS
SELECT LoaiXe,Gia
FROM BangGia;
--
Go
CREATE VIEW NhanVien_View AS
SELECT MaNV,TenNV,DiaChi,Sdt,Email,ChucVu
FROM NhanVien
Where NhanVien.NamSinh > '2000'


---------Functions
Go
CREATE FUNCTION TinhTien (@MaGiuXe INT)
RETURNS bigint AS
BEGIN
   DECLARE @tien bigint;
   SELECT @tien= datediff(DD,NgayGiu,getdate())*Gia FROM GiuXe join BangGia On BangGia.LoaiXe = GiuXe.LoaiXe WHERE MaGX=@MaGiuXe;
   RETURN @tien;
END

ALTER FUNCTION XemTaiKhoan (@Username nvarchar(30))
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

----------Procedures
----Update
GO

CREATE PROCEDURE sp_updateNhanVien
@name NVARCHAR(50), @birth char(4), @address nvarchar(500), @gender nvarchar(4), @nationid nvarchar(15), @phone nvarchar(12), @email nvarchar(100),  @level int, @id INT
AS
BEGIN 
    UPDATE dbo.NhanVien SET 
    TenNV=@name, 
    NamSinh=@birth,
	DiaChi=@address,
	GioiTinh=@gender,
	CMND=@nationid,
	Sdt=@phone,
	Email=@email,
	ChucVu=@level
    WHERE MaNV=@id
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

create proc sp_deleteAccount(@Username nvarchar(30))
as 
	begin
		delete from dbo.TaiKhoan where Username = @Username
	end


go
create proc sp_updateBangGia(@LoaiXe nvarchar(20), @Gia bigint, @MaKV int)
as
	BEGIN
		UPDATE dbo.BangGia set
		Gia = @Gia,
		MaKV = @MaKV
		WHERE LoaiXe = @LoaiXe
	END

----Add
go
create proc sp_addNhanVien(
@name NVARCHAR(50),
@birth char(4),
@address nvarchar(500),
@gender nvarchar(4),
@nationid nvarchar(15),
@phone nvarchar(12),
@email nvarchar(100),
@level int,
@id int
)
as
	BEGIN
		INSERT INTO dbo.NhanVien(MaNV,TenNV,NamSinh,DiaChi,GioiTinh,CMND,Sdt,Email,ChucVu) values(@id,@name,@birth,@address,@gender,@nationid,@phone,@email,@level)
	END

go
create proc sp_addAccount(@Username nvarchar(30), @Password nvarchar(30), @MaNV int, @Quyen int)
as
	BEGIN
		INSERT INTO dbo.TaiKhoan(Username,MatKhau,MaNV,Quyen) values (@Username,@Password,@MaNV,@Quyen)
	END


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

