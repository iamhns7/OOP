using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using CompanyTaskProjectManagement.Entities;

namespace CompanyTaskProjectManagement.Repositories.Sql
{
    /// <summary>
    /// SQL Server tabanlı görev repository implementasyonu
    /// ITaskRepository interface'ini implemente eder
    /// </summary>
    public class TaskSqlRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public TaskSqlRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["CompanyTaskDB"]?.ConnectionString;
            
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'CompanyTaskDB' bulunamadı!");
            }
        }

        public IEnumerable<Task> GetAll()
        {
            List<Task> tasks = new List<Task>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"SELECT Id, Title as Baslik, Description as Aciklama, Status as Durum, 
                                    Priority as Oncelik, DueDate as SonTarih, CreatedBy as AtananKullaniciId, 
                                    CategoryId as ProjeId 
                                    FROM Tasks";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Task task = new Task
                                {
                                    Id = reader.GetInt32(0),
                                    Baslik = reader.GetString(1),
                                    Aciklama = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    Durum = (TaskStatus)reader.GetInt32(3),
                                    Oncelik = (TaskPriority)(reader.GetInt32(4) - 1), // DB'de 1-3, enum'da 0-2
                                    SonTarih = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                    AtananKullaniciId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                                    ProjeId = reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
                                };
                                
                                tasks.Add(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Görevler getirilirken hata: {ex.Message}", ex);
            }

            return tasks;
        }

        public Task GetById(int id)
        {
            Task task = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"SELECT Id, Title, Description, Status, Priority, DueDate, CreatedBy, CategoryId 
                                    FROM Tasks WHERE Id = @id";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                task = new Task
                                {
                                    Id = reader.GetInt32(0),
                                    Baslik = reader.GetString(1),
                                    Aciklama = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    Durum = (TaskStatus)reader.GetInt32(3),
                                    Oncelik = (TaskPriority)(reader.GetInt32(4) - 1),
                                    SonTarih = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                    AtananKullaniciId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                                    ProjeId = reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Görev getirilirken hata: {ex.Message}", ex);
            }

            return task;
        }

        public void Add(Task entity)
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
                    
                    string query = @"INSERT INTO Tasks (Title, Description, CategoryId, Priority, Status, CreatedBy, DueDate) 
                                    VALUES (@baslik, @aciklama, @projeId, @oncelik, @durum, @atananKullaniciId, @sonTarih);
                                    SELECT CAST(SCOPE_IDENTITY() as int);";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@baslik", entity.Baslik);
                        command.Parameters.AddWithValue("@aciklama", (object)entity.Aciklama ?? DBNull.Value);
                        command.Parameters.AddWithValue("@projeId", entity.ProjeId > 0 ? (object)entity.ProjeId : DBNull.Value);
                        command.Parameters.AddWithValue("@oncelik", (int)entity.Oncelik + 1); // enum 0-2, DB 1-3
                        command.Parameters.AddWithValue("@durum", (int)entity.Durum);
                        command.Parameters.AddWithValue("@atananKullaniciId", (object)entity.AtananKullaniciId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@sonTarih", (object)entity.SonTarih ?? DBNull.Value);
                        
                        int newId = (int)command.ExecuteScalar();
                        entity.Id = newId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Görev eklenirken hata: {ex.Message}", ex);
            }
        }

        public void Update(Task entity)
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
                    
                    string query = @"UPDATE Tasks 
                                    SET Title = @baslik, 
                                        Description = @aciklama, 
                                        CategoryId = @projeId, 
                                        Priority = @oncelik, 
                                        Status = @durum, 
                                        CreatedBy = @atananKullaniciId, 
                                        DueDate = @sonTarih 
                                    WHERE Id = @id";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", entity.Id);
                        command.Parameters.AddWithValue("@baslik", entity.Baslik);
                        command.Parameters.AddWithValue("@aciklama", (object)entity.Aciklama ?? DBNull.Value);
                        command.Parameters.AddWithValue("@projeId", entity.ProjeId > 0 ? (object)entity.ProjeId : DBNull.Value);
                        command.Parameters.AddWithValue("@oncelik", (int)entity.Oncelik + 1);
                        command.Parameters.AddWithValue("@durum", (int)entity.Durum);
                        command.Parameters.AddWithValue("@atananKullaniciId", (object)entity.AtananKullaniciId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@sonTarih", (object)entity.SonTarih ?? DBNull.Value);
                        
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Görev güncellenirken hata: {ex.Message}", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "DELETE FROM Tasks WHERE Id = @id";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Görev silinirken hata: {ex.Message}", ex);
            }
        }

        public IEnumerable<Task> GetByProjectId(int projeId)
        {
            List<Task> tasks = new List<Task>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"SELECT Id, Title, Description, Status, Priority, DueDate, CreatedBy, CategoryId 
                                    FROM Tasks WHERE CategoryId = @projeId";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@projeId", projeId);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Task task = new Task
                                {
                                    Id = reader.GetInt32(0),
                                    Baslik = reader.GetString(1),
                                    Aciklama = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    Durum = (TaskStatus)reader.GetInt32(3),
                                    Oncelik = (TaskPriority)(reader.GetInt32(4) - 1),
                                    SonTarih = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                    AtananKullaniciId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                                    ProjeId = reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
                                };
                                
                                tasks.Add(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Proje görevleri getirilirken hata: {ex.Message}", ex);
            }

            return tasks;
        }

        public IEnumerable<Task> GetByUserId(int kullaniciId)
        {
            List<Task> tasks = new List<Task>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"SELECT Id, Title, Description, Status, Priority, DueDate, CreatedBy, CategoryId 
                                    FROM Tasks WHERE CreatedBy = @kullaniciId";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@kullaniciId", kullaniciId);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Task task = new Task
                                {
                                    Id = reader.GetInt32(0),
                                    Baslik = reader.GetString(1),
                                    Aciklama = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    Durum = (TaskStatus)reader.GetInt32(3),
                                    Oncelik = (TaskPriority)(reader.GetInt32(4) - 1),
                                    SonTarih = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                    AtananKullaniciId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                                    ProjeId = reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
                                };
                                
                                tasks.Add(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Kullanıcı görevleri getirilirken hata: {ex.Message}", ex);
            }

            return tasks;
        }

        public IEnumerable<Task> GetByStatus(TaskStatus durum)
        {
            List<Task> tasks = new List<Task>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"SELECT Id, Title, Description, Status, Priority, DueDate, CreatedBy, CategoryId 
                                    FROM Tasks WHERE Status = @durum";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@durum", (int)durum);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Task task = new Task
                                {
                                    Id = reader.GetInt32(0),
                                    Baslik = reader.GetString(1),
                                    Aciklama = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    Durum = (TaskStatus)reader.GetInt32(3),
                                    Oncelik = (TaskPriority)(reader.GetInt32(4) - 1),
                                    SonTarih = reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                                    AtananKullaniciId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),
                                    ProjeId = reader.IsDBNull(7) ? 0 : reader.GetInt32(7)
                                };
                                
                                tasks.Add(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Durum görevleri getirilirken hata: {ex.Message}", ex);
            }

            return tasks;
        }
    }
}
