using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using CompanyTaskProjectManagement.Entities;

namespace CompanyTaskProjectManagement.Repositories.Sql
{
    /// <summary>
    /// SQL Server tabanlı kullanıcı repository implementasyonu
    /// IUserRepository interface'ini implemente eder (Interface Implementation)
    /// </summary>
    public class UserSqlRepository : IUserRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor - App.config'den connection string okur
        /// </summary>
        public UserSqlRepository()
        {
            // App.config dosyasından connection string okuma
            _connectionString = ConfigurationManager.ConnectionStrings["CompanyTaskDB"]?.ConnectionString;
            
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'CompanyTaskDB' bulunamadı!");
            }
        }

        /// <summary>
        /// Tüm kullanıcıları SQL Server'dan getirir
        /// </summary>
        public IEnumerable<User> GetAll()
        {
            List<User> users = new List<User>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT Id, AdSoyad, KullaniciAdi, Sifre, Rol FROM Users";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    Id = reader.GetInt32(0),
                                    AdSoyad = reader.GetString(1),
                                    KullaniciAdi = reader.GetString(2),
                                    Sifre = reader.GetString(3),
                                    Rol = (UserRole)reader.GetInt32(4)
                                };
                                
                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"SQL Hatası: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Kullanıcılar getirilirken hata: {ex.Message}", ex);
            }

            return users;
        }

        /// <summary>
        /// Kullanıcı adına göre kullanıcı getirir
        /// </summary>
        public User GetByUsername(string kullaniciAdi)
        {
            User user = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    // Parametreli sorgu (SQL Injection'a karşı güvenli)
                    string query = "SELECT Id, AdSoyad, KullaniciAdi, Sifre, Rol FROM Users WHERE KullaniciAdi = @kullaniciAdi";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametre ekleme
                        command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    Id = reader.GetInt32(0),
                                    AdSoyad = reader.GetString(1),
                                    KullaniciAdi = reader.GetString(2),
                                    Sifre = reader.GetString(3),
                                    Rol = (UserRole)reader.GetInt32(4)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"SQL Hatası: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Kullanıcı getirilirken hata: {ex.Message}", ex);
            }

            return user;
        }

        /// <summary>
        /// Yeni kullanıcı ekler
        /// </summary>
        public void Add(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Kullanıcı nesnesi null olamaz!");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    // Parametreli INSERT sorgusu
                    string query = @"INSERT INTO Users (AdSoyad, KullaniciAdi, Sifre, Rol) 
                                    VALUES (@adSoyad, @kullaniciAdi, @sifre, @rol);
                                    SELECT CAST(SCOPE_IDENTITY() as int);";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametreler
                        command.Parameters.AddWithValue("@adSoyad", entity.AdSoyad);
                        command.Parameters.AddWithValue("@kullaniciAdi", entity.KullaniciAdi);
                        command.Parameters.AddWithValue("@sifre", entity.Sifre);
                        command.Parameters.AddWithValue("@rol", (int)entity.Rol);
                        
                        // Yeni eklenen kaydın ID'sini al
                        int newId = (int)command.ExecuteScalar();
                        entity.Id = newId;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"SQL Hatası: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Kullanıcı eklenirken hata: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kullanıcı doğrulama (authentication)
        /// </summary>
        public User Authenticate(string kullaniciAdi, string sifre)
        {
            User user = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    // Parametreli sorgu
                    string query = @"SELECT Id, AdSoyad, KullaniciAdi, Sifre, Rol 
                                    FROM Users 
                                    WHERE KullaniciAdi = @kullaniciAdi AND Sifre = @sifre";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                        command.Parameters.AddWithValue("@sifre", sifre);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    Id = reader.GetInt32(0),
                                    AdSoyad = reader.GetString(1),
                                    KullaniciAdi = reader.GetString(2),
                                    Sifre = reader.GetString(3),
                                    Rol = (UserRole)reader.GetInt32(4)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"SQL Hatası: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Kullanıcı doğrulama hatası: {ex.Message}", ex);
            }

            return user;
        }

        // IRepository<User> interface metodları
        // Bu metodlar henüz implement edilmedi - ihtiyaç halinde eklenebilir

        public User GetById(int id)
        {
            User user = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT Id, AdSoyad, KullaniciAdi, Sifre, Rol FROM Users WHERE Id = @id";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    Id = reader.GetInt32(0),
                                    AdSoyad = reader.GetString(1),
                                    KullaniciAdi = reader.GetString(2),
                                    Sifre = reader.GetString(3),
                                    Rol = (UserRole)reader.GetInt32(4)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Kullanıcı getirilirken hata: {ex.Message}", ex);
            }

            return user;
        }

        public void Update(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"UPDATE Users 
                                    SET AdSoyad = @adSoyad, 
                                        KullaniciAdi = @kullaniciAdi, 
                                        Sifre = @sifre, 
                                        Rol = @rol 
                                    WHERE Id = @id";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", entity.Id);
                        command.Parameters.AddWithValue("@adSoyad", entity.AdSoyad);
                        command.Parameters.AddWithValue("@kullaniciAdi", entity.KullaniciAdi);
                        command.Parameters.AddWithValue("@sifre", entity.Sifre);
                        command.Parameters.AddWithValue("@rol", (int)entity.Rol);
                        
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Kullanıcı güncellenirken hata: {ex.Message}", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "DELETE FROM Users WHERE Id = @id";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Kullanıcı silinirken hata: {ex.Message}", ex);
            }
        }
    }
}
