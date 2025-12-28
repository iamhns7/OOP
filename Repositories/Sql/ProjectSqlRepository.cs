using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;
using CompanyTaskProjectManagement.Entities;

namespace CompanyTaskProjectManagement.Repositories.Sql
{
    /// <summary>
    /// SQL Server tabanlı proje repository implementasyonu
    /// IProjectRepository interface'ini implemente eder
    /// </summary>
    public class ProjectSqlRepository : IProjectRepository
    {
        private readonly string _connectionString;

        public ProjectSqlRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["CompanyTaskDB"]?.ConnectionString;
            
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'CompanyTaskDB' bulunamadı!");
            }
        }

        public IEnumerable<Project> GetAll()
        {
            List<Project> projects = new List<Project>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT Id, ProjeAdi, Aciklama, BaslangicTarihi, BitisTarihi FROM Projects";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Project project = new Project
                                {
                                    Id = reader.GetInt32(0),
                                    ProjeAdi = reader.GetString(1),
                                    Aciklama = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    BaslangicTarihi = reader.GetDateTime(3),
                                    BitisTarihi = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                                };
                                
                                projects.Add(project);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Projeler getirilirken hata: {ex.Message}", ex);
            }

            return projects;
        }

        public Project GetById(int id)
        {
            Project project = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT Id, ProjeAdi, Aciklama, BaslangicTarihi, BitisTarihi FROM Projects WHERE Id = @id";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                project = new Project
                                {
                                    Id = reader.GetInt32(0),
                                    ProjeAdi = reader.GetString(1),
                                    Aciklama = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    BaslangicTarihi = reader.GetDateTime(3),
                                    BitisTarihi = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Proje getirilirken hata: {ex.Message}", ex);
            }

            return project;
        }

        public void Add(Project entity)
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
                    
                    string query = @"INSERT INTO Projects (ProjeAdi, Aciklama, BaslangicTarihi, BitisTarihi) 
                                    VALUES (@projeAdi, @aciklama, @baslangicTarihi, @bitisTarihi);
                                    SELECT CAST(SCOPE_IDENTITY() as int);";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@projeAdi", entity.ProjeAdi);
                        command.Parameters.AddWithValue("@aciklama", (object)entity.Aciklama ?? DBNull.Value);
                        command.Parameters.AddWithValue("@baslangicTarihi", entity.BaslangicTarihi);
                        command.Parameters.AddWithValue("@bitisTarihi", (object)entity.BitisTarihi ?? DBNull.Value);
                        
                        int newId = (int)command.ExecuteScalar();
                        entity.Id = newId;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Proje eklenirken hata: {ex.Message}", ex);
            }
        }

        public void Update(Project entity)
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
                    
                    string query = @"UPDATE Projects 
                                    SET ProjeAdi = @projeAdi, 
                                        Aciklama = @aciklama, 
                                        BaslangicTarihi = @baslangicTarihi, 
                                        BitisTarihi = @bitisTarihi 
                                    WHERE Id = @id";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", entity.Id);
                        command.Parameters.AddWithValue("@projeAdi", entity.ProjeAdi);
                        command.Parameters.AddWithValue("@aciklama", (object)entity.Aciklama ?? DBNull.Value);
                        command.Parameters.AddWithValue("@baslangicTarihi", entity.BaslangicTarihi);
                        command.Parameters.AddWithValue("@bitisTarihi", (object)entity.BitisTarihi ?? DBNull.Value);
                        
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Proje güncellenirken hata: {ex.Message}", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "DELETE FROM Projects WHERE Id = @id";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Proje silinirken hata: {ex.Message}", ex);
            }
        }

        public IEnumerable<Project> GetActiveProjects()
        {
            List<Project> projects = new List<Project>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"SELECT Id, ProjeAdi, Aciklama, BaslangicTarihi, BitisTarihi 
                                    FROM Projects 
                                    WHERE BitisTarihi IS NULL OR BitisTarihi >= GETDATE()";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Project project = new Project
                                {
                                    Id = reader.GetInt32(0),
                                    ProjeAdi = reader.GetString(1),
                                    Aciklama = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                    BaslangicTarihi = reader.GetDateTime(3),
                                    BitisTarihi = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4)
                                };
                                
                                projects.Add(project);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Aktif projeler getirilirken hata: {ex.Message}", ex);
            }

            return projects;
        }
    }
}
