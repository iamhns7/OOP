/*
 * SQL Server Express için veritabanı ve tablo oluşturma scripti
 * 
 * KULLANIM:
 * 1. SQL Server Management Studio (SSMS) veya Azure Data Studio'yu açın
 * 2. localhost\SQLEXPRESS instance'ına bağlanın (Windows Authentication)
 * 3. Bu scripti çalıştırın
 * 
 * NOT: Script otomatik olarak veritabanını oluşturur ve kullanır
 */

-- Veritabanı oluştur (eğer yoksa)
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CompanyTaskDB')
BEGIN
    CREATE DATABASE CompanyTaskDB;
    PRINT 'CompanyTaskDB veritabanı oluşturuldu.';
END
ELSE
BEGIN
    PRINT 'CompanyTaskDB veritabanı zaten mevcut.';
END
GO

-- Veritabanını kullan
USE CompanyTaskDB;
GO

-- Users tablosu oluştur
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users
    (
        Id INT PRIMARY KEY IDENTITY(1,1),
        AdSoyad NVARCHAR(100) NOT NULL,
        KullaniciAdi NVARCHAR(50) NOT NULL UNIQUE,
        Sifre NVARCHAR(100) NOT NULL,
        Rol INT NOT NULL,
        Onaylandi BIT NOT NULL DEFAULT 0  -- Admin onayı (0=Onaysız, 1=Onaylı)
    );
    PRINT 'Users tablosu oluşturuldu.';
END
ELSE
BEGIN
    PRINT 'Users tablosu zaten mevcut.';
    
    -- Mevcut tabloya Onaylandi kolonu ekle (eğer yoksa)
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Users') AND name = 'Onaylandi')
    BEGIN
        ALTER TABLE Users ADD Onaylandi BIT NOT NULL DEFAULT 0;
        PRINT 'Onaylandi kolonu eklendi.';
    END
END
GO

-- Örnek veri ekle (Admin ve test kullanıcıları)
IF NOT EXISTS (SELECT * FROM Users WHERE KullaniciAdi = 'admin')
BEGIN
    INSERT INTO Users (AdSoyad, KullaniciAdi, Sifre, Rol, Onaylandi)
    VALUES 
        ('Admin User', 'admin', 'admin123', 0, 1),  -- Rol: 0 = Admin, Onaylı
        ('Ahmet Yılmaz', 'ahmet', '123456', 1, 1),  -- Rol: 1 = Çalışan, Onaylı
        ('Ayşe Kaya', 'ayse', '123456', 1, 1),      -- Rol: 1 = Çalışan, Onaylı
        ('Mehmet Demir', 'mehmet', '123456', 1, 1); -- Rol: 1 = Çalışan, Onaylı
    
    PRINT 'Örnek kullanıcılar eklendi.';
END
ELSE
BEGIN
    PRINT 'Örnek kullanıcılar zaten mevcut.';
    
    -- Mevcut kullanıcıları onayla (eğer Onaylandi kolonu yeni eklendiyse)
    UPDATE Users SET Onaylandi = 1 WHERE KullaniciAdi IN ('admin', 'ahmet', 'ayse', 'mehmet');
    PRINT 'Mevcut kullanıcılar onaylandı.';
END
GO

-- Tabloyu sorgula
SELECT * FROM Users;
GO

/*
 * ROL DEĞERLERİ:
 * 0 = Admin
 * 1 = Çalışan (Calisan)
 * 
 * GİRİŞ BİLGİLERİ:
 * Admin: admin / admin123
 * Çalışan: ahmet / 123456
 * Çalışan: ayse / 123456
 * Çalışan: mehmet / 123456
 */
